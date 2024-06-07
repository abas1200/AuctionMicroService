using Microsoft.EntityFrameworkCore;
using AuctionMicroService;
using AuctionMicroService.Core;
using AuctionMicroService.Config;
using AuctionMicroService.Core.Data;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MassTransit.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTestMassTransit(builder.Configuration, x =>
{
    x.RegisterConsumers();

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCoreServices();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer",
                  new OpenApiSecurityScheme
                  {
                      Type = SecuritySchemeType.Http,
                      BearerFormat = "JWT",
                      In = ParameterLocation.Header,
                      Scheme = "bearer"
                  });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="bearer"
                }
            },
            new string[]{}
        }
    });

});

builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
