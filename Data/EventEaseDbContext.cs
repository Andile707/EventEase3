using Azure.Identity;
using Eventease.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Eventease.Data
{
    public class EventEaseDbContext : DbContext
    {
        public DbSet<BookingModel> Bookings { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<VenueModel> Venues { get; set; }
        public DbSet<EventTypeModel> EventTypes { get; set; }
        public EventEaseDbContext(DbContextOptions<EventEaseDbContext> options) : base(options)
        {
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = new SqlConnection(
                Configuration.GetConnectionString("AzureSqlConnection")
            );

            var credential = new DefaultAzureCredential();
            var token = credential.GetToken(
                new Azure.Core.TokenRequestContext(new[] { "https://st10538326srv.database.windows.net/" })
            );

            connection.AccessToken = token.Token;
            optionsBuilder.UseSqlServer(connection);
        }*/



    }
}
