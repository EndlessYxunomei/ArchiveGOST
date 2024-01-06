using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class CopyListDto
    {
        public required int Id { get; set; }
        public required int Number { get; set; }
        public static explicit operator CopyListDto(Copy copy)
        {
            return new CopyListDto() { Id = copy.Id, Number = copy.CopyNumber };
        }
    }
}
