using System.Data;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Parser2._0
{
    class ExcelTabPage : TabPage
    {
        internal DataGridView dataGridView;
        internal DataTable dataTable;
       /* internal ExcelTabPage()
        {
            dataGridView.Dock = DockStyle.Fill;
            dataGridView = new DataGridView();
            dataTable = new DataTable();
        }*/
        internal ExcelTabPage()
        {
            dataGridView.Dock = DockStyle.Fill;
            dataGridView = new DataGridView();
            dataTable = new DataTable();
        }
    }
}
