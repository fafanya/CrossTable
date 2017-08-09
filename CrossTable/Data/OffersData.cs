using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossTable
{
    public class OffersData
    {
        public List<Offers> offers { get; set; }
    }

    public class Offers
    {
        public string Area { get; set; }
        public string CagentID { get; set; }
        public string Category { get; set; }
        public string DelayDays { get; set; }
        public string DeliveryType { get; set; }
        public string Email { get; set; }
        public string ID { get; set; }
        public string INN { get; set; }
        public string IsNew { get; set; }
        public string PaymentType { get; set; }
        public string Title { get; set; }
        public string MailSendDate { get; set; }
        public List<Variants> variants { get; set; }
    }

    public class Variants
    {
        public string code { get; set; }
        public string measureUnit { get; set; }
        public string name { get; set; }
        public string requestId { get; set; }
        public string transferQuantity { get; set; }
        public string NomeclatureNameAnalog { get; set; }
        public string NomeclatureCodeAnalog { get; set; }
        public string CostInRub { get; set; }
        public string Cost { get; set; }
        public string Total { get; set; }
        public string Term { get; set; }
        public string ID { get; set; }
        public string Comment { get; set; }
        public string ExchangeRate { get; set; }
        public string ExchangeType { get; set; }
        public string Currency { get; set; }
        public bool AuthorHeadSelect { get; set; }
        public bool ManagerSelect { get; set; }
    }
}
