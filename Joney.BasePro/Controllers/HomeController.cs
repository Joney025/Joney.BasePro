using Joney.BasePro.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Joney.BasePro.Controllers
{
    
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var obj = new object();
            var set =new JsonSerializerSettings();
            
            var jsonStr = JsonConvert.SerializeObject(obj,Formatting.Indented,set);
            return View();
        }

        [MyFilter.ActionAttribut]//过滤器
        public ActionResult FilePreview()
        {
            return View();
        }

        [MyFilter.ActionAttribut]//过滤器
        public ActionResult WebUploader()
        {
            return View();
        }

        public ActionResult FilesTreeView()
        {
            return View();
        }


        public ActionResult FlowWork()
        {
            return View();
        }
        
        public ActionResult FlowChartDesign()
        {
            return View();
        }

        public ActionResult FlowDesign()
        {
            return View();
        }
        
        public ActionResult FlowDrawing()
        {
            return View();
        }

        /// <summary>
        /// 获取指定目录文件（夹）信息列表
        /// </summary>
        /// <returns></returns>
        [MyFilter.ActionAttribut(Code =1)]//过滤器
        public JsonResult GetFileDirList()
        {
            var res = new JsonResult();
            try
            {
                var dirStr =string.IsNullOrEmpty(Request.Params["DirPath"])?"": Request.Params["DirPath"];
                var spath = Server.MapPath("~/"+dirStr.Trim());
                
                var reqRoot = Request.Url.AbsoluteUri;
                var reqPath = Request.Url.AbsolutePath;
                var hostPath = reqRoot.Replace(reqPath,"/");
                var rootPath = Request.PhysicalApplicationPath;
                
                var list=IOHelper.GetDirFileList(hostPath, rootPath, spath, "0");
                var jsonStr = JsonConvert.SerializeObject(list);
                res.Data = jsonStr;
                res.ContentType = "1";
            }
            catch (Exception ex)
            {
                res.Data =ex.Message.ToString();
                res.ContentType = "-1";
            }
            return this.Json(res,JsonRequestBehavior.AllowGet);
        }

        [MyFilter.ActionAttribut(Code =1)]//过滤器
        public JsonResult ReadFileContent()
        {
            var res = new JsonResult();
            try
            {
                var dirStr = string.IsNullOrEmpty(Request.Params["FilePath"]) ? "" : Request.Params["FilePath"];
                var spath = Server.MapPath("~/" + dirStr);
                if (!System.IO.File.Exists(spath))
                {
                    res.Data = "";
                    res.ContentType = "0";
                }
                else
                {
                    FileInfo fi = new FileInfo(spath);
                    var fiext = fi.Extension;
                    dynamic jsonStr=null;
                    var txtFile =".txt|.css|.js|.cs";
                    var docFile = ".doc|.docx|.xlsx|.xls";
                    if (txtFile.IndexOf(fiext)>=0)
                    {
                        jsonStr = Joney.UtilityClassLibrary.LogHelper.ReadFile(spath);
                    }
                    else if (docFile.IndexOf(fiext) >= 0)
                    {
                        jsonStr = spath;
                    }
                    
                    res.Data = jsonStr;
                    res.ContentType = "1";
                }
                
            }
            catch (Exception ex)
            {
                res.Data = ex.Message.ToString();
                res.ContentType = "-1";
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetWeatherInfo()
        {
            var jres = new JsonResult();
            try
            {
                UtilController UC = new UtilController();
                var address = string.IsNullOrEmpty(Request.Params["CallUrl"])? "http://www.weather.com.cn/data/cityinfo/101010100.html" : Request.Params["CallUrl"];
                var param =string.IsNullOrEmpty(Request.Params["Code"])?"": Request.Params["Code"];
                var res = await UC._PostHttpInfo(address, param);
                jres.Data = res;
            }
            catch (Exception ex)
            {
                jres.Data = ex.Message.ToString();
            }
            return this.Json(jres,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// World 转PDF
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [MyFilter.ActionAttribut(Code = 1)]//过滤器
        public ActionResult World2PDFView(FormCollection form)
        {
            var res = new JsonResult();
            try
            {
                var filePath = form["LoadPath"];
                var savePath = "~/UploadFiles/WordFiles";
                savePath = Server.MapPath(savePath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string pdfName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";//PDF文件名
                savePath = string.Format("{0}\\{1}", savePath, pdfName);
                WebUnityHelper.ConvertWordPDF1(filePath, savePath);
                var reqUrl =Request.Url.Authority;
                res.Data = urlconvertor(reqUrl+"/"+savePath);//Request.MapPath(savePath);
                res.ContentType = "1";
            }
            catch (Exception ex)
            {
                res.Data = ex.Message.ToString();
                res.ContentType = "-1";
            }
            return this.Json(res,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        //[HttpPost]
        [MyFilter.ActionAttribut(Code = 1)]//过滤器
        public ActionResult FileUploader(FormCollection form,HttpPostedFileBase postfile)
        {
            var res = new JsonResult();
            try
            {
                //if (fileData == null || string.IsNullOrEmpty(fileData.FileName) || fileData.ContentLength == 0)
                //{
                //    return this.HttpNotFound();
                //}
                if (Request.Files.Count==0)
                {
                    return this.HttpNotFound();
                }
                var fileData =Request.Files[0];
                string fileName = Path.GetFileName(fileData.FileName);//当前文件名
                string fileExt = Path.GetExtension(fileData.FileName);//当前文件扩展名
                string uploadFolder = Request.Params["folder"] == null ? "~/UploadFiles" : "~/" + Request.Params["folder"].ToString();//文件上传的目录
                int fileLen = fileData.ContentLength;//文件大小
                var contentType = fileData.ContentType;//MiMe内容类型
                Stream fileStream = fileData.InputStream;
                byte[] fileByte = new byte[fileLen];//文件大杯具
                fileStream.Read(fileByte, 0, fileLen);//文件流转Byte字节数组
                fileStream.Seek(0, SeekOrigin.Begin);
                var imgWidth = 0;
                var imgHeight = 0;
                string imgTypes = ".bmp|.png|.gif|.jpeg|.jpg|.pcx|.tga|.exif|.fpx|.svg|.psd|.cdr|.pcd|.dxf|.ufo|.eps|.ai|.raw";
                if (imgTypes.Contains(fileExt.ToLower()))
                {
                    MemoryStream ms = new MemoryStream(fileByte);
                    Image image = Image.FromStream(ms);
                    imgWidth = image.Width;
                    imgHeight = image.Height;
                }

                #region 文件MD5加密

                //FileStream fs = new FileStream(svPath, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileStream);
                //fileStream.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                var fileMd5 = sb.ToString();

                #endregion

                #region 上传至本地服务器

                string newName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExt;//自定义文件名
                var gname = Guid.NewGuid().ToString("N")+fileExt;
                var cpath = Path.Combine(HttpRuntime.AppDomainAppPath, uploadFolder);
                var uploadPath = Server.MapPath(uploadFolder);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string virtualPath = string.Format("{0}/{1}", uploadFolder, newName);
                string path = this.Server.MapPath(virtualPath);
                fileData.SaveAs(path);

                fileStream.Close();

                #endregion

                #region 上传至文档服务器

                //if (1 == 0)
                //{
                //    var codeBS64 = MyUtil.EncryptBase64(mc.CompanyCode);//公司代码UTF-8转Base64.
                //    var fileNameBase64 = MyUtil.EncryptBase64(fileName);//文件名UTF-8转Base64.
                //    string ckAddress = string.Format("{0}/api/File/GetRemoteFileName/{1}/{2}", mc.FileUploadUrl, codeBS64, fileMd5);
                //    var ckRes = api._FileMD5Checking(ckAddress);//API校验MD5

                //    if (!string.IsNullOrEmpty(ckRes))
                //    {
                //        string fileServerName = ckRes;
                //        var resJson = new { fileServerPath = mc.FileDownLoadUrl, fileOldName = fileName, fileNewName = fileServerName, fileServerName = fileServerName, fileLength = fileLen, Width = imgWidth, Height = imgHeight };
                //        string res = JsonConvert.SerializeObject(resJson);
                //        jsonRes.Data = res;
                //    }
                //    else
                //    {
                //        string uploadUrl = string.Format("{0}/api/File/Post", mc.FileUploadUrl);
                //        string fExt = fileExt.Split('.')[1];
                //        var resName = "";//服务器返回的文件名
                //        var buffer = new byte[200 * 1024];//缓存容器大小：50 X 1024 KB
                //        Stream newStream = new MemoryStream(fileByte);
                //        var readLen = 0;
                //        var isDataEnd = 0;

                //        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                //        //sw.Start();
                //        while ((readLen = newStream.Read(buffer, 0, buffer.Length)) != 0)
                //        {
                //            if (readLen < buffer.Length)
                //            {
                //                isDataEnd = 1;
                //            }
                //            resName = await api._PostFileUploadQuery(uploadUrl, buffer, readLen, contentType, codeBS64, fileNameBase64, fExt, resName, isDataEnd);//API数据Post.并返回服务器保存的文件名
                //        }

                //        //sw.Stop();
                //        //var aa = sw.ElapsedMilliseconds / 1000;

                //        var resJson = new { fileServerPath = mc.FileDownLoadUrl, fileOldName = fileName, fileNewName = resName, fileServerName = resName, fileLength = fileLen, Width = imgWidth, Height = imgHeight };
                //        string res = JsonConvert.SerializeObject(resJson);
                //        jsonRes.Data = res;
                //    }
                //}
                //else
                //{
                //    string uploadUrl = string.Format("{0}/api/File/Post", mc.FileUploadUrl);
                //    string fExt = fileExt.Split('.')[1];
                //    var resName = "null";//服务器返回的文件名
                //    var buffer = new byte[512 * 1024];//缓存容器大小：512 X 1024 KB
                //    Stream newStream = new MemoryStream(fileByte);
                //    var readLen = 0;

                //    while ((readLen = newStream.Read(buffer, 0, buffer.Length)) != 0)
                //    {

                //        string serverUrl = string.Format("{0}/{1}/{2}/{3}/{4}", mc.FileUploadUrl, "UpLoadFile", mc.DocKey, resName, fileExt);
                //        string result = api._PostFileUpload(serverUrl, buffer, readLen, contentType);//API数据Post.并返回服务器保存的文件名
                //        resName = JsonConvert.DeserializeObject<dynamic>(result).FileName;//上传完成返回的文件名
                //    }
                //    var resJson = new { fileServerPath = mc.FileDownLoadUrl, fileOldName = fileName, fileNewName = resName, fileServerName = resName, fileLength = fileLen, Width = imgWidth, Height = imgHeight };
                //    string res = JsonConvert.SerializeObject(resJson);
                //    jsonRes.Data = res;

                //}

                #endregion

                var resJson = new {ServerPath = uploadPath,OldName = fileName,NewName = newName,ServerName = newName,Size = fileLen, Width = imgWidth, Height = imgHeight,Description="文件MD5值:"+ fileMd5 };
                string restr = JsonConvert.SerializeObject(resJson);
                res.Data = restr;
                res.ContentType = "1";
            }
            catch (Exception ex)
            {
                var resJsonEx = new { ServerPath = "", OldName = "", NewName = "", ServerName = "", Size =0, Width = 0, Height = 0,Description=""+ex.Message.ToString() };
                string reStr = JsonConvert.SerializeObject(resJsonEx);
                res.Data = reStr;
                res.ContentType = "-1";
            }
            return this.Json(res,JsonRequestBehavior.AllowGet);
        }

        //本地路径转换成URL相对路径
        private string urlconvertor(string imagesurl1)
        {
            string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            return imagesurl2;
        }
        //相对路径转换成服务器本地物理路径
        private string urlconvertorlocal(string imagesurl1)
        {
            string tmpRootDir = Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = tmpRootDir + imagesurl1.Replace(@"/", @"\"); //转换成绝对路径
            return imagesurl2;
        }
    }
}