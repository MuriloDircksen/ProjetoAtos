
using Cervejaria.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // cria politica 
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            

            builder.Services.AddDbContext<CervejariaContexto>(options =>
                                options.UseSqlServer(
                                    builder.Configuration.GetConnectionString("ServerConnection")));
            builder.Services.AddDbContext<CervejariaContexto>(); //libera injeção de dependência nas classes da controller

            builder.Services.AddControllers().AddNewtonsoftJson(
                x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //liberar acesso total a qualquer requisição externa a nossa api
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });

            }
           );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(MyAllowSpecificOrigins); //ADD Politica de segurança

            app.MapControllers();

            app.Run();
        }
    }
}