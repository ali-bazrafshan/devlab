using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hey");
app.MapGet("/person", () => new Person("Ali", "Bazrafshan"));
app.MapGet("/person/{firstName}/{lastName}", (string firstName, string lastName) => new Person(firstName, lastName));

app.Run();

public record Person(string FirstName, string LastName);
