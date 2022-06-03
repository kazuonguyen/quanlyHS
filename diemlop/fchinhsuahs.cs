using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Windows.Forms;
namespace diemlop
{
    public partial class fchinhsuahs : Form
    {
        string df = "maiindb";
        const string connectionString = "mongodb://localhost:27017";
        fchinh mainForm;
        public fchinhsuahs(string db, fchinh parent)
        {
            df = db;
            InitializeComponent();
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
        private void button1_Click(object sender, EventArgs e)
        {

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            var documents = collection.Find(new BsonDocument() { { "id", textBox1.Text.ToString() } }).ToList();
            foreach (var doc in documents)
            {
                var update = Builders<BsonDocument>.Update.Set((comboBox1.SelectedItem as ComboboxItem).Text.ToString().ToString(), int.Parse(textBox2.Text));
                var filter = Builders<BsonDocument>.Filter.Eq("id", doc["id"].ToString());
                var options = new UpdateOptions { IsUpsert = true };
                collection.UpdateOne(filter, update, options);
            }


            mainForm.loading();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
