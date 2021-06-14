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
        List<DataTable> excel_dataTables = null;
        DataTable db_dataTable = null;
        DataTable json_dataTable = null;
        DataTable localdata_dataTable = null;
        internal string TMP_For_Find = "null";

        internal void Refresh_DataGridView_LocalData()
        {
            localdata_datagrid.DataSource = null;
            localdata_dataTable.Rows.Clear();
            if (fileWork_Manager.GetLocalData() != null)
            {
                List<Object> list = new List<Object>();
                list = fileWork_Manager.GetLocalData();
                for (int i = 0; i < list.Count; i++)
                {
                    localdata_dataTable.Rows.Add(localdata_dataTable.Rows.Count + 1, list[i].ToString());
                }
            }
            localdata_datagrid.DataSource = localdata_dataTable;
            localdata_datagrid.DefaultCellStyle.ForeColor = Color.Black;
            localdata_datagrid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
            //localdata_datagrid.Columns[0].Width = 35;
        }

        public MainForm()
        {
            InitializeComponent();
            dataBase_Manager = new DataBase_Manager(this);
            dataBase_Manager.ConnectToDB();
            excel_Manager = new MSExcel_Manager(this);
            expertSystem_Manager = new ExpertSystem_Manager(this);
            fileWork_Manager = new FileWork_Manager(this);
            json_Manager = new JSON_Manager(this);
            localdata_dataTable = new DataTable();
            localdata_dataTable.Columns.Add("№");
            localdata_dataTable.Columns.Add("Значение");
            //localdata_dataTable.Columns.Add("Откуда/Что");
            localdata_datagrid.DataSource = localdata_dataTable;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            excel_Manager.Dispose();
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.BuildComboBoxDataGridView(dataGridView2);
            comboBox1.Items.AddRange(dataBase_Manager.GetTableNames().ToArray());
            
        }

        private void button_LoadExcel_Click(object sender, EventArgs e)
        {
            excel_dataTables = excel_Manager.LoadExcelSheetsXML();
            if (excel_dataTables != null)
            {
                for (int i = 0; i < excel_dataTables.Count; i++)
                {
                    this.tabControl1.TabPages.Add(new CustomTabPage(excel_dataTables[i], excel_dataTables[i].Rows[0][1].ToString(), "ExcelType"));
                }
                if ((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).Type != "DataBaseType")
                {
                    expertSystem_Manager.Get_ValueInFile((this.tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView, dataGridView2);
                }
            }
        }

        private void button_ExecuteRegulations_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 1)
            {
                expertSystem_Manager.GetRegulations(dataGridView2);
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
                                dataGridView2.Rows[e.RowIndex].Cells[1].Value = (tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView.Rows[arr[0]].Cells[arr[1]].Value.ToString();
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
                                for (int i = 0; i < ((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView.DataSource as DataTable).Columns.Count; i++)
                                {
                                    for (int j = 0; j < ((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView.DataSource as DataTable).Rows.Count; j++)
                                    {
                                        if (((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView.DataSource as DataTable).Rows[j][i].ToString() == str)
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                db_dataTable = dataBase_Manager.SelectAll_DataTable("SELECT * FROM " + comboBox1.SelectedItem.ToString());
                this.tabControl1.TabPages.Add(new CustomTabPage(db_dataTable, this.comboBox1.SelectedItem.ToString(), "DataBaseType"));
                comboBox1.SelectedIndex = -1;
            }
        }

        private void button_LoadRegulations_Click(object sender, EventArgs e)
        {
            if (excel_dataTables != null && excel_dataTables.Count > 0)
            {
                (dataGridView2.DataSource as DataTable).Clear();
                (dataGridView2.DataSource as DataTable).Rows.Clear();
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
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        try
                        {
                            string str = dataTable.Rows[i][0].ToString();
                            if (str != "Varriable")
                            {
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
                                dataGridView2.Rows[i].Cells[1].Value = ((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView.DataSource as DataTable).Rows[arr[0]][arr[1]].ToString();
                                dataGridView2.Rows[i].Cells[2].Value = dataTable.Rows[i][2].ToString();
                                dataGridView2.Rows[i].Cells[3].Value = dataTable.Rows[i][3].ToString();
                                dataGridView2.Rows[i].Cells[4].Value = dataTable.Rows[i][4].ToString();
                                dataGridView2.Rows[i].Cells[5].Value = dataTable.Rows[i][5].ToString();
                            }
                            else
                            {
                                dataGridViewComboBoxColumns[0].Items.Add(dataTable.Rows[i][0].ToString());
                                dataGridView2.Rows[i].Cells[0].Value = dataTable.Rows[i][0].ToString();
                                //dataGridView2.Rows[i].Cells[1].Value = "";
                                dataGridView2.Rows[i].Cells[2].Value = dataTable.Rows[i][2].ToString();
                                dataGridView2.Rows[i].Cells[3].Value = dataTable.Rows[i][3].ToString();
                                dataGridView2.Rows[i].Cells[4].Value = dataTable.Rows[i][4].ToString();
                                dataGridView2.Rows[i].Cells[5].Value = dataTable.Rows[i][5].ToString();
                            }
                            
                        }
                        catch
                        {
                            continue;
                        }
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
            if (tabControl1.SelectedIndex != -1 && (tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).Type != "DataBaseType")
            {
                expertSystem_Manager.Get_ValueInFile((tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView, dataGridView2);
            }
        }

        private void button_SaveRegulations_Click(object sender, EventArgs e)
        {
            fileWork_Manager.SaveParsingRegulations(dataGridView2, localdata_datagrid);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.Refresh_DataGridView_LocalData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (excel_dataTables != null && excel_dataTables.Count > 0)
            {
                for(int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if((tabControl1.TabPages[i] as CustomTabPage).Type == "ExcelType")
                    {
                        tabControl1.TabPages.RemoveAt(i--);
                    }
                }
                excel_Manager.CloseExcelFile();
                button4_Click(sender, e);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedIndex != -1 && !String.IsNullOrEmpty(this.comboBox2.SelectedItem.ToString()))
            {
                json_dataTable = json_Manager.LoadJsonFile(this.comboBox2.SelectedItem.ToString());
                if (json_dataTable != null)
                {
                    this.comboBox1.SelectedIndex = -1;
                    this.tabControl1.TabPages.Add(new CustomTabPage(json_dataTable, "JSON_" + this.comboBox2.SelectedItem.ToString(), "JSONType"));
                    this.comboBox2.SelectedIndex = -1;
                    json_Manager.Get_ValueInFile((this.tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView, dataGridView2);
                }
            }
            else
            {
                MessageBox.Show("Выберите алгоритм роботы с файлом!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (json_dataTable != null)
            {
                for(int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if((tabControl1.TabPages[i] as CustomTabPage).Type == "JSONType")
                    {
                        tabControl1.TabPages.RemoveAt(i--);
                    }
                }
                json_dataTable = null;
                button4_Click(sender, e);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            LocalVarriable_Manager localVarriable_Manager = new LocalVarriable_Manager(this);
            this.Hide();
            localVarriable_Manager.StartPosition = FormStartPosition.CenterScreen;
            localVarriable_Manager.ShowDialog();
            this.Show();
            this.timer1.Enabled = true;
        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Refresh_DataGridView_LocalData();
        }

        private void localdata_datagrid_MouseEnter(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
        }

        private void localdata_datagrid_MouseLeave(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (db_dataTable != null)
            {
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if ((tabControl1.TabPages[i] as CustomTabPage).Type == "DataBaseType")
                    {
                        tabControl1.TabPages.RemoveAt(i--);
                    }
                }
                button4_Click(sender, e);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex != -1 && (tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).Type != "DataBaseType") 
            {
                Program.BuildComboBoxDataGridView(dataGridView2);
                expertSystem_Manager.Get_ValueInFile((this.tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataGridView, dataGridView2);
            }
            else
            {
                Program.BuildComboBoxDataGridView(dataGridView2);
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DialogResult result = MessageBox.Show("Вы действительно желаете удалить вкладку?", "Удаление вкладки!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    (sender as TabControl).TabPages.RemoveAt((sender as TabControl).SelectedIndex);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            excel_Manager.Dispose();
        }

        private void regulationsUpdateMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegulationsUpdateMaster regulationsUpdateMaster = new RegulationsUpdateMaster((this.tabControl1.TabPages[tabControl1.SelectedIndex] as CustomTabPage).dataTable, (dataGridView2.DataSource as DataTable));
            this.Hide();
            regulationsUpdateMaster.ShowDialog();
            this.Show();
        }
    }
}
