var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello.");
app.MapGet("/person", () => Person.All);
app.MapGet("/person/{id}", (int id) => Person.All[id]);
app.MapPost("/person", Handlers.AddPerson);
app.MapPut("/person", Handlers.ReplacePerson);
app.MapDelete("/person/{id}", Handlers.DeletePerson);

app.Run();

public record Person(string FirstName, string LastName)
{
    public static readonly Dictionary<int, Person> All = new();
}

class Handlers
{
    public static void AddPerson(Person person)
    {
        Person.All.Add(Person.All.Count + 1, person);
    }

    public static void ReplacePerson(int id, Person person)
    {
        Person.All[id] = person;
    }
    public static void DeletePerson(int id)
    {
        Person.All.Remove(id);
    }
}
