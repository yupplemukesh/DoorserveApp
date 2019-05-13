using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace TogoFogo
{
    public class ExcelExportHelper
    {

        public static string ExcelContentType
        {
            get
            { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; }
        }

        public static DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
       
        public static byte[] ExportExcel(DataTable dataTable, string heading = "", bool showSrNo = false, params string[] columnsToTake)
        {

            byte[] result = null;
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(String.Format("{0} Data", heading));
                int startRowFrom = String.IsNullOrEmpty(heading) ? 1 : 3;

                if (showSrNo)
                {
                    DataColumn dataColumn = dataTable.Columns.Add("#SerialNo", typeof(int));
                    dataColumn.SetOrdinal(0);
                    int index = 1;
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item[0] = index;
                        index++;
                    }
                }


                // add the content into the Excel file  
                workSheet.Cells["A" + startRowFrom].LoadFromDataTable(dataTable, true);

                // autofit width of cells with small content  
                int columnIndex = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    ExcelRange columnCells = workSheet.Cells[workSheet.Dimension.Start.Row, columnIndex, workSheet.Dimension.End.Row, columnIndex];
                    foreach (var item in columnCells)
                    {
                        
                            if (item.Value != null)
                            {
                                var CellText = item.Value.ToString();
                                if (CellText.Contains("CRN"))
                                    CellText = "CallId";
                           if(columnsToTake.Contains(CellText))
                            {
                                if(CellText.Contains("IsUser"))
                                    item.Value = "IsUser";

                                else if(CellText.Contains("IsServiceCenter"))
                                    item.Value = "IsServiceCenter";
                                else if (CellText.Contains("GSTNumber"))
                                    item.Value = "GST Number";
                                else if (CellText.Contains("GSTCategory"))
                                    item.Value = "GST Category";
                                else if (CellText.Contains("OrganizationIECNumber"))
                                    item.Value = "Organization IEC Number";
                                else if (CellText.Contains("PANCardNumber"))
                                    item.Value = "PAN Card Number";
                                else if (CellText.Contains("ContactPAN"))
                                    item.Value = "Contact PAN";
                                else if (CellText.Contains("ContactPAN"))
                                    item.Value = "Contact PAN";
                                else if (CellText.Contains("DOP") == true)
                                    item.Value = "DOP";
                               else if (CellText.Contains("DeviceIMEIOne") == true)
                                    item.Value = "Device IMEI First";
                               else if (CellText.Contains("DeviceIMEISecond") == true)
                                    item.Value = "Device IMEI Second";
                                else
                                    item.Value = string.Concat(CellText.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
                            }
                            if (column.DataType == System.Type.GetType("System.DateTime"))
                            {
                                try
                                {
                                    item.Value = Convert.ToDateTime(item.Value.ToString()).ToShortDateString();
                                }
                                catch
                                {

                                }
                            }
                        }                        
                    }

                    int maxLength = columnCells.Max(cell => cell.Value != null ? cell.Value.ToString().Count() : 0);
                    if (maxLength < 150)
                    {
                        workSheet.Column(columnIndex).AutoFit();
                    }


                    columnIndex++;
                }

                // format header - bold, yellow on black  
                using (ExcelRange r = workSheet.Cells[startRowFrom, 1, startRowFrom, dataTable.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#1fb5ad"));
                }

                if (dataTable.Rows.Count > 0)
                {
                    // format cells - add borders  
                    using (ExcelRange r = workSheet.Cells[startRowFrom + 1, 1, startRowFrom + dataTable.Rows.Count, dataTable.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }
                }
                // removed ignored columns  
                for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
                {
                    if (i == 0 && showSrNo)
                    {
                        continue;
                    }
                    if (!columnsToTake.Contains(dataTable.Columns[i].ColumnName))
                    {
                        workSheet.DeleteColumn(i + 1);
                    }
                }

                if (!String.IsNullOrEmpty(heading))
                {
                    workSheet.Cells["A1"].Value = heading;
                    workSheet.Cells["A1"].Style.Font.Size = 20;

                    workSheet.InsertColumn(1, 1);
                    workSheet.InsertRow(1, 1);
                    workSheet.Column(1).Width = 5;
                }

                result = package.GetAsByteArray();
            }

            return result;
        }       
        public static byte[] ExportExcel<T>(List<T> data, string Heading = "", bool showSlno = false, params string[] ColumnsToTake)
        {
            return ExportExcel(ListToDataTable<T>(data), Heading, showSlno, ColumnsToTake);
        }
    }
}