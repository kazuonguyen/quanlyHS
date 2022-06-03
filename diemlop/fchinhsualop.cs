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
    public partial class fchinhsualop : Form
    {
        string df = "maiindb";
        const string connectionString = "mongodb://localhost:27017";
        diemct mainForm;

        public fchinhsualop(string db, diemct parent)
        {
            df = db;
            InitializeComponent();
            loadingComboBox();
            mainForm = parent;
        }
        private void loadingComboBox()
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
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);
            var database2 = client.GetDatabase("loivplop");
            var collection1 = database.GetCollection<BsonDocument>("tmdiemlop");
            var collection2 = database2.GetCollection<BsonDocument>("loiVPLOP");

            var documentsloiVP = collection2.Find(new BsonDocument()).ToList();
            var documentsHS = collection1.Find(new BsonDocument()).ToList();
            foreach (var doc in documentsHS)
            {
                foreach (var docloiVP in documentsloiVP)
                {
                    var update = Builders<BsonDocument>.Update.Set(docloiVP["name"].ToString(), 0);
                    var filter = Builders<BsonDocument>.Filter.Eq("name", doc["name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collection1.UpdateOne(filter, update, options);
                }
            }
            mainForm.loading();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
