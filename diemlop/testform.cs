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
    public partial class testform : Form
    {
        public testform(string db)
        {
            InitializeComponent();
            label1.Text = db;
        }
        const string connectionString = "mongodb://localhost:27017";

        private void button1_Click(object sender, EventArgs e) 
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("tuan2");

            var collection1 = database.GetCollection<BsonDocument>("hocsinh");
            var collection2 = database.GetCollection<BsonDocument>("loiVP");

            var documentsloiVP = collection2.Find(new BsonDocument()).ToList();
            var documentsHS = collection1.Find(new BsonDocument()).ToList();
            foreach (var doc in documentsHS)
            {
                foreach(var docloiVP in documentsloiVP)
                {
                    var update = Builders<BsonDocument>.Update.Set(docloiVP["name"].ToString(), 0);
                    var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collection1.UpdateOne(filter, update, options);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("tuan2");
            var collection1 = database.GetCollection<BsonDocument>("hocsinh");
            var collection2 = database.GetCollection<BsonDocument>("loiVP");

            var documentsloiVP = collection2.Find(new BsonDocument()).ToList();
            var documentsHS = collection1.Find(new BsonDocument()).ToList();
            foreach (var doc in documentsHS)
            {
                int s = 0;
                foreach (var docloiVP in documentsloiVP)
                {
                    string tm = docloiVP["name"].ToString();
                    s += doc[tm].ToInt32()*docloiVP["value"].ToInt32();
                }
                var update = Builders<BsonDocument>.Update.Set("Score", s);
                var filter = Builders<BsonDocument>.Filter.Eq("Name", doc["Name"].ToString());
                    var options = new UpdateOptions { IsUpsert = true };
                    collection1.UpdateOne(filter, update, options);
            }
        }
    }
}
