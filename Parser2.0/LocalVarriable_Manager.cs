using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Parser2._0
{
    public partial class LocalVarriable_Manager : Form
    {
        DataTable dataTable;
        MainForm mainForm;
        public LocalVarriable_Manager()
        {
            InitializeComponent();
        }
        public LocalVarriable_Manager(MainForm mainForm_)
        {
            InitializeComponent();
            mainForm = mainForm_;
            
        }

        protected void RefreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataTable;
        }

        List<Object> DataTableToList(DataTable dataTable)
        {
            List<Object> list = new List<Object>();
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                list.Add(dataTable.Rows[i][1]);
            }
            return list;
        }

        private void LocalVarriable_Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.fileWork_Manager.PushArrayLocalData(DataTableToList(dataTable));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                dataTable.Rows.Add(dataTable.Rows.Count + 1, textBox1.Text);
                textBox1.Text = "";
                textBox2.Text = "";
                RefreshDataGridView();
                MessageBox.Show("Добавлено!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentCell != null && !String.IsNullOrEmpty(textBox1.Text))
            {
                dataTable.Rows[dataGridView1.CurrentCell.RowIndex][1] = textBox1.Text;
                textBox1.Text = "";
                textBox2.Text = "";
                RefreshDataGridView();
                MessageBox.Show("Отредактировано!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow dataGridViewRow in dataGridView1.SelectedRows)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (dataTable.Rows[i][0] == dataGridViewRow.Cells[0] && dataTable.Rows[i][1] == dataGridViewRow.Cells[1])
                            {
                                dataTable.Rows.RemoveAt(--i);
                            }
                        }
                        dataGridView1.Rows.Remove(dataGridViewRow);
                    }
                    textBox1.Text = "";
                    textBox2.Text = "";
                    RefreshDataGridView();
                    MessageBox.Show("Удалено!");
                }
            }
        }

        private void LocalVarriable_Manager_Load(object sender, EventArgs e)
        {
            dataTable = mainForm.localdata_datagrid.DataSource as DataTable;
            RefreshDataGridView();
        }

        private void очиститьЛокальнуюПеременнуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainForm.fileWork_Manager.ClearLocalDate();
            dataTable.Rows.Clear();
            mainForm.fileWork_Manager.PushArrayLocalData(DataTableToList(dataTable));
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.CurrentCell != null)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }
    }
}
