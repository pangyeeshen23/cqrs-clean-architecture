using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories.Model.Common
{
    public class PaginatedFilterModel
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
