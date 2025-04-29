using System.Globalization;
using System.Runtime.InteropServices;
using System.Web;

namespace NetSentinel.SpeedTest
{
    internal sealed class HttpClient : global::System.Net.Http.HttpClient
    {
        public HttpClient()
        {
            var frameworkInfo = RuntimeInformation.FrameworkDescription.Split();
            var osInfo = RuntimeInformation.OSDescription.Split();

            DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
            DefaultRequestHeaders.Add("User-Agent", string.Join(" ",
            [
                "Mozilla/5.0",
                $"({osInfo[0]}-{osInfo[1]}; U; {RuntimeInformation.ProcessArchitecture}; en-us)",
                $"{frameworkInfo[0]}/{frameworkInfo[1]}",
                "(KHTML, like Gecko)",
                $"SpeedTest.Net/{typeof(ISpeedTestClient).Assembly.GetName().Version}"
            ]));
        }

        public async Task<T?> GetConfig<T>(string url)
        {
            var data = await GetStringAsync(AddTimeStamp(new Uri(url)));

            return data.FromXml<T>();
        }

        private static Uri AddTimeStamp(Uri address)
        {
            var uriBuilder = new UriBuilder(address);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["x"] = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}
