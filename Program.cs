var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding CORS Policy for major frontends
builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        // Dev
        corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
    options.AddPolicy("ProdCors", (corsBuilder) =>
    {
        // Prod
        corsBuilder.WithOrigins("https://MyProdSite.com").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCors");
}
else
{
    // This should only be production because HTTPs is not required in development
    app.UseHttpsRedirection();
    app.UseCors("ProdCors");

}


app.UseAuthorization();

app.MapControllers();

app.Run();
