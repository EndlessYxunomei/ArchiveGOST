using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Applicability: FullAuditableModel
    {
        //к какому документу относится
        public virtual List<Original> Originals { get; set; } = new List<Original>();
        //Применяемость
        [StringLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        public required string Description { get; set; }
    }
}
