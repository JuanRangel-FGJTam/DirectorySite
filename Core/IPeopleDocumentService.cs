using System;
using DirectorySite.Models;

namespace DirectorySite.Core;

public interface IPeopleDocumentService
{
    Task<IEnumerable<PersonDocumentResponse>> GetPeopleDocuments(Guid personId);
    Task<IEnumerable<PersonDocumentResponse>> GetPeopleDocumentsByType(Guid personId, int typeId);
}
