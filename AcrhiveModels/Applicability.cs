using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Applicability: FullAuditableModel
    {
        //к какому документу относится
        public int OriginalId { get; set; }
        //Применяемость
        public required string Description { get; set; }
    }
}
