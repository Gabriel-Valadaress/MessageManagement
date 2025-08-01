using System;
using MessageManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageManagement.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Message> Messages => Set<Message>();
}