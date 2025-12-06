var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger());

builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddHttpClient<AdventOfCodeClient>(client =>
    client.BaseAddress = new Uri("https://adventofcode.com/"))
    .ConfigurePrimaryHttpMessageHandler(sp =>
    {
        var cookieContainer = new CookieContainer();
        var sessionCookie = sp.GetRequiredService<IConfiguration>()["AdventOfCode:SessionCookie"];
        cookieContainer.Add(new Uri("https://adventofcode.com/"), new Cookie("session", sessionCookie));

        return new SocketsHttpHandler { CookieContainer = cookieContainer };
    });

var solutionTypes = typeof(Program).Assembly.GetTypes()
    .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(ISolution)));

foreach (var type in solutionTypes)
    builder.Services.AddTransient(type);

var host = builder.Build();
var solution = host.Services.GetRequiredService<Day07_2025>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();

// Run the solution
logger.LogInformation("Solving {Solution}", solution.GetType().Name);
var timestamp = Stopwatch.GetTimestamp();
logger.LogInformation("Part 1: {Part1} elapsed {Elapsed}ms",
    await solution.SolvePart1(),
    Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds);

timestamp = Stopwatch.GetTimestamp();
logger.LogInformation("Part 2: {Part2} elapsed {Elapsed}ms",
    await solution.SolvePart2(),
    Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds);
