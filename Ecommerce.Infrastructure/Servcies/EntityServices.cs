using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Servcies
{
    public static class EntityServices
    {
        public static async Task<List<T>> PageEntityAsync<T>(
            this IQueryable<T> queryable,
            int pageSize = 20,
            int pageIndex = 1,
            CancellationToken cancellationToken = default)
        {
            // Đảm bảo pageIndex và pageSize luôn hợp lệ (tránh âm hoặc bằng 0)
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize <= 0 ? 20 : pageSize;

            // Thực thi phân trang và trả về List<T>
            return await queryable
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }
    }
}
