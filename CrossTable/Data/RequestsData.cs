using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossTable
{
    public class RequestsData
    {
        public List<Requests> requests { get; set; }
    }
    public class Requests
    {
        public string ID { get; set; }
        public string NomenclatureId { get; set; }
        public string RequestNum { get; set; }
        public string Priority { get; set; }
        public string NomenclatureType { get; set; }
        public string NomenclatureName { get; set; }
        public string NomenclatureCode { get; set; }
        public string NomenclatureUMeasure { get; set; }
        public string Author { get; set; }
        public string Area { get; set; }
        public string ReceivePerson { get; set; }
        public string MinCost { get; set; }
        public string MaxCost { get; set; }
        public string Currency { get; set; }
        public string DeliveryStartDate { get; set; }
        public string DeliveryEndDate { get; set; }
        public string NomenclatureQuantity { get; set; }
        public string NomeclatureRentaId { get; set; }
        public string TotalQuantityMO { get; set; }
        public string TransferQuantityMO { get; set; }
        public string Availability { get; set; }
    }
}
