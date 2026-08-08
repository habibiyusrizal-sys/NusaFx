using Microsoft.EntityFrameworkCore;
using NusaFx.Application.Interfaces;
using NusaFx.Application.Middleware;
using NusaFx.Application.Services;
using NusaFx.Infrastructure.Persistence;
using NusaFx.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionServices>();

builder.Services.AddScoped<IConversionLog, ConversionLogRepository>();
builder.Services.AddHttpClient<ICurrencyService, CurrencyService>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpClient<AiRateService>();
builder.Services.AddScoped<IAiRateService, AiRateService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "NusaFx_";
});

builder.Services.AddDbContext<NusaFxDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();
