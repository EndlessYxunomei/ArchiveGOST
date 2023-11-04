using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Delivery: FullAuditableModel
    {
        public int CopyId { get; set; }
        public int PersonId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryDocumentId { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ReturnDocumentId { get; set;}
        public bool IsDelivered { get; set; }
    }
}
