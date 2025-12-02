using infoCrudDatabaseModels.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<InfoContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("DefaultConnection")!
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapGet("/api/infos", async (InfoContext db) =>
{
    List<Info> infos = await db.Infos.ToListAsync();
    return Results.Ok(infos);
});

app.MapPost("/api/infos", async (InfoContext db, Info info) =>
{
    info.Id = 0;
    await db.Infos.AddAsync(info);
    await db.SaveChangesAsync();
    return Results.Ok(new {
        info.Name,
        info.Address,
    });
});

app.MapDelete("/api/infos/{id}", async (InfoContext db, int id) =>
{
    var info = await db.Infos.FindAsync(id);
    if (info == null)
    {
        return Results.NotFound();
    }
    db.Infos.Remove(info);
    await db.SaveChangesAsync();
    return Results.Ok(new {
        message = $"info {id} removed successfully",
        info = new {
            info.Id,
            info.Name,
            info.Address,
        }
    });
});

app.MapPut("/api/infos/{id}", async (InfoContext db, int id, Info updatedInfo) =>
{
    var info = await db.Infos.FindAsync(id);
    if (info == null)
    {
        return Results.NotFound();
    }
    info.Name = updatedInfo.Name;
    info.Address = updatedInfo.Address;
    await db.SaveChangesAsync();
    return Results.Ok(new {
        message = $"info {id} updated successfully",
        info = new {
            info.Id,
            info.Name,
            info.Address,
        }
    });
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
