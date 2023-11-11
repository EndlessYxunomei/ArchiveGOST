using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Delivery: FullAuditableModel
    {
        public virtual List<Copy> Copies { get; set; } = new List<Copy>();
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryDocumentId { get; set; }
        public virtual Document? DeliveryDocument { get; set; }
        public DateTime ReturnDate { get; set; }
        public int? ReturnDocumentId { get; set;}
        public virtual Document? ReturnDocument { get; set; }
    }
}
