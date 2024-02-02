using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Entities;

public partial class BookStoreDbContext : DbContext
{
    public BookStoreDbContext()
    {
    }

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__books__490D1AE1D7206F62");

            entity.ToTable("books");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category");
            entity.Property(e => e.CopiesInUse).HasColumnName("copies_in_use");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Isbn)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("isbn");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TotalCopies).HasColumnName("total_copies");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public IEnumerable<Book> BooksByAuthor(string searchValue)
    {
        return Books.FromSqlInterpolated($"SELECT * FROM books WHERE LOWER(first_name) LIKE '%{searchValue.ToLower()}%' OR LOWER(last_name) LIKE '%{searchValue.ToLower()}%'").ToList();
    }

    public IEnumerable<Book> BooksByISBN(string searchValue)
    {
        return Books.FromSqlInterpolated($"SELECT * FROM books WHERE isbn LIKE '%{searchValue}%1").ToList();
    }

    public IEnumerable<Book> BooksByAvailableCopies(string searchValue)
    {
        return Books.FromSqlInterpolated($"SELECT * FROM books WHERE total_copies - copies_in_use > {int.Parse(searchValue)}").ToList();
    }

    public IEnumerable<Book> BooksByTotalCopies(string searchValue)
    {
        return Books.FromSqlInterpolated($"SELECT * FROM books WHERE total_copies > {int.Parse(searchValue)}").ToList();
    }
}
