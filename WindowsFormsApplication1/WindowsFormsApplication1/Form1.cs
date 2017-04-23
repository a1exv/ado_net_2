using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        DataTable factories = null;
        DbProviderFactory factory = null;
        DbConnection conn = null;
        DbDataAdapter dda = null;
        DataSet table = new DataSet();
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
            factories = DbProviderFactories.GetFactoryClasses();
            factory = DbProviderFactories.GetFactory(factories.Rows[1]["InvariantName"].ToString());
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            conn = factory.CreateConnection();
            conn.ConnectionString = settings[1].ConnectionString;
           // conn.Open();
            dda = factory.CreateDataAdapter();
            dda.SelectCommand = conn.CreateCommand();
            dda.SelectCommand.CommandText = "Select * from Clients";
            dda.Fill(table, "Clients");
            dda.SelectCommand.CommandText = "Select * from Sellers";
            dda.Fill(table, "Sellers");
            dda.SelectCommand.CommandText = "Select * from Bargains";
            dda.Fill(table, "Bargains");
            dataGridView1.DataSource = dt;
            comboBox1.Items.Add("Clients");
            comboBox1.Items.Add("Sellers");
            comboBox1.Items.Add("Bargains");
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dda.SelectCommand.CommandText = String.Format("select * from " + comboBox1.SelectedItem.ToString());
            dt = new DataTable();
            dda.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cmdText = textBox1.Text;
                DbCommand comm = conn.CreateCommand();
                comm.CommandText = cmdText;
                string AsynchEnabled = "Asynchronous Processing=true";
             //  if (!conn.ConnectionString.Contains(AsynchEnabled))
             //  {
             //      conn.ConnectionString = String.Format("{0}; {1}", conn.ConnectionString, AsynchEnabled);
             //  }
                dda.SelectCommand = comm;
                dt = new DataTable();
                dda.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
