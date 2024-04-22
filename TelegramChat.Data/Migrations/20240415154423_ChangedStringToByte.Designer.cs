﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramChat.Data;

#nullable disable

namespace TelegramChat.Data.Migrations
{
    [DbContext(typeof(TelegramChatDbContext))]
    [Migration("20240415154423_ChangedStringToByte")]
    partial class ChangedStringToByte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TelegramChat.Domain.MessageHistory", b =>
                {
                    b.Property<long>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("MessageID"));

                    b.Property<long>("Id1")
                        .HasColumnType("bigint");

                    b.Property<long>("Id2")
                        .HasColumnType("bigint");

                    b.Property<byte[]>("Text")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("MessageID");

                    b.HasIndex("Id1", "Id2");

                    b.ToTable("MessageHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
