using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// build
var crono = new System.Diagnostics.Stopwatch();
crono.Start();
MoogleEngine.ModificarTexto.Normalizar();
System.Console.WriteLine(crono.ElapsedMilliseconds /1000 + "secs NORMALIZAR Done");
MoogleEngine.ModificarTexto.TermFrecuency();
System.Console.WriteLine(crono.ElapsedMilliseconds /1000 + "secs TF Done");
MoogleEngine.ModificarTexto.InverseDocFrecuency();
System.Console.WriteLine(crono.ElapsedMilliseconds /1000 + "secs IDF Done");
MoogleEngine.ModificarTexto.TermFrecuency_InverseDocFrecuency();
System.Console.WriteLine(crono.ElapsedMilliseconds /1000 + "secs TFxIDF Done");
MoogleEngine.ModificarTexto.Snippet();
System.Console.WriteLine(crono.ElapsedMilliseconds /1000 + "secs snippets Done");
app.Run();
crono.Stop();
