namespace ArchT.Services.Trade

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Swashbuckle.AspNetCore.Swagger
open Configurations

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1) |> ignore

        services.AddSwaggerGen(fun swagger ->
            let info = Info()
            info.Title <- "Trade Service API"; info.Version <- "v1"
            swagger.SwaggerDoc("v1", info)) |> ignore

        services.AddCors(fun opt ->
            opt.AddPolicy("CorsPolicy", fun b -> b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials() |> ignore)
        ) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseMvc()
            .UseCors("CorsPolicy")
            .UseSwagger()
            .UseSwaggerUI(fun opt ->
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1")
                opt.RoutePrefix <- ""
             ) |> ignore

    member val Configuration : IConfiguration = null with get, set