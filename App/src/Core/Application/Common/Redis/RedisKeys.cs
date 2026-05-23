using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Redis
{
    public static class RedisKeys
    {
        public static string TagList = "tag:list";
        public static string PostList = "post:list:user:{user_id}";
    }
}
