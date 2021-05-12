﻿// <auto-generated />
using System;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FinanceManager.Data.Migrations
{
    [DbContext(typeof(FinanceManagerDbContext))]
    partial class FinanceManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FinanceManager.Data.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Iban")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("RowVersion")
                        .IsRequired()
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("UpdatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<byte[]>("RowVersion")
                        .IsRequired()
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("UpdatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<double>("BalanceAfter")
                        .HasColumnType("float");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("FromAccountId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsRequired()
                        .HasColumnType("timestamp");

                    b.Property<int>("ToAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOnAt")
                        .HasColumnType("datetime2(7)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("FromAccountId");

                    b.HasIndex("ToAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("FinanceManager.Data.Entities.Transaction", b =>
                {
                    b.HasOne("FinanceManager.Data.Entities.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinanceManager.Data.Entities.Account", "FromAccount")
                        .WithMany("TransactionsFrom")
                        .HasForeignKey("FromAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FinanceManager.Data.Entities.Account", "ToAccount")
                        .WithMany("TransactionsTo")
                        .HasForeignKey("ToAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
