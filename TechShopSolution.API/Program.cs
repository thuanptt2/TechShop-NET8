using Microsoft.EntityFrameworkCore;
using TechShopSolution.Infrastructure.DBContext;
using TechShopSolution.Domain.Repositories;
using TechShopSolution.Infrastructure.Repositories;
using TechShopSolution.Application.Services.Products;
using TechShopSolution.Application.Services.Categories;
using TechShopSolution.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TechShopDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("TechShopDB")));

// Injecting Service
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
