using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    class ExpertSystem_Manager
    {
        MainForm mainForm; 
        private string InputData; //0
        private string ValueInFile; //1
        private string OutputDataType; //2
        private string Command; //3
        private string Options; //4
        private string Result; //5
        internal void Get_ValueInFile(DataGridView dataGridView, DataGridView dataGridView1)
        {
            if (dataGridView.DataSource != null)
            {
                DataGridViewComboBoxColumn dataGridViewComboBoxColumn = new DataGridViewComboBoxColumn();
                dataGridViewComboBoxColumn = dataGridView1.Columns[0] as DataGridViewComboBoxColumn;
                dataGridViewComboBoxColumn.Items.Clear();
                dataGridViewComboBoxColumn.Items.Add("Varriable");
                DataGridViewComboBoxColumn dataGridViewComboBoxColumn1 = new DataGridViewComboBoxColumn();
                dataGridViewComboBoxColumn1 = dataGridView1.Columns[1] as DataGridViewComboBoxColumn;
                dataGridViewComboBoxColumn1.Items.Clear();
                dataGridViewComboBoxColumn1.Items.Add("Varriable");
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
                if (!Program.IsDataGridViewRowEmpty(dataGridViewRow))
                {
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
                    InputData = "";
                    ValueInFile = "";
                    OutputDataType = "";
                    Command = "";
                    Options = "";
                    Result = "";
                }
            }
            MessageBox.Show("Команды успешно выполнены!");
        }
        private void Save()
        {
            mainForm.fileWork_Manager.PushLocalData(ValueInFile);               
        }
        private void Find()
        {
            //Порядок полей
            //Название таблицы, Поле условия   ||   result = Желаемое поле,
            DataBase_Manager dataBase_Manager = new DataBase_Manager();
            dataBase_Manager.ConnectToDB();
            List<string> list = this.ParseOptions();
            Object code = dataBase_Manager.ExecScalar("SELECT " + this.Result.Replace(";","") + " FROM " + list[0].ToString() + " WHERE " + list[1].ToString() + " = '" + this.ValueInFile.Trim() + "'");
            if(code != null)
            {
                DialogResult result = MessageBox.Show("Записать в переменную?", "Найдено!", MessageBoxButtons.YesNo,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    mainForm.TMP_For_Find = code.ToString();
                    mainForm.fileWork_Manager.PushLocalData(code.ToString());
                    //MessageBox.Show("Значение записано в переменную!");
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Ввести значение с клавиатуры?", "Не найдено!", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.Yes)
                {
                    NotFindForm notFindForm = new NotFindForm();
                    notFindForm.ShowDialog();
                    mainForm.TMP_For_Find = notFindForm.textBox1.Text;
                    mainForm.fileWork_Manager.PushLocalData(notFindForm.textBox1.Text);
                    //MessageBox.Show("Новое значение записано в переменную!");
                }
            }
        }
        private void Edit()
        {

        }
        private void Add()
        {
            try
            {
                if (InputData == "Varriable")
                {
                    //Порядок полей
                    //Название таблицы, количество полей
                    List<string> list = ParseOptions();
                    List<MySqlParameter> mySqlParameters = new List<MySqlParameter>();
                    List<Object> tempdata = new List<Object>();
                    tempdata.Add(mainForm.dataBase_Manager.GetNextPrimaryKey(list[0],"id").ToString());
                    tempdata.AddRange(mainForm.fileWork_Manager.GetLocalData(Convert.ToInt32(list[1])));
                    for(int i = 0; i < Convert.ToInt32(list[1])+1; i++)
                    {
                        mySqlParameters.Add(new MySqlParameter("@" + i.ToString(), tempdata[i]));
                    }                    
                    mainForm.dataBase_Manager.Insert(list[0], mySqlParameters);
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:  " + ex);
            }
        }
        List<string> ParseOptions()
        {
            List<string> response = new List<string>();
            string str = "";
            for(int i = 0; i < Options.Length;i++)
            {
                if (Options[i] != ';')
                {
                    str += Options[i];
                }
                else
                {
                    response.Add(str);
                    str = "";
                }
            }
            return response;
        }
        internal ExpertSystem_Manager(MainForm form)
        {
            mainForm = form;   
        }
    }
}
