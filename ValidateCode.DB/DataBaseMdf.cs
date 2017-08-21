namespace ValidateCode.DB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Model;
    public partial class DataBaseMdf : DbContext
    {
        public DataBaseMdf()
            : base("name=DbRepository")
        {
        }

        public virtual DbSet<admin_user> admin_user { get; set; }
        public virtual DbSet<app_user> app_user { get; set; }
        public virtual DbSet<app_user_bill> app_user_bill { get; set; }
        public virtual DbSet<cat_device> cat_device { get; set; }
        public virtual DbSet<invite_bill> invite_bill { get; set; }
        public virtual DbSet<phone> phone { get; set; }
        public virtual DbSet<project> project { get; set; }
        public virtual DbSet<project_log> project_log { get; set; }
        public virtual DbSet<project_task> project_task { get; set; }
        public virtual DbSet<sms_device> sms_device { get; set; }
        public virtual DbSet<sms_inbox> sms_inbox { get; set; }
        public virtual DbSet<sms_send> sms_send { get; set; }
        public virtual DbSet<user_token> user_token { get; set; }
        public virtual DbSet<withdrawals> withdrawals { get; set; }
        public virtual DbSet<recharge> recharge { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<invite_bill>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<sms_device>()
                .Property(e => e.config)
                .IsUnicode(false);

            modelBuilder.Entity<sms_inbox>()
                .Property(e => e.phone_text_pdu)
                .IsUnicode(false);
        }
    }
}
