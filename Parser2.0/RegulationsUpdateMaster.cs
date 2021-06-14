using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    public partial class RegulationsUpdateMaster : Form
    {
        DataTable dataTable_data = null;
        DataTable dataTable_regulations = null;
        public RegulationsUpdateMaster(DataTable dataTable_data_, DataTable dataTable_regulations_)
        {
            InitializeComponent();
            dataTable_data = dataTable_data_;
            dataGridView1.DataSource = dataTable_data;
            dataTable_regulations = dataTable_regulations_;
            Program.BuildComboBoxDataGridView(dataGridView2);
            dataGridView2.DataSource = dataTable_regulations;
        }

        private List<List<int>> ParseAdresses()
        {
            if (dataTable_data != null)
            {
                List<List<int>> arr = new List<List<int>>();
                string result = "";
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    /*List<int> tmp = new List<int>();
                    string str = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    for (int b = 0; b < str.Length; b++)
                    {
                        if (str[b] != '[' && str[b] != ']' && str[b] != ':')
                        {
                            result += str[b];
                        }
                        else if (str[b] == ']' || str[b] == ':')
                        {
                            arr.Add(Convert.ToInt32(result));
                            result = "";
                        }
                    }*/
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        private void BrushCells(List<List<int>> Adresses)
        {
            for(int i = 0; i < Adresses.Count;i++)
            {
                dataGridView1.Rows[Adresses[i][i]].Cells[Adresses[i][1]].Style.BackColor = System.Drawing.Color.Yellow;
            }
        }

        private void RegulationsUpdateMaster_Load(object sender, EventArgs e)
        {
            List<List<int>> vs = new List<List<int>>();
            BrushCells(ParseAdresses());
        }
    }
}
