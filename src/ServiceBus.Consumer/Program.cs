using Microsoft.Extensions.Azure;
using ServiceBus.Consumer.Configurations;
using ServiceBus.Consumer.Services;
using ServiceBus.Consumer.Services.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddHostedService<CustomerConsumerService>();
// Register Azure Service Bus client using AddAzureClients
builder.Services.AddAzureClients(acfBuilder =>
{
    // Register Azure Service Bus client
    acfBuilder.AddServiceBusClient(builder.Configuration.GetSection("ServiceBus:ConnectionString"));

});

builder.Services.Configure<AzureServiceBusQueuesAndTopicsConfiguration>(
            builder.Configuration.GetSection("ServiceBus"));

builder.Services.AddTransient<IMessageSubscriber, MessageSubscriber>();

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
