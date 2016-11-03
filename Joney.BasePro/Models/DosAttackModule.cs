using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace Joney.BasePro.Models
{
    public class DosAttackModule : IHttpModule
    {
        void IHttpModule.Dispose()
        {
            throw new NotImplementedException();
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private static Dictionary<string, short> _IpAddress = new Dictionary<string, short>();
        private static Stack<string> _Banned = new Stack<string>();
        private static Timer _Timer =CreateTimer();
        private static Timer _BannedTimer = CreateBanningTimer();
        private const int BANNED_REQUESTS = 1;//规定时间内访问的最大次数
        private const int REDUCTION_INTERVAL = 1000;//1秒。检查访问次数的时间段
        private const int RELEASE_INTERVAL = 5 * 60 * 1000;//5分钟。清除一个禁止IP的时间段

        private void context_BeginRequest(object sender, EventArgs e)
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            if (_Banned.Contains(ip))
            {
                HttpContext.Current.Response.StatusCode = 403;
                HttpContext.Current.Response.End();
            }
            CheckIpAddress(ip);
        }

        /// <summary>
        /// 检查访问IP
        /// </summary>
        /// <param name="ip"></param>
        private static void CheckIpAddress(string ip)
        {
            if (!_IpAddress.ContainsKey(ip))//如果没有当前访问IP的记录就将访问次数设为1
            {
                _IpAddress[ip]= 1;
            }else if (_IpAddress[ip]==BANNED_REQUESTS)//如果当前IP访问次数等于规定时间段的最大访问次数就加入黑名单
            {
                _Banned.Push(ip);
                _IpAddress.Remove(ip);
            }else
            {
                _IpAddress[ip]++;
            }
        }


        /// <summary>
        /// 创建定时器，从_IpAddress减去一个请求
        /// </summary>
        /// <returns></returns>
        private static Timer CreateTimer()
        {
            Timer timer = GetTimer(RELEASE_INTERVAL);
            timer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            return timer;
        }

        /// <summary>
        /// 创建定时器，消除一个禁止访问的IP地址
        /// </summary>
        /// <returns></returns>
        private static Timer CreateBanningTimer()
        {
            Timer timer = GetTimer(RELEASE_INTERVAL);
            timer.Elapsed += delegate { _Banned.Pop(); };//消除一个禁止的IP
            return timer;
        }

        /// <summary>
        /// 创建定时器，并启动它
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        private static Timer GetTimer(int interval)
        {
            Timer timer = new Timer();
            timer.Interval = interval;
            timer.Start();
            return timer;
        }

        /// <summary>
        /// 减去从集合中的每个IP地址的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TimerElapsed(object sender,ElapsedEventArgs e)
        {
            foreach (var item in _IpAddress.Keys)
            {
                _IpAddress[item]--;
                if (_IpAddress[item]==0)
                {
                    _IpAddress.Remove(item);
                }
            }
        }
    }
}