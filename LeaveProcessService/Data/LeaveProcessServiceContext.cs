using Oracle.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace LeaveProcessService.Data
{
    public class LeaveProcessServiceContext:DbContext 
    {
        public LeaveProcessServiceContext(DbContextOptions<LeaveProcessServiceContext> options)
           : base(options)
        {
        }
        public  DbSet<LeaveProcessService.Entities.LeaveType> LeaveTypes { get; set; }
        public  DbSet<LeaveProcessService.Entities.LeaveSymbol > LeaveSymbols   { get; set; }
        public  DbSet<LeaveProcessService.Entities.LeaveShift> LeaveShifts { get; set; }
        public  DbSet<LeaveProcessService.Entities.OtherListType> OtherListTypes { get; set; }
        public DbSet<LeaveProcessService.Entities.Leavesheet> LeaveSheets { get; set; }
        public DbSet<LeaveProcessService.Entities.LeavesheetDetail> LeavesheetDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveType>().ToTable("AT_FML");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveShift>().ToTable("OT_OTHER_LIST");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveSymbol>().ToTable("AT_TIME_MANUAL");
            modelBuilder.Entity<LeaveProcessService.Entities.OtherListType>().ToTable("OT_OTHER_LIST_TYPE");
            modelBuilder.Entity<LeaveProcessService.Entities.Leavesheet>().ToTable("AT_LEAVESHEET_PH");
            modelBuilder.Entity<LeaveProcessService.Entities.LeavesheetDetail>().ToTable("AT_LEAVESHEET");
            modelBuilder.Entity<LeaveProcessService.Entities.LeavesheetDetail>()
                .HasOne(p => p.Leavesheet)
                .WithMany(b => b.LeavesheetDetails)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
