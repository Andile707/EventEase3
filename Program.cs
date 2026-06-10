using Azure.Identity;
using Azure.Storage.Blobs;
using Eventease.Data;
using Eventease.Models;
using Eventease.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Eventease
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);




            // Add services to the container.
            builder.Services.AddControllersWithViews();


            // Add the database context to the services container

            

            builder.Services.AddDbContext<EventEaseDbContext>(options =>
                 options.UseSqlServer(
                     builder.Configuration.GetConnectionString("AzureSqlConnection")));








            //var blobStorageConnectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
            //var blobContainerName = builder.Configuration["AzureBlobContainerName"];

            builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("AzureOptions"));
            //builder.Services.AddSingleton<IAzureService>(new AzureService(blobStorageConnectionString, blobContainerName));
            builder.Services.AddScoped<IAzureService, AzureBlobService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}