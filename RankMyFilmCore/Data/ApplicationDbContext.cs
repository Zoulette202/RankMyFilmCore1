using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore.Models;
using RankMyFilmCore.Models.AccountViewModels;

namespace RankMyFilmCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<UserModel> userModel { get; set; }
        public virtual DbSet<FriendsModel> friendsModel { get; set; }
        public virtual DbSet<FilmModel> filmModel { get; set; }

        public virtual DbSet<RankModel> rankModel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RankModel>().HasKey(r => new { r.idUser, r.idFilm });
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<RankMyFilmCore.Models.ApplicationUser> ApplicationUser { get; set; }

        //public DbSet<RankMyFilmCore.Models.AccountViewModels.RegisterViewModel> RegisterViewModel { get; set; }
    }
}
