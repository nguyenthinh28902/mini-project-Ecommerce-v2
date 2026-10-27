using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Servcies
{
    public interface ICacheService
    {
        /// <summary>
        /// Lấy giá trị từ Cache theo Key
        /// </summary>
        T Get<T>(string key);

        /// <summary>
        /// Lưu hoặc cập nhật giá trị vào Cache (hỗ trợ thời gian sống, size và độ ưu tiên)
        /// </summary>
        void Set<T>(
            string key,
            T value,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? slidingExpireTime = null,
            int? size = null,
            CacheItemPriority priority = CacheItemPriority.Normal);

        /// <summary>
        /// Xóa Cache theo Key
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// Lấy dữ liệu từ Cache, nếu chưa có thì thực thi hàm factory để lấy mới, lưu vào Cache và trả về kết quả
        /// </summary>
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? absoluteExpireTime = null,
            int? size = null,
            CacheItemPriority priority = CacheItemPriority.Normal);
    }
}
