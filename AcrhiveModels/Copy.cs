using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Copy: FullAuditableModel
    {
        //к какому оригиналу отностится
        public int OriginalId { get; set; }
        //номер копии (не является id)
        public int CopyNumber { get; set; }
        //Обоснование создания копии
        public int CreationDocumentId { get; set; }
        //обоснование уничтожения копии
        public int DeletionDocumentId { get; set; }
        //Датат удничтожения копии
        public DateTime DelitionDate { get; set; }
    }
}
