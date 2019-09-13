﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minenetred.web.Context;

namespace Minenetred.web.Migrations
{
    [DbContext(typeof(MinenetredContext))]
    partial class MinenetredContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Minenetred.web.Context.ContextModels.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("LastKeyUpdatedDate");

                    b.Property<string>("RedmineKey");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
