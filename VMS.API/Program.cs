using VMS.API.Helper.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string policyName = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigHelper.ConfigureService(builder);
try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors(policyName);

    app.UseAuthorization();

    app.MapControllers();

    ConfigHelper.MigrateDatabase(app);

    app.Run();
}
catch(Exception ex)
{
    var f = ex.Message.ToString();
}