using System;
using DirectorySite.Models;

namespace DirectorySite.Interfaces;

public interface IPeopleBanService
{
    Task<PagedResponse<PersonResponse>> GetPeopleBanned(int page, int pageSize);
    Task BanPerson(Guid personId, string reason);
    Task UnbanPerson(Guid personId, string reason);
    Task<IEnumerable<object>> GetPersonBanHistory();
}
