namespace Celli_Mind
{
    public static class LanguageController
    {
        /// <summary>
        /// Random used to determine an item
        /// </summary>
        public static RandomController Random;

        /// <summary>
        /// Language type statement
        /// </summary>
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

        /// <summary>
        /// Language type response
        /// </summary>
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

        /// <summary>
        /// Language type question
        /// </summary>
        public static class Question
        {
            public static string Quit => new string[]
            {
                "Are you sure you want to quit?"
            }.Random();
        }
    }
    public static class LanguageControllerExtensions
    {
        /// <summary>
        /// Picks random item from <paramref name="source"/>
        /// </summary>
        /// <typeparam name="T">Type of array</typeparam>
        /// <param name="source">Source array</param>
        /// <returns>A random item from <paramref name="source"/></returns>
        public static T Random<T>(this T[] source)
            => source[LanguageController.Random.Number(source.Length)];
    }
}
