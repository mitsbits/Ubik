using Mehdime.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Basis;
using Ubik.Web.Basis.Contracts;
using Ubik.Web.EF;

namespace Ubik.Web.Client.Backoffice
{
    public class ErrorLogManager : IErrorLogManager
    {
        private readonly ICRUDRespoditory<PersistedExceptionLog> _repo;
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        public ErrorLogManager(ICRUDRespoditory<PersistedExceptionLog> repo, IDbContextScopeFactory dbContextScopeFactory)
        {
            _repo = repo;
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        public async Task<int> ClearLogs(DateTime startUtc, DateTime endUtc)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _repo.DeleteAsync(x => x.TimeUtc >= startUtc && x.TimeUtc <= endUtc);
                return await db.SaveChangesAsync();
            }
        }

        public async Task<int> ClearLogs(IEnumerable<string> ids)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                await _repo.DeleteAsync(x => ids.Contains(x.ErrorId.ToString()));
                return await db.SaveChangesAsync();
            }
        }

        public async Task<int> ClearLog(string id)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                var errorId = Guid.Parse(id);

                await _repo.DeleteAsync(x => x.ErrorId == errorId);

                return await db.SaveChangesAsync();
            }
        }

        public async Task<PagedResult<ErrorLog>> GetLogs(int pageNumer, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _repo.FindAsync(x => true,
                    new[] { new OrderByInfo<PersistedExceptionLog>() { Ascending = false, Property = l => new { l.TimeUtc } } },
                    pageNumer, pageSize);
                return new PagedResult<ErrorLog>(data.Data.Select(MapToDomain), data.PageNumber, data.PageSize,
                    data.TotalRecords);
            }
        }

        public Task LogException(Exception ex)
        {
            //Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            return Task.FromResult(false);
        }

        private static ErrorLog MapToDomain(PersistedExceptionLog data)
        {
            return new ErrorLog() { ErrorCode = data.StatusCode, ErrorDatetTimeUtc = data.TimeUtc, ErrorMessage = data.Message, ErrorType = data.Type, Host = data.Host, Id = data.ErrorId.ToString(), User = data.User };
        }
    }
}