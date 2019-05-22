﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SmartMeal.Data.Data;

namespace SmartMeal.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190522194314_IngredientDeleteOnCascade")]
    partial class IngredientDeleteOnCascade
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("SmartMeal.Models.Models.Ingredient", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Amount");

                    b.Property<int>("Metric");

                    b.Property<long?>("ProductId");

                    b.Property<long?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Photo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentType");

                    b.Property<string>("Filename");

                    b.Property<long>("Size");

                    b.Property<long?>("UploadById");

                    b.HasKey("Id");

                    b.HasIndex("UploadById");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedById");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<long?>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ImageId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Recipe", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedById");

                    b.Property<string>("Description");

                    b.Property<long?>("ImageId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ImageId");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Timetable", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("MealDay");

                    b.Property<int>("MealTime");

                    b.Property<long?>("OwnerId");

                    b.Property<long?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Timetables");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<long>("FacebookId");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Ingredient", b =>
                {
                    b.HasOne("SmartMeal.Models.Models.Product", "Product")
                        .WithMany("Ingredients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartMeal.Models.Models.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Photo", b =>
                {
                    b.HasOne("SmartMeal.Models.Models.User", "UploadBy")
                        .WithMany("Photos")
                        .HasForeignKey("UploadById");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Product", b =>
                {
                    b.HasOne("SmartMeal.Models.Models.User", "CreatedBy")
                        .WithMany("Products")
                        .HasForeignKey("CreatedById");

                    b.HasOne("SmartMeal.Models.Models.Photo", "Image")
                        .WithMany("Products")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Recipe", b =>
                {
                    b.HasOne("SmartMeal.Models.Models.User", "CreatedBy")
                        .WithMany("Recipes")
                        .HasForeignKey("CreatedById");

                    b.HasOne("SmartMeal.Models.Models.Photo", "Image")
                        .WithMany("Recipes")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("SmartMeal.Models.Models.Timetable", b =>
                {
                    b.HasOne("SmartMeal.Models.Models.User", "Owner")
                        .WithMany("Timetables")
                        .HasForeignKey("OwnerId");

                    b.HasOne("SmartMeal.Models.Models.Recipe", "Recipe")
                        .WithMany("Timetables")
                        .HasForeignKey("RecipeId");
                });
#pragma warning restore 612, 618
        }
    }
}
