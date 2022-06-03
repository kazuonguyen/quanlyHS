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
    public partial class quanlyhs : Form
    {
        const string connectionString = "mongodb://localhost:27017";

        string df = "maiindb";
        fchinh mainForm;
        public quanlyhs(string kk, fchinh parent)
        {
            df = kk;
            InitializeComponent();
            loadingTable();
            loadingCblop(df);
            loadingCombobox2();
            loadingCombobox();
            mainForm = parent;
        }
        private void loadingCombobox()
        {
            comboBox1.Items.Clear();

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivp");

            var collection = database.GetCollection<BsonDocument>("loiVP");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToInt64();

                comboBox1.Items.Add(item);
            }

        }
        private void loadingCblop(string db)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(db);
            var collection = database.GetCollection<BsonDocument>("diemlop");
            var documents = collection.Find(new BsonDocument()).ToList();
            textBox3.Items.Clear();
            foreach (var doc in documents)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["name"].ToString();
                textBox3.Items.Add(item);

            }

        }
        private void loadingCombobox2()
        {
            chonTuan2.Items.Clear();

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("selectWeeks");

            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem2 item = new ComboboxItem2();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToString();

                chonTuan2.Items.Add(item);


            }


        }
        public void loading()
        {
            loadingTable();
            mainForm.loading();
        }
        private void loadingTable()
        {
            textBox2.Items.Clear();


            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);
            var database1 = client.GetDatabase("loivp");

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            var collection1 = database1.GetCollection<BsonDocument>("loiVP");

            var documents = collection.Find(new BsonDocument()).ToList();
            var documents1 = collection1.Find(new BsonDocument()).ToList();
            var documentsHS = collection.Find(new BsonDocument()).ToList();

            foreach (var doc in documentsHS)
            {
                int s = 0;
                foreach (var docloiVP in documents1)
                {
                    string tm = docloiVP["name"].ToString();
                    s += doc[tm].ToInt32() * docloiVP["value"].ToInt32();
                }
                var update = Builders<BsonDocument>.Update.Set("Score", s);
                var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                /*add item hs in cb*/
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["Name"].ToString();
                textBox2.Items.Add(item);

                /*--------------------*/
                collection.UpdateOne(filter, update, options);
            }
            collection = database.GetCollection<BsonDocument>("hocsinh");
            documents = collection.Find(new BsonDocument()).ToList();
            documents1 = collection1.Find(new BsonDocument()).ToList();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Tên");
            dt.Columns.Add("Lớp");
            dt.Columns.Add("Điểm");
            dt.Columns.Add("Ghi chú");

            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["Name"];
                dr[1] = doc["Class"];
                dr[2] = doc["Score"];
                foreach (var doc1 in documents1)
                {
                    if (doc[doc1["name"].ToString()] != 0)
                        dr[3] += doc1["name"].ToString() + " (" + doc[doc1["name"].ToString()] + "), ";
                }
                dt.Rows.Add(dr);
            }
            BindingSource bs = new BindingSource(); // introduce a BindingSource
            bs.DataSource = dt;
            dataGridView1.DataSource = bs;
            this.dataGridView1.Columns[0].Width = 100;
            this.dataGridView1.Columns[1].Width = 100;
            this.dataGridView1.Columns[2].Width = 50;
            this.dataGridView1.Columns[3].Width = 150;

        }

        private void button6_Click(object sender, EventArgs e)
        {
          //  fxoahs f = new fxoahs(df, this);
         //   f.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // fchinhsuahs f = new fchinhsuahs(df, this);
          //  f.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chonTuan2.SelectedIndex == -1 || comboBox1.SelectedIndex == -1)
            {
                string message = "Bạn chưa nhập đày đủ";
                string title = "Cảnh báo";
                MessageBox.Show(message, title);
            }
            else
            {
                var document = new BsonDocument
            {

                {"Name",textBox2.Text.ToString()},
            { "Class",textBox3.Text.ToString()},
            { "Score",     (comboBox1.SelectedItem as ComboboxItem).Value.ToString()}
            };

                var client = new MongoClient(connectionString);

                var database = client.GetDatabase((chonTuan2.SelectedItem as ComboboxItem2).Value.ToString());
                var database1 = client.GetDatabase("loivp");

                var collection = database.GetCollection<BsonDocument>("hocsinh");

                var collection1 = database1.GetCollection<BsonDocument>("loiVP");
                var documents1 = collection1.Find(new BsonDocument()).ToList();
                var database2 = client.GetDatabase("timedb");
                var collection2 = database2.GetCollection<BsonDocument>(textBox7.Text);
                foreach (var doc in documents1)
                {
                    if (doc["name"].ToString() == (comboBox1.SelectedItem as ComboboxItem).Text.ToString())
                        document.Add(doc["name"].ToString(), 1);
                    else document.Add(doc["name"].ToString(), 0);
                }
                var documents = collection.Find(new BsonDocument() { { "Name", textBox2.Text.ToString() }, { "Class", textBox3.Text.ToString() } }).ToList();
                var documents2 = collection2.Find(new BsonDocument() { { "Name", textBox2.Text.ToString() }, { "Class", textBox3.Text.ToString() } }).ToList();
                if (documents.Count == 0) collection.InsertOne(document);
                if (documents2.Count == 0) collection2.InsertOne(document);
                else
                {
                    foreach (var doc in documents)
                    {
                        var update = Builders<BsonDocument>.Update.Set((comboBox1.SelectedItem as ComboboxItem).Text.ToString().ToString(), int.Parse(doc[(comboBox1.SelectedItem as ComboboxItem).Text.ToString()].ToString()) + 1);
                        var filter = Builders<BsonDocument>.Filter.Eq("Name", textBox2.Text.ToString());
                        var options = new UpdateOptions { IsUpsert = true };
                        collection.UpdateOne(filter, update, options);
                    }
                    foreach (var doc in documents2)
                    {
                        var update = Builders<BsonDocument>.Update.Set((comboBox1.SelectedItem as ComboboxItem).Text.ToString().ToString(), int.Parse(doc[(comboBox1.SelectedItem as ComboboxItem).Text.ToString()].ToString()) + 1);
                        var filter = Builders<BsonDocument>.Filter.Eq("Name", textBox2.Text.ToString());
                        var options = new UpdateOptions { IsUpsert = true };
                        collection2.UpdateOne(filter, update, options);
                    }
                }
                var documentsHS = collection.Find(new BsonDocument()).ToList();

                foreach (var doc in documentsHS)
                {
                    int s = 0;
                    foreach (var docloiVP in documents1)
                    {
                        string tm = docloiVP["name"].ToString();
                        s += doc[tm].ToInt32() * docloiVP["value"].ToInt32();
                    }
                    var update = Builders<BsonDocument>.Update.Set("Score", s);
                    var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collection.UpdateOne(filter, update, options);
                }
                loading();
            }
        }
        private void loadingTable1(string lop, string db)
        {
            if (lop == "") loadingTable();
            else
            {
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase(db);

                var collection = database.GetCollection<BsonDocument>("hocsinh");
                var documents = collection.Find(new BsonDocument() { { "Class", lop } }).ToList();
                var database1 = client.GetDatabase("loivp");

                var collection1 = database1.GetCollection<BsonDocument>("loiVP");

                var documents1 = collection1.Find(new BsonDocument()).ToList();

                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Tên");
                dt.Columns.Add("Lớp");
                dt.Columns.Add("Điểm");
                dt.Columns.Add("Ghi chú");
                foreach (var doc in documents)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = doc["Name"];
                    dr[1] = doc["Class"];
                    dr[2] = doc["Score"];
                    foreach (var doc1 in documents1)
                    {
                        if (doc[doc1["name"].ToString()] != 0)
                            dr[3] += doc1["name"].ToString() + " (" + doc[doc1["name"].ToString()] + "), ";
                    }
                    dt.Rows.Add(dr);
                }

                BindingSource bs = new BindingSource(); // introduce a BindingSource
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                this.dataGridView1.Columns[0].Width = 100;
                this.dataGridView1.Columns[1].Width = 100;
                this.dataGridView1.Columns[2].Width = 50;
                this.dataGridView1.Columns[3].Width = 150;

            }

        }
        private void button5_Click(object sender, EventArgs e)
        {
            loadingTable1(textBox1.Text.ToString(), df);

        }

    }
}
