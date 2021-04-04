using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Parser2._0
{
    class MSExcel_Manager
    {
        private Excel.Application ExcelApplication = null; 
        private Excel.Workbook Workbook = null; 
        private Excel.Worksheet Worksheet = null; 
        private Excel.Range lastCell; 
        internal DataTable LoadExcelFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CloseExcelFile();
                DataTable dataTable = new DataTable();
                Workbook = ExcelApplication.Workbooks.Open(openFileDialog.FileName);
                Worksheet = Workbook.ActiveSheet;
                lastCell = Worksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
                for (int i = 0; i < lastCell.Column; i++)
                {
                    dataTable.Columns.Add();
                    for (int j = 0; j < lastCell.Row; j++)
                    {
                        dataTable.Rows.Add();
                        dataTable.Rows[j][i] = Worksheet.Cells[j + 1, i + 1].Text;
                    }
                }
                return dataTable;
            }
            else
            {
                return null;
            }
        }
        private void CloseExcelFile()
        {
            if (Worksheet != null)
            {
                Workbook = null;
                Worksheet = null;
            }
        }
        internal MSExcel_Manager()
        {
            ExcelApplication = new Excel.Application();
        }
        ~MSExcel_Manager()
        {
            ExcelApplication.Workbooks.Close();
            ExcelApplication.Quit();
        }
    }
}
