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
        public const string AddLibrary = $"{Base}/Add";
    }

    public static class Book
    {
        public const string Base = "/Books";
        public const string Details = $"{Base}/Details{{id}}";
        public const string Popular = $"{Base}/Popular";
        public const string AddBook = $"{Base}/AddBook/{{libraryId}}";
        public const string Search = $"{Base}/Search/{{query}}";

    }
}