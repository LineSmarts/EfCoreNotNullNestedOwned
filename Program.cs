using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EfCoreNotNullNestedOwned
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ExampleContext())
            {
                db.Add(new RootEntity());
                db.SaveChanges();
            }


            using (var db = new ExampleContext())
            {
                RootEntity rootEntity = db.RootEntities.First();

                NestedOwnedEntity? nestedOwned = rootEntity.NestedOwned;

                if (nestedOwned != null)
                    Console.WriteLine("This owned property suppose to be NULL, but it is unexpectedly not NULL.");

                SimpleOwnedEntity? simpleOwned = rootEntity.SimpleOwned;

                if (simpleOwned == null)
                    Console.WriteLine("This owned property suppose to be NULL, and it is NULL as expected.");

            }
        }
    }

    public class ExampleContext : DbContext
    {
        public DbSet<RootEntity> RootEntities { get; set; }
        
        public string DbPath { get; }

        public ExampleContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "blogging.db");
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }

    public class RootEntity
    {
        public Int32 Id { get; set; }

        public NestedOwnedEntity? NestedOwned { get; set; }

        public SimpleOwnedEntity? SimpleOwned { get; set; }
    }

    [Owned]
    public class NestedOwnedEntity
    {
        public SomeOwnedEntity? SomeOwned { get; set; }        
    }

    [Owned]
    public class SimpleOwnedEntity
    {
        public String? AProperty { get; set; }
    }

    [Owned]
    public class SomeOwnedEntity
    {
        public String? BProperty { get; set; }
    }
}
