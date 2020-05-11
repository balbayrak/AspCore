using AspCore.DataAccess.EntityFramework;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public partial class AspCoreTestDbContext : CoreDbContext
    {
        public AspCoreTestDbContext()
        {
        }

        public AspCoreTestDbContext(DbContextOptions<AspCoreTestDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
        }



        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonAddress> PersonAddress { get; set; }
        public virtual DbSet<PersonCv> PersonCv { get; set; }
        public virtual DbSet<PersonRole> PersonRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //migration sırasında aşagıdaki satır açık olmalıdır
           // optionsBuilder.UseSqlServer("Server=TESTSQLAGL04,44696;Database=AspCoreTestDb;MultipleActiveResultSets=true;User Id=testUserSQLAGL;Password=1a2s3d4f");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            base.OnModelCreating(modelBuilder);
        }
    }
}
