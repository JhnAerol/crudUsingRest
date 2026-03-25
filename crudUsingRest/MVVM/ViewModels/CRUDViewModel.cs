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
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;

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
    private string imagesBook;

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
        var exists = _books.Any(b =>
        b.title.Trim().ToLower() == Title.Trim().ToLower());

        if (exists)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                "Book title already exists!",
                "OK");

            return; 
        }


        var url = $"{baseUrl}/Book";

        var book = new Book
        {
            title = Title,
            author = Author,
            datePublished = DatePublished,
            isDeleted = false,
            imagesBook = ImagesBook
        };

        string json = JsonSerializer.Serialize(book, _option);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            LoadBooks();

            Title = string.Empty;
            Author = string.Empty;
            ImagesBook = string.Empty;
        }
    });

    public ICommand SelectImageCommand => new Command(async () =>
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Please select an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                var image = PlatformImage.FromStream(stream);
                if (image != null)
                {
                    var resizedImage = image.Downsize(400, true);
                    using var memoryStream = new MemoryStream();
                    resizedImage.Save(memoryStream, ImageFormat.Jpeg, 0.7f);
                    var bytes = memoryStream.ToArray();
                    ImagesBook = $"data:image/jpeg;base64,{Convert.ToBase64String(bytes)}";
                }
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to pick image: {ex.Message}", "OK");
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
        SelectedBook.imagesBook = ImagesBook;

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
            _softDeletedBooks.Remove(book);
        }

        LoadBooks();
        LoadSoftDeletedBooks();
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

    public ICommand RemoveFromSoftDeleteCommand => new Command(async () =>
    {
        if (SelectedBook == null)
            return;

        var url = $"{baseUrl}/Book/{SelectedBook.id}";

        SelectedBook.isDeleted = false;

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