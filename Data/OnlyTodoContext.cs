using Microsoft.EntityFrameworkCore;
using TodoTask = OnlyTodo.Models.Task;

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

    public virtual DbSet<TodoTask> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Database=only_todo_dev; Username=dev; Password=dev");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoTask>(entity =>
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
