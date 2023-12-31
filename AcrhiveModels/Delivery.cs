﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Delivery: FullAuditableModel
    {
        public virtual List<Copy> Copies { get; set; } = [];
        public int PersonId { get; set; }
        public virtual Person? Person { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public int? DeliveryDocumentId { get; set; }
        [ForeignKey("DeliveryDocumentId")]
        public virtual Document? DeliveryDocument { get; set; }
        public DateOnly ReturnDate { get; set; }
        public int? ReturnDocumentId { get; set;}
        [ForeignKey("ReturnDocumentId")]
        public virtual Document? ReturnDocument { get; set; }
    }
}
