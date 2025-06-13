using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

// DbContext is an object - referencing to the current db session
public class GameStoreContext(DbContextOptions<GameStoreContext> options)
        : DbContext(options)
{
    // Dbset is an object to the actual table in relational db, like querying and save instances
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genre => Set<Genre>();

    // via this function we can populate the static data 
    // ONLY FOR THOSE ENTITIES WHERE DATA IS PRACTICALLY STATIC AND NON-CHANGABLE 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new { Id = 1, Name = "Figthing" },
            new { Id = 2, Name = "Kids & Family" },
            new { Id = 3, Name = "RolePlaying" },
            new { Id = 5, Name = "Sports" }
        );

        // modelBuilder.Entity<Game>().HasData(
        //     new { Id = 1, Name = "" }
        // );
    }
}
