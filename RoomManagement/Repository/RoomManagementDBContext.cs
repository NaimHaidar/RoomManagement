using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using RoomManagement.Repository.Models;

namespace RoomManagement.Repository;

public partial class RoomManagementDBContext : DbContext
{
    public RoomManagementDBContext()
    {
    }

    public RoomManagementDBContext(DbContextOptions<RoomManagementDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Attendee> Attendees { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Meeting> Meetings { get; set; }

    public virtual DbSet<MinutesOfMeeting> MinutesOfMeetings { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomFeature> RoomFeatures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC0727F32A29");

            entity.HasOne(d => d.Meeting).WithMany(p => p.Attachments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attachmen__Meeti__414EAC47");
        });

        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendee__3214EC07609B4A47");

            entity.HasOne(d => d.Meeting).WithMany(p => p.Attendees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendee__Meetin__3D7E1B63");

            entity.HasOne(d => d.User).WithMany(p => p.Attendees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendee__UserId__3E723F9C");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC0767173E2A");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC071D807D28");
        });

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Meeting__3214EC0769F3A369");

            entity.HasOne(d => d.Room).WithMany(p => p.Meetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Meeting__RoomId__33008CF0");

            entity.HasOne(d => d.User).WithMany(p => p.Meetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Meeting__UserId__320C68B7");
        });

        modelBuilder.Entity<MinutesOfMeeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MinutesO__3214EC0773C3E798");

            entity.HasOne(d => d.Meeting).WithMany(p => p.MinutesOfMeetings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MinutesOf__Meeti__36D11DD4");

            entity.HasOne(d => d.User).WithMany(p => p.MinutesOfMeetings).HasConstraintName("FK__MinutesOf__UserI__35DCF99B");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC073B5F038F");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__442B18F2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07ED155069");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC077F5923F4");

            entity.HasOne(d => d.Location).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__LocationId__2F2FFC0C");
        });

        modelBuilder.Entity<RoomFeature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoomFeat__3214EC076C2D1076");

            entity.HasOne(d => d.Feature).WithMany(p => p.RoomFeatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomFeatu__Featu__39AD8A7F");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomFeatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomFeatu__RoomI__3AA1AEB8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07AEFD0A0C");

            //entity.HasOne(d => d.Role).WithMany(p => p.Users).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__User__RoleId__2C538F61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
