using BookService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookService
{
    public class Startup
    {
        string myAllowedOrigins = "allowedSites";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Enable CORS
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();                
            }));

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Books API",
                    Version = "v1",
                    Description = "Books API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Bernardino Chin",
                        Email = "bernardino.chin@gmail.com",
                        Url = new System.Uri("https://github.com/bernardinoChin2105")
                    }
                });
            });

            services.AddDbContext<BooksDbContext>(options =>
            {
                options.UseInMemoryDatabase("Books").UseLazyLoadingProxies();
                //options.UseSqlServer(Configuration.GetConnectionString("BooksDB")).UseLazyLoadingProxies();

            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //CORS
            app.UseCors("MyPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/v1/swagger.json", "Books API");
                options.RoutePrefix = string.Empty;
            });

            //Se inicializan datos para poder trabajar con categorías.
            var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetService<BooksDbContext>();
            db.Add(new Models.Domain.Category() { Id = System.Guid.NewGuid(), Name = "Literatura" });
            db.Add(new Models.Domain.Category() { Id = System.Guid.NewGuid(), Name = "Historia" });
            db.Add(new Models.Domain.Category() { Id = System.Guid.NewGuid(), Name = "Tecnología" });
            db.Add(new Models.Domain.Category() { Id = System.Guid.NewGuid(), Name = "Ciencia" });
            db.Add(new Models.Domain.Category() { Id = System.Guid.NewGuid(), Name = "Cocina" });
            db.SaveChanges();
        }
    }
}
