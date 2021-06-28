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
        public  DbSet<LeaveProcessService.Entities.LeaveManual> LeaveManuals { get; set; }
        public  DbSet<LeaveProcessService.Entities.LeaveSymbol > LeaveSymbols   { get; set; }
        public  DbSet<LeaveProcessService.Entities.LeaveTime> LeaveTimes { get; set; }
        public  DbSet<LeaveProcessService.Entities.OtherListType> OtherListTypes { get; set; }
        public DbSet<LeaveProcessService.Entities.Leavesheet> LeaveSheets { get; set; }
        public DbSet<LeaveProcessService.Entities.LeavesheetDetail> LeavesheetDetails { get; set; }
        public DbSet<LeaveProcessService.Entities.Employee> Employees { get; set; }
        public DbSet<LeaveProcessService.Entities.Holiday> Holidays { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveManual>().ToTable("AT_FML");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().ToTable("AT_HOLIDAY");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveTime>().ToTable("OT_OTHER_LIST");
            modelBuilder.Entity<LeaveProcessService.Entities.LeaveSymbol>().ToTable("AT_TIME_MANUAL");
            modelBuilder.Entity<LeaveProcessService.Entities.OtherListType>().ToTable("OT_OTHER_LIST_TYPE");
            modelBuilder.Entity<LeaveProcessService.Entities.Leavesheet>().ToTable("AT_LEAVESHEET_PH");
            modelBuilder.Entity<LeaveProcessService.Entities.LeavesheetDetail>().ToTable("AT_LEAVESHEET");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().ToTable("HU_EMPLOYEE");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().Property(i => i.employeeCode).HasColumnName("EMPLOYEE_CODE");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().Property(i => i.Id).HasColumnName("ID");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().Property(i => i.fullName).HasColumnName("FULLNAME_VN");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().Property(i => i.is3B).HasColumnName("IS_3B");
            modelBuilder.Entity<LeaveProcessService.Entities.Employee>().Property(i => i.terLastDate).HasColumnName("TER_LAST_DATE");
            //modelBuilder
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.code).HasColumnName("CODE");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.id).HasColumnName("ID");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.nameVn).HasColumnName("NAME_VN");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.isNb).HasColumnName("IS_NB");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.workingDay).HasColumnName("WORKINGDAY");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.note).HasColumnName("NOTE");
            modelBuilder.Entity<LeaveProcessService.Entities.Holiday>().Property(i => i.actFlg).HasColumnName("ACTFLG");
        }
    }
}
