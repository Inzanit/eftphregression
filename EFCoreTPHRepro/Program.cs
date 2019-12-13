﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace EFCoreTPHRepro
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Migrating EFCore...");

            await using (var db = new EntityContext())
            {
                await db.Database.MigrateAsync();
            }

            Console.WriteLine("Migrated!");

            Console.WriteLine("Adding 4 shapes to the database...");

            await using (var db = new EntityContext())
            {
                db.Shapes.Add(new Circle
                {
                    Name = Guid.NewGuid().ToString("n"),
                });
                db.Shapes.Add(new Rectangle
                {
                    Name = Guid.NewGuid().ToString("n"),
                });
                db.Shapes.Add(new Square
                {
                    Name = Guid.NewGuid().ToString("n"),
                });
                db.Shapes.Add(new Pentagon
                {
                    Name = Guid.NewGuid().ToString("n"),
                });

                await db.SaveChangesAsync();
            }

            await using (var db = new EntityContext())
            {
                var count = await db.Shapes.CountAsync();

                Console.WriteLine($"We have {count} shapes in the EFCore database!");
            }

            Console.WriteLine("Done EFCore!");
            Console.ReadLine();
        }
    }

    public class EntityContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=EFCoreTPHRepo;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Shape> Shapes { get; set; }
        public virtual DbSet<Circle> Cirlces { get; set; }
        public virtual DbSet<Rectangle> Rectangles { get; set; }
        public virtual DbSet<Pentagon> Pentagons { get; set; }
    }

    public abstract class Shape
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Circle : Shape
    {
    }

    public class Rectangle : Shape
    {
    }

    public class Square : Shape
    {
    }

    public class Pentagon : Shape
    {
    }
}
