using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tasks.Infrastructure;

namespace Tasks.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<TasksDbContext>(options => options.UseInMemoryDatabase("tasks"))
                .AddScoped<TasksSchema>()
                .AddScoped<TasksQuery>()
                .AddScoped<TasksMutation>()
                .AddGraphQL(options => options.EnableMetrics = true)
                .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = true)
                .AddSystemTextJson()
                .AddGraphTypes(typeof(TasksSchema));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<TasksSchema>();
            app.UseGraphQLPlayground();
        }
    }
}
