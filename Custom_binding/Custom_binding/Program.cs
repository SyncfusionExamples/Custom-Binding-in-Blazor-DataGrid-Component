using Custom_binding.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpHaV5CQmFJfFBmQmlbf1R0c0U3HVdTRHRcQl9jQX5UdEdgUXtYcnY=;Mgo+DSMBPh8sVXJ0S0J+XE9AflRBQmJJYVF2R2BJeFRydF9CZ0wgOX1dQl9gSXxTfkVkW31deHBSTmI=;ORg4AjUWIQA/Gnt2VVhkQlFacldJXnxIfkx0RWFab1l6dlVMZFtBNQtUQF1hSn5Rdk1jXn1cc3xRT2Zb;MTA4ODQ2M0AzMjMwMmUzNDJlMzBhVWw1KzUwV3hkMGFJNjhpanVGSVZ1V1pDUnNYN0MrNElscjBUVGk2OHFrPQ==;MTA4ODQ2NEAzMjMwMmUzNDJlMzBvTzE3YzU1bHAwNE1pSnNGamlXR2xjZXZpRjU2WlVOWjdENXFsNFRBbHFBPQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmBWf1NpR2NbfE53fldGalhWVAciSV9jS31TdEVrWXhccnddQ2ZaUw==;MTA4ODQ2NkAzMjMwMmUzNDJlMzBMa2c0ZmgxY3dRaUo3T2JjbkVTTk1ZQjdJZlpuTXEzUlBoN2JXY3hCQ3QwPQ==;MTA4ODQ2N0AzMjMwMmUzNDJlMzBrVWtwNTliVlB2bURsbGJuNmNXbTdBaW9VUmlBdEhqak41SE5XaU9SQ2pjPQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXnxIfkx0RWFab1l6dlVMZFtBNQtUQF1hSn5Rdk1jXn1cc3xTRWde;MTA4ODQ2OUAzMjMwMmUzNDJlMzBsSXhTaVNXM2w0dkM4QnFhMHhyT3JPa0h0N1UvREkzVkJJRkE5aGxVODNNPQ==;MTA4ODQ3MEAzMjMwMmUzNDJlMzBTaWlFcGFiVldkMXNHSkFRRWNFcnVOTU8zY1BZQjVxMFRNc1VUQlFwcU93PQ==;MTA4ODQ3MUAzMjMwMmUzNDJlMzBMa2c0ZmgxY3dRaUo3T2JjbkVTTk1ZQjdJZlpuTXEzUlBoN2JXY3hCQ3QwPQ==");

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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
