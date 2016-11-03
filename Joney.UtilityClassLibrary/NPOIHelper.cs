using System;
using System.Collections.Generic;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Eval;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;

namespace Joney.UtilityClassLibrary
{
    public class NPOIHelper
    {

        //private static LogHelper wl = new LogHelper();

        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static HSSFWorkbook CreateHSSFWorkbook(string filePath)
        {
            HSSFWorkbook hworkbook;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hworkbook = new HSSFWorkbook(fs);
                    return hworkbook;
                }
            }
            catch (System.Exception ex)
            {
                //文件流读取异常
                throw ex;
            }
        }

        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MemoryStream ReadFileToMemoryStream(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                if (!File.Exists(filePath))
                {
                    return null;
                }
                try
                {
                    using (FileStream FS = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        FS.Seek(0, SeekOrigin.Begin);
                        byte[] data = new byte[FS.Length];
                        FS.Read(data, 0, data.Length);
                        FS.Flush();
                        FS.Close();
                        MemoryStream ms = new MemoryStream(data);
                        return ms;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return null;
        }

        #region NPOI Demo

        public static void CreateSheet()
        {
            IWorkbook workbook = new HSSFWorkbook();//创建Workbook对象

            ISheet sheet = workbook.CreateSheet("Sheet1");//创建工作表

            IRow row = sheet.CreateRow(0);//在工作表中添加一行

            ICell cell = row.CreateCell(0);//在行中添加一列

            cell.SetCellValue("test");//设置列的内容
        }

        public static void GetSheet(Stream stream)
        {

            IWorkbook workbook = new HSSFWorkbook(stream);//从流内容创建Workbook对象

            ISheet sheet = workbook.GetSheetAt(0);//获取第一个工作表

            IRow row = sheet.GetRow(0);//获取工作表第一行

            ICell cell = row.GetCell(0);//获取行的第一列

            string value = cell.ToString();//获取列的值

        }

        public static void CreateHSSFWorkbook()
        {
            HSSFWorkbook wb = new HSSFWorkbook();

            ICellStyle cellStyle = wb.CreateCellStyle();///实例化一个样式实例。
                                                        ///边框样式
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            ///文本水平&垂直对齐方式
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            //cellStyle.WrapText = true;///文本是否自动换行
            cellStyle.ShrinkToFit = true;///缩小字体填充(与上面的换行同时设置的话会产生冲突,此时只有换行生效)

            ICellStyle newCellStyle = wb.CreateCellStyle();///新建样式实例
            newCellStyle.CloneStyleFrom(cellStyle);///克隆主样式
            newCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;///样式重写

            ISheet sheet = wb.CreateSheet();
            wb.SetSheetName(0, "第一个表");//设置表名
            wb.SetActiveSheet(0);//设置默认打开第几个表
            ICell cell = sheet.CreateRow(0).CreateCell(0);
            cell.CellStyle = cellStyle;
            cell.SetCellValue("DEMO CELL STYLE.");

        }

        #endregion

        /// <summary>
        /// 获取表的数目
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int GetSheetNumber(string filePath)
        {
            int number = 0;
            try
            {
                FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
                number = hssfworkbook.NumberOfSheets;

            }
            catch (Exception exception)
            {
                LogHelper.WriteLogs(exception.ToString());
            }
            return number;
        }

        /// <summary>
        /// 获取表名数组列表：table Name
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ArrayList GetSheetName(string filePath)
        {
            ArrayList arrayList = new ArrayList();
            try
            {
                FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
                for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    arrayList.Add(hssfworkbook.GetSheetName(i));
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteLogs(exception.ToString());
            }
            return arrayList;
        }

        /// <summary>
        /// 判断是否为Numeric类型
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool isNumeric(String message, out double result)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加工作表Sheet.Table~X
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetname"></param>
        /// <param name="dt"></param>
        public static void InsertSheet(string filePath, string sheetname, DataTable dt)
        {
            FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //IWorkbook hssfworkbook = WorkbookFactory.Create(readfile);//NPOI 2.0版本
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            int num = hssfworkbook.GetSheetIndex(sheetname);
            ISheet sheet1;
            if (num >= 0)
                sheet1 = hssfworkbook.GetSheet(sheetname);
            else
            {
                sheet1 = hssfworkbook.CreateSheet(sheetname);
            }

            try
            {
                if (sheet1.GetRow(0) == null)
                {
                    sheet1.CreateRow(0);
                }
                for (int coluid = 0; coluid < dt.Columns.Count; coluid++)
                {
                    if (sheet1.GetRow(0).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(0).CreateCell(coluid);
                    }

                    sheet1.GetRow(0).GetCell(coluid).SetCellValue(dt.Columns[coluid].ColumnName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
                throw;
            }


            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                try
                {
                    if (sheet1.GetRow(i) == null)
                    {
                        sheet1.CreateRow(i);
                    }
                    for (int coluid = 0; coluid < dt.Columns.Count; coluid++)
                    {
                        if (sheet1.GetRow(i).GetCell(coluid) == null)
                        {
                            sheet1.GetRow(i).CreateCell(coluid);
                        }

                        sheet1.GetRow(i).GetCell(coluid).SetCellValue(dt.Rows[i - 1][coluid].ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLogs(ex.ToString());
                    //throw;
                }
            }
            try
            {
                readfile.Close();

                FileStream writefile = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
            }
        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string filePath, string sheetname, string[][] updateData, int[] coluids, int rowid)
        {
            FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int j = 0; j < coluids.Length; j++)
            {
                for (int i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLogs(ex.ToString());
                    }
                }
            }
            try
            {
                FileStream writefile = new FileStream(filePath, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
            }
        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string filePath, string sheetname, double[] updateData, int coluid, int rowid)
        {
            FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLogs(ex.ToString());
                    throw;
                }
            }
            try
            {
                readfile.Close();
                FileStream writefile = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
            }

        }

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluids">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string filePath, string sheetname, double[][] updateData, int[] coluids, int rowid)
        {
            FileStream readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int j = 0; j < coluids.Length; j++)
            {
                for (int i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowid) == null)
                        {
                            sheet1.CreateRow(i + rowid);
                        }
                        if (sheet1.GetRow(i + rowid).GetCell(coluids[j]) == null)
                        {
                            sheet1.GetRow(i + rowid).CreateCell(coluids[j]);
                        }
                        sheet1.GetRow(i + rowid).GetCell(coluids[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLogs(ex.ToString());
                    }
                }
            }
            try
            {
                FileStream writefile = new FileStream(filePath, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
            }
        }

        #region NPOI 2.X.X Version

        #region 从datatable中将数据导出到excel

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream ExportDT(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;

            #region 右击文件 属性信息

            //{
            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //    dsi.Company = "http://www.Joney.com/";
            //    workbook.DocumentSummaryInformation = dsi;

            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //    si.Author = "Joney"; //填加xls文件作者信息
            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
            //    si.LastAuthor = "Joney yu"; //填加xls文件最后保存者信息
            //    si.Comments = "说明信息"; //填加xls文件作者信息
            //    si.Title = "NPOI测试"; //填加xls文件标题信息
            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
            //    si.CreateDateTime = DateTime.Now;
            //    workbook.SummaryInformation = si;
            //}

            #endregion

            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }

                    #region 表头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;

                        sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }

                    #endregion


                    #region 列头及样式

                    {
                        HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;


                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 2;
                }

                #endregion

                #region 填充内容

                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (isNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    //无法访问已关闭的流
            //}
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            //ms.Dispose();
            return ms;
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="fs"></param>
        public static void ExportDTI(DataTable dtSource, string strHeaderText, FileStream fs)
        {
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet = workbook.CreateSheet() as XSSFSheet;

            #region 右击文件 属性信息

            //{
            //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //    dsi.Company = "http://www.Joney.com/";
            //    workbook.DocumentSummaryInformation = dsi;

            //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //    si.Author = "Joney"; //填加xls文件作者信息
            //    si.ApplicationName = "NPOI测试程序"; //填加xls文件创建程序信息
            //    si.LastAuthor = "Joney Y"; //填加xls文件最后保存者信息
            //    si.Comments = "说明信息"; //填加xls文件作者信息
            //    si.Title = "NPOI测试"; //填加xls文件标题信息
            //    si.Subject = "NPOI测试Demo"; //填加文件主题信息
            //    si.CreateDateTime = DateTime.Now;
            //    workbook.SummaryInformation = si;
            //}

            #endregion

            XSSFCellStyle dateStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFDataFormat format = workbook.CreateDataFormat() as XSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 0)
                {
                    #region 表头及样式
                    {
                        XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        XSSFFont font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;

                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1));
                        //headerRow.Dispose();
                    }

                    #endregion


                    #region 列头及样式

                    {
                        XSSFRow headerRow = sheet.CreateRow(0) as XSSFRow;


                        XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        XSSFFont font = workbook.CreateFont() as XSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);


                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            //sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);

                        }
                        //headerRow.Dispose();
                    }

                    #endregion

                    rowIndex = 1;
                }

                #endregion

                #region 填充内容

                XSSFRow dataRow = sheet.CreateRow(rowIndex) as XSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    XSSFCell newCell = dataRow.CreateCell(column.Ordinal) as XSSFCell;

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            double result;
                            if (isNumeric(drValue, out result))
                            {

                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                                break;
                            }

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;
                        case "System.Boolean": //布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }
            workbook.Write(fs);
            fs.Close();
        }

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void ExportDTtoExcel(DataTable dtSource, string strHeaderText, string strFileName)
        {
            string[] temp = strFileName.Split('.');

            if (temp[temp.Length - 1] == "xls" && dtSource.Columns.Count < 256 && dtSource.Rows.Count < 65536)
            {
                using (MemoryStream ms = ExportDT(dtSource, strHeaderText))
                {
                    using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
            else
            {
                if (temp[temp.Length - 1] == "xls")
                    strFileName = strFileName + "x";

                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    ExportDTI(dtSource, strHeaderText, fs);

                }

            }
        }

        #endregion

        #region 从excel中将数据导出到datatable

        /// <summary>
        /// 读取excel默认第一行为表头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName)
        {
            DataTable dt = new DataTable();
            try
            {
                IWorkbook wb;
                using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
                {
                    wb = WorkbookFactory.Create(file);
                }
                ISheet sheet = wb.GetSheetAt(0);
                dt = ImportDt(sheet, 0, true);
                DelDirFile(strFileName);//读取问数据后将临时文件删除
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }



        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="SheetName">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string SheetName, int HeaderRowIndex)
        {
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheet(SheetName);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, true);
            sheet = null;
            return table;
        }


        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="SheetIndex">需要导出的sheet序号</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int SheetIndex, int HeaderRowIndex)
        {
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet isheet = wb.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            table = ImportDt(isheet, HeaderRowIndex, true);
            isheet = null;
            return table;
        }


        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="SheetName">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, string SheetName, int HeaderRowIndex, bool needHeader)
        {
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheet(SheetName);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, needHeader);
            sheet = null;
            return table;
        }


        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="strFileName">excel文件路径</param>
        /// <param name="SheetIndex">需要导出的sheet序号</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        public static DataTable ImportExceltoDt(string strFileName, int SheetIndex, int HeaderRowIndex, bool needHeader)
        {
            IWorkbook wb;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                wb = WorkbookFactory.Create(file);
            }
            ISheet sheet = wb.GetSheetAt(SheetIndex);
            DataTable table = new DataTable();
            table = ImportDt(sheet, HeaderRowIndex, needHeader);
            sheet = null;
            return table;
        }


        /// <summary>
        /// 将指定表（sheet）中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        static DataTable ImportDt(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                //table.Columns.Add(column);//这里会添加一列空的列
                            }

                        }
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                LogHelper.WriteLogs(exception.ToString());
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {
                        LogHelper.WriteLogs(exception.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteLogs(exception.ToString());
            }
            return table;
        }

        /// <summary>
        /// 将指定表（sheet）中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <param name="needHeader"></param>
        /// <returns></returns>
        static DataSet ImportDs(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataSet ds = new DataSet();
            DataTable table = new DataTable(sheet.SheetName);

            IRow headerRow;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0 || !needHeader)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);
                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                //table.Columns.Add(column);//这里会添加一列空的列
                            }

                        }
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }

                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.String:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str.ToString();
                                            }
                                            else
                                            {
                                                dataRow[j] = null;
                                            }
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                            break;
                                        case CellType.Formula:
                                            switch (row.GetCell(j).CachedFormulaResultType)
                                            {
                                                case CellType.String:
                                                    string strFORMULA = row.GetCell(j).StringCellValue;
                                                    if (strFORMULA != null && strFORMULA.Length > 0)
                                                    {
                                                        dataRow[j] = strFORMULA.ToString();
                                                    }
                                                    else
                                                    {
                                                        dataRow[j] = null;
                                                    }
                                                    break;
                                                case CellType.Numeric:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                                    break;
                                                default:
                                                    dataRow[j] = "";
                                                    break;
                                            }
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                LogHelper.WriteLogs(exception.ToString());
                            }
                        }
                        table.Rows.Add(dataRow);
                    }
                    catch (Exception exception)
                    {
                        LogHelper.WriteLogs(exception.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteLogs(exception.ToString());
            }
            ds.Tables.Add(table);
            return ds;
        }

        #endregion

        #region 更新excel中的数据

        /// <summary>
        /// 更新Excel表格
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetname">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="coluid">需更新的列号</param>
        /// <param name="rowid">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetname, string[] updateData, int coluid, int rowid)
        {
            //FileStream readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            IWorkbook hssfworkbook = WorkbookFactory.Create(outputFile);
            //HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);
            ISheet sheet1 = hssfworkbook.GetSheet(sheetname);
            for (int i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowid) == null)
                    {
                        sheet1.CreateRow(i + rowid);
                    }
                    if (sheet1.GetRow(i + rowid).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(i + rowid).CreateCell(coluid);
                    }

                    sheet1.GetRow(i + rowid).GetCell(coluid).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLogs(ex.ToString());
                    throw;
                }
            }
            try
            {
                //readfile.Close();
                FileStream writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.ToString());
            }

        }

        #endregion

        #region 生成自定义模板

        /// <summary>
        /// 生成自定义模板
        /// </summary>
        /// <param name="templeName">模板名称</param>
        /// <param name="headerNames">表头名字字段</param>
        /// <param name="columnsAttrs">约束数组</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileExt">模板类型后缀</param>
        public static string CreateExcelTemple(string templeName, string[] headerNames, string columnsAttrs, string savePath)
        {
            string resPath = string.Empty;
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet1 = workbook.CreateSheet("Sheet1") as HSSFSheet;//实例化表格1

                int rowIndex = 0;//行号：0=第一行
                for (int i = 0; i < headerNames.Length; i++)
                {
                    HSSFRow headerRow = sheet1.CreateRow(rowIndex) as HSSFRow;
                    headerRow.HeightInPoints = 30;
                    sheet1.CreateRow(rowIndex).CreateCell(i).SetCellValue(headerNames[i]);//表头名称
                }

                List<CellsAttrs> objList = new List<CellsAttrs>();
                if (!string.IsNullOrEmpty(columnsAttrs))
                {
                    objList = JsonConvert.DeserializeObject<List<CellsAttrs>>(columnsAttrs);
                    HSSFSheet dicSheet = workbook.CreateSheet("ShtDictionary") as HSSFSheet;
                    for (int i = 0; i < objList.Count; i++)
                    {
                        CellsAttrs CA = objList[i];
                        var cellsNum = Convert.ToInt32(CA.cells);
                        var items = CA.items;
                        string[] itemStr = items.Split(',');
                        var cellArray = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                        var cIndex = cellArray[cellsNum].ToString();
                        var cBegin = cIndex + 0;
                        var cEnd = cIndex + itemStr.Length;
                        for (int k = 0; k < itemStr.Length; k++)
                        {
                            dicSheet.CreateRow(k).CreateCell(cellsNum).SetCellValue(itemStr[k]);
                        }
                        HSSFName range = workbook.CreateName() as HSSFName;
                        range.NameName = "dicRange" + i;
                        range.RefersToFormula = string.Format("ShtDictionary!{0}:{1}", cBegin, cEnd);

                        CellRangeAddressList regions = new CellRangeAddressList(rowIndex + 1, 65535, cellsNum, cellsNum);//单元格属性
                        DVConstraint constraint = DVConstraint.CreateFormulaListConstraint("dicRange" + i);
                        HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                        dataValidate.CreateErrorBox("错误", "输入不合法,请输入下拉列表中的值.");
                        sheet1.AddValidationData(dataValidate);

                        //CellRangeAddressList regionsDemo = new CellRangeAddressList(1, 65535,0,0);
                        //DVConstraint constraintDemo = DVConstraint.CreateDateConstraint(OperatorType.BETWEEN,"1900-01-01","2999-12-31","yyyy-MM-dd");
                        //HSSFDataValidation dataValidateDemo = new HSSFDataValidation(regions, constraintDemo);
                        //dataValidateDemo.CreateErrorBox("ERROR", "You must input a date.");
                        //sheet1.AddValidationData(dataValidateDemo);
                    }
                }

                FileStream fs = File.Create(savePath + templeName);
                workbook.Write(fs);
                fs.Close();
                resPath = savePath + templeName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resPath;
        }

        #endregion

        #endregion

        /// <summary>
        /// 删除指定目录文件
        /// </summary>
        /// <param name="path"></param>
        public static void DelDirFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 表格》数据》数据有效性
    /// </summary>
    public class CellsAttrs
    {
        /// <summary>
        /// 列
        /// </summary>
        public virtual string cells { get; set; }
        /// <summary>
        /// 有效性
        /// </summary>
        public virtual string items { get; set; }
    }
}
