using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace Joney.UtilityClassLibrary
{
    public class CheckCodeCreater
    {
        public CheckCodeCreater()
        {
            //构造函数
        }

        #region 纯数字组合

        ///<summary>
        /// 验证码的最大长度
        ///</summary>
        public int MaxLength
        {
            get { return 10; }
        }

        ///<summary>
        /// 验证码的最小长度
        ///</summary>
        public int MinLength
        {
            get { return 1; }
        }

        ///<summary>
        /// 生成验证码
        ///</summary>
        ///<param name="length">指定验证码的长度</param>
        ///<returns></returns>
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        ///<summary>
        /// 创建验证码的图片
        ///</summary>
        ///<param name="containsPage">要输出到的page对象</param>
        ///<param name="validateNum">验证码</param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion

        #region 数字&字母组合
        Random ran = new Random();

        public string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                number = ran.Next();
                if (number % 2 == 0)
                {
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    code = (char)('A' + (char)(number % 26));
                }
                if (code == '0' || code == 'o' || code == 'O')
                {
                    code = '0';
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }

        public byte[] CreateCheckCodeImage(string checkCode)
        {
            byte[] bytes;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 14.5)), 23);
            Graphics g = Graphics.FromImage(img);
            try
            {
                g.Clear(Color.White);
                for (int i = 0; i < 25; i++)
                {
                    int x1 = ran.Next(img.Width);
                    int x2 = ran.Next(img.Width);
                    int y1 = ran.Next(img.Height);
                    int y2 = ran.Next(img.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                for (int j = 0; j < 100; j++)
                {
                    int x = ran.Next(img.Width);
                    int y = ran.Next(img.Height);
                    img.SetPixel(x, y, Color.FromArgb(ran.Next()));
                }
                g.DrawRectangle(new Pen(Color.Black), 0, 0, img.Width - 1, img.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                bytes = ms.ToArray();
            }
            finally
            {
                g.Dispose();
                img.Dispose();
            }
            return bytes;
        }

        #endregion
    }
}
