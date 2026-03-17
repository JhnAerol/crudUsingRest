using CommunityToolkit.Mvvm.ComponentModel;
using crudUsingRest.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace crudUsingRest.MVVM.ViewModels;

public partial class CRUDViewModel : ObservableObject
{
    HttpClient _client;
    JsonSerializerOptions _option;
    string baseUrl = "https://69b58dbebe587338e7162461.mockapi.io";
    public ObservableCollection<Book> _books { get; set; } = new();
    public ObservableCollection<Book> _softDeletedBooks { get; set; } = new();

    [ObservableProperty]
    private string title;
    [ObservableProperty]
    private string author;
    [ObservableProperty]
    private DateTime datePublished = DateTime.Now;
    [ObservableProperty]
    private string isDeleted;

    [ObservableProperty]
    private Book selectedBook;

    public CRUDViewModel()
    {
        _client = new HttpClient();
        _option = new JsonSerializerOptions 
        {
            WriteIndented = true,
        };
        LoadBooks();
        LoadSoftDeletedBooks();
    }

    public async void LoadSoftDeletedBooks()
    {
        var url = $"{baseUrl}/Book";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var data = await JsonSerializer.DeserializeAsync<List<Book>>(responseStream, _option);
                _softDeletedBooks.Clear();

                foreach (var book in data.Where(d => d.isDeleted == true))
                {
                    _softDeletedBooks.Add(book);
                }
            }
        }
    }

    public async void LoadBooks()
    {
        var url = $"{baseUrl}/Book";
        var response = await _client.GetAsync(url);
        if(response.IsSuccessStatusCode)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var data = await JsonSerializer.DeserializeAsync<List<Book>>(responseStream, _option);
                _books.Clear();

                foreach (var book in data.Where(d => d.isDeleted == false))
                {
                    _books.Add(book);
                }
            }
        }
    }

    public ICommand CreateBookCommand => new Command(async () =>
    {
        var url = $"{baseUrl}/Book";

        var book = new Book
        {
            title = Title,
            author = Author,
            datePublished = DatePublished,
            isDeleted = false
        };

        string json = JsonSerializer.Serialize(book, _option);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            LoadBooks();
        }
    });

    public ICommand UpdateBookCommand => new Command(async () =>
    {
        if (SelectedBook == null)
            return;

        var url = $"{baseUrl}/Book/{SelectedBook.id}";

        SelectedBook.title = Title;
        SelectedBook.author = Author;
        SelectedBook.datePublished = DatePublished;

        string json = JsonSerializer.Serialize(SelectedBook, _option);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            LoadBooks();
        }
    });

    public ICommand DeleteBookCommand => new Command<Book>(async (book) =>
    {
        if (book == null)
            return;

        var url = $"{baseUrl}/Book/{book.id}";

        var response = await _client.DeleteAsync(url);

        if (response.IsSuccessStatusCode)
        {
            _books.Remove(book);
        }
    });

    public ICommand SoftDeleteCommand => new Command(async () =>
    {
        if (SelectedBook == null)
            return;

        var url = $"{baseUrl}/Book/{SelectedBook.id}";

        SelectedBook.isDeleted = true;

        string json = JsonSerializer.Serialize(SelectedBook, _option);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            LoadBooks();
            LoadSoftDeletedBooks();
        }

    });
}