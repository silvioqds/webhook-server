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


//Transient: O cont�iner sempre retorna uma inst�ncia diferente, sendo criada a cada vez que � solicitada. 
//Scoped: O cont�iner retorna uma nova inst�ncia por solicita��o, sendo criada quando a solicita��o � recebida e descartada quando � conclu�da

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
