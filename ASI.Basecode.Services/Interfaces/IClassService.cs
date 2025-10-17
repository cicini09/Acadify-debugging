using System.Collections.Generic;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IClassService
    {
        Task<List<Class>> GetAllClassesAsync();
        Task<Class?> GetClassByEdpAsync(int edpCode);
        Task CreateClassAsync(Class model, HttpRequest request);
        Task<bool> UpdateClassAsync(Class model, HttpRequest request);
        Task<(bool Success, string Message)> DeleteClassAsync(int edpCode);
    }
}
