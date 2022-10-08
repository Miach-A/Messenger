﻿// <auto-generated />
using System;
using MessengerData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MessengerData.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221008075059_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MessengerModel.Chat", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("MessengerModel.DeletedMessage", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MessageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Date", "MessageGuid", "ChatGuid", "UserGuid");

                    b.HasIndex("ChatGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("DeletedMessage");
                });

            modelBuilder.Entity("MessengerModel.Message", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChatGuid1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CommentedMessageDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CommentedMessageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CommentedMessageGuid1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Date", "Guid");

                    b.HasIndex("ChatGuid");

                    b.HasIndex("ChatGuid1");

                    b.HasIndex("UserGuid");

                    b.HasIndex("CommentedMessageDate", "CommentedMessageGuid1");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("MessengerModel.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MessengerModel.UserChats", b =>
                {
                    b.Property<Guid>("ChatGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChatGuid", "UserGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("UserChats");
                });

            modelBuilder.Entity("MessengerModel.UserContacts", b =>
                {
                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContactGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserGuid", "ContactGuid");

                    b.HasIndex("ContactGuid");

                    b.ToTable("UserContacts");
                });

            modelBuilder.Entity("MessengerModel.DeletedMessage", b =>
                {
                    b.HasOne("MessengerModel.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatGuid")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.Message", "Message")
                        .WithMany()
                        .HasForeignKey("Date", "MessageGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessengerModel.Message", b =>
                {
                    b.HasOne("MessengerModel.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatGuid")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.Chat", null)
                        .WithMany("DeletedMessages")
                        .HasForeignKey("ChatGuid1");

                    b.HasOne("MessengerModel.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.Message", "CommentedMessage")
                        .WithMany()
                        .HasForeignKey("CommentedMessageDate", "CommentedMessageGuid1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("CommentedMessage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessengerModel.UserChats", b =>
                {
                    b.HasOne("MessengerModel.Chat", "Chat")
                        .WithMany("ChatUsers")
                        .HasForeignKey("ChatGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.User", "User")
                        .WithMany("UserChats")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessengerModel.UserContacts", b =>
                {
                    b.HasOne("MessengerModel.User", "Contact")
                        .WithMany("Contacts")
                        .HasForeignKey("ContactGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MessengerModel.User", "User")
                        .WithMany()
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contact");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MessengerModel.Chat", b =>
                {
                    b.Navigation("ChatUsers");

                    b.Navigation("DeletedMessages");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("MessengerModel.User", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("Messages");

                    b.Navigation("UserChats");
                });
#pragma warning restore 612, 618
        }
    }
}
