﻿// <auto-generated />
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    partial class RestaurantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Domain.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DishType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("ServingTime")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Dishes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DishType = 1,
                            MaxQuantity = 1,
                            Name = "steak",
                            ServingTime = 1
                        },
                        new
                        {
                            Id = 2,
                            DishType = 2,
                            MaxQuantity = 2147483647,
                            Name = "potato",
                            ServingTime = 1
                        },
                        new
                        {
                            Id = 3,
                            DishType = 3,
                            MaxQuantity = 1,
                            Name = "wine",
                            ServingTime = 1
                        },
                        new
                        {
                            Id = 4,
                            DishType = 4,
                            MaxQuantity = 1,
                            Name = "cake",
                            ServingTime = 1
                        },
                        new
                        {
                            Id = 5,
                            DishType = 1,
                            MaxQuantity = 1,
                            Name = "egg",
                            ServingTime = 0
                        },
                        new
                        {
                            Id = 6,
                            DishType = 2,
                            MaxQuantity = 1,
                            Name = "toast",
                            ServingTime = 0
                        },
                        new
                        {
                            Id = 7,
                            DishType = 3,
                            MaxQuantity = 2147483647,
                            Name = "coffee",
                            ServingTime = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
