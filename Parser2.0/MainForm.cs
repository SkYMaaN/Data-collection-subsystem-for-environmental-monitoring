using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    public partial class MainForm : Form
    {
        internal DataBase_Manager dataBase_Manager;
        internal MSExcel_Manager excel_Manager;
        internal ExpertSystem_Manager expertSystem_Manager;
        internal FileWork_Manager fileWork_Manager;
        DataTable db_dataTable;
        DataTable excel_dataTable;
        DataTable parsingRegulations_dataTable;

        public MainForm()
        {
            InitializeComponent();
            dataBase_Manager = new DataBase_Manager(this);
            dataBase_Manager.ConnectToDB();
            excel_Manager = new MSExcel_Manager(this);
            expertSystem_Manager = new ExpertSystem_Manager(this);
            fileWork_Manager = new FileWork_Manager(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.BuildComboBoxDataGridView(dataGridView2);
            comboBox1.Items.AddRange(dataBase_Manager.GetTableNames().ToArray());
        }

        private void button_LoadExcel_Click(object sender, EventArgs e)
        {
            excel_dataTable = excel_Manager.LoadExcelFile();
            dataGridView1.DataSource = excel_dataTable;
            expertSystem_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                db_dataTable = dataBase_Manager.SelectAll_DataTable("SELECT * FROM " + comboBox1.SelectedItem.ToString());
                dataGridView1.DataSource = db_dataTable;
        }

        private void button_ExecuteRegulations_Click(object sender, EventArgs e)
        {
            expertSystem_Manager.GetRegulations(dataGridView2);
        }

        private void button_RefreshVarriable_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (fileWork_Manager.GetLocalData() != null)
            {
                listBox1.Items.AddRange(fileWork_Manager.GetLocalData().ToArray());
            }
        }

        private void button_InsertButton_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            switch(e.ColumnIndex)
            {
                case 0:
                    {
                        try
                        {
                            string str = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                            string result = "";
                            List<int> arr = new List<int>();
                            for (int i = 0; i < str.Length; i++)
                            {
                                if (str[i] != '[' && str[i] != ']' && str[i] != ':')
                                {
                                    result += str[i];
                                }
                                else if (str[i] == ']' || str[i] == ':')
                                {
                                    arr.Add(Convert.ToInt32(result));
                                    result = "";
                                }
                            }
                            dataGridView2.Rows[e.RowIndex].Cells[1].Value = dataGridView1.Rows[arr[0]].Cells[arr[1]].Value.ToString();
                        }
                        catch
                        {

                        }
                        break;
                    }
                case 1:
                    {
                        
                        break;
                    }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show(e.RowIndex.ToString() + " " + e.ColumnIndex.ToString());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db_dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (excel_dataTable != null)
            {
                dataGridView1.DataSource = excel_dataTable;
                expertSystem_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
            }
            else
            {
                MessageBox.Show("Сначало откройте Excel файл!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileWork_Manager.ClearLocalDate();
        }

        private void button_LoadRegulations_Click(object sender, EventArgs e)
        {
            if (excel_dataTable != null)
            {
                DataTable dataTable = fileWork_Manager.LoadParsingRegulations();
                List<DataGridViewComboBoxColumn> dataGridViewComboBoxColumns = new List<DataGridViewComboBoxColumn>();
                dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                dataGridViewComboBoxColumns[0] = dataGridView2.Columns[0] as DataGridViewComboBoxColumn;
                dataGridViewComboBoxColumns[1] = dataGridView2.Columns[2] as DataGridViewComboBoxColumn;
                dataGridViewComboBoxColumns[2] = dataGridView2.Columns[3] as DataGridViewComboBoxColumn;
                try
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string str = dataTable.Rows[i][0].ToString();
                        string result = "";
                        List<int> arr = new List<int>();
                        for (int j = 0; j < str.Length; j++)
                        {
                            if (str[j] != '[' && str[j] != ']' && str[j] != ':')
                            {
                                result += str[j];
                            }
                            else if (str[j] == ']' || str[j] == ':')
                            {
                                arr.Add(Convert.ToInt32(result));
                                result = "";
                            }
                        }
                        dataGridViewComboBoxColumns[0].Items.Add(dataTable.Rows[i][0].ToString());
                        dataGridView2.Rows[i].Cells[0].Value = dataTable.Rows[i][0].ToString();
                        //
                        dataGridViewComboBoxColumns[1].Items.Add(dataTable.Rows[i][2].ToString());
                        dataGridView2.Rows[i].Cells[2].Value = dataTable.Rows[i][2].ToString();
                        //
                        dataGridViewComboBoxColumns[2].Items.Add(dataTable.Rows[i][3].ToString());
                        dataGridView2.Rows[i].Cells[3].Value = dataTable.Rows[i][3].ToString();
                        //
                        dataGridView2.Rows[i].Cells[1].Value = excel_dataTable.Rows[arr[0]][arr[1]].ToString();
                    }
                }
                catch
                {

                }
            }
            else
            {
                MessageBox.Show("Сначало откройте Excel таблицу!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Program.BuildComboBoxDataGridView(dataGridView2);
        }

        private void button_SaveRegulations_Click(object sender, EventArgs e)
        {
            fileWork_Manager.SaveParsingRegulations(dataGridView2);
        }
    }
}
