
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("Frontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string connectionString =
    builder.Configuration.GetConnectionString("PortfolioDb")!;

// Health Check
app.MapGet("/api/health", () =>
{
    return Results.Ok(new { status = "healthy" });
});

// Profile API
app.MapGet("/api/profile", () =>
{
    return Results.Ok(new
    {
        name = "Vicky Ojha",
        title = ".NET Full Stack Developer",
        experience = "4.5 years",
        location = "Pune, Maharashtra",
        email = "vickyojha97@gmail.com",
        phone = "+91 8805394283"
    });
});

// Get Projects
app.MapGet("/api/projects", async () =>
{
    var projects = new List<object>();

    await using var connection =
        new SqlConnection(connectionString);

    await connection.OpenAsync();

    string query = @"
        SELECT ProjectId, Title, Description, Technologies
        FROM dbo.Project
        WHERE IsActive = 1
        ORDER BY DisplayOrder";

    await using var command =
        new SqlCommand(query, connection);

    await using var reader =
        await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        projects.Add(new
        {
            id = reader.GetInt32(0),
            title = reader.GetString(1),
            description = reader.GetString(2),
            technologies = reader.IsDBNull(3)
                ? Array.Empty<string>()
                : reader.GetString(3).Split(',')
        });
    }

    return Results.Ok(projects);
});

// Contact API
app.MapPost("/api/contact", async (ContactRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) ||
        string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrWhiteSpace(request.Message))
    {
        return Results.BadRequest(
            new { message = "All fields are required." });
    }

    await using var connection =
        new SqlConnection(connectionString);

    await connection.OpenAsync();

    string query = @"
        INSERT INTO dbo.ContactMessage
        (Name, Email, Message)
        VALUES
        (@Name, @Email, @Message)";

    await using var command =
        new SqlCommand(query, connection);

    command.Parameters.AddWithValue(
        "@Name", request.Name.Trim());

    command.Parameters.AddWithValue(
        "@Email", request.Email.Trim());

    command.Parameters.AddWithValue(
        "@Message", request.Message.Trim());

    await command.ExecuteNonQueryAsync();

    return Results.Ok(
        new { message = "Message saved successfully." });
});

app.Run();

record ContactRequest(
    string Name,
    string Email,
    string Message
);
