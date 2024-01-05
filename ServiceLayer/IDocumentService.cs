using AcrhiveModels;
using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IDocumentService
    {
        Task<List<DocumentListDto>> GetDocumentListAsync();
        Task<List<DocumentListDto>> GetDocumentList(DocumentType type);
        Task CreateDocument (DocumentDetailDto document);
    }
}
