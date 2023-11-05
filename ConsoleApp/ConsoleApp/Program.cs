using Flurl.Http;

var data = await "http://localhost:5000/api/users/summary".GetStringAsync();


Console.WriteLine(data);
Console.ReadLine();