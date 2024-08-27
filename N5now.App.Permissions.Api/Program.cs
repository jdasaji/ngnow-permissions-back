using N5now.App.Infrastructure.Elasticsearch;
using N5now.App.Infrastructure.Kafka;
using N5now.App.Permissions.Common.Exceptions.Middleware;
using N5now.App.Permissions.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ElasticsearchSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));
builder.Services.Configure<kafkaSettings>(builder.Configuration.GetSection("kafkaSettings"));


builder.Services.AddInjectionInterface(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
