using MessengerData.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MessengerData.Extensions
{
    public static class DbContextExtentions
    {
        public static async Task<SaveResult> SaveAsync(this DbContext _context, string providerName)
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
    }
}
