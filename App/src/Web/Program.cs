using Application;
using Infrastructure;
using Presentation;
using System.IdentityModel.Tokens.Jwt;
using Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddPresentation();
RateLimiterMiddleware.Register(builder.Services);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers()
    .RequireRateLimiting("ipPolicy");
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseRateLimiter();
app.Run();
