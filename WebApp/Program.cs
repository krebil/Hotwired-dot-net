using WebApp;
using HotwiredDotNet.Core.Extensions;
using Sockets.Socket;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.RegisterHotwiredDotNetCore();
builder.Services.AddWebSocketManager();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();  
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};
app.UseWebSockets(webSocketOptions);
var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
var streamTestHandler = serviceProvider.GetService<StreamTestHandler>();
if (streamTestHandler != null)
{
    app.MapWebSocketManager("/streamtesthandler", streamTestHandler);
}

app.Run();