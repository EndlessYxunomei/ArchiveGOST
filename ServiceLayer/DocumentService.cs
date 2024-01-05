using AcrhiveModels;
using AcrhiveModels.DTOs;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepo documentRepo;
        public DocumentService(IDocumentRepo documentRepo)
        {
            this.documentRepo = documentRepo;
        }
        public DocumentService(ArchiveDbContext dbContext)
        {
            documentRepo = new DocumentRepo(dbContext);
        }

        public Task CreateDocument(DocumentDetailDto document)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DocumentListDto>> GetDocumentListAsync()
        {
            var nelist = await documentRepo.GetDocumentList();
            List<DocumentListDto> result = [];
            foreach (var document in nelist)
            {
                DocumentListDto dto = (DocumentListDto)document;
                result.Add(dto);
            }
            return result;
        }
        public async Task<List<DocumentListDto>> GetDocumentList(DocumentType type)
        {
            var nelist = await documentRepo.GetDocumentList(type);
            List<DocumentListDto> result = [];
            foreach (var document in nelist)
            {
                DocumentListDto dto = (DocumentListDto)document;
                result.Add(dto);
            }
            return result;
        }
    }
}
