using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadMeAppLecture.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Formats.Tar;
using ReadMeAppLecture.Services;

namespace ReadMeAppLecture.ViewModels
{
    public partial class BookViewModel : ObservableObject

    {
        [ObservableProperty]
        private ObservableCollection<Book> books = new() { };


        [ObservableProperty]
        private string bookEntry = "";
        [ObservableProperty]
        private string bookEntryBack = "";

        private async Task AddBook(string title, string epub, DateTime createdAt, DateTime updatedAt )
        {
            var card = new Book { Title = title, Epub = epub, CreatedAt = createdAt, UpdatedAt= updatedAt };
            //using (var dbContext = new CardContext){
            //dbContext.Add(wish);
            //await dbContext.SaveChangesAsync();
            //}q
            Books.Add(card);
            BookEntry = "";
            BookEntryBack = "";
        }
        public BookViewModel() {
            RefreshBooksFromDB();
        }

        private void RefreshBooksFromDB(BookContext? context = null)
        {
            Books.Clear();
            using (var dbContext = context ?? new BookContext())
            {
                foreach(var dbBook in dbContext.Books)
                {
                    Books.Add(dbBook);
                }
            }
        }

    }
}
