using Barbu.Api.Services;
using Barbu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Le Barbu API", Version = "v1" });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxt", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                new Uri(origin).Host == "localhost")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Database
builder.Services.AddDbContext<BarbuDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("Barbu.Infrastructure")
    )
);

// Register Services
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IGamesService, GamesService>();
builder.Services.AddScoped<IChampionshipsService, ChampionshipsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Le Barbu API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowNuxt");
app.UseAuthorization();
app.MapControllers();

app.Run();
