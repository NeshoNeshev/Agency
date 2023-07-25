using Agency.Api.DTOModels.Journey;
using Agency.Api.DTOModels.Ticket;
using Agency.Api.DTOModels.Vehicle;
using Agency.Core.Contracts;
using Agency.Core.Services.JourneyServices;
using Agency.Core.Services.TicketServices;
using Agency.Core.Services.VehicleServices;
using Agency.Data.DB;
//using Agency.Data.Model;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IBusService, BusService>();
builder.Services.AddScoped<IBoatService, BoatService>();
builder.Services.AddScoped<IAirplaneService, AirplaneService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IJourneyService, JourneyService>();
builder.Services.AddScoped<ITicketNode, TicketNode>();
builder.Services.AddScoped<IJourneyNode, JourneyNode>();
builder.Services.AddScoped<IVehicleNode, VehicleNode>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddMvcCore().AddDataAnnotations();

//connect the database
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); ;
builder.Services.AddDbContext<AgencyDBContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
ServiceLifetime.Scoped);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    
    IServiceProvider serviceProvider = serviceScope.ServiceProvider;
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AgencyDBContext>();
    dbContext.Database.Migrate();

    

}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//added
app.UseRouting();

app.UseCors();
//
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
