using System.Text.RegularExpressions;

namespace Crawler_cw_1
{
    class Crawler
    {
        static async Task Main(string[] args)
        {
            var websiteUrl = args[0];

            if (websiteUrl == null) throw new ArgumentNullException("First argument must be passed!");

            AssertIsValidUrl(websiteUrl);

            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync(websiteUrl);
                var body = await response.Content.ReadAsStringAsync();
                var mailRegex = new Regex(
                    @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");

                var matches = mailRegex.Matches(body);
                var uniqueMatches = new HashSet<string>();

                foreach (var match in matches)
                {
                    var maybeEmail = match.ToString();

                    if (maybeEmail != null) uniqueMatches.Add(maybeEmail);
                }

                if (uniqueMatches.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono adresów email");
                }
                else
                {
                    Console.WriteLine("Unique emails: " + string.Join(", ", uniqueMatches));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }


            httpClient.Dispose();
        }

        private static void AssertIsValidUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new ArgumentException("Specified url is not valid!");
        }
    }
}