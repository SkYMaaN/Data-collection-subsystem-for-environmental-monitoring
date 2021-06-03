﻿using System;
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
            dataTable = new DataTable();
            mainForm = mainForm_;
            
        }

        protected void RefreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataTable;
        }

        void ListToDataTable(List<Object> list)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    dataTable.Rows.Add((list[i] as LocalData).source, (list[i] as LocalData).value);
                }
                dataGridView1.DataSource = dataTable;
            }
        }

        List<Object> DataTableToList(DataTable dataTable)
        {
            List<Object> list = new List<Object>();
            for(int i = 0; i < dataTable.Rows.Count; i++)
            {
                list.Add(new LocalData(dataTable.Rows[i][0].ToString(), dataTable.Rows[i][1]));
            }
            return list;
        }

        private void LocalVarriable_Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainForm.fileWork_Manager.ClearLocalDate();
            mainForm.fileWork_Manager.PushArrayLocalData(DataTableToList(dataTable));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && comboBox1.SelectedIndex != -1)
            {
                dataTable.Rows.Add(comboBox1.SelectedItem.ToString(), textBox1.Text);
                textBox1.Text = "";
                comboBox1.SelectedIndex = -1;
                RefreshDataGridView();
                MessageBox.Show("Добавлено!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
            {
                dataTable.Rows[dataGridView1.CurrentCell.RowIndex][0] = comboBox1.SelectedItem.ToString();
                dataTable.Rows[dataGridView1.CurrentCell.RowIndex][1] = textBox1.Text;
                textBox1.Text = "";
                comboBox1.SelectedIndex = -1;
                RefreshDataGridView();
                MessageBox.Show("Отредактировано!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
            {
                dataTable.Rows[dataGridView1.CurrentCell.RowIndex][0] = null;
                dataTable.Rows[dataGridView1.CurrentCell.RowIndex][1] = null;
                for(int i = dataGridView1.CurrentCell.RowIndex+1; i < dataTable.Rows.Count; i++)
                { 
                    for(int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        dataTable.Rows[i-1][j] = dataTable.Rows[i][j];
                        dataTable.Rows[i][j] = null;
                    }
                }
                textBox1.Text = "";
                comboBox1.SelectedIndex = -1;
                RefreshDataGridView();
                MessageBox.Show("Удалено!");
            }
        }

        private void LocalVarriable_Manager_Load(object sender, EventArgs e)
        {
            dataTable.Columns.Add("Data Source");
            dataTable.Columns.Add("Data Value");
            ListToDataTable(mainForm.fileWork_Manager.GetLocalData());
            RefreshDataGridView();
        }
    }
}
