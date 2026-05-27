using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Queries;
using FluentValidation;

namespace Application.Common.FluentValidation
{
    public class PaginatedQueryValidator<T> : AbstractValidator<T> where T : PaginatedQuery
    {
        public PaginatedQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be higher than zero");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than zero");
        }
    }
}
