using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SLSInvoicer.Contracts;
using SLSInvoicer.Extractors;
using Spire.Pdf;

namespace SLSInvoicer
{
    public partial class Form1 : Form
    {
        private const string Path = @"c:\temp\nalinipdftemp\new\test\";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] fileEntries = Directory.GetFiles(Path);
            var customerLIst = new List<Customer>();

            foreach (string fileName in fileEntries.Where(x => x.EndsWith(".pdf")))
            {
                customerLIst.Add(ExstractData(fileName));
            }
            WriteObjectToFile(customerLIst);
        }

        public void WriteObjectToFile(List<Customer> listCustomers)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            
            using(StreamWriter sw = new StreamWriter(Path+"customers.txt"))
            using(JsonWriter writer = new JsonTextWriter(sw))
            {
                    serializer.Serialize(writer, listCustomers);
            }
        }

        public Customer ExstractData(string fileName)
        {

            var pdfText = GetTextFromPdf(fileName);

            var customerName = CustomerExtractor.GetCustomerName(pdfText);
            var bestelnummer = BestelnummerExtractor.GetBestelnummer(pdfText);
            var postalCode = PostalCodeExtractor.GetPostalCode(pdfText);
            var city = CityExtractor.GetCity(pdfText);

            PutInListBox($"{customerName}\t{fileName}\t {bestelnummer}\t {postalCode}\t City:{city}");

            var customer = new Customer()
            {
                UserName = customerName,
                City = city,
                PostalCode = postalCode,
                BestelNummer = bestelnummer
            };
            return customer;
        }

        public void RenameFiles(string fileName)
        {
            var pdfText = GetTextFromPdf(fileName);
            var bestelnummer = BestelnummerExtractor.GetBestelnummer(pdfText);
            RenameFile(fileName, bestelnummer);
        }

        public void WriteTextToFile(string fileName, string pdfText)
        {
            //save text
            File.WriteAllText(fileName + ".txt", pdfText);
        }

        public void PutInListBox(string bestelnummer)
        {
            listBox1.Items.Add(bestelnummer);
        }

        public void RenameFile(string originalName, string bestelNummer)
        {
            string writeFileName = originalName.Replace(".pdf", " " + bestelNummer);
            File.Move(originalName, writeFileName + ".pdf");
        }

        public string GetTextFromPdf(string fileName)
        {
            //Create a pdf document.
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(fileName);

            StringBuilder buffer = new StringBuilder();

            foreach (PdfPageBase page in doc.Pages)
            {
                buffer.Append(page.ExtractText());
            }

            doc.Close();

            return buffer.ToString();
        }

        // rename files button
        private void button2_Click(object sender, EventArgs e)
        {
            string[] fileEntries = Directory.GetFiles(Path);

            foreach (string fileName in fileEntries.Where(x => x.EndsWith(".pdf")))
            {
                var pdfText = GetTextFromPdf(fileName);
                var bestelnummer = BestelnummerExtractor.GetBestelnummer(pdfText);
                RenameFile(fileName, bestelnummer);
            }
        }

        // write text content of pdf to file.
        private void button3_Click(object sender, EventArgs e)
        {
            string[] fileEntries = Directory.GetFiles(Path);

            foreach (string fileName in fileEntries.Where(x => x.EndsWith(".pdf")))
            {
                var pdfText = GetTextFromPdf(fileName);
                WriteTextToFile(fileName, pdfText);
            }
        }
    }
}
