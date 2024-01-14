using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;

namespace StealthGPT
{
    internal class gptAPI
    {
        public static string currentTitle = "";
        public static string currentContext = "";
        static string requestURL = "https://api.openai.com/v1/chat/completions";
        public static string queryQuiz()
        {
            Program.updateTitle("Querying quiz..");
            if (currentTitle == "") { return "Insufficient data!"; }

            string prompt = "";

            prompt = Program.config._gptStartPrompt;
            prompt = prompt.Replace("%title%", currentTitle);
            prompt = prompt.Replace("%context%", currentContext);

            List<dynamic> messages = new List<dynamic>();
            dynamic userMessage = new ExpandoObject();
            userMessage.role = "user";
            userMessage.content = prompt;
            messages.Add(userMessage);

            return sendRequest(false, messages);
        }

        public static string queryImage(string base64image)
        {
            Program.updateTitle("Querying Image..");
            if (base64image == "") { return "Insufficient data!"; }

            List<dynamic> messages = new List<dynamic>();
            dynamic userMessage = new ExpandoObject();
            userMessage.role = "user";
            userMessage.content = new List<dynamic>
            {
                new { type = "text", text = "On the provided image, there is a question. Read the question, and output ONLY the correct answer." },
                new
                {
                    type = "image_url",
                    image_url = new { url = $"data:image/jpeg;base64,{base64image}" }
                }
            };
            messages.Add(userMessage);
            return sendRequest(true, messages);
        }

        public static string sendRequest(bool image, List<dynamic> messages)
        {
            var client = new RestClient(requestURL);
            var request = new RestRequest("", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {Program.config._gptApiKey}");
            request.AddHeader("Request-Url", requestURL);
            dynamic data = new ExpandoObject();
            data.max_tokens = 100;
            data.model = Program.config._gptModel;
            data.messages = messages.ToArray();
            string datajson = JsonConvert.SerializeObject(data);
            request.AddJsonBody(datajson);
            RestResponse response = client.Execute(request);
            try
            {
                if (image)
                {
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    JValue contentValue = json.choices[0].message.content;
                    Program.updateTitle("Finished image query!");
                    return contentValue.ToString();
                }
                else
                {
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    Program.updateTitle("Finished quiz query!");
                    return json.choices[0].message.content;
                }
            }
            catch
            {
                if (response.Content.Contains("\"code\": \"invalid_api_key\"") || response.Content.Contains("https://platform.openai.com/account/api-keys"))
                    return "ERROR: Invalid API Key";

                if (response.Content.Contains("\"message\": \"The model `") && response.Content.Contains("` does not exist\""))
                    return "ERROR: Model 'gpt-3.5-turbo' does not exist";

                return "ERROR!\nResponse: " + response.Content;
            }
        }
    }
}
