using MessageManagement.Data;
using MessageManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MessagesDB"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/messages", async (AppDbContext db) =>
{
    Results<Ok<List<Message>>, NoContent> result;

    var messages = await db.Messages.ToListAsync();
    if (messages.Any())
    {
        result = TypedResults.Ok(messages);
    }
    else
    {
        result = TypedResults.NoContent();
    }
    return result;
});

app.MapGet("/messages/{email}", async (string email, AppDbContext db) =>
{
    Results<Ok<List<Message>>, NotFound> result;

    var messages = await db.Messages
        .Where(m => m.Email == email)
        .ToListAsync();

    if (messages.Any())
    {
        result = TypedResults.Ok(messages);
    }
    else
    {
        result = TypedResults.NotFound();
    }
    
    return result;
});

app.MapPost("/messages", async (Message message, AppDbContext db) =>
{
    message.Date = DateTime.UtcNow;
    db.Messages.Add(message);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/messages/{message.Id}", message);
});

app.MapPut("/messages/{id}", async (int id, Message inputmessage, AppDbContext db) =>
{
    Results<NoContent, NotFound> result;

    var message = await db.Messages.FindAsync(id);

    if (message is null)
    {
        result = TypedResults.NotFound();
    }
    else
    {
        message.Email = inputmessage.Email;
        message.Text = inputmessage.Text;

        await db.SaveChangesAsync();
        result = TypedResults.NoContent();
    }

    return result;
});

app.MapDelete("/messages/{id}", async (int id, AppDbContext db) =>
{
    Results<Ok<Message>, NotFound> result;

    if (await db.Messages.FindAsync(id) is Message message)
    {
        db.Messages.Remove(message);
        await db.SaveChangesAsync();
        result = TypedResults.Ok(message);
    }
    else
    {
        result = TypedResults.NotFound();
    }
    return result;
});

app.Run();