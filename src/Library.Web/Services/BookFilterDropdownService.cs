using System;
using System.Collections.Generic;
using System.Linq;
using Library.Web.Code;
// using Library.Data;
using Library.Web.Models.BookViewModels;
using Library.Repo;
using Microsoft.EntityFrameworkCore;

namespace Library.Web.Services
{
    public class BookFilterDropdownService
    {
        LibraryDbContext context;
        public BookFilterDropdownService(LibraryDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DropdownTuple> GetFilterDropdownValues(BooksFilterBy filterBy)
        {
            switch (filterBy)
            {
                case BooksFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                case BooksFilterBy.Genre:
                    return context.Terms
                    .Include(t => t.TermSet)
                    .Where(t => t.TermSet.Name.Equals("genre"))
                    .Select(genre => new DropdownTuple { Value = genre.Id.ToString(), Text = genre.Name}).ToList();
                case BooksFilterBy.Author:
                    return context.Authors
                    .Select(author => new DropdownTuple { Value = author.Id.ToString(), Text = author.LastName +", " + author.FirstName }).ToList();

                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);

            }
        }

    }
}