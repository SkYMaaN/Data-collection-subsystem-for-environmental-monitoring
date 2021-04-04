using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Parser2._0
{
    class DataBase_Manager //MySQL
    {
        private string path = "Server=" + "195.54.163.133" + ";Database=" + "h34471c_KPI_KEEM" + ";port=" + 3306 + ";User Id=" + "h34471c" + ";password=" + "8uUUS-97[1Aahm";
        private MySqlConnection sqlConnection;
        private MySqlCommand sqlCommand;
        internal void ConnectToDB()
        {
            sqlConnection.ConnectionString = path;
        }
        internal List<List<Object>> SelectAll(string TableName)
        {
            try
            {
                List<List<Object>> response = new List<List<object>>();
                sqlCommand.CommandText = "SELECT * FROM " + TableName;
                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    List<Object> row = new List<Object>();
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        row.Add(sqlDataReader[i]);
                    }
                    response.Add(row);
                }
                sqlConnection.Close();
                return response;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:  " + ex.Message);
                return null;
            }
        }
        internal void SelectAll(string TableName, DataGridView dataGridView)
        {
            try
            {
                sqlCommand.CommandText = "SELECT * FROM " + TableName;
                sqlConnection.Open();
                MySqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                dataGridView.DataSource = null;
                FillDataGridView(dataGridView, sqlDataReader);
                sqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error:  " + ex.Message);
            }
        }
        private void FillDataGridView(DataGridView dataGridView, MySqlDataReader sqlDataReader)
        {
            try
            {
                dataGridView.Columns.Clear();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    dataGridView.Columns.Add(sqlDataReader.GetName(i), sqlDataReader.GetName(i));
                }
                while (sqlDataReader.Read())
                {
                    String[] data = new string[sqlDataReader.FieldCount];
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        data[i] = sqlDataReader[i].ToString();
                    }
                    dataGridView.Rows.Add(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        internal void ExecSQL(string query)
        {
            try
            {
                sqlCommand.CommandText = query;
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error:  " + ex.Message);
            }
        }
        internal void Insert(string tablename, string[] fields, string[] values)
        {
            try
            {
                sqlCommand.CommandText = "INSERT INTO " + tablename + "(" + String.Join(",", fields) + ") VALUES (" + String.Join(",", values) + ")";
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error:  " + ex.Message);
            }
        }
        internal void Delete(string tablename, string condition = null)
        {
            try
            {
                if (condition != null)
                {
                    sqlCommand.CommandText = "DELETE FROM " + tablename + "WHERE " + condition;
                }
                else if (condition == null)
                {
                    sqlCommand.CommandText = "DELETE FROM " + tablename;
                }
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error:  " + ex.Message);
            }
        }
        internal void Update(string tablename, string field, string newvalue, string conditionfield, string conditionvalue)
        {
            try
            {
                sqlCommand.CommandText = "UPDATE " + tablename + "SET " + field + " = '" + newvalue + "' WHERE " + conditionfield + " = '" + conditionvalue + "'";
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error:  " + ex.Message);
            }
        }
        internal DataTable SelectAll_DataTable(string query)
        {
            try
            {
                sqlConnection.Open();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(query, sqlConnection);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                sqlConnection.Close();
                return dataSet.Tables[0];
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error:  " + ex);
                return null;
            }
        }
        internal List<string> GetTableNames()
        {
            try
            {
                List<string> result = new List<string>();
                sqlConnection.Open();
                sqlCommand.CommandText = "SHOW TABLES";
                MySqlDataReader mySqlDataReader = sqlCommand.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    result.Add(mySqlDataReader[0].ToString());
                }
                sqlConnection.Close();
                return result;
            }
            catch (MySqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + ex);
                return null;
            }
        }
        internal int GetNextPrimaryKey(string tablename, string IdName)
        {
            try
            {
                sqlConnection.Open();
                sqlCommand.CommandText = "SELECT MAX(" + IdName + ") FROM " + tablename;
                Object response = sqlCommand.ExecuteScalar();
                if (response == DBNull.Value)
                    response = 0;
                sqlConnection.Close();
                return Convert.ToInt32(response) + 1;
            }
            catch
            {
                sqlConnection.Close();
                return -1;
            }
        }
        internal DataBase_Manager()
        {
            sqlConnection = new MySqlConnection();
            sqlCommand = new MySqlCommand();
            sqlCommand.Connection = sqlConnection;
        }
    }
}
