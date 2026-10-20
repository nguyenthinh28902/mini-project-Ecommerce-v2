using Ecommerce.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces
{
    public interface IBrandRepository
    {

        Task AddBrandAsync(Brand brand, CancellationToken cancellationToken = default);
        Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Brand?> GetBrandBySlugAsync(string Slug, CancellationToken cancellationToken = default);
        Task<IEnumerable<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
        void UpdateBrand(Brand brand);
        void DeleteBrand(Brand brand);
    }
}
