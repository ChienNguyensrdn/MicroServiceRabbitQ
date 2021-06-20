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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveType>().ToTable("AT_FML", "SNP_CR");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveShift>().ToTable("OT_OTHER_LIST", "SNP_CR");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveSymbol>().ToTable("AT_TIME_MANUAL", "SNP_CR");
            modelBuilder.Entity<LeaveProcessService.Entities.OtherListType>().ToTable("OT_OTHER_LIST_TYPE", "SNP_CR");
        }
    }
}
