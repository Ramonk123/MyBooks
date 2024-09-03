namespace MyBooks.Config;

public static class Routes
{
    public static class Account
    {
        public static string Base = "/Account";
        public static string Login = $"{Base}/Login";
        public static string Register = $"{Base}/Register";
        public static string Logout = $"{Base}/Logout";
    }

    public static class Library
    {
        public static string Base = "/Library";
    }
}