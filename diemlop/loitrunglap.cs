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
    public partial class loitrunglap : Form
    {
        const string connectionString = "mongodb://localhost:27017";
        fchinh mainForm;
        string df = "";
        string dk = "maiindb";
        public loitrunglap(fchinh parent, string dg)
        {
            mainForm = parent;
            InitializeComponent();
            if (df != "")
            {
                loadingTable(df);
            }
            dk = dg;
            loadingCombobox();
            loadingCombobox1();
        }
        private void loading()
        {
            loadingTable(df);
            mainForm.loading();
        }
        private void loadingTable(string dd)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("timedb");
            var collection = database.GetCollection<BsonDocument>(dd);
            var documents = collection.Find(new BsonDocument()).ToList();
            var database1 = client.GetDatabase("loivp");
            var collection1 = database1.GetCollection<BsonDocument>("loiVP");
            var documentsloi = collection1.Find(new BsonDocument()).ToList();

            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("Tên");
            dt.Columns.Add("Lớp");
            dt.Columns.Add("Ghi chú");

            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["Name"];
                dr[1] = doc["Class"];
                int s = 0;
                foreach (var doc1 in documentsloi)
                {
                    try
                    {

                        if (int.Parse(doc[doc1["name"].ToString()].ToString()) >= 2)
                        {

                            dr[2] += doc1["name"].ToString() + " (" + doc[doc1["name"].ToString()] + "), ";
                            s++;




                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                if (s > 0) dt.Rows.Add(dr);
            }
            BindingSource bs = new BindingSource(); // introduce a BindingSource
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].Width = 100;
            this.dataGridView1.Columns[1].Width = 100;
            this.dataGridView1.Columns[2].Width = 100;
        }
        private void loadingCombobox()
        {
            comboBox1.Items.Clear();
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("timedb");
            foreach (var item in database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result)
            {
                ComboboxItem items = new ComboboxItem();
                items.Text = item["name"].ToString();
                comboBox1.Items.Add(items);

            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            df = (comboBox1.SelectedItem as ComboboxItem).Text.ToString();
            loadingTable(df);
        }
        public void loadingCombobox1()
        {
            comboBox2.Items.Clear();

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivp");

            var collection = database.GetCollection<BsonDocument>("loiVP");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToInt64();

                comboBox2.Items.Add(item);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(dk);

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            var documents = collection.Find(new BsonDocument() { { "Name", textBox1.Text.ToString() }, { "Class", textBox2.Text.ToString() } }).ToList();
            foreach (var doc in documents)
            {

                string tm = doc[(comboBox2.SelectedItem as ComboboxItem).Text.ToString().ToString()].ToString();
                int tm1 = int.Parse(tm); var update = Builders<BsonDocument>.Update.Set((comboBox2.SelectedItem as ComboboxItem).Text.ToString().ToString(), tm1 - int.Parse(textBox4.Text));
                var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }
            var database1 = client.GetDatabase("timedb");

            var collection1 = database1.GetCollection<BsonDocument>(df);
            var documents1 = collection1.Find(new BsonDocument() { { "Name", textBox1.Text.ToString() }, { "Class", textBox2.Text.ToString() } }).ToList();
            foreach (var doc in documents1)
            {
                string tm = doc[(comboBox2.SelectedItem as ComboboxItem).Text.ToString().ToString()].ToString();
                int tm1 = int.Parse(tm);
                var update = Builders<BsonDocument>.Update.Set((comboBox2.SelectedItem as ComboboxItem).Text.ToString().ToString(), tm1 - int.Parse(textBox4.Text));
                var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection1.UpdateOne(filter, update, options);
            }
            loading();
        }
    }
}
