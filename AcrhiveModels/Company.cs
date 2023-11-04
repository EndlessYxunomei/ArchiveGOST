using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Company: FullAuditableModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
