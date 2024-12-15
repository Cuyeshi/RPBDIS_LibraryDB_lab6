﻿using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RPBDIS_LibraryDB_lab6.Models;

namespace RPBDIS_LibraryDB_lab6.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<LoanedBook> LoanedBooks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Reader> Readers { get; set; }


    }

}
