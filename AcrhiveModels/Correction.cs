using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Correction: FullAuditableModel
    {
        //К какому Оригиналу отностися
        public int OrigonalId { get; set; }
        //номер изменения
        public int CorrectionNumber { get; set; }
        //Обоснование изменения
        public int DocumentId { get; set; }
        //Описание изменений
        public required string Description { get; set; }

    }
}
