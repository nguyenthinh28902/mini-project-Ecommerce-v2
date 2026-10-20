using Ecommerce.Infrastructure.Dbcontext;
using Ecommerce.Infrastructure.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Servcies
{
    public class BrandRepository : IBrandRepository
    {
        private readonly EcommerceDbContext _dbContext;
        public BrandRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddBrandAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            await _dbContext.Brands.AddAsync(brand, cancellationToken);
        }   

        public async Task<Brand?> GetBrandByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Brands.FindAsync(new object[] { id }, cancellationToken);
        }
        public async Task<Brand?> GetBrandBySlugAsync(string Slug, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Brands.FirstOrDefaultAsync(b => b.Slug == Slug, cancellationToken);
        }
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Brands.ToListAsync(cancellationToken);
        }
        public void UpdateBrand(Brand brand)
        {
            _dbContext.Brands.Update(brand);
        }
        public void DeleteBrand(Brand brand)
        {
            _dbContext.Brands.Remove(brand);
        }

    }
}
