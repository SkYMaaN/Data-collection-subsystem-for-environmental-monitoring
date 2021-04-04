using System;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace Parser2._0
{
    class FileWork_Manager
    {
        private List<string> datalist;
        internal void SaveData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "(*.json) | *.json";
            saveFileDialog.Filter = "(*.json) | *.json";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string JSONstring = Newtonsoft.Json.JsonConvert.SerializeObject(datalist);
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    fs.Write(System.Text.Encoding.Default.GetBytes(JSONstring), 0, System.Text.Encoding.Default.GetBytes(JSONstring).Length);
                }
            }
        }
        internal void LoadData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "(*.json) | *.json";
            openFileDialog.Filter = "(*.json) | *.json";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    datalist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(System.Text.Encoding.Default.GetString(array));
                }
            }
        }
        internal List<string> GetData()
        {
            if (datalist.Count > 0)
            {
                return datalist;
            }
            else
            {
                return null;
            }
        }
        internal bool isEmpty()
        {
            if (datalist.Count < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal void PushData(string Data)
        {
            datalist.Add(Data);
        }
        internal void SaveParsingRegulations(DataGridView dataGridView)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "(*.json) | *.json";
                saveFileDialog.Filter = "(*.json) | *.json";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = new DataTable();
                    dataTable = dataGridView.DataSource as DataTable;
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        dataTable.Rows.Add();
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            dataTable.Columns.Add();
                            if (dataGridView.Rows[i].Cells[j].Value != null)
                            {
                                dataTable.Rows[i][j] = dataGridView.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                    }
                    string JSONstring = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                    using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        fs.Write(System.Text.Encoding.Default.GetBytes(JSONstring), 0, System.Text.Encoding.Default.GetBytes(JSONstring).Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }
        internal DataTable LoadParsingRegulations()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "(*.json) | *.json";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dataTable = new DataTable();
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                    {
                        byte[] array = new byte[fs.Length];
                        fs.Read(array, 0, array.Length);
                        dataTable = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(System.Text.Encoding.Default.GetString(array));
                    }
                    return dataTable;
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                return null;
            }
        }
        internal FileWork_Manager()
        {
            datalist = new List<string>();
        }
    }
}
