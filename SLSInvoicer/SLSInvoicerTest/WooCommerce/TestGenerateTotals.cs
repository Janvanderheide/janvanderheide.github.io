using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v1;
using Order = WooCommerceNET.WooCommerce.v2.Order;
using WCObject = WooCommerceNET.WooCommerce.v2.WCObject;

namespace SLSInvoicerTest.WooCommerce
{

    public class TestGenerateTotals
    {
        [Test]
        public async Task Test()
        {
            // from date will fetch everything from woocommerce from this date:


            // Juli
            //var fromDate = new DateTime(2018, 7, 1);
            //var toDate = new DateTime(2018, 7, 31, 23, 59, 59);
            //var month = "Juli";

            // aug
            var fromDate = new DateTime(2018, 8, 1, 00, 00, 00);
            var toDate = new DateTime(2018, 8, 31, 23, 59, 59);
            var month = "Augustus";

            // Mei
            //var fromDate = new DateTime(2018, 9, 1);
            //var toDate = new DateTime(2018, 9, 30, 23, 59, 59);
            //var month = "September";


            // moeder aarde woo commerce getting only
            string wooClientIdentifier = "ck_30f80961c3194f7a8f65803aa6d9c39948552d38";
            string wooSecret = "cs_d7ec3d6d6d49dda0c0ffa86caf315c3e85937fa8";
            string wooUrl = "https://moederaarde.nl";


            //http://www.example.com/wp-json/wc/v2/
            var rest = new RestAPI("https://moederaarde.nl/wp-json/wc/v2/", wooClientIdentifier, wooSecret);
            var wc = new WCObject(rest);

            var orders = await wc.Order.GetAll(new Dictionary<string, string>() {
                { "after",  fromDate.ToString("o")}, 
                { "before", toDate.ToString("o")},
                { "per_page", "100"},
                { "page", "1"}
            });


            MakeTotalDocument(orders, month);

            var manualOrders = orders.Where(x => x.payment_method_title != "iDEAL" || int.Parse(x.number) == 1280).ToList();
            MakeListManualOrders(manualOrders, month);

            orders = orders.Where(x => x.payment_method_title == "iDEAL" && int.Parse(x.number) != 1280).ToList();// filter only the paid orders.
            ProcessOders(orders, month);


        }

        private void MakeTotalDocument(List<Order> orders, string month)
        {
            using (var file = new System.IO.StreamWriter($@"C:\temp\moederaarde\{month}Totaal.txt"))
            {
                file.WriteLine("------Totaal overzicht van alle verkochte items--------");
                var totalOmzet = orders.Sum(x => x.total);

                var totalTax = orders.Sum(x => x.total_tax);
                var totalOrders = orders.Count;

                Assert.IsTrue(totalOrders < 100);// if there are more you need to have paging. now it only gets 100 sales items.
                file.WriteLine($"Total orders is:{totalOrders}");
                file.WriteLine($"Total Omzet is:{ totalOmzet}");
                file.WriteLine($"Total tax is:{ totalTax}");
                file.WriteLine("--------------------------------");
            }
        }

        private void MakeListManualOrders(List<Order> manualOrders, string month)
        {
            using (var file = new System.IO.StreamWriter($@"C:\temp\moederaarde\{month} Manual list.txt"))
            {
                file.WriteLine("------Totaal overzicht--------");
                var totalOmzet = manualOrders.Sum(x => x.total);

                var totalTax = manualOrders.Sum(x => x.total_tax);
                var totalOrders = manualOrders.Count;

                Assert.IsTrue(totalOrders < 100);// if there are more you need to have paging. now it only gets 100 sales items.
                file.WriteLine($"Waar total orders is:{totalOrders}");
                file.WriteLine($"Waar total Omzet is:{ totalOmzet}");
                file.WriteLine($"Waar total tax is:{ totalTax}");
                file.WriteLine("--------------------------------");

                foreach (var item in manualOrders)
                {
                    file.WriteLine($"Orderid: {item.id}");
                    if (item.meta_data.FirstOrDefault(x => x.key == "_wcpdf_invoice_number") != null)
                    {
                        file.WriteLine($"Factuurnummer:{item.meta_data.FirstOrDefault(x => x.key == "_wcpdf_invoice_number").value}");
                    }
                }
            }


        }

        private void ProcessOders(List<Order> orders, string month)
        {

            using (var file = new System.IO.StreamWriter($@"C:\temp\moederaarde\{month}booklines.txt"))
            {
                file.WriteLine("------Totaal overzicht--------");
                var totalOmzet = orders.Sum(x => x.total);

                var totalTax = orders.Sum(x => x.total_tax);
                var totalOrders = orders.Count;

                Assert.IsTrue(totalOrders < 100);// if there are more you need to have paging. now it only gets 100 sales items.
                file.WriteLine($"Waar total orders is:{totalOrders}");
                file.WriteLine($"Waar total Omzet is:{ totalOmzet}");
                file.WriteLine($"Waar total tax is:{ totalTax}");
                file.WriteLine("--------------------------------");

                // Kraam pakket totals:
                var totalOmzetKraampakket = orders.SelectMany(x => x.line_items)
                    .Where(x => IsKraamPakketProduct(x.product_id)).Sum(x => x.total);
                var totalOmzetKraampakketTax = orders.SelectMany(x => x.line_items)
                    .Where(x => IsKraamPakketProduct(x.product_id)).Sum(x => x.total_tax);

                file.WriteLine("---------kraampakket verkopen----------");
                file.WriteLine("Kraampakket boeking regel");
                file.WriteLine($"Kraam pakket Totaal: {totalOmzetKraampakket} Kraampakket tax low: {totalOmzetKraampakketTax}");
                file.WriteLine("--------------------------------");

                // Bereken btw's rest omzet
                var totalOmzetRestTaxLow = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id))
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "2") //2 is tax low
                    .Sum(x => x.total_tax);

                var totalOmzetRestTaxHigh = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id))
                    .Where(x=> x.taxes.Count > 0) // list should not be empty
                    .Where(x=> x.taxes.First().id == "1") //1 is tax high
                    .Sum(x => x.total_tax);

                var totalOmzetRestLow = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id))
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "2") //2 is tax low
                    .Sum(x => x.total);

                var totalOmzetRestHigh = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id))
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "1") //1 is tax high
                    .Sum(x => x.total);

                file.WriteLine("----------diversen verkopen------------");
                file.WriteLine("Rest omzet high regel:");
                file.WriteLine($"Rest Totaal High: {totalOmzetRestHigh} Omzet Tax High: {totalOmzetRestTaxHigh}");

                file.WriteLine("Rest omzet low regel:");
                file.WriteLine($"Rest omzet  low: {totalOmzetRestLow} Omzet Tax Low: {totalOmzetRestTaxLow}");
                
                // controle regel voor totalen:
                var totalOmzetrest = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id)).Sum(x => x.total);
                var totalOmzetRestTax = orders.SelectMany(x => x.line_items)
                    .Where(x => !IsKraamPakketProduct(x.product_id)).Sum(x => x.total_tax);
                file.WriteLine($"Rest Totaal: {totalOmzetrest} Totaalrest Tax: {totalOmzetRestTax}");
                file.WriteLine("--------------------------------");

                // Bereken btw's rest omzet
                var totalOmzetShippingTaxLow = orders.SelectMany(x => x.shipping_lines)
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "2")
                    .Sum(x => x.total_tax);

                var totalOmzetShippingTaxHigh = orders.SelectMany(x => x.shipping_lines)
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "1")
                    .Sum(x => x.total_tax);

                var totalOmzetShippingLow = orders.SelectMany(x => x.shipping_lines)
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "2")
                    .Sum(x => x.total);

                var totalOmzetShippingHigh = orders.SelectMany(x => x.shipping_lines)
                    .Where(x => x.taxes.Count > 0) // list should not be empty
                    .Where(x => x.taxes.First().id == "1")
                    .Sum(x => x.total);
                // Todo test if there is not one with not 1 or 2.


                file.WriteLine("-------------Verzendkosten--------------");
                file.WriteLine("Tax high regel Verzenkosten");
                file.WriteLine($"Rest Totaal High: {totalOmzetShippingHigh} Tax High: {totalOmzetShippingTaxHigh}");

                file.WriteLine("Tax low regel Verzenkosten");
                file.WriteLine($"Rest Totaal low: {totalOmzetShippingLow} Tax Low: {totalOmzetShippingTaxLow}");
                file.WriteLine("--------------------------------");

                foreach (var item in orders)
                {
                    if (item.meta_data.FirstOrDefault(x => x.key == "_wcpdf_invoice_number") != null)
                    {
                        file.WriteLine($"Factuurnummer:{item.meta_data.FirstOrDefault(x => x.key == "_wcpdf_invoice_number").value} Order number: {item.number}");
                        file.WriteLine(
                            $"FactuurDatum:{item.meta_data.First(x => x.key == "_wcpdf_invoice_date_formatted").value}");
                    }
                    else
                    {
                        file.WriteLine("No invoice nubmer");
                    }
                    file.WriteLine($"Total price: {item.total}");
                    foreach (var taxLine in item.tax_lines)
                    {
                        var taxType = taxLine.rate_id == "1" ? "High" : "Low";
                        file.WriteLine($"Tax {taxType}: {taxLine.tax_total}");
                    }
                    foreach (var lineItem in item.line_items)
                    {
                        file.WriteLine($"Product naam: {lineItem.name} Price: {lineItem.price}");
                        
                    }
                    file.WriteLine("--------------------------------");
                }
               
            }
            
        }

        public bool IsKraamPakketProduct(int? productId)
        {
            if ((int)Products.Cordring == productId || (int) Products.Overvloed == productId || (int) Products.Luxe == productId) return true;
            return false;
        }
        public enum Products
        {
            Overvloed = 718,
            Luxe = 719,
            Cordring = 714
        }

        public enum TaxType
        {
            high = 1,
            low = 2

        }
    }
}
