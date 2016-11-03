using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joney.MainConsoleTest.CommonClass
{
    public class LambdaShow
    {
        public delegate void NoReturnNoPara();
        public delegate void NoWithReturnPara(int id,string name);
        public delegate void NoWithReturnWithOnePara(int id);
        public delegate int WithReturnNoPara();
        public delegate int WithReturnWithPara(int id,string name);

        public static void Show()
        {
            {
                NoWithReturnPara method = new NoWithReturnPara(ShowStudent);//实例化委托
                method.Invoke(186,"joney");//调用委托
            }
            {
                NoWithReturnPara method = delegate(int id, string name)//匿名方法
                {
                    Console.WriteLine("id={0},name={1}正在上班。", id, name);
                };
                method.Invoke(213,"Saly");
            }

            {
                NoWithReturnPara method = (id, name) => 
                {
                    Console.WriteLine("id={0},name={1}正在开会。",id,name);
                };
                method.Invoke(88,"Jamer");
            }

            {
                NoWithReturnPara method = (id, name) => Console.WriteLine("id={0},name={1}正在执行终极任务。",id,name);
                method.Invoke(168,"Hammer");
            }

            {
                NoReturnNoPara method = () => { };
                NoWithReturnWithOnePara method1=i=>Console.WriteLine(i);
                method1.Invoke(12);
            }

            {
                WithReturnWithPara method = (m, n) => { return DateTime.Now.Second; };

                Console.WriteLine("当前时间second={0}",method.Invoke(1,"jj"));
            }

            {
                WithReturnWithPara method = (m, n) => DateTime.Now.Second;

                Console.WriteLine("当前时间second={0}", method.Invoke(1, "kk"));
            }
            {
                Action method = () => { };
                Action<int> act1 = i => { };

            }
        }

        private static void ShowStudent(int id, string name)
        {
            Console.WriteLine("id={0},name={1}正在上课！",id,name);
        }

        private static void ShowStudent1(int id, string name)
        {
            Console.WriteLine("id={0},name={1}正在上班！", id, name);
        }

        private static void ShowStudent2(int id, string name)
        {
            Console.WriteLine("id={0},name={1}正在开会！", id, name);
        }

        private static void ShowStudent3(int id, string name)
        {
            Console.WriteLine("id={0},name={1}正在执行终极任务！", id, name);
        }


    }
}
