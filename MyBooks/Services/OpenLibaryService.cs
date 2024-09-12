using Data.Data;
using MyBooks.Services.Data.Enums;
using Newtonsoft.Json.Linq;

namespace MyBooks.Services
{
    public class OpenLibaryService
    {
        private static string host = "https://openlibrary.org/";
        private readonly HttpClient _httpClient;
        private readonly MyBooksDbContext _context;

        public OpenLibaryService(HttpClient httpClient, MyBooksDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<List<string>> Query(string query, SearchTypes searchType)
        {
            string sanitizedQuery = query.Replace(" ", "+");

            string path = searchType switch
            {
                SearchTypes.TITLE => String.Format("search.json?title={0}", sanitizedQuery),
                SearchTypes.AUTHOR => "search.json?author=",
                _ => throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null)
            };

            string resultLimiter = "&limit=10";

            string parametizedQuery = host + path + resultLimiter;

            var response = await _httpClient.GetAsync(parametizedQuery);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return _formatResponse(responseString);
            }

            if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
            {
                var redirectUrl = response.Headers.Location.ToString();
                response = await _httpClient.GetAsync(redirectUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    return _formatResponse(responseString);
                }
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Unexpected response from Open Library API: {responseBody}");
        }

        private List<string> _formatResponse(string content)
        {
            var parsedObject = JObject.Parse(content);

            if (parsedObject["docs"] == null)
            {
                return new List<string>();
            }

            var books = new List<JToken>();

            foreach (var book in parsedObject["docs"])
            {
                books.Add(book);
            }

            var booksSortedByRating = books
                .OrderBy(book => book["rating"])
                .Select(book => book["title"].Value<string>())
                .ToList();

            return booksSortedByRating;
        }
    }
}