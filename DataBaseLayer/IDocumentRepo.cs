using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface IDocumentRepo
    {
        Task<Document> GetDocumentAsync(int id);
        Task<List<Document>> GetDocumentList();
        Task<List<Document>> GetDocumentList(DocumentType type);
        Task<List<Document>> GetDocumentsByCompany(int companyId);
        Task<int> UpsertDocument(Document document);
        Task UpsertDocuments(List<Document> documents);
        Task DeleteDocument(int id);
        Task DeleteDocuments(List<int> documentIds);
        Task<bool> CheckDocument(string name, DateOnly date);
    }
}
