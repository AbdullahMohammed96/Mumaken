using Mumaken.Domain.Entities;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.Chat;
using Mumaken.Domain.Entities.Copon;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Persistence.Seeds;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Mumaken.Domain.Entities.Location;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.ViewModel.Notification;
using System.Reflection.Emit;

namespace Mumaken.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationDbUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LogExption> LogExption { get; set; }
        public DbSet<CommonQuestion> CommonQuestions { get; set; }
        public DbSet<SplashScreen> SplashScreens { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<DeviceId> DeviceIds { get; set; }
        public DbSet<NotifyUser> NotifyUsers { get; set; }
        public DbSet<NotifyDelegt> NotifyDelegts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Copon> Copon { get; set; }
        public DbSet<CoponRentalCompany> CoponRentalCompanies { get; set; }

        public DbSet<CoponUsed> CoponUsed { get; set; }
        public DbSet<ConnectUser> ConnectUser { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCar> OrderCars { get; set; }
        public DbSet<OrderDeliverCompany> OrderDeliverCompanies { get; set; }
        public DbSet<RenewOrderdata> RenewOrderdata { get; set; }
        public DbSet<HistoryNotify> HistoryNotify { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Distract> Distract { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<OrderCycleIntro> OrderCycleIntros { get; set; }
        public DbSet<DataForDeliverApp> DataForDeliverApps { get; set; }
        public DbSet<BankTransfer> BankTransfers { get; set; }
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public DbSet<AdminBankAccount> AdminBankAccounts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<GetAllNotificationViewModel> GetAllNotificationViewModels { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GetAllNotificationViewModel>(e => { e.HasNoKey().ToView(null); });

            builder.Entity<ApplicationDbUser>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<ContactUs>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Copon>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<City>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<CarCategory>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<CarType>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<CarModel>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Car>().HasQueryFilter(c => !c.IsDeleted);
            builder.Entity<Nationality>().HasQueryFilter(c => !c.IsDeleted);
            //builder.Seed();

            builder.Entity<ApplicationDbUser>()
            .HasMany(c => c.Sender)
            .WithOne(o => o.Sender)
            .HasForeignKey(o => o.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationDbUser>()
                       .HasMany(c => c.Receiver)
                       .WithOne(o => o.Receiver)
                       .HasForeignKey(o => o.ReceiverId)
                       .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<BankTransfer>()
               .Property(c => c.AdminBankId)
               .IsRequired(false);
            //builder.Entity<ApplicationDbUser>()
            //        .HasMany(c => c.Orders)
            //        //.WithOne(o => o.User)
            //        .HasForeignKey(o => o.UserId)
            //        .OnDelete(DeleteBehavior.NoAction);
        }

    }

}
