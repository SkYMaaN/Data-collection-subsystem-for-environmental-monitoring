
using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    class ExpertSystem_Manager
    {
        
        private string InputData; //0
        private string ValueInFile; //1
        private string OutputDataType; //2
        private string Command; //3
        private string Options; //4
        private string Result; //5
        internal void Get_ValueInFile(DataGridView dataGridView, DataGridView dataGridView1)
        {
            DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn = dataGridView1.Columns[0] as DataGridViewComboBoxColumn;
            DataGridViewComboBoxColumn dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
            dataGridViewComboBoxColumn1 = dataGridView1.Columns[1] as DataGridViewComboBoxColumn;
            DataTable dataTable = new DataTable();
            dataTable = dataGridView.DataSource as DataTable;
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    if (dataTable.Rows[j][i].ToString() != "")
                    {
                        dataGridViewComboBoxColumn.Items.Add("[" + j + ":" + i + "]");
                        dataGridViewComboBoxColumn1.Items.Add(dataTable.Rows[j][i].ToString());
                    }
                }
            }
        }
        private void ExecuteCommand()
        {
            switch (Command)
            {
                case "Add":
                    {
                        this.Add();
                        break;
                    }
                case "Edit":
                    {
                        this.Edit();
                        break;
                    }
                case "Find":
                    {
                        this.Find();
                        break;
                    }
                case "Save":
                    {
                        this.Save();
                        break;
                    }
            }
        }
        internal void GetRegulations(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow = dataGridView.Rows[i];
                if (dataGridViewRow.Cells[0].Value != null)
                {
                    InputData = dataGridViewRow.Cells[0].Value.ToString();
                }
                if (dataGridViewRow.Cells[1].Value != null)
                {
                    ValueInFile = dataGridViewRow.Cells[1].Value.ToString();
                }
                if (dataGridViewRow.Cells[2].Value != null)
                {
                    OutputDataType = dataGridViewRow.Cells[2].Value.ToString();
                }
                if (dataGridViewRow.Cells[3].Value != null)
                {
                    Command = dataGridViewRow.Cells[3].Value.ToString();
                }
                if (dataGridViewRow.Cells[4].Value != null)
                {
                    Options = dataGridViewRow.Cells[4].Value.ToString();
                }
                if (dataGridViewRow.Cells[5].Value != null)
                {
                    Result = dataGridViewRow.Cells[5].Value.ToString();
                }
                this.ExecuteCommand();
            }
        }
        private void Save()
        {
                       
        }
        private void Find()
        {
            
        }
        private void Edit()
        {

        }
        private void Add()
        {
            try
            {
                 
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error:  " + ex);
            }
        }
        internal ExpertSystem_Manager()
        {
            
        }
    }
}
