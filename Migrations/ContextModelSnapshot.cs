﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Scratch;

#nullable disable

namespace Scratch.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Scratch.Entity", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("binary(16)")
                        .HasDefaultValueSql("(UUID_TO_BIN(UUID(), 1))");

                    b.Property<byte[]>("UUID")
                        .IsRequired()
                        .HasColumnType("binary(16)");

                    b.HasKey("Id");

                    b.ToTable("Entities");
                });
#pragma warning restore 612, 618
        }
    }
}
