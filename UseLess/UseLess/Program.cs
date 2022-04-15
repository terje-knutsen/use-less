using UseLess.Api;
using UseLess.EndToEndTest;
using UseLess.Messages;
using UseLess.Services.Api;
using UseLess.Services.Budgets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var store = new LocalStore();
var query = new Queries(store);
builder.Services
    .AddSingleton<IAggregateStore>(store)
    .AddSingleton<IQueryStore<ReadModels.Budget,Guid>>(store)
    .AddSingleton<IQueryService<ReadModels.Budget>>(query)
    .AddSingleton<IApplicationService,BudgetService>().AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
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
