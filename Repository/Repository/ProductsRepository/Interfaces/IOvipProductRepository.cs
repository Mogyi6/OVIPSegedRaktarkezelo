using Models.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.ProductsRepository.Interfaces
{
    public interface IOvipProductRepository
    {
        Task<List<OvipProduct>> GetAllAsync();
        Task<OvipProduct?> GetByIdAsync(int id);
        Task<OvipProduct> CreateAsync(OvipProduct product);
        Task<OvipProduct?> UpdateAsync(OvipProduct product);
        Task<bool> DeleteAsync(int id);
    }
}
