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
    public partial class flop : Form
    {
        const string connectionString = "mongodb://localhost:27017";

        public flop()
        {
            InitializeComponent();
            loadingTable();
            loadingCombobox();
        }
        private void loadingTable()
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("maiindb");

            var collection = database.GetCollection<BsonDocument>("diemlop");
            var documents = collection.Find(new BsonDocument()).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["name"];
                dr[1] = doc["value"];
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;
        }
        private void loadingCombobox()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("selectWeeks");

            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem2 item = new ComboboxItem2();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToString();

                comboBox1.Items.Add(item);
                comboBox2.Items.Add(item);

            }
        }
        private void diem()
        {
            
        }
        private int diemlop(string name, string db)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(db);

            var collection = database.GetCollection<BsonDocument>("diemlop");
            var documents = collection.Find(new BsonDocument() { { "name", name } }).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                s += int.Parse(doc["value"].ToString());
            }
            return s;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            List<string> tuan = new List<string>();
            for (int i = comboBox1.SelectedIndex; i <= comboBox2.SelectedIndex; i++) tuan.Add((comboBox1.Items[i] as ComboboxItem2).Value.ToString());
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("manyClass");
            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                int s = 0;
                foreach (var tu in tuan)
                {
                    s += diemlop(doc["name"].ToString(), tu);
                }
                var update = Builders<BsonDocument>.Update.Set("value", s);
                var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }
            documents = collection.Find(new BsonDocument()).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Score");
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
            this.dataGridView1.Columns[0].Width = 200;
            this.dataGridView1.Columns[1].Width = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("maiindb");

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            
                    var update = Builders<BsonDocument>.Update.Set("Lmao", 20);
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    var options = new UpdateOptions { IsUpsert = true };
                    collection.UpdateMany(filter, update, options);
                
            

        }
    }
}
