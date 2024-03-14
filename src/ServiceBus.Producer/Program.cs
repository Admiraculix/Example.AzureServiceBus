using Microsoft.Extensions.Azure;
using ServiceBus.Producer.Configurations;
using ServiceBus.Producer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Register Azure Service Bus client using AddAzureClients
builder.Services.AddAzureClients(acfBuilder =>
{
    // Register Azure Service Bus client
    acfBuilder.AddServiceBusClient(builder.Configuration.GetSection("ServiceBus:ConnectionString"));

});
builder.Services.Configure<AzureServiceBusQueuesAndTopicsConfiguration>(
            builder.Configuration.GetSection("ServiceBus"));

// Register your custom wrappers
//builder.Services.AddTransient<IServiceBusSenderWrapper, ServiceBusSenderWrapper>();
//builder.Services.AddSingleton<IServiceBusClientWrapper, ServiceBusClientWrapper>();
// Register your MessagePublisher class
builder.Services.AddTransient<IMessagePublisher, MessagePublisher>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
