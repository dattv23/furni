using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace furni.Services
{
    public class ImgurService
    {
        private readonly string clientId;

        public ImgurService(string clientId)
        {
            this.clientId = clientId;
        }

        public async Task<string> UploadImageAsync(byte[] image)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", clientId);

                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new ByteArrayContent(image), "image");

                    var response = await client.PostAsync("https://api.imgur.com/3/image", content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Failed to upload image to Imgur. Status: {response.StatusCode}, Response: {jsonResponse}");
                    }

                    dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                    return result.data.link;
                }
            }
        }
    }
}
