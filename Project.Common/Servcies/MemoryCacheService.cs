using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Servcies
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        // Cập nhật hàm Set hỗ trợ Size (cho thuật toán giới hạn) và Priority (Độ ưu tiên giữ lại)
        public void Set<T>(string key, T value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, int? size = null, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            var options = new MemoryCacheEntryOptions();

            if (absoluteExpireTime.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpireTime.Value);
            }

            if (slidingExpireTime.HasValue)
            {
                options.SetSlidingExpiration(slidingExpireTime.Value);
            }

            // Gán mức độ ưu tiên (Low, Normal, High, NeverRemove)
            // - NeverRemove: Không bao giờ bị thuật toán xóa tự động
            // - High / Normal / Low: Mức độ ưu tiên bị xóa khi cache đầy
            options.Priority = priority;

            // Gán kích thước cho item (Bắt buộc nếu có cấu hình SizeLimit ở Program.cs)
            if (size.HasValue)
            {
                options.SetSize(size.Value);
            }

            _memoryCache.Set(key, value, options);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpireTime = null, int? size = null, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            if (!_memoryCache.TryGetValue(key, out T result))
            {
                result = await factory();

                var options = new MemoryCacheEntryOptions();
                options.SetAbsoluteExpiration(absoluteExpireTime ?? TimeSpan.FromMinutes(15));
                options.Priority = priority;

                if (size.HasValue)
                {
                    options.SetSize(size.Value);
                }

                _memoryCache.Set(key, result, options);
            }

            return result;
        }
    }
}
