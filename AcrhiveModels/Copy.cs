using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Copy: FullAuditableModel
    {
        //к какому оригиналу отностится
        public int OriginalId { get; set; }
        public virtual Original? Original { get; set; }
        //номер копии (не является id)
        public int CopyNumber { get; set; }
        //Обоснование создания копии
        public int? CreationDocumentId { get; set; }
        [ForeignKey("CreationDocumentId")]
        public virtual Document? CreationDocument { get; set; }
        //обоснование уничтожения копии
        public int? DeletionDocumentId { get; set; }
        [ForeignKey("DeletionDocumentId")]
        public virtual Document? DeletionDocument { get; set; }
        //Датат удничтожения копии
        public DateTime DelitionDate { get; set; }
        public List<Delivery> Deliveries { get; set; } = new List<Delivery>();
    }
}
