using System;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections.Generic;

namespace Parser2._0
{
    class FileWork_Manager
    {
        MainForm mainForm;
        internal List<Object> datalist;
        internal void ClearLocalDate()
        {
            if (datalist.Count > 0)
            {
                datalist.Clear();
            }
        }
        internal List<Object> GetLocalData()
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
        internal List<Object> GetLocalData(int count)
        {
            if (datalist.Count > 0) 
            {
                List<Object> response = new List<Object>();
                for(int i = 0; i < count;i++)
                {
                    response.Add(datalist[i]);
                }
                for(int i = 0; i < count;i++)
                {
                    datalist.RemoveAt(0);
                }
                return response;
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
        internal void PushLocalData(Object Data)
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
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        if (!Program.IsDataGridViewRowEmpty(dataGridView.Rows[i]))
                        {
                            dataTable.Rows.Add();
                            for (int j = 0; j < dataGridView.Columns.Count; j++)
                            {
                                dataTable.Columns.Add();
                                if (dataGridView.Rows[i].Cells[j].Value != null)
                                {
                                    dataTable.Rows[i][j] = dataGridView.Rows[i].Cells[j].Value.ToString();
                                }
                                else
                                {
                                    dataTable.Rows[i][j] = null;
                                }
                            }
                        }
                    }
                    for(int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i][1] = null;
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
        internal int Count()
        {
            return datalist.Count;
        }
        internal void PushArrayLocalData(List<Object> list)
        {
            this.ClearLocalDate();
            datalist = list;
        }
        internal FileWork_Manager(MainForm form)
        {
            datalist = new List<Object>();
            mainForm = form;
        }
    }
}
