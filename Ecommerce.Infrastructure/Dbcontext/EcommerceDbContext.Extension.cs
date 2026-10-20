using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Dbcontext
{
    public partial class EcommerceDbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // Ví dụ: cấu hình thêm các tính năng riêng không muốn bị mất khi scaffold lại
        }
    }
}