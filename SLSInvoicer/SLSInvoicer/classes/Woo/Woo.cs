using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLSInvoicer.classes.Woo
{



    public class OrdersRootObject
    {
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string number { get; set; }
        public string order_key { get; set; }
        public string created_via { get; set; }
        public string version { get; set; }
        public string status { get; set; }
        public string currency { get; set; }

        public DateTime date_created { get; set; }
        //public DateTime date_created_gmt { get; set; }
        //public DateTime date_modified { get; set; }
        //public DateTime date_modified_gmt { get; set; }
        public string discount_total { get; set; }
        public string discount_tax { get; set; }
        public string shipping_total { get; set; }
        public string shipping_tax { get; set; }
        public string cart_tax { get; set; }
        public string total { get; set; }
        public string total_tax { get; set; }
        public bool prices_include_tax { get; set; }
        public int customer_id { get; set; }
        public string customer_ip_address { get; set; }
        public string customer_user_agent { get; set; }
        public string customer_note { get; set; }
        public Billing billing { get; set; }
        //  public Shipping shipping { get; set; }
        public string payment_method { get; set; }
        public string payment_method_title { get; set; }
        public string transaction_id { get; set; }
        //public DateTime date_paid { get; set; }
        //public DateTime date_paid_gmt { get; set; }
        //public DateTime? date_completed { get; set; }
        //public DateTime? date_completed_gmt { get; set; }
        public string cart_hash { get; set; }
        public object meta_data { get; set; }
        //public Line_Items[] line_items { get; set; }
        //public Tax_Lines[] tax_lines { get; set; }
        public Shipping_Lines[] shipping_lines { get; set; }
        //public object[] fee_lines { get; set; }
        //public object[] coupon_lines { get; set; }
        //public object[] refunds { get; set; }
        //public _Links _links { get; set; }
    }

    public class Billing
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class Shipping
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
    }

    public class _Links
    {
        public Self[] self { get; set; }
        public Collection[] collection { get; set; }
        public Customer[] customer { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class Customer
    {
        public string href { get; set; }
    }

    public class Line_Items
    {
        public int id { get; set; }
        public string name { get; set; }
        public int product_id { get; set; }
        public int variation_id { get; set; }
        public int quantity { get; set; }
        public string tax_class { get; set; }
        public string subtotal { get; set; }
        public string subtotal_tax { get; set; }
        public string total { get; set; }
        public string total_tax { get; set; }
        public Tax[] taxes { get; set; }
        public object[] meta_data { get; set; }
        public string sku { get; set; }
        public float price { get; set; }
    }

    public class Tax
    {
        public int id { get; set; }
        public string total { get; set; }
        public string subtotal { get; set; }
    }

    public class Tax_Lines
    {
        public int id { get; set; }
        public string rate_code { get; set; }
        public int rate_id { get; set; }
        public string label { get; set; }
        public bool compound { get; set; }
        public string tax_total { get; set; }
        public string shipping_tax_total { get; set; }
        public object[] meta_data { get; set; }
    }

    public class Shipping_Lines
    {
        public int id { get; set; }
        public string method_title { get; set; }
        public string method_id { get; set; }
        public string total { get; set; }
        public string total_tax { get; set; }
        public Tax1[] taxes { get; set; }
        public Meta_Data[] meta_data { get; set; }
    }

    public class Tax1
    {
        public int id { get; set; }
        public string total { get; set; }
        public string subtotal { get; set; }
    }

    public class Meta_Data
    {
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }

}
