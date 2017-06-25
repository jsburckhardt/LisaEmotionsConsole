using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Libraries required for the API
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace EmotionsConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var test1 = args[0];
            var test2 = args[1];
            //Console.Write("Enter the path to a JPEG image file:");
            //string imageFilePath = Console.ReadLine();
            if (!File.Exists(test1)){
                Console.WriteLine("Grow up, file doesn't exist");
                Console.ReadLine();
                return;
            }

            //Console.Write("Enter the key for Cognitive services:");
            //string cognitiveKey = Console.ReadLine();
            if (test2 == null) {
                Console.WriteLine("Grow up, you need to put some value");
                Console.ReadLine();
                return;
            }

            MakeRequest(test1, test2);
            //Console.ReadKey();
        }

        static void MakeRequest(string imageFilePath, string cognitiveKey)
        {
            
            var client = new HttpClient();
            

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", cognitiveKey);

            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;
            string responseContent;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = client.PostAsync(uri, content).Result;
                responseContent = response.Content.ReadAsStringAsync().Result;
            }

            //A peak at the JSON response.
            var json = JArray.Parse(responseContent);
            string formatted = json.ToString();
            Console.WriteLine(formatted);
            
        }
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
