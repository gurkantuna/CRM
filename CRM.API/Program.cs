using CRM.Business.Extensions.DependencyResolvers.DotNetCore;
using CRM.Business.Utilities.IoC.DotNetCore;
using CRM.Core.Extensions;
using CRM.Core.Utilities.IoC.DotNetCore;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddIdentityWithDbContext<User, IdentityRole<int>, int, CrmDbContext>();
builder.Services.AddCoreDI([new CoreDIModule(builder.Environment, builder.Configuration)]);
builder.Services.AddBusinessDI([new BusinessDIModule()]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
