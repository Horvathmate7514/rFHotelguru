using Hotelguru.DataContext.Context;
using Hotelguru.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace hotelguru
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HotelguruDb;Trusted_Connection=True;TrustServerCertificate=True;"));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HotelGuru API", Version = "v1" });
            });

            //builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IRoleService, RoleService>();


           
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelGuru API v1");
                });
            }

          
            app.UseCors("AllowFrontend");
          

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}