using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<MoviesTheaters> MoviesTheaters { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<MoviesGenres>()
                .HasKey(mg => new { mg.GenreId, mg.MovieId });

            mb.Entity<MoviesActors>()
                .HasKey(ma => new { ma.ActorId, ma.MovieId });

            mb.Entity<MoviesTheaters>()
                .HasKey(mt => new { mt.TheaterId, mt.MovieId });

            SeedData(mb); 

            base.OnModelCreating(mb); 
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var rolAdminId = "735217af-72b5-4a8b-92b7-efe779101f16";
            var userAdminId = "4d686c30-d6af-4d54-b69e-8bc516219f86";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passHasher = new PasswordHasher<IdentityUser>();

            var adminUser = new IdentityUser()
            {
                Id = userAdminId,
                UserName = "ymora@hotmail.com",
                NormalizedUserName = "ymora@hotmail.com",
                Email = "ymora@hotmail.com",
                NormalizedEmail = "ymora@hotmail.com",
                PasswordHash = passHasher.HashPassword(null,"Facil.123456!")
            };

            modelBuilder.Entity<IdentityUser>()
                .HasData(adminUser);

            modelBuilder.Entity<IdentityRole>()
                .HasData(rolAdmin);

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasData(new IdentityUserClaim<string>()
                {
                    Id = 1,
                    ClaimType = ClaimTypes.Role,
                    UserId = userAdminId,
                    ClaimValue = "Admin"
                }
                );

            var aventura = new Genre() { Id = 4, Name = "Aventura" };
            var animation = new Genre() { Id = 5, Name = "Animación" };
            var suspenso = new Genre() { Id = 6, Name = "Suspenso" };
            var romance = new Genre() { Id = 7, Name = "Romance" };

            modelBuilder.Entity<Genre>()
                .HasData(new List<Genre>
                {
                    aventura, animation, suspenso, romance
                });

            var jimCarrey = new Actor() { Id = 5, Name = "Jim Carrey", DateOfBirth = new DateTime(1962, 01, 17) };
            var robertDowney = new Actor() { Id = 6, Name = "Robert Downey Jr.", DateOfBirth = new DateTime(1965, 4, 4) };
            var chrisEvans = new Actor() { Id = 7, Name = "Chris Evans", DateOfBirth = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            var endgame = new Movie()
            {
                Id = 2,
                Title = "Avengers: Endgame",
                InTheaters = true,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var iw = new Movie()
            {
                Id = 3,
                Title = "Avengers: Infinity Wars",
                InTheaters = false,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var sonic = new Movie()
            {
                Id = 4,
                Title = "Sonic the Hedgehog",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 02, 28)
            };
            var emma = new Movie()
            {
                Id = 5,
                Title = "Emma",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 02, 21)
            };
            var wonderwoman = new Movie()
            {
                Id = 6,
                Title = "Wonder Woman 1984",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Movie>()
                .HasData(new List<Movie>
                {
                    endgame, iw, sonic, emma, wonderwoman
                });

            modelBuilder.Entity<MoviesGenres>().HasData(
                new List<MoviesGenres>()
                {
                    new MoviesGenres(){MovieId = endgame.Id, GenreId = suspenso.Id},
                    new MoviesGenres(){MovieId = endgame.Id, GenreId = aventura.Id},
                    new MoviesGenres(){MovieId = iw.Id, GenreId = suspenso.Id},
                    new MoviesGenres(){MovieId = iw.Id, GenreId = aventura.Id},
                    new MoviesGenres(){MovieId = sonic.Id, GenreId = aventura.Id},
                    new MoviesGenres(){MovieId = emma.Id, GenreId = suspenso.Id},
                    new MoviesGenres(){MovieId = emma.Id, GenreId = romance.Id},
                    new MoviesGenres(){MovieId = wonderwoman.Id, GenreId = suspenso.Id},
                    new MoviesGenres(){MovieId = wonderwoman.Id, GenreId = aventura.Id},
                });

            modelBuilder.Entity<MoviesActors>().HasData(
                new List<MoviesActors>()
                {
                    new MoviesActors(){MovieId = endgame.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = endgame.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = iw.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
                });
        }

    }
}