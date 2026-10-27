using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Enum.Entities
{
    public static class PermissionKey
    {
        public const string system_full_access = "system_full_access";
        //key quyền quản lý key quyền hệ thống 
        public const string system_permission_key_view = "system_permission_key_view";
        public const string system_permission_key_create = "system_permission_key_create";
        public const string system_permission_key_update = "system_permission_key_update";
        // key quyền quản lý role hệ thống
        public const string system_role_key_view = "system_role_key_view";
        public const string system_role_key_create = "system_role_key_create";
        public const string system_role_key_update = "system_role_key_update";
    }
}
