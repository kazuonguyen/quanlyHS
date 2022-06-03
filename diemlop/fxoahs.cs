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
    public partial class fxoahs : Form
    {
        string df = "maiindb";
        const string connectionString = "mongodb://localhost:27017";
        fchinh mainForm;

        public fxoahs(string db, fchinh parent)
        {
            df = db;
            InitializeComponent();
            mainForm = parent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase(df);

            var collection = database.GetCollection<BsonDocument>("hocsinh");
            var documents = collection.Find(new BsonDocument() { { "id", textBox1.Text.ToString() } }).ToList();
            var deleteFilter = Builders<BsonDocument>.Filter.Eq("id", textBox1.Text.ToString());
            collection.DeleteOne(deleteFilter);

            mainForm.loading();

        }
    }
}
