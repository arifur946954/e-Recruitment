using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataModel.JobEntityModel.HrJobTable
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAcmp> TAcmps { get; set; } = null!;
        public virtual DbSet<TDept> TDepts { get; set; } = null!;
        public virtual DbSet<TDist> TDists { get; set; } = null!;
        public virtual DbSet<TDivision> TDivisions { get; set; } = null!;
        public virtual DbSet<TDsig> TDsigs { get; set; } = null!;
        public virtual DbSet<TUpzl> TUpzls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=erpprd)(PORT=1525))(CONNECT_DATA=(SERVICE_NAME=hrdpdb)));User Id=hr_job_app;Password=hr_job_it;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HR_JOB_APP")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<TAcmp>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_ACMP");

                entity.ToTable("T_ACMP", "HR");

                entity.HasIndex(e => e.AcmpText, "UK_TEXT_ACMP")
                    .IsUnique();

                entity.Property(e => e.Oid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.AcmpActv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_ACTV");

                entity.Property(e => e.AcmpAdd1)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_ADD1");

                entity.Property(e => e.AcmpBnam)
                    .HasMaxLength(200)
                    .HasColumnName("ACMP_BNAM");

                entity.Property(e => e.AcmpCcom)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_CCOM");

                entity.Property(e => e.AcmpMcom)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_MCOM");

                entity.Property(e => e.AcmpName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_NAME");

                entity.Property(e => e.AcmpSeqn)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACMP_SEQN");

                entity.Property(e => e.AcmpText)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ACMP_TEXT");

                entity.Property(e => e.Edat)
                    .HasColumnType("DATE")
                    .HasColumnName("EDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.Idat)
                    .HasColumnType("DATE")
                    .HasColumnName("IDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.ItaxFlag)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ITAX_FLAG")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.SlocOid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SLOC_OID");

                entity.Property(e => e.Udt)
                    .HasColumnType("DATE")
                    .HasColumnName("UDT")
                    .HasDefaultValueSql("to_date(sysdate, 'dd/mm/yyyy')");
            });

            modelBuilder.Entity<TDept>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_DEPT");

                entity.ToTable("T_DEPT", "HR");

                entity.HasIndex(e => e.DeptText, "UK_TEXT_DEPT")
                    .IsUnique();

                entity.Property(e => e.Oid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.DeptAcmp)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_ACMP");

                entity.Property(e => e.DeptActv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_ACTV")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.DeptBnam)
                    .HasMaxLength(100)
                    .HasColumnName("DEPT_BNAM");

                entity.Property(e => e.DeptName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_NAME")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DeptText)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_TEXT");

                entity.Property(e => e.Edat)
                    .HasColumnType("DATE")
                    .HasColumnName("EDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("EUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.Idat)
                    .HasColumnType("DATE")
                    .HasColumnName("IDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.InvFlag)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("INV_FLAG")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.NavFlag)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("NAV_FLAG")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SptvFlag)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SPTV_FLAG")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Udt)
                    .HasColumnType("DATE")
                    .HasColumnName("UDT")
                    .HasDefaultValueSql("to_date(sysdate,'dd/mm/yyyy')");
            });

            modelBuilder.Entity<TDist>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("DIST_OID_PK");

                entity.ToTable("T_DIST", "HR");

                entity.Property(e => e.Oid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.DistBnam)
                    .HasMaxLength(200)
                    .HasColumnName("DIST_BNAM");

                entity.Property(e => e.DistName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DIST_NAME");

                entity.Property(e => e.DistText)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DIST_TEXT");

                entity.Property(e => e.DivOid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DIV_OID");

                entity.Property(e => e.DtInserted)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_INSERTED");

                entity.Property(e => e.DtModified)
                    .HasColumnType("DATE")
                    .HasColumnName("DT_MODIFIED");

                entity.Property(e => e.Euser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EUSER");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("'Y'");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IUSER");

                entity.Property(e => e.Roguid)
                    .HasMaxLength(32)
                    .HasColumnName("ROGUID");
            });

            modelBuilder.Entity<TDivision>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("T_DIVISION", "HR");

                entity.HasIndex(e => e.Oid, "T_DIVISION_PK")
                    .IsUnique();

                entity.Property(e => e.Active)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVE")
                    .HasDefaultValueSql("'Y'");

                entity.Property(e => e.Cancelby)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CANCELBY");

                entity.Property(e => e.Cancelon)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCELON");

                entity.Property(e => e.Cancelpc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CANCELPC");

                entity.Property(e => e.Createby)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATEBY");

                entity.Property(e => e.Createon)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATEON");

                entity.Property(e => e.Createpc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATEPC");

                entity.Property(e => e.DivName)
                    .HasMaxLength(50)
                    .HasColumnName("DIV_NAME");

                entity.Property(e => e.DivNameBn)
                    .HasMaxLength(50)
                    .HasColumnName("DIV_NAME_BN");

                entity.Property(e => e.Oid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.Updateby)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UPDATEBY");

                entity.Property(e => e.Updateon)
                    .HasColumnType("DATE")
                    .HasColumnName("UPDATEON");

                entity.Property(e => e.Updatepc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UPDATEPC");
            });

            modelBuilder.Entity<TDsig>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("PK_DSIG");

                entity.ToTable("T_DSIG", "HR");

                entity.HasIndex(e => e.DsigText, "UK_TEXT_DSIG")
                    .IsUnique();

                entity.Property(e => e.Oid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.DsigActv)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DSIG_ACTV")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DsigBnam)
                    .HasMaxLength(200)
                    .HasColumnName("DSIG_BNAM");

                entity.Property(e => e.DsigName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DSIG_NAME");

                entity.Property(e => e.DsigSeqn)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DSIG_SEQN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.DsigText)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("DSIG_TEXT");

                entity.Property(e => e.Edat)
                    .HasColumnType("DATE")
                    .HasColumnName("EDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.Euser)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("EUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.Idat)
                    .HasColumnType("DATE")
                    .HasColumnName("IDAT")
                    .HasDefaultValueSql("sysdate");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IUSER")
                    .HasDefaultValueSql("user");

                entity.Property(e => e.NavSeqn)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NAV_SEQN")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Udt)
                    .HasColumnType("DATE")
                    .HasColumnName("UDT")
                    .HasDefaultValueSql("to_date(sysdate,'dd/mm/yyyy')");
            });

            modelBuilder.Entity<TUpzl>(entity =>
            {
                entity.HasKey(e => e.Oid)
                    .HasName("UPZL_OID_PK");

                entity.ToTable("T_UPZL", "HR");

                entity.Property(e => e.Oid)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("OID");

                entity.Property(e => e.Edat)
                    .HasColumnType("DATE")
                    .HasColumnName("EDAT");

                entity.Property(e => e.Euser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("EUSER");

                entity.Property(e => e.Idat)
                    .HasColumnType("DATE")
                    .HasColumnName("IDAT");

                entity.Property(e => e.Iuser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IUSER");

                entity.Property(e => e.UpzlActv)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("UPZL_ACTV")
                    .HasDefaultValueSql("'Y'");

                entity.Property(e => e.UpzlBnam)
                    .HasMaxLength(100)
                    .HasColumnName("UPZL_BNAM");

                entity.Property(e => e.UpzlDist)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UPZL_DIST");

                entity.Property(e => e.UpzlName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UPZL_NAME");

                entity.Property(e => e.UpzlText)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("UPZL_TEXT");

                entity.HasOne(d => d.UpzlDistNavigation)
                    .WithMany(p => p.TUpzls)
                    .HasForeignKey(d => d.UpzlDist)
                    .HasConstraintName("UPZL_DIST_FK");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
