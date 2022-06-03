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
using Microsoft.Office.Interop.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace diemlop
{
    public partial class fchinh : Form
    {
        const string connectionString = "mongodb://localhost:27017";
        string df = "maiindb";

        public fchinh()
        {
            InitializeComponent();

            loadingTable(df);
            loadingCombobox();
            loadingCombobox2();
        }
        public void loading()
        {
            loadingTable(df);
        }
        public void loadingCB()
        {
            loadingCombobox2();
        }

        private void loadingTable(string db)
        {

            label10.Text = "Loading...";
            if (label10.Text == "Loading...")
            {
                var client = new MongoClient(connectionString);

                var database = client.GetDatabase(db);
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

                    /*--------------------*/
                    collection.UpdateOne(filter, update, options);
                }
                collection = database.GetCollection<BsonDocument>("hocsinh");
                documents = collection.Find(new BsonDocument()).ToList();
                documents1 = collection1.Find(new BsonDocument()).ToList();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Tên");
                dt.Columns.Add("Class");
                dt.Columns.Add("Score");
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
                            dr[3] += doc1["name"].ToString() + " (" + doc[doc1["name"].ToString()] + "), " + ghichu(doc1["name"].ToString(), doc[doc1["name"].ToString()].ToString()) + ", ";
                    }
                    dt.Rows.Add(dr);
                }
                BindingSource bs = new BindingSource(); // introduce a BindingSource
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                this.dataGridView1.Columns[0].Width = 150;
                this.dataGridView1.Columns[1].Width = 100;
                this.dataGridView1.Columns[2].Width = 50;
                this.dataGridView1.Columns[3].Width = 300;

                label10.Text = " ";

            }
        }
        private string ghichu(string s, string n)
        {
            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("ghichu");
            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument() { { "name", s } }).ToList();
            var ss = "";
            foreach (var doc in documents)
            {
                try
                {
                    ss = doc[n].ToString();
                }
                catch
                {
                    continue;
                }
            }
            return ss;
        }
        private void loadingTable1(string lop, string db)
        {
            if (lop == "") loadingTable(df);
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
                dt.Columns.Add("Class");
                dt.Columns.Add("Score");
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
        public void loadingCombobox()
        {

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("loivp");

            var collection = database.GetCollection<BsonDocument>("loiVP");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToInt64();

            }

        }
        private void loadingCombobox2()
        {
            chonTuan.Items.Clear();

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("selectWeeks");

            var collection = database.GetCollection<BsonDocument>("indexx");
            var documents = collection.Find(new BsonDocument()).ToList();
            foreach (var doc in documents)
            {
                ComboboxItem2 item = new ComboboxItem2();
                item.Text = doc["name"].ToString();
                item.Value = doc["value"].ToString();

                chonTuan.Items.Add(item);


            }
            chonTuan.SelectedIndex = 0;


        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            flop fl = new flop();
            fl.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            diemct d = new diemct(df);
            d.ShowDialog();

        }


        private void button5_Click(object sender, EventArgs e)
        {
            df = (chonTuan.SelectedItem as ComboboxItem2).Value.ToString();
            loadingTable1(textBox1.Text.ToString(), df);

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }



        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void chonTuan_SelectedIndexChanged(object sender, EventArgs e)
        {
            df = (chonTuan.SelectedItem as ComboboxItem2).Value.ToString();
            loadingTable(df);
            loadingCombobox();
        }

        private void button3_Click_1(object sender, EventArgs e)
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


        private void button7_Click(object sender, EventArgs e)
        {
            flop fl = new flop();
            fl.ShowDialog();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            testform f = new testform(df);
            f.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);
            var database1 = client.GetDatabase("selectWeeks");
            var collection1 = database1.GetCollection<BsonDocument>("indexx");
            var documents = collection1.Find(new BsonDocument()).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                s++;
            }
            fthemtuan f = new fthemtuan(this, s);
            f.ShowDialog();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void fchinh_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void textBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void sửaTênHSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void xóaHọcSinhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thêmTuầnToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var client = new MongoClient(connectionString);
            var database1 = client.GetDatabase("selectWeeks");
            var collection1 = database1.GetCollection<BsonDocument>("indexx");
            var documents = collection1.Find(new BsonDocument()).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                s++;
            }
            fthemtuan f = new fthemtuan(this, s);
            f.ShowDialog();
        }

        private void xuấtFileExcelToolStripMenuItem_Click(object sender, EventArgs e)
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
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
        }



        private void quảnLýLỗiVPToolStripMenuItem_Click(object sender, EventArgs e)
        {
           quanlyloiVP f = new quanlyloiVP(this);
           f.ShowDialog();
        }

        private void xuấtFilePdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;
                string ARIALUNI_TFF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "C:/Users/carzy/Downloads/ARIALUNI.TTF");

                //Create a base font object making sure to specify IDENTITY-H
                BaseFont bf = BaseFont.CreateFont(ARIALUNI_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                //Create a specific font object
                iTextSharp.text.Font f = new iTextSharp.text.Font(bf, 12);
                iTextSharp.text.Font fb = new iTextSharp.text.Font(bf, 24);
                fb.SetStyle(0);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            //     pdfTable.HorizontalAlignment = Element.ALIGN_CENTER; //cc
                            pdfTable.HorizontalAlignment = 1;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;

                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(new Phrase(cell.Value.ToString(), f));
                                }
                            }
                            iTextSharp.text.Paragraph kk = new iTextSharp.text.Paragraph("DANH SÁCH VI PHẠM", fb);
                            iTextSharp.text.Paragraph cach = new iTextSharp.text.Paragraph("      ");

                            iTextSharp.text.Paragraph tuan = new iTextSharp.text.Paragraph((chonTuan.SelectedItem as ComboboxItem2).Text.ToString(), f);
                            tuan.Alignment = Element.ALIGN_CENTER;
                            kk.Alignment = Element.ALIGN_CENTER;
                            f.SetStyle(0);
                            iTextSharp.text.Paragraph ghichu = new iTextSharp.text.Paragraph("Ghi chú: Mọi thắc mắc GVCN vui lòng gửi về địa chỉ email: doantruong.pnh1234@gmail.com trước 11h20 thứ hai của tuần tiếp theo.", f);
                            ghichu.Alignment = Element.ALIGN_LEFT;
                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(kk);
                                pdfDoc.Add(tuan);
                                pdfDoc.Add(cach);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Add(cach);
                                pdfDoc.Add(ghichu);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void tìmLỗiTrùngLặpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loitrunglap f = new loitrunglap(this, df);
            f.ShowDialog();
        }

        private void quảnLýHSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quanlyhs f = new quanlyhs(df, this);
            f.ShowDialog();
        }

        private void nhắcNhởToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ghichu f = new ghichu(this);
           f.ShowDialog();

        }

        private void ttToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connectionString);
            var database1 = client.GetDatabase("selectWeeks");
            var collection1 = database1.GetCollection<BsonDocument>("indexx");
            var documents = collection1.Find(new BsonDocument()).ToList();
            int s = 0;
            foreach (var doc in documents)
            {
                s++;
            }
        }

        private void nhắcNhởToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void chỉnhSửaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void điểmLớpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            diemct f = new diemct(df);
            f.ShowDialog();
        }
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public Int64 Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
    public class ComboboxItem2
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
