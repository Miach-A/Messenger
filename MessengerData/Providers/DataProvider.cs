using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MessengerData.Providers
{
    public class DataProvider
    {
        protected readonly ApplicationDbContext _context;
        public DataProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SaveResult> SaveAsync(string providerName)
        {
            try
            {
                await _context.SaveChangesAsync();

                return new SaveResult().SetResultTrue();
            }
            catch (DbUpdateException exeption)
            {
                var result = new SaveResult();
                if ((exeption.InnerException as SqlException)?.Number == 2601)
                {
                    result.ErrorMessage.Add(string.Concat(providerName, "Save changes. Duplicate field"));
                }

                if (result.ErrorMessage.Count() == 0)
                {
                    result.ErrorMessage.Add(string.Concat(providerName, "Save changes. Unhandled db update exception"));
                }

                return result;
            }
            catch
            {
                return new SaveResult(string.Concat(providerName, "Save changes. Unhandled exception."));
            }
        }

        public bool GetUserGuid(ClaimsPrincipal user, out Guid guid)
        {
            guid = Guid.Empty;
            string? userGuidString = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userGuidString == null)
            {
                return false;
            }

            try
            {
                guid = new Guid(userGuidString);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool GetClaimValue(ClaimsPrincipal collection, string type, out string claimValue)
        {
            claimValue = string.Empty;
            string? value = collection.FindFirst(x => x.Type == type)?.Value;
            if (value == null)
            {
                return false;
            }
            else
            {
                claimValue = value;
                return true;
            }
        }

    }
}
