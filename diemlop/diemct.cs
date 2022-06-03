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
    public partial class diemct : Form
    {
        const string connectionString = "mongodb://localhost:27017";
        string df = "maiindb";
        public diemct(string db)
        {
            df = db;
            InitializeComponent();
            loadingTable(df);
            loadingCombobox();
        }
        public void loading()
        {
            loadingTable(df);
        }
        private void capnhatdiem() {

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);
            var database1 = client.GetDatabase("loivplop");

            var collection = database.GetCollection<BsonDocument>("tmdiemlop");
            var collection1 = database1.GetCollection<BsonDocument>("loiVPLOP");

            var documents = collection.Find(new BsonDocument()).ToList();
            var documents1 = collection1.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                int s = 0;
                foreach (var docloiVP in documents1)
                {
                    string tm = docloiVP["name"].ToString();
                    s += doc[tm].ToInt32() * docloiVP["value"].ToInt32();
                }
                var update = Builders<BsonDocument>.Update.Set("value", s);
                var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }
        }
        private void loadingTable(string db)
        {
            capnhatdiem();
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(db);


            var collection = database.GetCollection<BsonDocument>("diemlop");
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var doc in documents)
            {
                var update = Builders<BsonDocument>.Update.Set("value", congdiem(doc["name"].ToString()) + diemlop(doc["name"].ToString()));
                var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"]);
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }
            DataTable dt = new DataTable();

            documents = collection.Find(new BsonDocument()).ToList();
            dt.Columns.Add("Lớp");
            dt.Columns.Add("Điểm");
            foreach (var doc in documents)
            {
                DataRow dr = dt.NewRow();
                dr[0] = doc["name"];
                dr[1] = doc["value"];
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;
            this.dataGridView1.Columns[0].Width = 200;
            this.dataGridView1.Columns[1].Width = 200;
        }
        private int congdiem(string condit)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            var documents = collection.Find(new BsonDocument() { { "Class", condit } }).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                string tm = doc["Score"].ToString();
                s = s + int.Parse(tm);

            }
            return s;
        }
        private int diemlop(string name)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);

            var collection = database.GetCollection<BsonDocument>("tmdiemlop");
            var documents = collection.Find(new BsonDocument() { { "name", name } }).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                s += int.Parse(doc["value"].ToString());
            }
            return s;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void loadingCombobox()
        {
            comboBox1.Items.Clear();
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivplop");

            var collection = database.GetCollection<BsonDocument>("loiVPLOP");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToInt64();

                comboBox1.Items.Add(item);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == ""||comboBox1.SelectedIndex==-1)
            {

                string message = "Bạn chưa nhập đày đủ";
                string title = "Cảnh báo";
                MessageBox.Show(message, title);
            }
            else
            {
                var document = new BsonDocument
            {
                {"name",textBox1.Text.ToString()},
            { "value",     (comboBox1.SelectedItem as ComboboxItem).Value.ToString()}
            };
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase(df);

                var collection = database.GetCollection<BsonDocument>("tmdiemlop");
                var documents = collection.Find(new BsonDocument() { { "name", textBox1.Text.ToString() } }).ToList();


                foreach (var doc in documents)
                {
                    var update = Builders<BsonDocument>.Update.Set((comboBox1.SelectedItem as ComboboxItem).Text.ToString(), int.Parse(doc[(comboBox1.SelectedItem as ComboboxItem).Text.ToString()].ToString()) + 1);
                    var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collection.UpdateOne(filter, update, options);
                }


                loadingTable(df);
            }
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

                var database = client.GetDatabase("loivplop");

                var collection = database.GetCollection<BsonDocument>("loiVPLOP");
                var database1 = client.GetDatabase("selectWeeks");
                var collection1 = database1.GetCollection<BsonDocument>("indexx");
                var documents = collection1.Find(new BsonDocument()).ToList();
                foreach (var doc in documents)
                {
                    var databaseHS = client.GetDatabase(doc["value"].ToString());
                    var collectionHS = databaseHS.GetCollection<BsonDocument>("tmdiemlop");
                    var documentsHS = collectionHS.Find(new BsonDocument()).ToList();
                    foreach (var docHS in documentsHS)
                    {
                        var update = Builders<BsonDocument>.Update.Set(textBox4.Text.ToString(), 0);
                        var filter = Builders<BsonDocument>.Filter.Eq("name", docHS["name"].ToString());
                        var options = new UpdateOptions { IsUpsert = true };
                        collectionHS.UpdateOne(filter, update, options);
                    }

                }
                collection.InsertOne(document);

                loadingCombobox();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application  
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook  
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            // see the excel sheet behind the program  
            app.Visible = true;
            // get the reference of first sheet. By default its name is Sheet1.  
            // store its reference to worksheet  
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            // changing the name of active sheet  
            worksheet.Name = "Exported from gridview";
            // storing header part in Excel  
            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet  
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fchinhsualop f = new fchinhsualop(df,this);
            f.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
