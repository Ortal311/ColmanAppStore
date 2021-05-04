﻿// <auto-generated />
using System;
using ColmanAppStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ColmanAppStore.Migrations
{
    [DbContext(typeof(ColmanAppStoreContext))]
    [Migration("20210502134309_User")]
    partial class User
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ColmanAppStore.Models.Apps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeveloperName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<float>("Raiting")
                        .HasColumnType("real");

                    b.Property<int?>("ReviewId")
                        .HasColumnType("int");

                    b.Property<float>("Size")
                        .HasColumnType("real");

                    b.Property<DateTime>("publishDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ReviewId");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("ColmanAppStore.Models.AppsImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("AppsImage");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Body")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Raiting")
                        .HasColumnType("real");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserNameId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserNameId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("ColmanAppStore.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Apps", b =>
                {
                    b.HasOne("ColmanAppStore.Models.Category", "Category")
                        .WithMany("Apps")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ColmanAppStore.Models.Review", "Review")
                        .WithMany()
                        .HasForeignKey("ReviewId");

                    b.Navigation("Category");

                    b.Navigation("Review");
                });

            modelBuilder.Entity("ColmanAppStore.Models.AppsImage", b =>
                {
                    b.HasOne("ColmanAppStore.Models.Apps", "App")
                        .WithMany("Images")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Review", b =>
                {
                    b.HasOne("ColmanAppStore.Models.User", "UserName")
                        .WithMany()
                        .HasForeignKey("UserNameId");

                    b.Navigation("UserName");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Apps", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("ColmanAppStore.Models.Category", b =>
                {
                    b.Navigation("Apps");
                });
#pragma warning restore 612, 618
        }
    }
}
