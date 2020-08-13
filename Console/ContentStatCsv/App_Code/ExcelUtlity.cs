using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;

namespace Excel
{
    public class ExcelUtlity
    {
        /// <summary>
        /// FUNCTION FOR EXPORT TO EXCEL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <returns></returns>
        
        public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string ReporType)
        {
            ExcelRange excelCellrange;
            
            try
            {
                var saveFile = new FileInfo(saveAsLocation);

                if (saveFile.Exists)
                    saveFile.Delete();

                using (var package = new OfficeOpenXml.ExcelPackage(saveFile))
                {
                    var excelSheet = package.Workbook.Worksheets.Add(worksheetName);
                    
                    

                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        excelSheet.Cells[3, i].Value = dataTable.Columns[i - 1].ColumnName;
                        excelSheet.Cells[3, i].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#666"));
                        //excelSheet.Cells[1, i].Style.Font.Color.SetColor(System.Drawing.Color.White);
                    }


                    // loop through each row and add values to our sheet
                    int rowcount = 4;

                    foreach (DataRow datarow in dataTable.Rows)
                    {

                        for (int i = 1; i <= dataTable.Columns.Count; i++)
                        {
                            
                            excelSheet.Cells[rowcount, i].Value = datarow[i - 1].ToString();
                            excelCellrange = excelSheet.Cells[rowcount, 1, rowcount, dataTable.Columns.Count];
                            excelCellrange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            string hexA = Convert.ToString(ConfigurationSettings.AppSettings["PARAM_UPLOAD_XL_COLOR"]);

                            
                            excelCellrange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(hexA));
                            
                            excelCellrange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            excelCellrange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            excelCellrange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            excelCellrange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            //for alternate rows
                            if (rowcount > 3)
                            {
                                if (i == dataTable.Columns.Count)
                                {
                                    if (rowcount % 2 == 0)
                                    {
                                        excelCellrange = excelSheet.Cells[rowcount, 1, rowcount, dataTable.Columns.Count];
                                        excelCellrange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        string hex = Convert.ToString(ConfigurationSettings.AppSettings["PARAM_UPLOAD_XL_COLOR"]);
                                        excelCellrange.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml(hex));
                                        excelCellrange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    }
                                    else
                                    {
                                        excelCellrange = excelSheet.Cells[rowcount, 1, rowcount, dataTable.Columns.Count];
                                        excelCellrange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        excelCellrange.Style.Fill.BackgroundColor.SetColor(Color.White);
                                        excelCellrange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                        excelCellrange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    }

                                }
                            }

                        }
                        rowcount += 1;

                    }


                    package.Save();
                }

                return true;
            }
            catch (Exception ex)
            {
                //tracerMailException.SendMail(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return false;
            }
            finally
            {
                /*excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;*/
            }

        }
        
    }
}
