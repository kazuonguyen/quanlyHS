using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
namespace diemlop
{
    public partial class quanlyloiVP : Form
    {
        fchinh mainForm;
        public quanlyloiVP(fchinh parent)
        {
            InitializeComponent();
            loadingTable();
            loadingTable2();
            mainForm = parent;
        }
        const string connectionString = "mongodb://localhost:27017";
        private void loadingTable()
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("loivp");
            var collection = database.GetCollection<BsonDocument>("loiVP");
            var documents = collection.Find(new BsonDocument()).ToList();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Tên VP");
            dt.Columns.Add("Điểm trừ");
            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["name"];
                dr[1] = doc["value"];
                dt.Rows.Add(dr);

            }
            BindingSource bs = new BindingSource(); // introduce a BindingSource
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].Width = 100;
            this.dataGridView1.Columns[1].Width = 100;
        }
        private void loadingTable2()
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("loivplop");
            var collection = database.GetCollection<BsonDocument>("loiVPLOP");
            var documents = collection.Find(new BsonDocument()).ToList();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Tên VP");
            dt.Columns.Add("Điểm trừ");
            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["name"];
                dr[1] = doc["value"];
                dt.Rows.Add(dr);

            }
            BindingSource bs = new BindingSource(); // introduce a BindingSource
            bs.DataSource = dt;
            dataGridView2.DataSource = bs;
            this.dataGridView2.Columns[0].Width = 100;
            this.dataGridView2.Columns[1].Width = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "" || textBox5.Text == "")
            {
                string message = "Bạn chưa nhập đày đủ";
                string title = "Cảnh báo";
                MessageBox.Show(message, title);
            }
            else
            {
                var document = new BsonDocument
            {
                {"name",textBox4.Text.ToString()},
            { "value",textBox5.Text.ToString()},
            };
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase("loivp");

                var collection = database.GetCollection<BsonDocument>("loiVP");


                collection.InsertOne(document);
                loadingTable();
                mainForm.loadingCombobox();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivp");

            var collection = database.GetCollection<BsonDocument>("loiVP");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("name", textBox1.Text.ToString());
            collection.DeleteMany(deleteFilter);
            loadingTable();
            mainForm.loadingCombobox();
        }

        private void quanlyloiVP_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || textBox3.Text == "")
            {
                string message = "Bạn chưa nhập đày đủ";
                string title = "Cảnh báo";
                MessageBox.Show(message, title);
            }
            else
            {
                var document = new BsonDocument
            {
                {"name",textBox6.Text.ToString()},
            { "value",textBox3.Text.ToString()},
            };
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase("loivplop");

                var collection = database.GetCollection<BsonDocument>("loiVPLOP");

                collection.InsertOne(document);
                loadingTable2();
                mainForm.loadingCombobox();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivplop");

            var collection = database.GetCollection<BsonDocument>("loiVPLOP");
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("name", textBox2.Text.ToString());
            collection.DeleteMany(deleteFilter);
            loadingTable2();
            mainForm.loadingCombobox();
        }
    }
}
