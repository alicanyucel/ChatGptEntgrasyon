using Newtonsoft.Json;
using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        private static readonly string apiKey = "gizliapikey";
        private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
        public static readonly string systemprompt = @"Sen avukat Ali Can Yücelin'ün asistanısın. 
Sadece şu sorulara cevap ver:
Soru: istinaftan dönen dosyaya dilekçe verilir mi?
Cevap: beyan dilekçesi verebilirsiniz.
Soru: avukata nasıl vekalet veririm?
Cevap: Türkiye'de iseniz herhangi bir notere giderek, yurt dışındaysanız TC konsolosluklarına giderek vekalet çıkarabilirsiniz.

Hukuki soruların ardından, her somut olayın kendisine özgü özellikler taşıdığını ve cevabın tahmini olarak verildiğini hatırlat 
Bu konular haricinde konuşma.";
        static async Task Main(string[] args)
        {

            string model = "gpt-4o";
            string prompt = Console.ReadLine();

            var response = await GetChatGPTResponse(model, prompt);
            Console.WriteLine("ChatGPT Response: ");
            Console.WriteLine(response);
        }

        static async Task<string> GetChatGPTResponse(string model, string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = systemprompt },
                    new { role = "user", content = prompt },
                }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
                    return jsonResponse.choices[0].message.content.ToString();
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

    }
}
