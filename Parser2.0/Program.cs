using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser2._0
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        internal static void BuildComboBoxDataGridView(DataGridView dataGridView)
        {
            ClearDataGridView(dataGridView);
            dataGridView.DataSource = new DataTable();
            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            //===============================
            dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.HeaderText = "Input Data";
            dataGridViewComboBoxColumn.Items.Add("Varriable");
            dataGridView.Columns.Add(dataGridViewComboBoxColumn);
            ////
            dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.HeaderText = "Value In File";
            dataGridViewComboBoxColumn.Items.Add("Varriable");
            dataGridView.Columns.Add(dataGridViewComboBoxColumn);
            ////
            dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.HeaderText = "Output Data";
            dataGridViewComboBoxColumn.Items.Add("To Data Base");
            dataGridViewComboBoxColumn.Items.Add("In Varriable");
            dataGridView.Columns.Add(dataGridViewComboBoxColumn);
            ////
            dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn.HeaderText = "Command";
            dataGridViewComboBoxColumn.Items.Add("Add");
            dataGridViewComboBoxColumn.Items.Add("Save");
            dataGridViewComboBoxColumn.Items.Add("Find");
            //dataGridViewComboBoxColumn.Items.Add("Edit");
            dataGridView.Columns.Add(dataGridViewComboBoxColumn);
            ////
            DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn.HeaderText = "Options";
            dataGridView.Columns.Add(dataGridViewTextBoxColumn);
            ////
            dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn.HeaderText = "Result";
            dataGridView.Columns.Add(dataGridViewTextBoxColumn);
            //
            DataColumn autoIncrementColumn = new DataColumn();
            autoIncrementColumn.ColumnName = "№";
            (dataGridView.DataSource as DataTable).Columns.Add(autoIncrementColumn);

            (dataGridView.DataSource as DataTable).TableNewRow += DataTableNewRow_AutoIncrement;

        }
        internal static void DataTableNewRow_AutoIncrement(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["№"] = (sender as DataTable).Rows.Count + 1;
            if((sender as DataTable).Rows.Count > 0)
            { 
                for (int i = 1; i < Convert.ToInt32(e.Row["№"]); i++)
                {
                    (sender as DataTable).Rows[i-1]["№"] = i;
                }
            }
        }
        static void ClearDataGridView(DataGridView dataGridView)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns.Clear();
            }
        }
        internal static bool IsDataGridViewRowEmpty(DataGridViewRow dataGridViewRow)
        {
            bool isEmpty = true;
            foreach(DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
            {
                if(dataGridViewCell.Value != null)
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }
    }
}
