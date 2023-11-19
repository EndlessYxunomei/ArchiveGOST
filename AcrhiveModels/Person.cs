using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Person: FullAuditableModel
    {
        [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
        public string? FirstName { get; set; }
        [StringLength(ArchiveConstants.MAX_PERSON_NAME_LENGTH)]
        public required string LastName { get; set; }
        [StringLength(ArchiveConstants.MAX_PERSON_DEPARTMENT_LENGTH)]
        public string? Department { get; set; }
    }
}
