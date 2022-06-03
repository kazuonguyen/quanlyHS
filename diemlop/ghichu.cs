using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
namespace diemlop
{
    public partial class ghichu : Form
    {
        fchinh mainForm;
        public ghichu(fchinh parent)
        {
            InitializeComponent();
            loadingTable();
            mainForm = parent;
        }

        const string connectionString = "mongodb://localhost:27017";
        private void loadingTable()
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("ghichu");
            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument()).ToList();

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Tên lỗi VP");

            dt.Columns.Add("1");
            dt.Columns.Add("2");


            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["name"];
                dr[1] = doc["1"];

                dr[2] = doc["2"];

                dt.Rows.Add(dr);
            }
            BindingSource bs = new BindingSource(); // introduce a BindingSource
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].Width = 100;
            this.dataGridView1.Columns[1].Width = 100;
            this.dataGridView1.Columns[2].Width = 100;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("ghichu");
            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument() { { "name", textBox1.Text.ToString() } }).ToList();

            foreach (var doc in documents)
            {

                var update = Builders<BsonDocument>.Update.Set(comboBox1.SelectedItem.ToString(), textBox2.Text.ToString());
                var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }
            loadingTable();
            mainForm.loading();
        }
    }
}
