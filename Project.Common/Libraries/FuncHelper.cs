using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Libraries
{
    public static class FuncHelper
    {
        public static string JsonModleToString<T>(T model)
        {
            if (model == null)
            {
                return string.Empty;
            }
            return System.Text.Json.JsonSerializer.Serialize(model);
        }
    }
}
