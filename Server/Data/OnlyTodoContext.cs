using Microsoft.EntityFrameworkCore;
using OnlyTodo.Models;

namespace OnlyTodo.Data;

public partial class OnlyTodoContext : DbContext
{
    public OnlyTodoContext()
    {
    }

    public OnlyTodoContext(DbContextOptions<OnlyTodoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TaskSchema> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskSchema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Completed).HasColumnName("completed");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
