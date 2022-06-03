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

    public partial class fthemtuan : Form
    {
        const string connectionString = "mongodb://localhost:27017";

        fchinh mainForm;
        int kk;
        public fthemtuan(fchinh parent, int sotuan)
        {
            InitializeComponent();
            mainForm = parent;
            kk = sotuan;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("tuan" + kk.ToString());

            var database1 = client.GetDatabase("loivplop");

            var collection1 = database1.GetCollection<BsonDocument>("loiVPLOP");

            var documents = collection1.Find(new BsonDocument()).ToList();
            var collection = database.GetCollection<BsonDocument>("tmdiemlop");
            var collection3 = database.GetCollection<BsonDocument>("diemlop");
            string[] arr = { "11TT", "12C1", "11CV", "11CA", "12CL" };
            for (int i = 0; i < 5; i++)
            {
                var document = new BsonDocument
            {
                {"name",arr[i]},
            { "value", 0 }

            };
                collection.InsertOne(document);
                collection3.InsertOne(document);
            }

            collection = database.GetCollection<BsonDocument>("tmdiemlop");
            foreach (var doc in documents)
            {
                var collectionHS = database.GetCollection<BsonDocument>("tmdiemlop");
                var documentsHS = collectionHS.Find(new BsonDocument()).ToList();
                foreach (var docHS in documentsHS)
                {
                    var update = Builders<BsonDocument>.Update.Set(doc["name"].ToString(), 0);
                    var filter = Builders<BsonDocument>.Filter.Eq("name", docHS["name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collectionHS.UpdateOne(filter, update, options);
                }



            }
            var document1 = new BsonDocument
            {
                { "name",textBox1.Text},
                {"value","tuan"+kk.ToString() }
            };


            var database2 = client.GetDatabase("selectWeeks");

            var collection2 = database2.GetCollection<BsonDocument>("indexx");
            collection2.InsertOne(document1);
            mainForm.loadingCB();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
