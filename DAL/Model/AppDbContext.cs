using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=MINGHUNGF\SQLEXPRESS;Initial Catalog=PushLand;Integrated Security=True");
        }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Customer_Information> Customer_Information { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<News_Image> News_Image { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Project>Project{ get; set; }
        public virtual DbSet<Project_Delete_History> Project_Delete_History { get; set; }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Role_Function>Role_Function { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<System_Image> System_Image { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        public virtual DbSet<PermissionView> PermissionView { get; set; }
        public virtual DbSet<CustomerView> CustomerView { get; set; }
        public virtual DbSet<Ward>Ward { get; set; }
        public virtual DbSet<District>District { get; set; }
        public virtual DbSet<City>City { get; set; }
        public virtual DbSet<Project_Document>Project_Document { get; set; }
    }
}
