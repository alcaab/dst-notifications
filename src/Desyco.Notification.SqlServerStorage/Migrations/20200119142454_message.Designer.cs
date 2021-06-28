﻿// <auto-generated />
using System;
using Desyco.Notification.SqlServerStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Desyco.Notification.SqlServerStorage.Migrations
{
    [DbContext(typeof(SqlServerNotificationDbContext))]
    [Migration("20200119142454_message")]
    partial class message
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Desyco.Notification.EntityFramework.AttachmentEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("MediaSubType")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("MediaType")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("MessageId")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("NotificationAttachment","dls");
                });

            modelBuilder.Entity("Desyco.Notification.EntityFramework.DeliveryErrorEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.Property<int>("DeliveryAttempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("ErrorTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageId")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("NotificationError","dls");
                });

            modelBuilder.Entity("Desyco.Notification.EntityFramework.MessageEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryAttempts")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Group")
                        .HasColumnType("nvarchar(36)")
                        .HasMaxLength(36);

                    b.Property<int>("NotificationMethod")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("TemplateKey")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("TextFormat")
                        .HasColumnType("int");

                    b.Property<int>("UrgencyLevel")
                        .HasColumnType("int");

                    b.Property<string>("_data")
                        .HasColumnName("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_from")
                        .HasColumnName("From")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_replyTo")
                        .HasColumnName("ReplyTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_to")
                        .HasColumnName("To")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notification","dls");
                });

            modelBuilder.Entity("Desyco.Notification.EntityFramework.AttachmentEntity", b =>
                {
                    b.HasOne("Desyco.Notification.EntityFramework.MessageEntity", null)
                        .WithMany("Attachments")
                        .HasForeignKey("MessageId");
                });

            modelBuilder.Entity("Desyco.Notification.EntityFramework.DeliveryErrorEntity", b =>
                {
                    b.HasOne("Desyco.Notification.EntityFramework.MessageEntity", null)
                        .WithMany("Errors")
                        .HasForeignKey("MessageId");
                });
#pragma warning restore 612, 618
        }
    }
}
