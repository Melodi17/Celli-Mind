namespace Celli_Mind
{
    public static class LanguageController
    {
        public static RandomController Random;
        public static class Statement
        {
            public static string Greeting => new string[]
            {
                "Hi",
                "Hii",
                "Hello",
                "Hey",
                $"Good {(DateTime.Now.ToString("tt") == "AM" ? "morning" : "evening")}"
            }.Random();
        }
        public static class Response
        {
            public static string Time => new string[]
            {
                $"The time is {DateTime.Now:h:m}",
                $"It is {DateTime.Now:h:m tt}"
            }.Random();

            public static string Day => new string[]
            {
                $"It is {DateTime.Now:D}"
            }.Random();
        }
        public static class Confirmation
        {
            public static string Quit => new string[]
            {
                "Are you sure you want to quit?"
            }.Random();
        }
    }
    public static class LanguageControllerExtensions
    {
        public static T Random<T>(this T[] arr)
            => arr[LanguageController.Random.Number(arr.Length)];
    }
}
