using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    class CustomTabPage : TabPage
    {
        internal DataGridView dataGridView;
        internal DataTable dataTable;
        internal string Type;
        internal CustomTabPage(DataTable dataTable_, string sheetname, string Type_)
        {
            this.Type = Type_;
            this.Text = sheetname;
            dataGridView = new DataGridView();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            this.Controls.Add(dataGridView);
            dataTable = dataTable_;
            dataGridView.DataSource = dataTable;
        }
        CustomTabPage(DataTable dataTable_, string sheetname)
        {
            this.Text = sheetname;
            dataGridView = new DataGridView();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            this.Controls.Add(dataGridView);
            dataTable = dataTable_;
            dataGridView.DataSource = dataTable;
        }
        CustomTabPage(DataTable dataTable_)
        {         
            dataGridView = new DataGridView();
            dataGridView.ReadOnly = true;
            dataGridView.Dock = DockStyle.Fill;
            this.Controls.Add(dataGridView);
            dataTable = dataTable_;
            dataGridView.DataSource = dataTable;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
