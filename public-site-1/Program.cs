using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

string connetionString = Environment.GetEnvironmentVariable("db_conn_string");

app.MapGet("/db", () =>
{
    string result = string.Empty;

    try
    {
        using (var sqlConn = new SqlConnection(connetionString))
        {
            var command = new SqlCommand("select * from dbo.tabelateste", sqlConn);
            sqlConn.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result += String.Format("{0}, {1}", reader[0], reader[1]) + "\r\n";
            }

            // Call Close when done reading.
            reader.Close();
        }
    }
    catch (Exception ex)
    {
        result = "Error: " + ex.Message;
    }

    return result;
});

app.MapGet("/", () =>
{
    return "This is public site 1!";
});

app.MapGet("/download", () =>
{
    Random rnd = new Random();
    byte[] b = new byte[100000 * 1024];
    rnd.NextBytes(b);

    return Results.Bytes(b, "application/octet-stream");
});

app.Run();