using StackExchange.Redis;

var muxer = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
var db = muxer.GetDatabase();

await db.StringSetAsync("testKey", "testValue");
string? result = await db.StringGetAsync("testKey");
Console.WriteLine(result);