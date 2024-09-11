using Microsoft.AspNetCore.HttpLogging;
using WebhookService.Interfaces;
using WebhookService.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IWebHookService, WebHookService>(); //Para a instancia ser mantida mesmo apos uma nova solicitacao


//Transient: O contêiner sempre retorna uma instância diferente, sendo criada a cada vez que é solicitada. 
//Scoped: O contêiner retorna uma nova instância por solicitação, sendo criada quando a solicitação é recebida e descartada quando é concluída

builder.Services.AddW3CLogging(log =>
{
    log.LoggingFields = W3CLoggingFields.All;
    log.FlushInterval = TimeSpan.FromSeconds(5);
});

var app = builder.Build();
app.UseW3CLogging();    //Ativa o log

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
