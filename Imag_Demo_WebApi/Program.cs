
using Microsoft.AspNetCore.Mvc.Formatters; // IoutputFormatter, OutputFormatter
using Imag.Demo.Shared; // Customer
using Imag_Demo.WebApi.Repositories; // ICustomerRepository
using Swashbuckle.AspNetCore.SwaggerUI; // SubmitMethod
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore; // HttpLoggingFields

//database creation & seeding for demo putposes

string connString = Environment.GetEnvironmentVariable("connstring");
using (var ctx = new ImagDataContext(connString))
{
    bool canConnect = false;
    while (canConnect == false)
    {
        try
        {
            ctx.Database.EnsureCreated();
        }
        catch (Exception)
        {

            throw;
        }


        if (ctx.Database.CanConnect()) canConnect = true;

    }


    //check to not duplicate data
    if (ctx.Customers.Count() == 0)
    {
        ctx.Customers.Add(new Customer() { Name = "Super Rolnicze Sp. Z o.o", Adress = "F. Żukowa", NIP = "5743631775", });
        ctx.Customers.Add(new Customer() { Name = "Kwiatosława & Leona Hyper Sp. Z o.o.", Adress = "Rozalii Rzewuskiej", NIP = "2245236044", });
        ctx.Customers.Add(new Customer() { Name = "SuperpofelaliHyper.com", Adress = "Irlandzka", NIP = "8598580610", });
        ctx.Customers.Add(new Customer() { Name = "Ultra krótkie Słodycze s.c", Adress = "Dalsza", NIP = "7331045371", });
        ctx.Customers.Add(new Customer() { Name = "Morzytagaty Paka s.c.", Adress = "Sabinówek", NIP = "6486911880", });
        ctx.SaveChanges();
    } 
    

    
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ImagDataContext>(options =>
{
    options.UseSqlServer(connString);    
});

//builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    Console.WriteLine("Default output formatters:");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;
        if (mediaFormatter is null)
        {
            Console.WriteLine($" {formatter.GetType().Name}");
        }
        else // OutputFormatter class has SupportedMediaTypes
        {
            Console.WriteLine(" {0}, Media types: {1}",
            arg0: mediaFormatter.GetType().Name,
            arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
        }
    }
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096; // default is 32k
    options.ResponseBodyLogLimit = 4096; // default is 32k
});

builder.Services.AddHealthChecks()
  .AddDbContextCheck<ImagDataContext>()
  // execute SELECT 1 using the specified connection string 
  .AddSqlServer(connString);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
        "ImagDB Service API Version 1");
        c.SupportedSubmitMethods(new[] {
            SubmitMethod.Get,
            SubmitMethod.Post,
            SubmitMethod.Put,
            SubmitMethod.Delete });
    });
}
app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHealthChecks(path: "/howdoyoufeel");

//app.UseMiddleware<SecurityHeaders>();


app.MapControllers();

app.Run();


