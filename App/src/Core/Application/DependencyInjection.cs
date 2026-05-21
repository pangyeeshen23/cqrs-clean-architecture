using Application.Handler;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddScoped<IUserHandler, UserHandler>();
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
