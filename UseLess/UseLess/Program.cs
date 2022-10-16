using Eveneum;
using Useless.AzureStore;
using UseLess.Api;
using UseLess.Domain;
using UseLess.Domain.Values;
using UseLess.EndToEndTest;
using UseLess.Messages;
using UseLess.Services.Api;
using UseLess.Services.Budgets;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseResponse = await ReadClient.Instance.CreateDatabaseIfNotExistsAsync("useless-dev");
var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(
    new Microsoft.Azure.Cosmos.ContainerProperties("Events","/StreamId"));
var options = new EventStoreOptions 
{
    TypeProvider = new TypeProvider()
};
IEventStore eventStore = new EventStore(ReadClient.Instance, "useless-dev", "Events");
await eventStore.Initialize();
var store = new ReadStore();
await store.Initialize(databaseResponse.Database);
var budgetQueryService = new BudgetQueryService(store,store,store,store,store,store,store,store);
builder.Services
    .AddSingleton<IAggregateStore>(new AggregateStore(eventStore,eventStore,store))
    .AddSingleton<IQueryUpdate>(store)
    .AddSingleton<IBudgetQueryService>(budgetQueryService)
    .AddSingleton<IApplicationService,BudgetService>().AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["azure_development_connection:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["azure_development_connection:queue"], preferMsi: true);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
