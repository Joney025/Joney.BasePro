using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Office.Core;

namespace Joney.BasePro.Models
{
    public class WebUnityHelper
    {
        /// <summary>
        /// JSON转字典对象
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <param name="objName">检索的KEY对象</param>
        /// <returns></returns>
        public static Dictionary<string,object> JsonToDictionary(string json,string objName)
        {
            try
            {
                JavaScriptSerializer JSS = new JavaScriptSerializer();
                Dictionary<string, object> dic=JSS.Deserialize<Dictionary<string, object>>(json);
                Dictionary<string, object> dataSet = (Dictionary<string,object>)dic["dataSet"];
                Dictionary<string, object> res = new Dictionary<string, object>();
                foreach (KeyValuePair<string,object> item in dataSet)
                {
                    if (item.Key.ToString()==objName)
                    {
                        var childItem = (Dictionary<string, object>)item.Value;
                        foreach (var str in childItem)
                        {
                            res.Add(str.Key, str.Value);
                        }
                    }
                    else
                    {
                        res.Add(item.Key, item.Value);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region OfficeHelper

        #region 关于设置

        //设置Word DCOM访问权限
        //（命令打开：Dcomcnfg.exe）
        //组件服务――计算机――我的电脑――DCOM配置――找到 Microsoft Word xxoo 文档
        //点击属性
        //选择“安全性” 
        //选定“使用自定义访问权限”和“使用自定义启动权限” 
        //分别编辑权限，添加Everyone，IIS用户
        //选择“身份标识”，在选定“交互式用户” 即可

        //在Web.config里加<identity impersonate= "true" />

        #endregion
            
        /// <summary>
        /// Word转PDF文档。
        /// </summary>
        /// <param name="sourcePath">源文件路径 c:/xxoo.doc</param>
        /// <param name="targetPath">目标文件路径 c:/xxoo.pdf</param>
        /// <returns>true=成功</returns>
        public static bool ConvertWordToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            object paramMissing = Type.Missing;
            Microsoft.Office.Interop.Word.WdExportFormat expFm = Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF;
            Microsoft.Office.Interop.Word.Application app = null;
            Microsoft.Office.Interop.Word.Document doc = null;
            try
            {
                app = new Microsoft.Office.Interop.Word.Application();
                //app.Visible = false;
                //doc = app.Documents.Open(sourcePath);
                //doc.SaveAs2();
                //doc.ExportAsFixedFormat(targetPath,expFm);

                object paramSourceDocPath = sourcePath;
                string paramExportFilePath = targetPath;
                Microsoft.Office.Interop.Word.WdExportFormat paramExportFormat = expFm;
                bool paramOpenAfterExport = false;
                Microsoft.Office.Interop.Word.WdExportOptimizeFor paramExportOptimizeFor = Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
                Microsoft.Office.Interop.Word.WdExportRange paramExportRange = Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument;
                int paramStartPage = 0;
                int paramEndPage = 0;
                Microsoft.Office.Interop.Word.WdExportItem paramExportItem = Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent;
                bool paramIncludeDocProps = true;
                bool paramKeepIRM = true;
                Microsoft.Office.Interop.Word.WdExportCreateBookmarks paramCreateBookmarks = Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
                bool paramDocStructureTags = true;
                bool paramBitmapMissingFonts = true;
                bool paramUseISO19005_1 = false;

                doc = app.Documents.Open(
                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing, ref paramMissing, ref paramMissing,
                ref paramMissing);

                if (doc != null)
                    doc.ExportAsFixedFormat(paramExportFilePath,
                    paramExportFormat, paramOpenAfterExport,
                    paramExportOptimizeFor, paramExportRange, paramStartPage,
                    paramEndPage, paramExportItem, paramIncludeDocProps,
                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
                    paramBitmapMissingFonts, paramUseISO19005_1,
                    ref paramMissing);


                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close();
                    doc = null;
                }
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// Excel转PDF文档。
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=成功</returns>
        public static bool ConvertExcelToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object miss = Type.Missing;
            Microsoft.Office.Interop.Excel.Application app = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                workbook = app.Workbooks.Open(sourcePath);
                workbook.SaveAs();
                workbook.ExportAsFixedFormat(targetType, targetPath);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(true, miss, miss);
                    workbook = null;
                }
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }

        /// <summary>
        /// Powerpoint转PDF文档。
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns></returns>
        public static bool ConvertPowerPointToPDF(string sourcePath, string targetPath)
        {
            bool result = false;
            object miss = Type.Missing;
            Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType targetType = Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            Microsoft.Office.Interop.PowerPoint.Presentation present = null;
            Microsoft.Office.Interop.PowerPoint.Application app = null;
            try
            {
                app = new Microsoft.Office.Interop.PowerPoint.Application();
                present = app.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                present.SaveAs(targetPath, targetType, Microsoft.Office.Core.MsoTriState.msoTrue);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                if (present != null)
                {
                    present.Close();
                    present = null;
                }
                if (app != null)
                {
                    app.Quit();
                    app = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }


        /// <summary>
        /// 转换word为pdf
        /// </summary>
        /// <param name="filename">doc文件路径</param>
        /// <param name="savefilename">pdf保存路径</param>
        public static void ConvertWordPDF1(object filename, object savefilename)
        {
            Object Nothing = System.Reflection.Missing.Value;
            //创建一个名为WordApp的组件对象
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            //创建一个名为WordDoc的文档对象并打开
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(ref filename, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            //设置保存的格式
            object filefarmat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
            //保存为PDF
            doc.SaveAs(ref savefilename, ref filefarmat, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            //关闭文档对象
            doc.Close(ref Nothing, ref Nothing, ref Nothing);
            //退出组件
            wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
        }

        /// <summary>
        /// 转换word为pdf
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="savefilename"></param>
        public static void ConvertWordPDF2(object filename, object savefilename)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();

            Type wordType = wordApp.GetType();

            Microsoft.Office.Interop.Word.Documents docs = wordApp.Documents;

            Type docsType = docs.GetType();

            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open",
            System.Reflection.BindingFlags.InvokeMethod, null, (object)docs, new Object[] { filename, true, true });

            Type docType = doc.GetType();

            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { savefilename, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF });

            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);

            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, wordApp, null);
        }

        /// <summary>
        ///  word 转换为html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成的html的保存路径</param>
        /// <param name="wordFileName">转换后html文件的名字</param>
        public static void WordToHtml(string path, string savePath, string wordFileName)
        {
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Type wordType = word.GetType();
            Microsoft.Office.Interop.Word.Documents docs = word.Documents;
            Type docsType = docs.GetType();
            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
            Type docType = doc.GetType();
            string strSaveFileName = savePath + wordFileName + ".html";
            object saveFileName = (object)strSaveFileName;
            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
        }

        /// <summary>
        /// excel 转换为html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成的html的保存路径</param>
        /// <param name="wordFileName">转换后html文件的名字</param>
        public static void ExcelToHtml(string path, string savePath, string wordFileName)
        {
            string str = string.Empty;
            Microsoft.Office.Interop.Excel.Application repExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            workbook = repExcel.Application.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            object htmlFile = savePath + wordFileName + ".html";
            object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            workbook.SaveAs(htmlFile, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            object osave = false;
            workbook.Close(osave, Type.Missing, Type.Missing);
            repExcel.Quit();
        }

        /// <summary>
        /// ppt转换为html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成的html的保存路径</param>
        /// <param name="wordFileName">转换后html文件的名字</param>
        public static void PPTToHtml(string path, string savePath, string wordFileName)
        {
            Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
            string strSourceFile = path;
            string strDestinationFile = savePath + wordFileName + ".html";
            Microsoft.Office.Interop.PowerPoint.Presentation prsPres = ppApp.Presentations.Open(strSourceFile, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            prsPres.SaveAs(strDestinationFile, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsHTML, MsoTriState.msoTrue);
            prsPres.Close();
            ppApp.Quit();
        }

        #endregion

        #region iTextSharp PDF.

        public static BaseFont BF_Light = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        public static string imagePath = "../Data/分辨率使用情况报告_2014.04-2014.06.jpg";

        public static void GeneratePDFReport()
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            using (document)
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(string.Format("{0}-{1}.pdf", DateTime.Now.Second, DateTime.Now.Millisecond),
                     FileMode.Create));

                document.Open();
                document.Add(GetBaseTable());
                document.Add(GetBaseAndImageTable());

                DirectDrawReport(writer.DirectContentUnder);

                document.NewPage();

                document.Add(GetAllInfoTable());
            }
        }
        
        public static PdfPTable GetAllInfoTable()
        {
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100f;
            table.SetWidths(new int[] { 1, 3, 2, 18 });

            PdfPCell cell;
            iTextSharp.text.Paragraph p;
            int rowNum = 0;
            int textColumNum = 3;

            using (TextFieldParser parser = new TextFieldParser("../../Data/screen.csv", UTF8Encoding.UTF8, true))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (rowNum > 0)
                    {
                        for (int i = 0; i < textColumNum; i++)
                        {
                            p = new iTextSharp.text.Paragraph(fields[i], new iTextSharp.text.Font(BF_Light, 10));
                            cell = new PdfPCell(p);
                            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(cell);
                        }

                        string fileName = string.Format("../../Data/{0}.csv", fields[1]);
                        float yMax = float.Parse(fields[3]);
                        float yScale = float.Parse(fields[4]);


                        cell = new PdfPCell(new Phrase(""));
                        cell.MinimumHeight = 55f;
                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //画图的类，和cell关联                        
                        ResolutionChart chart = new ResolutionChart(fileName, yMax, yScale);
                        cell.CellEvent = chart;
                        table.AddCell(cell);
                        rowNum++;

                    }
                    else
                    {
                        rowNum++;
                    }
                }
            }


            return table;
        }

        public static PdfPTable GetBaseAndImageTable()
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100f;

            PdfPCell cell;

            //图片
            Image image = Image.GetInstance(imagePath);
            cell = new PdfPCell(image, true);
            table.AddCell(cell);

            //表格
            PdfPTable baseTable = GetBaseTable();
            cell = new PdfPCell(baseTable);
            table.AddCell(cell);

            table.SpacingBefore = 20f;

            return table;
        }

        public static PdfPTable GetBaseTable()
        {
            PdfPTable table = new PdfPTable(3);

            PdfPCell cell;
            iTextSharp.text.Paragraph p;
            int mainColumn = 3;
            int rowNum = 0;

            using (TextFieldParser parser = new TextFieldParser("../Data/screen.csv", UTF8Encoding.UTF8, true))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields.Length > mainColumn - 1)
                    {
                        for (int i = 0; i < mainColumn; i++)
                        {
                            //要设置字体和大小
                            p = new iTextSharp.text.Paragraph(fields[i], new iTextSharp.text.Font(BF_Light, 13));
                            cell = new PdfPCell(p);
                            //设置cell属性
                            //cell.Border = Rectangle.NO_BORDER;
                            if (rowNum == 0)
                            {
                                cell.BackgroundColor = BaseColor.GRAY;
                            }
                            if (i == mainColumn - 1)
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            //添加单元格
                            table.AddCell(cell);
                        }
                        rowNum++;
                    }
                }
            }

            return table;
        }

        public static void DirectDrawReport(PdfContentByte canvas)
        {
            //画线
            canvas.SaveState();
            canvas.SetLineWidth(2f);
            canvas.MoveTo(100, 100);
            canvas.LineTo(200, 200);
            canvas.Stroke();
            canvas.RestoreState();

            //文本
            ColumnText.ShowTextAligned(canvas, Element.ALIGN_RIGHT, new Phrase("Joney测试", new iTextSharp.text.Font(BF_Light, 10)), 100, 20, 0);
        }

        class ResolutionChart : IPdfPCellEvent
        {
            string fileName;
            float yMax;
            float yScaleNum;
            float marginLeft;
            float marginRight;
            float marginTop;
            float marginBottom;
            BaseFont BF_Light = BaseFont.CreateFont(ConfigurationManager.AppSettings["ReportFont"], BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            public ResolutionChart(string fileName, float yMax, float yScaleNum)
            {
                this.fileName = fileName;
                this.yMax = yMax;
                this.yScaleNum = yScaleNum;
                marginBottom = marginRight = marginTop = 5f;
                marginLeft = 10f;
            }

            public void CellLayout(PdfPCell cell, iTextSharp.text.Rectangle position, PdfContentByte[] canvases)
            {
                PdfContentByte cb = canvases[PdfPTable.BACKGROUNDCANVAS];
                PdfContentByte cbline = canvases[PdfPTable.LINECANVAS];

                cbline.SaveState();
                cb.SaveState();

                int totalMonths = 91;

                float leftX = position.Left + marginLeft;
                float bottomY = position.Bottom + marginBottom;

                float righX = position.Right - marginRight;
                float topY = position.Top - marginTop;

                float xScale = (righX - leftX) / totalMonths;
                float yScale = (topY - bottomY) / yMax;

                cb.SetLineWidth(0.4f);
                cbline.SetLineWidth(0.4f);
                //y 轴
                cb.MoveTo(leftX, bottomY);
                cb.LineTo(leftX, topY);
                cb.Stroke();
                //y 轴突出的短横线
                float yAxiseTextLinetWidth = 3f;
                float yAxisTextSpaceAdjust = 2.5f;
                for (float y = yScaleNum; y < yMax; y += yScaleNum)
                {
                    float yPoint = bottomY + (yScale * y);
                    cb.MoveTo(leftX, yPoint);
                    cb.LineTo(leftX - yAxiseTextLinetWidth, yPoint);
                    cb.Stroke();
                }
                //y 轴文本
                for (float y = yScaleNum; y < yMax; y += yScaleNum)
                {
                    float yPoint = bottomY + (yScale * y);
                    ColumnText.ShowTextAligned(cb, Element.ALIGN_RIGHT, new Phrase(string.Format("{0}%", y), new iTextSharp.text.Font(BF_Light, 5)), leftX - yAxiseTextLinetWidth, yPoint - yAxisTextSpaceAdjust, 0);
                }

                //x 轴
                cb.MoveTo(leftX, bottomY);
                cb.LineTo(righX, bottomY);
                cb.Stroke();

                int monthNum = 1;

                cb.SetRGBColorStroke(0xFF, 0x45, 0x00);

                cbline.SetLineDash(2f);

                using (TextFieldParser parser = new TextFieldParser(fileName, UTF8Encoding.UTF8, true))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (fields.Length > 2)
                        {
                            float oneMonthValue = float.Parse(fields[1].TrimEnd('%'));
                            float xPoint = (monthNum - 1) * xScale + leftX;
                            float yPoint = bottomY + (yScale * oneMonthValue);

                            if (monthNum == 1)
                            {
                                cb.MoveTo(xPoint, yPoint);
                            }
                            else
                            {
                                cb.LineTo(xPoint, yPoint);
                            }

                            if (monthNum % 7 == 0)
                            {
                                cbline.MoveTo(xPoint, yPoint);
                                cbline.LineTo(xPoint, bottomY);

                                cbline.Stroke();

                                string month = fields[0].Substring(7, 5);
                                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(month, new iTextSharp.text.Font(BF_Light, 5)), xPoint, bottomY - 5f, 0);
                            }
                        }
                        monthNum++;
                    }
                }

                cb.Stroke();

                cb.RestoreState();
                cbline.RestoreState();
            }
        }

        #endregion

        #region iTextSharp NPOI.

        ///// <summary>
        ///// 导出Pdf
        ///// </summary>
        ///// <param name="path">文件保存路径</param>
        ///// <param name="dtSource">数据源</param>
        ///// <param name="HorV">页面横竖（为空表示竖，有非空值为横）</param>
        //public static void ExportPDF(string localFilePath, DataTable dtSource, string HorV)
        //{
        //    iTextSharp.text.io.StreamUtil.AddToResourceSearch("iTextAsian.dll");
        //    iTextSharp.text.io.StreamUtil.AddToResourceSearch("iTextAsianCmaps.dll");

        //    BaseFont bf;
        //    string basepath = Application.StartupPath;
        //    try
        //    {
        //        bf = BaseFont.CreateFont(basepath + "\\FONTS\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //    }
        //    catch
        //    {

        //        bf = BaseFont.CreateFont("C:\\WINDOWS\\Fonts\\STSONG.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        //    }
        //    iTextSharp.text.Font font = new iTextSharp.text.Font(bf);

        //    iTextSharp.text.Document pdf;
        //    if (string.IsNullOrEmpty(HorV))
        //        pdf = new iTextSharp.text.Document();
        //    else
        //        pdf = new iTextSharp.text.Document(PageSize.A4.Rotate());

        //    PdfPTable table = new PdfPTable(dtSource.Columns.Count);
        //    table.HorizontalAlignment = Element.ALIGN_CENTER;

        //    PdfPCell cell;

        //    for (int i = 0; i < dtSource.Rows.Count + 1; i++)
        //    {
        //        for (int j = 0; j < dtSource.Columns.Count; j++)
        //        {
        //            if (i == 0)
        //            {
        //                cell = new PdfPCell(new Phrase(dtSource.Columns[j].ColumnName, font));
        //            }
        //            else
        //            {
        //                cell = new PdfPCell(new Phrase(dtSource.Rows[i - 1][j].ToString(), font));
        //            }

        //            table.AddCell(cell);
        //        }
        //    }

        //    using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
        //    {

        //        PdfWriter.GetInstance(pdf, fs);
        //        pdf.Open();
        //        pdf.Add(table);
        //        pdf.Close();
        //    }
        //}

        ///// <summary>
        ///// 导出Xls
        ///// </summary>
        ///// <param name="localFilePath">文件保存路径</param>
        ///// <param name="dtSource">数据源</param>
        //public static void ExportXls(string localFilePath, DataTable dtSource)
        //{
        //    HSSFWorkbook workbook = new HSSFWorkbook();
        //    HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

        //    HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
        //    HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
        //    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

        //    //取得列宽
        //    int[] arrColWidth = new int[dtSource.Columns.Count];
        //    foreach (DataColumn item in dtSource.Columns)
        //    {
        //        arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
        //    }
        //    for (int i = 0; i < dtSource.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtSource.Columns.Count; j++)
        //        {
        //            int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
        //            if (intTemp > arrColWidth[j])
        //            {
        //                arrColWidth[j] = intTemp;
        //            }
        //        }
        //    }
        //    int rowIndex = 0;
        //    foreach (DataRow row in dtSource.Rows)
        //    {
        //        #region 新建Sheet，填充列头，样式
        //        if (rowIndex == 65535 || rowIndex == 0)
        //        {
        //            if (rowIndex != 0)
        //            {
        //                sheet = (HSSFSheet)workbook.CreateSheet();
        //            }

        //            #region 列头及样式
        //            {
        //                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
        //                HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
        //                headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //                HSSFFont font = (HSSFFont)workbook.CreateFont();
        //                font.FontHeightInPoints = 10;
        //                font.Boldweight = 700;
        //                headStyle.SetFont(font);
        //                foreach (DataColumn column in dtSource.Columns)
        //                {
        //                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        //                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
        //                    //设置列宽
        //                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
        //                }

        //            }
        //            #endregion
        //            rowIndex = 1;
        //        }
        //        #endregion

        //        #region 填充内容
        //        HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
        //        foreach (DataColumn column in dtSource.Columns)
        //        {
        //            HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

        //            string drValue = row[column].ToString();

        //            switch (column.DataType.ToString())
        //            {
        //                case "System.String"://字符串类型
        //                    newCell.SetCellValue(drValue);
        //                    break;
        //                case "System.DateTime"://日期类型
        //                    DateTime dateV;
        //                    DateTime.TryParse(drValue, out dateV);
        //                    newCell.SetCellValue(dateV);
        //                    newCell.CellStyle = dateStyle;//格式化显示
        //                    break;
        //                case "System.Boolean"://布尔型
        //                    bool boolV = false;
        //                    bool.TryParse(drValue, out boolV);
        //                    newCell.SetCellValue(boolV);
        //                    break;
        //                case "System.Int16"://整型
        //                case "System.Int32":
        //                case "System.Int64":
        //                case "System.Byte":
        //                    int intV = 0;
        //                    int.TryParse(drValue, out intV);
        //                    newCell.SetCellValue(intV);
        //                    break;
        //                case "System.Decimal"://浮点型
        //                case "System.Double":
        //                    double doubV = 0;
        //                    double.TryParse(drValue, out doubV);
        //                    newCell.SetCellValue(doubV);
        //                    break;
        //                case "System.DBNull"://空值处理
        //                    newCell.SetCellValue("");
        //                    break;
        //                default:
        //                    newCell.SetCellValue("");
        //                    break;
        //            }
        //        }
        //        #endregion
        //        rowIndex++;
        //    }

        //    using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
        //    {
        //        workbook.Write(fs);
        //    }

        //}

        ///// <summary>
        ///// 导出Xlsx
        ///// </summary>
        ///// <param name="localFilePath">文件保存路径</param>
        ///// <param name="dtSource">数据源</param>
        //public static void ExportXlsx(string localFilePath, DataTable dtSource)
        //{
        //    XSSFWorkbook workbook = new XSSFWorkbook();
        //    XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet();

        //    XSSFCellStyle dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
        //    XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
        //    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

        //    //取得列宽
        //    int[] arrColWidth = new int[dtSource.Columns.Count];
        //    foreach (DataColumn item in dtSource.Columns)
        //    {
        //        arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
        //    }
        //    for (int i = 0; i < dtSource.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dtSource.Columns.Count; j++)
        //        {
        //            int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
        //            if (intTemp > arrColWidth[j])
        //            {
        //                arrColWidth[j] = intTemp;
        //            }
        //        }
        //    }
        //    int rowIndex = 0;
        //    foreach (DataRow row in dtSource.Rows)
        //    {
        //        #region 新建表，填充列头，样式
        //        if (rowIndex == 65535 || rowIndex == 0)
        //        {
        //            if (rowIndex != 0)
        //            {
        //                sheet = (XSSFSheet)workbook.CreateSheet();
        //            }

        //            #region 列头及样式
        //            {
        //                XSSFRow headerRow = (XSSFRow)sheet.CreateRow(0);
        //                XSSFCellStyle headStyle = (XSSFCellStyle)workbook.CreateCellStyle();
        //                headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        //                XSSFFont font = (XSSFFont)workbook.CreateFont();
        //                font.FontHeightInPoints = 10;
        //                font.Boldweight = 700;
        //                headStyle.SetFont(font);
        //                foreach (DataColumn column in dtSource.Columns)
        //                {
        //                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
        //                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
        //                    //设置列宽
        //                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
        //                }
        //            }
        //            #endregion
        //            rowIndex = 1;
        //        }
        //        #endregion

        //        #region 填充内容
        //        XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
        //        foreach (DataColumn column in dtSource.Columns)
        //        {
        //            XSSFCell newCell = (XSSFCell)dataRow.CreateCell(column.Ordinal);

        //            string drValue = row[column].ToString();

        //            switch (column.DataType.ToString())
        //            {
        //                case "System.String"://字符串类型
        //                    newCell.SetCellValue(drValue);
        //                    break;
        //                case "System.DateTime"://日期类型
        //                    DateTime dateV;
        //                    DateTime.TryParse(drValue, out dateV);
        //                    newCell.SetCellValue(dateV);
        //                    newCell.CellStyle = dateStyle;//格式化显示
        //                    break;
        //                case "System.Boolean"://布尔型
        //                    bool boolV = false;
        //                    bool.TryParse(drValue, out boolV);
        //                    newCell.SetCellValue(boolV);
        //                    break;
        //                case "System.Int16"://整型
        //                case "System.Int32":
        //                case "System.Int64":
        //                case "System.Byte":
        //                    int intV = 0;
        //                    int.TryParse(drValue, out intV);
        //                    newCell.SetCellValue(intV);
        //                    break;
        //                case "System.Decimal"://浮点型
        //                case "System.Double":
        //                    double doubV = 0;
        //                    double.TryParse(drValue, out doubV);
        //                    newCell.SetCellValue(doubV);
        //                    break;
        //                case "System.DBNull"://空值处理
        //                    newCell.SetCellValue("");
        //                    break;
        //                default:
        //                    newCell.SetCellValue("");
        //                    break;
        //            }

        //        }
        //        #endregion
        //        rowIndex++;
        //    }
        //    using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
        //    {
        //        workbook.Write(fs);
        //    }
        //}

        ///// <summary>
        ///// 导出Docx
        ///// </summary>
        ///// <param name="localFilePath">文件保存路径</param>
        ///// <param name="dtSource">数据源</param>
        //public static void ExportDocx(string localFilePath, DataTable dtSource)
        //{

        //    XWPFDocument doc = new XWPFDocument();

        //    XWPFTable table = doc.CreateTable(dtSource.Rows.Count + 1, dtSource.Columns.Count);

        //    for (int i = 0; i < dtSource.Rows.Count + 1; i++)
        //    {
        //        for (int j = 0; j < dtSource.Columns.Count; j++)
        //        {
        //            if (i == 0)
        //            {
        //                table.GetRow(i).GetCell(j).SetText(dtSource.Columns[j].ColumnName);
        //            }
        //            else
        //            {
        //                table.GetRow(i).GetCell(j).SetText(dtSource.Rows[i - 1][j].ToString());
        //            }
        //        }
        //    }

        //    using (FileStream fs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
        //    {
        //        doc.Write(fs);
        //    }
        //}

        #endregion


    }
}