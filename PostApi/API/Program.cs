using API.Swagger;
using Application;
using Core.Authentication;
using Core.HttpLogic;
using Core.Logging;
using Core.Mapping;
using Core.Middlewares;
using Core.TraceIdLogic;
using Persistence;
using ProfileConnection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Host.AddLoggingServices();
builder.Services.AddMappingServices();

builder.Services.TryAddTraceId();

builder.Services.AddGlobalExceptionHandlerServices();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddAuthenticationHelper();

builder.Services.AddHttpRequestService();

builder.Services.AddProfileConnectionServices(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

