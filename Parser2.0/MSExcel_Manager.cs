using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

using Excel = Microsoft.Office.Interop.Excel;

namespace Parser2._0
{
    class MSExcel_Manager
    {

        MainForm mainForm;
        List<Excel.Worksheet> worksheets_list;
        List<string> worksheets_names;
        private Excel.Application ExcelApplication = null; 
        private Excel.Workbook Workbook = null;  
        private Excel.Range lastCell;
        internal List<DataTable> LoadExcelSheetsXML()
        {
            List<DataTable> dataTables = new List<DataTable>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.xlsx, *.xls) | *.xlsx; *.xls";
            openFileDialog.Title = " Выберите документ для загрузки данных";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CloseExcelFile();
                Workbook = ExcelApplication.Workbooks.Open(openFileDialog.FileName);
                foreach (Excel.Worksheet worksheet in Workbook.Worksheets)
                {
                    worksheets_names.Add(worksheet.Name);
                }
                DataTable dataTable;               
                for (int i = 0; i < worksheets_names.Count; i++)
                {
                    dataTable = new DataTable();
                    //Stopwatch stopWatch = new Stopwatch();
                    //stopWatch.Start();
                    string connectionstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openFileDialog.FileName + ";Extended Properties='Excel 12.0 XML;HDR=YES;IMEX=1';";
                    System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection(connectionstr);
                    oleDbConnection.Open();
                    DataSet dataSet = new DataSet();
                    string select = String.Format("SELECT * FROM [{0}]", worksheets_names[i] + '$');
                    System.Data.OleDb.OleDbDataAdapter oleDbDataAdapter = new System.Data.OleDb.OleDbDataAdapter(select, oleDbConnection);
                    oleDbDataAdapter.Fill(dataSet);
                    dataTable = dataSet.Tables[0];
                    DataRow dataRow = dataTable.NewRow();
                    dataTable.Rows.InsertAt(dataRow, 0);
                    dataTable.Rows[0][0] = worksheets_names[i];
                    dataTable.Columns.Add("!");
                    dataTable.Columns["!"].SetOrdinal(0);
                    dataTables.Add(dataTable);
                    oleDbConnection.Close();
                    oleDbConnection.Dispose();
                    oleDbDataAdapter.Dispose();
                    //stopWatch.Stop();
                    //MessageBox.Show("Затрачено времени: " + stopWatch.Elapsed.TotalSeconds.ToString());
                }
                return dataTables;
            }
            else
            {
                return null;
            }        
        }
        internal List<DataTable> LoadExcelSheets()
        {
            List<DataTable> dataTables = new List<DataTable>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.xlsx, *.xls) | *.xlsx; *.xls";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CloseExcelFile();
                DataTable dataTable;
                Workbook = ExcelApplication.Workbooks.Open(openFileDialog.FileName);
                foreach (Excel.Worksheet worksheet in Workbook.Worksheets)
                {
                    worksheets_list.Add(worksheet);
                }
                for(int i = 0; i < worksheets_list.Count; i++)
                {
                    dataTable = new DataTable();
                    DataColumn dataColumn = new DataColumn();
                    dataColumn.ColumnName = "Row №";
                    dataColumn.AutoIncrement = true;
                    dataColumn.AutoIncrementStep = 1;
                    dataColumn.AutoIncrementSeed = 1;
                    dataTable.Columns.Add(dataColumn);                 
                    lastCell = worksheets_list[i].Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
                    dataTable.Rows.Add();
                    dataTable.Columns.Add();
                    for (int j = 1; j < lastCell.Column; j++)
                    {
                        dataTable.Columns.Add();
                        for (int k = 1; k < lastCell.Row; k++)
                        {
                            dataTable.Rows.Add();
                            dataTable.Rows[k][j] = worksheets_list[i].Cells[k + 1, j].Text;
                        }
                    }
                    dataTable.Rows[0][1] = worksheets_list[i].Name;
                    dataTables.Add(dataTable);
                }
                return dataTables;
            }
            else
            {
                return null;
            }
        }
        internal void CloseExcelFile()
        {
            if (mainForm.excel_dataTables != null && mainForm.excel_dataTables.Count > 0)
            {
                mainForm.excel_dataTables = null;
            }
            if (worksheets_names.Count > 0)
            {
                worksheets_names.Clear();
            }
            if (worksheets_list.Count > 0)
            {
                worksheets_list.Clear();
            }
            Workbook = null;
        }
        internal void Dispose()
        {
            ExcelApplication.Workbooks.Close();
            ExcelApplication.Quit();
        }
        internal MSExcel_Manager(MainForm form)
        {
            ExcelApplication = new Excel.Application();
            worksheets_list = new List<Excel.Worksheet>();
            worksheets_names = new List<string>();
            mainForm = form;
        }
        ~MSExcel_Manager()
        {
            ExcelApplication.Workbooks.Close();
            ExcelApplication.Quit();
        }
    }
}
