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
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("cityName");
            dataTable.Columns.Add("stationName");
            dataTable.Columns.Add("localName");
            dataTable.Columns.Add("timezone");
            dataTable.Columns.Add("latitude");
            dataTable.Columns.Add("longitude");
            dataTable.Columns.Add("pollutants");
            for(int i = 0; i < saveEcoBot_Objects.Count; i++)
            {
                List<string> pollutant = new List<string>();
                for (int j = 0; j < saveEcoBot_Objects[i].pollutants.Count; j++)
                {
                    pollutant.Add(saveEcoBot_Objects[i].pollutants[j].pol + " " + saveEcoBot_Objects[i].pollutants[j].unit + " " + saveEcoBot_Objects[i].pollutants[j].time + " " + saveEcoBot_Objects[i].pollutants[j].value + " " + saveEcoBot_Objects[i].pollutants[j].averaging);
                }
                dataTable.Rows.Add(saveEcoBot_Objects[i].id, saveEcoBot_Objects[i].cityName, saveEcoBot_Objects[i].stationName, saveEcoBot_Objects[i].localName, saveEcoBot_Objects[i].timezone, saveEcoBot_Objects[i].latitude, saveEcoBot_Objects[i].longitude, String.Join(" , ", pollutant));
                pollutant.Clear();
            }
        }
        internal JSON_Manager(MainForm mainForm_)
        {
            mainForm = mainForm_;
        }
    }
}
