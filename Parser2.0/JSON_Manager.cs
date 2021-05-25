using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Parser2._0
{
    class JSON_Manager
    {
        List<Object> json_datalist;
        MainForm mainForm;
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
        internal DataTable LoadJsonFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dataTable = null;
                JObject jsonObject = null;
                JArray jsonArray = null;
                string jsonStr = File.ReadAllText(openFileDialog.FileName);
                var token = JToken.Parse(jsonStr);
                StreamReader file = File.OpenText(openFileDialog.FileName);
                JsonTextReader jsonTextReader = new JsonTextReader(file);
                if (token is JArray)
                {
                    jsonArray = JToken.ReadFrom(jsonTextReader) as JArray;
                    dataTable = JsonConvert.DeserializeObject<DataTable>(jsonArray.ToString());
                }
                else if (token is JObject)
                {
                    jsonObject = JToken.ReadFrom(jsonTextReader) as JObject;
                    dataTable = JsonConvert.DeserializeObject<DataTable>(jsonObject.ToString()); 
                }
                return dataTable;
            }
            else
            {
                return null;
            }
        }
        internal JSON_Manager(MainForm mainForm_)
        {
            mainForm = mainForm_;
            json_datalist = new List<Object>();
        }
    }
}
