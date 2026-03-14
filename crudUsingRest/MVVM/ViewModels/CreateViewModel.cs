using crudUsingRest.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace crudUsingRest.MVVM.ViewModels;

public class CreateViewModel
{
    HttpClient _client;
    JsonSerializerOptions _option;
    string baseUrl = "https://69b58dbebe587338e7162461.mockapi.io";
    public ObservableCollection<Book> _books { get; set; } = new();

    public CreateViewModel()
    {
        _client = new HttpClient();
        _option = new JsonSerializerOptions 
        {
            WriteIndented = true,
        };
        LoadBooks();
    }

    private async void LoadBooks()
    {
        var url = $"{baseUrl}/Book";
        var response = await _client.GetAsync(url);
        if(response.IsSuccessStatusCode)
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                var data = await JsonSerializer.DeserializeAsync<List<Book>>(responseStream, _option);
                _books.Clear();

                foreach (var book in data)
                {
                    _books.Add(book);
                }
            }
        }
    }
}
