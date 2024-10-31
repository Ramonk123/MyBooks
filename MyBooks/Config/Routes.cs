namespace MyBooks.Config;

public static class Routes
{
    public static class Account
    {
        public const string Base = "/Account";
        public const string Login = $"{Base}/Login";
        public const string Register = $"{Base}/Register";
        public const string Logout = $"{Base}/Logout";
    }

    public static class Library
    {
        public const string Base = "/Library";
        public const string Add = $"{Base}/Add";
        public const string Get = $"{Base}/Get/{{id}}";
        public const string Edit = $"{Base}/Edit/{{id}}";
        public const string Delete = $"{Base}/Delete/{{id}}";
    }

    public static class Book
    {
        public const string Base = "/Books";
        public const string Details = $"{Base}/Details/{{id}}";
        public const string Popular = $"{Base}/Popular";
        public const string AddBookToLibrary = $"{Base}/Add/{{bookId}}";
        public const string Search = $"{Base}/Search/{{query}}";
        public const string Delete = $"{Base}/Delete";
    }
}