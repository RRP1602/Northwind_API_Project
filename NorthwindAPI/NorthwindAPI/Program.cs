using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Services;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindBusiness
{
    class Program
    {
        [ExcludeFromCodeCoverage]
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration["default"]));
            builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("NorthwindMemory"));
            builder.Services.AddScoped<IProductService, ProductService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(); 

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
