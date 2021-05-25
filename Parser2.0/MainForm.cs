using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Parser2._0
{
    public partial class MainForm : Form
    {
        internal DataBase_Manager dataBase_Manager;
        internal MSExcel_Manager excel_Manager;
        internal ExpertSystem_Manager expertSystem_Manager;
        internal FileWork_Manager fileWork_Manager;
        internal JSON_Manager json_Manager;
        DataTable db_dataTable = null;
        DataTable excel_dataTable = null;
        DataTable json_dataTable = null;
        internal string TMP_For_Find = "null";

        public MainForm()
        {
            InitializeComponent();
            dataBase_Manager = new DataBase_Manager(this);
            dataBase_Manager.ConnectToDB();
            excel_Manager = new MSExcel_Manager(this);
            expertSystem_Manager = new ExpertSystem_Manager(this);
            fileWork_Manager = new FileWork_Manager(this);
            json_Manager = new JSON_Manager(this);
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
            if (excel_dataTable != null)
            {
                dataGridView1.DataSource = excel_dataTable;
                expertSystem_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
            }
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

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            switch(e.ColumnIndex)
            {
                case 0:
                    {
                        try
                        {
                            string str = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                            if(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString() == "Varriable")
                            {
                                dataGridView2.Rows[e.RowIndex].Cells[1].Value = "Varriable";
                            }
                            else
                            {
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
                        }
                        catch
                        {

                        }
                        break;
                    }
                case 1:
                    {
                        try
                        {
                            if(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString() == "Varriable")
                            {
                                dataGridView2.Rows[e.RowIndex].Cells[0].Value = "Varriable";
                            }
                            else
                            {
                                string str = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                                for (int i = 0; i < (dataGridView1.DataSource as DataTable).Columns.Count; i++)
                                {
                                    for (int j = 0; j < (dataGridView1.DataSource as DataTable).Rows.Count; j++)
                                    {
                                        if ((dataGridView1.DataSource as DataTable).Rows[j][i].ToString() == str)
                                        {
                                            dataGridView2.Rows[e.RowIndex].Cells[0].Value = "[" + j + ":" + i + "]";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        break;
                    }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*for(int i = 0; i < (dataGridView2.DataSource as DataTable).Rows.Count;i++)
            {
                if(dataGridView2.Rows[i].Cells[1].Value == null)
                {
                    dataGridView2.Rows[i].Cells[1].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
            }*/
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (db_dataTable != null)
            {
                dataGridView1.DataSource = db_dataTable;
            }
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
                (dataGridView2.DataSource as DataTable).Clear();
                DataTable dataTable = fileWork_Manager.LoadParsingRegulations();
                if (dataTable != null)
                {                   
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        (dataGridView2.DataSource as DataTable).Rows.Add();
                    }
                    List<DataGridViewComboBoxColumn> dataGridViewComboBoxColumns = new List<DataGridViewComboBoxColumn>();
                    dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                    dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                    dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                    dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                    dataGridViewComboBoxColumns.Add(new DataGridViewComboBoxColumn());
                    dataGridViewComboBoxColumns[0] = dataGridView2.Columns[0] as DataGridViewComboBoxColumn;
                    dataGridViewComboBoxColumns[1] = dataGridView2.Columns[1] as DataGridViewComboBoxColumn; //not using
                    dataGridViewComboBoxColumns[2] = dataGridView2.Columns[2] as DataGridViewComboBoxColumn;
                    dataGridViewComboBoxColumns[3] = dataGridView2.Columns[3] as DataGridViewComboBoxColumn;
                    dataGridViewComboBoxColumns[4] = dataGridView2.Columns[4] as DataGridViewComboBoxColumn;
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
                            dataGridView2.Rows[i].Cells[1].Value = excel_dataTable.Rows[arr[0]][arr[1]].ToString();
                            //
                            dataGridView2.Rows[i].Cells[2].Value = dataTable.Rows[i][2].ToString();
                            //
                            dataGridView2.Rows[i].Cells[3].Value = dataTable.Rows[i][3].ToString();
                            //
                            dataGridView2.Rows[i].Cells[4].Value = dataTable.Rows[i][4].ToString();
                            dataGridView2.Rows[i].Cells[5].Value = dataTable.Rows[i][5].ToString();

                        }

                    }
                    catch
                    {

                    }
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
            expertSystem_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
        }

        private void button_SaveRegulations_Click(object sender, EventArgs e)
        {
            fileWork_Manager.SaveParsingRegulations(dataGridView2);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            button_RefreshVarriable_Click(sender, e);
            listBox2.Items.Clear();
            listBox2.Items.Add(TMP_For_Find);
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (excel_dataTable != null)
            {
                excel_dataTable = null;
                button4_Click(sender, e);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(json_dataTable != null)
            {
                dataGridView1.DataSource = json_dataTable;
            }
            else
            {
                MessageBox.Show("Сначало загрузите JSON файл");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            json_dataTable = json_Manager.LoadJsonFile();
            if(json_dataTable != null)
            {
                dataGridView1.DataSource = json_dataTable;
                json_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (json_dataTable != null)
            {
                dataGridView1.DataSource = null;
                button4_Click(sender, e);
                json_dataTable = null;
            }
        }
    }
}
