using System;
using DirectorySite.Models;

namespace DirectorySite.Interfaces;

public interface IPeopleBanService
{
    Task<PagedResponse<PersonResponse>> GetPeopleBanned(int page, int pageSize);
    Task BanPerson(Guid personId);
    Task UnbanPerson(Guid personId);
    Task<IEnumerable<object>> GetPersonBanHistory();
}
