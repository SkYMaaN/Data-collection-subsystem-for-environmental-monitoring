using System;
using System.Windows.Forms;

namespace Parser2._0
{
    public partial class MainForm : Form
    {
        private DataBase_Manager dataBase_Manager;
        private MSExcel_Manager excel_Manager;
        private ExpertSystem_Manager expertSystem_Manager;
        private FileWork_Manager fileWork_Manager;

        public MainForm()
        {
            InitializeComponent();
            dataBase_Manager = new DataBase_Manager();
            dataBase_Manager.ConnectToDB();
            excel_Manager = new MSExcel_Manager();
            expertSystem_Manager = new ExpertSystem_Manager();
            fileWork_Manager = new FileWork_Manager();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.BuildComboBoxDataGridView(dataGridView2);
            comboBox1.Items.AddRange(dataBase_Manager.GetTableNames().ToArray());
        }

        private void button_LoadExcel_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = excel_Manager.LoadExcelFile();
            expertSystem_Manager.Get_ValueInFile(dataGridView1, dataGridView2);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dataBase_Manager.SelectAll_DataTable("SELECT * FROM " + comboBox1.SelectedItem.ToString());
        }

        private void button_ExecuteRegulations_Click(object sender, EventArgs e)
        {
            expertSystem_Manager.GetRegulations(dataGridView2);
        }
    }
}
