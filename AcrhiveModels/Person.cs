using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Person: FullAuditableModel
    {
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Department { get; set; }
    }
}
