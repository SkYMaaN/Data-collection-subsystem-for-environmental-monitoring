﻿using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Parser2._0
{
    class MSExcel_Manager
    {
        MainForm mainForm;
        List<Excel.Worksheet> worksheets_list;
        private Excel.Application ExcelApplication = null; 
        private Excel.Workbook Workbook = null;  
        private Excel.Range lastCell; 
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
            if (worksheets_list.Count > 0)
            {
                worksheets_list.Clear();
                Workbook = null;
            }
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
            mainForm = form;
        }
        ~MSExcel_Manager()
        {
            ExcelApplication.Workbooks.Close();
            ExcelApplication.Quit();
        }
    }
}
