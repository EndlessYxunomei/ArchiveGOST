using AcrhiveModels;
using ArchiveGOST_DbLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace DataBaseLayer
{
    public class DocumentRepo(ArchiveDbContext context) : IDocumentRepo
    {
        public readonly ArchiveDbContext _context = context;

        public async Task DeleteDocument(int id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
            if (document == null) {  return; }
            document.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteDocuments(List<int> documentIds)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var documentId in documentIds)
                    {
                        await DeleteDocument(documentId);
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                    TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var documentId in documentIds)
                    {
                        await DeleteDocument(documentId);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }*/
        }
        public async Task<int> UpsertDocument(Document document)
        {
            if (document.Id > 0)
            {
                return await UpdateDocument(document);
            }
            return await CreateDocument(document);
        }
        private async Task<int>CreateDocument(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            if (document.Id <= 0) { throw new Exception("Could not Create the document as expected"); }
            return document.Id;
        }
        private async Task<int> UpdateDocument(Document document)
        {
            var dbDocument = await _context.Documents.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == document.Id) ?? throw new Exception("Document not found");
            dbDocument.Name = document.Name;
            dbDocument.Description = document.Description;
            dbDocument.Date = document.Date;
            dbDocument.CompanyId = document.CompanyId;
            dbDocument.IsDeleted = document.IsDeleted;
            
            await _context.SaveChangesAsync();
            return document.Id;
        }
        public async Task UpsertDocuments(List<Document> documents)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var document in documents)
                    {
                        var success = await UpsertDocument(document) > 0;
                        if (!success) { throw new Exception($"Error saving the document {document.Name}"); }
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var document in documents)
                    {
                        var success = await UpsertDocument(document) > 0;
                        if (!success) { throw new Exception($"Error saving the document {document.Name}"); }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }*/
        }

        public async Task<List<Document>> GetDocumentList()
        {
            return await _context.Documents.ToListAsync();
        }
        public async Task<List<Document>> GetDocumentList(DocumentType type)
        {
            return await _context.Documents.Where(x => x.DocumentType == type).ToListAsync();
        }
        public async Task<Document> GetDocumentAsync(int id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.Id == id);
            return document ?? throw new Exception("Document not found");
        }
    }
}
