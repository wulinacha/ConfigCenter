using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConfigCenter.Common
{
    public class Entities : IDisposable
    {
        private string fileName = null; //文件名
        private static IWorkbook workbook = null;
        private static FileStream fs = null;
        private bool disposed;

        public Entities(string fileName)
        {
            this.fileName = fileName;
            disposed = false;

        }

        #region 将excel中的数据导入到DataTable中
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        ///  <param name="fileNameurl">表名</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTableHelper(HttpPostedFileBase file, string sheetName)
        {
            return ExcelToDataTableHelper(file, sheetName, 1);
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        ///  <param name="fileNameurl">表名</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="ColumnNum">第几行是表头</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTableHelper(HttpPostedFileBase file, string sheetName, int ColumnNum)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            var stream = file.InputStream;
            try
            {
                if (file.FileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(stream);
                else if (file.FileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(stream);
                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow;

                    // firstRow = sheet.GetRow(0);
                    if (ColumnNum > 0)
                    {
                        firstRow = sheet.GetRow(ColumnNum - 1);
                    }
                    else
                    {
                        firstRow = sheet.GetRow(0);
                    }
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            string cellValue = cell.StringCellValue;
                            if (cellValue != null && cellValue != "")
                            {
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                    }
                    cellCount = data.Columns.Count;

                    //找出第几行是列名
                    startRow = ColumnNum;

                    //startRow = firstDataNum-1;
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            {
                                //dataRow[j] = row.GetCell(j).ToString();
                                //读取Excel格式，根据格式读取数据类型
                                ICell cell = row.GetCell(j);
                                dataRow[j] = parseExcel(cell);
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        //格式转换
        private static String parseExcel(ICell cell)
        {
            string result = "";
            switch (cell.CellType)
            {
                case CellType.Formula:
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(workbook);
                    result = e.Evaluate(cell).StringValue;
                    break;
                case CellType.Numeric:// 数字类型  
                    if (HSSFDateUtil.IsCellDateFormatted(cell))
                    {// 处理日期格式、时间格式  
                        string sdf = "";
                        if (cell.CellStyle.DataFormat == HSSFDataFormat
                                .GetBuiltinFormat("h:mm"))
                        {
                            sdf = "HH:mm";
                        }
                        else
                        {// 日期  
                            sdf = "yyyy-MM-dd";
                        }
                        DateTime date = cell.DateCellValue;
                        result = date.ToString(sdf);
                    }
                    else if (cell.CellStyle.DataFormat == 58)
                    {
                        // 处理自定义日期格式：m月d日(通过判断单元格的格式id解决，id的值是58)  
                        string sdf = "yyyy-MM-dd";
                        double value = cell.NumericCellValue;
                        DateTime date = new DateTime(1899, 12, 30); // 起始时间
                        date = date.AddDays(value);
                        result = date.ToString(sdf);
                    }
                    else
                    {
                        result = cell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.String:// String类型  
                    result = cell.StringCellValue;
                    break;
                case CellType.Blank:
                    result = "";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取Exce工作薄名称
        /// </summary>
        /// <param name="fileNameurl"></param>
        /// <returns></returns>
        public static List<string> GetSheetNames(string fileNameurl)
        {
            using (FileStream sr = new FileStream(fileNameurl, FileMode.OpenOrCreate))
            {
                //根据路径通过已存在的excel来创建HSSFWorkbook，即整个excel文档
                HSSFWorkbook workbook = new HSSFWorkbook(sr);
                int x = workbook.Workbook.NumSheets;
                List<string> sheetNames = new List<string>();
                for (int i = 0; i < x; i++)
                {
                    sheetNames.Add(workbook.Workbook.GetSheetName(i));
                }
                return sheetNames;
            }
        }

        //资源释放
        public void Dispose()
        {
            //释放资源
            Dispose(true);
            //告诉垃圾回收器不要调用指定对象的Dispose方法
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (fs != null)
                        fs.Close();
                }

                fs = null;
                disposed = true;
            }
        }
        #endregion
    }
}
