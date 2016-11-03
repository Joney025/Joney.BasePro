using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Joney.BasePro.Models
{
    public class IOHelper
    {
        public static string GetCurrentAppUrl(string param)
        {
            try
            {
                var resPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                if (string.IsNullOrEmpty(param))
                {
                    resPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                }
                else
                {
                    switch (param)
                    {
                        case "Process":
                            //获取新的 Process 组件并将其与当前活动的进程关联的主模块的完整路径，包含文件名(进程名)。
                            resPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                            break;
                        case "CurrentDirectory":
                            //获取和设置当前目录（即该进程从中启动的目录）的完全限定路径。
                            resPath = System.Environment.CurrentDirectory;
                            break;
                        case "Thread":
                            //获取当前 Thread 的当前应用程序域的基目录，它由程序集冲突解决程序用来探测程序集。
                            resPath = System.AppDomain.CurrentDomain.BaseDirectory;
                            break;
                        case "ApplicationBase":
                            //获取和设置包含该应用程序的目录的名称。 
                            resPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                            break;
                        case "StartupPath":
                            //获取启动了应用程序的可执行文件的路径，不包括可执行文件的名称。
                            //resPath = System.Windows.Forms.Application.StartupPath;
                            break;
                        case "ExecutablePath":
                            //获取启动了应用程序的可执行文件的路径，包括可执行文件的名称。
                            //resPath = System.Windows.Forms.Application.ExecutablePath;
                            break;
                        case "GetCurrentDirectory":
                            //获取应用程序的当前工作目录(不可靠)。
                            resPath = System.IO.Directory.GetCurrentDirectory();
                            break;
                        default:
                            break;
                    }
                }
                

                return resPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static string GetRelativePath(string absolutePath,string relativeTo)
        {
            try
            {
                if (string.IsNullOrEmpty(relativeTo))
                {
                    System.Uri uri = new Uri(absolutePath);
                    System.Uri uri2 = new Uri(@"..\");
                    return uri.MakeRelativeUri(uri2).ToString();
                }
                string[] absoluteDirectories = absolutePath.Split('\\');
                string[] relativeDirectories = relativeTo.Split('\\');
                int leng = absoluteDirectories.Length < relativeDirectories.Length ? absoluteDirectories.Length:relativeDirectories.Length;

                int lastCommonRoot = -1;
                int index;
                for (index = 0; index < leng; index++)
                {
                    if (absoluteDirectories[index]==relativeDirectories[index])
                    {
                        lastCommonRoot = index;
                    }
                    else
                    {
                        break;
                    }
                }
                if (lastCommonRoot==-1)
                {
                    throw new ArgumentException("Paths do not have a common base.");
                }
                StringBuilder sb = new StringBuilder();
                for (index = 0; index < absoluteDirectories.Length; index++)
                {
                    if (absoluteDirectories[index].Length>0)
                    {
                        sb.Append("..\\");
                    }
                }
                for (index= 0; index < relativeDirectories.Length-1; index++)
                {
                    sb.Append(relativeDirectories[index]+"\\");
                }
                sb.Append(relativeDirectories[relativeDirectories.Length-1]);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public static string GetRelativePath(string absolutePath)
        {
            //绝对路径
            absolutePath = @""+ absolutePath;
            //相对路径
            string relativePath = @"..\";
            //预计拼接结果
            string splicingResult = string.Empty;
            
            if (!Path.IsPathRooted(relativePath))
            {
                //匹配相对路径，匹配需要向上推的目录层数
                Regex regex = new Regex(@"^\\|([..]+)");
                int backUp = regex.Matches(relativePath).Count;
                List<string> pathes = absolutePath.Split("\\".ToCharArray()).ToList();
                pathes.RemoveRange(pathes.Count - backUp, backUp);
                //匹配文件名，匹配需要附加的目录层数
                regex = new Regex(@"^\\|([a-zA-Z0-9]+)");
                MatchCollection matches = regex.Matches(relativePath);
                foreach (Match match in matches)
                {
                    pathes.Add(match.Value);
                }
                //驱动器地址取绝对路径中的驱动器地址
                pathes[0] = Path.GetPathRoot(absolutePath);
                foreach (string p in pathes)
                {
                    splicingResult = Path.Combine(splicingResult, p);
                }
            }
            
            var resPath = Path.Combine(absolutePath, relativePath);
            return splicingResult;
        }

        public static List<object> GetDirFileList(string hostPath,string rootPath,string dirPath,string pid)
        {
            List<object> fileList = new List<object>();
            try
            {
                
                if (!Directory.Exists(dirPath))
                {
                    return null;
                }
                
                DirectoryInfo dirFiles = new DirectoryInfo(dirPath);
                FileInfo[] infos = dirFiles.GetFiles();
                foreach (FileInfo item in infos)
                {
                    var id = Guid.NewGuid().ToString("N");
                    var fPath = hostPath+item.FullName.Replace(rootPath,"").Replace(@"\",@"/");
                    var fileInfo = new {ID=id,PID=pid,Name = item.Name, ModifyDate = item.LastWriteTime, FileExt = item.Extension, FileSize = item.Length, FilePath = fPath, children = new List<object>() };
                    fileList.Add(fileInfo);
                }

                string[] dirArr = System.IO.Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.TopDirectoryOnly);//.AllDirectories读取所有子文件夹下的所有子目录（历遍）
                string[] rootFileArr = System.IO.Directory.GetFiles(dirPath);
                for (int i = 0; i < dirArr.Length; i++)
                {
                    DirectoryInfo dirChild = new DirectoryInfo(dirArr[i]);
                    List<object> childList = new List<object>();
                    var id = Guid.NewGuid().ToString("N");
                    childList = GetDirFileList(hostPath,rootPath, dirArr[i], id);
                    var fPath = hostPath + dirChild.FullName.Replace(rootPath, "").Replace(@"\", @"/");
                    var fileInfo = new {ID=id,PID=pid,Name = dirChild.Name, ModifyDate = dirChild.LastWriteTime, FileExt = dirChild.Extension, FileSize =0,FilePath= fPath, children = childList };
                    fileList.Add(fileInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileList;
        }
        
    }
}