using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASI.Basecode.Data.Data;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;

        public ClassRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Classes
                .Include(c => c.Teacher)
                .OrderBy(c => c.CourseId)
                .ToListAsync();
        }

        public async Task<Class?> GetByEdpAsync(int edpCode)
        {
            return await _context.Classes
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == edpCode);
        }

        public async Task AddAsync(Class entity)
        {
            _context.Classes.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class entity)
        {
            _context.Classes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Class entity)
        {
            _context.Classes.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
