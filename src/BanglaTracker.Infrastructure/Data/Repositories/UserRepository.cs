using BanglaTracker.Core.Entities;
using BanglaTracker.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BanglaTracker.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly TrackerDbContext _context;

        public UserRepository(TrackerDbContext context) : base(context) // Pass context to the base constructor
        {
            _context = context;
        }

        public async Task<AppUser?> FetchUserByInstallationIDAsync(
            Guid installationId)
        {
            return await _context.AppUsers
                .Where(user => user.InstallationId == installationId)
                .FirstOrDefaultAsync();
        }
        
    }

}
