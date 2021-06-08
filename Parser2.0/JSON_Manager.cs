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
        internal DataTable LoadJsonFile(string Algorithm_)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.json, *.txt) | *.json; *.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataTable dataTable = new DataTable();
                JsonTextReader jsonTextReader = new JsonTextReader(File.OpenText(openFileDialog.FileName));
                if (JToken.Parse(File.ReadAllText(openFileDialog.FileName)) is JArray)
                {
                    JArray jsonArray = JToken.ReadFrom(jsonTextReader) as JArray;
                    switch (Algorithm_)
                    {
                        case "SaveEcoBot":
                            {
                                Deserialize_SaveEcoBot(jsonArray, dataTable);
                                break;
                            }
                    }
                }
                return dataTable;
            }
            else
            {
                return null;
            }
        }
        protected void Deserialize_SaveEcoBot(JArray jsonArray, DataTable dataTable)
        {
            List<SaveEcoBot_Object> saveEcoBot_Objects = new List<SaveEcoBot_Object>();
            foreach (JObject obj in jsonArray)
            {
                SaveEcoBot_Object saveEcoBot_Object = new SaveEcoBot_Object();
                saveEcoBot_Object.id = obj["id"].Value<string>();
                saveEcoBot_Object.cityName = obj["cityName"].Value<string>();
                saveEcoBot_Object.stationName = obj["stationName"].Value<string>();
                saveEcoBot_Object.localName = obj["localName"].Value<string>();
                saveEcoBot_Object.timezone = obj["timezone"].Value<string>();
                saveEcoBot_Object.latitude = obj["latitude"].Value<float>();
                saveEcoBot_Object.longitude = obj["longitude"].Value<float>();
                foreach (JObject pollutantobj in obj["pollutants"])
                {
                    SaveEcoBotObject_Pollutant saveEcoBotObject_Pollutant = new SaveEcoBotObject_Pollutant();
                    saveEcoBotObject_Pollutant.pol = pollutantobj["pol"].Value<string>();
                    saveEcoBotObject_Pollutant.unit = pollutantobj["unit"].Value<string>();
                    saveEcoBotObject_Pollutant.time = pollutantobj["time"].Value<string>();
                    saveEcoBotObject_Pollutant.value = pollutantobj["value"].Value<string>();
                    saveEcoBotObject_Pollutant.averaging = pollutantobj["averaging"].Value<string>();
                    saveEcoBot_Object.pollutants.Add(saveEcoBotObject_Pollutant);
                }
                saveEcoBot_Objects.Add(saveEcoBot_Object);
            }
            //========================================================================================================
            //========================================================================================================
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("cityName");
            dataTable.Columns.Add("stationName");
            dataTable.Columns.Add("localName");
            dataTable.Columns.Add("timezone");
            dataTable.Columns.Add("latitude");
            dataTable.Columns.Add("longitude");
            //dataTable.Columns.Add("pollutants");
            //========================================================================================================
            //========================================================================================================
            int max_pollutant_count = 0;
            for (int i = 0; i < saveEcoBot_Objects.Count; i++)
            {
                int tmp = 0;
                for (int j = 0; j < saveEcoBot_Objects[i].pollutants.Count; j++)
                {
                    tmp++;
                }
                if(tmp > max_pollutant_count)
                {
                    max_pollutant_count = tmp;
                    tmp = 0;
                }
            }
            for(int i = 0; i < max_pollutant_count; i++)
            {
                dataTable.Columns.Add("Pollutant № "+i);
                dataTable.Columns.Add("Value № "+i);
            }
            //========================================================================================================
            //========================================================================================================
            DataRow dataRow;
            for (int i = 0; i < saveEcoBot_Objects.Count; i++)
            {
                dataRow = dataTable.NewRow();
                dataRow["id"] = saveEcoBot_Objects[i].id;
                dataRow["cityName"] = saveEcoBot_Objects[i].cityName;
                dataRow["stationName"] = saveEcoBot_Objects[i].stationName;
                dataRow["localName"] = saveEcoBot_Objects[i].localName;
                dataRow["timezone"] = saveEcoBot_Objects[i].timezone;
                dataRow["latitude"] = saveEcoBot_Objects[i].latitude;
                dataRow["longitude"] = saveEcoBot_Objects[i].longitude;
                for (int j = 0, idx = j; j < saveEcoBot_Objects[i].pollutants.Count; idx += 2, j++) 
                {
                    dataRow[7 + idx] = saveEcoBot_Objects[i].pollutants[j].pol;
                    dataRow[8 + idx] = saveEcoBot_Objects[i].pollutants[j].value;
                }
                dataTable.Rows.Add(dataRow);
            }
        }
        internal JSON_Manager(MainForm mainForm_)
        {
            mainForm = mainForm_;
        }
    }
}
