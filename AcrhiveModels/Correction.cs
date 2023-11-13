using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Correction: FullAuditableModel
    {
        //К каким Оригиналам отностися
        public virtual List<Original> Originals { get; set; } = new List<Original>();
        //номер изменения
        public int CorrectionNumber { get; set; }
        //Обоснование изменения
        public int DocumentId { get; set; }
        public virtual Document? Document { get; set; }
        //Описание изменений
        public required string Description { get; set; }

    }
}
