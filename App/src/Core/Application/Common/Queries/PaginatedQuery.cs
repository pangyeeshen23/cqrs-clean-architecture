using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Queries
{
    public record PaginatedQuery(
        int PageSize = 10,
        int Page = 1
    );
}
