using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;
using Newtonsoft.Json;
using Mantis.Support.Module.Dnn.Mantis_Support.Models;
using System;

public class SendTicketController : DnnController
{
    private readonly HttpClient _httpClient;

    public SendTicketController()
    {
        _httpClient = new HttpClient();
        // Configure the HttpClient if needed (base address, timeouts, etc.)
    }



    [HttpPost]
    public async Task<ActionResult> SendMessage(TicketData formData)
    {
        var jsonBody = new
        {
            summary = string.Format("[Domania_SUPPORT] - {0}", formData.Subject),
            description = string.Format("Sender: {0}\nEmail: {1}\nMessage: {2}", formData.Name, formData.Email, formData.Message),
            project = new { id = 49, name = "Forró Torták" },
            category = new { id = 5, name = "Design" }, 
            handler = new { id = 146, name = "therigozsolti" },
            view_state = new { id = 10, name = "public" },
            priority = new { id = 30, name = "normal" },
            severity = new { id = 10, name = "kérés" },
            reproducibility = new { name = "nem próbáltam" }
        };

        var json = JsonConvert.SerializeObject(jsonBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // Send the POST request to the Mantis API
            HttpResponseMessage response = await _httpClient.PostAsync("https://rf.uni-corvinus.hu/mantis/api/rest/issues", content);

            // Await the response and ensure it is successful
            if (response.IsSuccessStatusCode)
            {
                // You can log the success or perform additional actions here if necessary
                return Json(new { success = true, message = "Ticket successfully sent to Mantis." });
            }
            else
            {
                // Read the response content for more details and log it if necessary
                var errorContent = await response.Content.ReadAsStringAsync();
                // Log the error content or handle it as required
                return Json(new { success = false, message = "Failed to send ticket to Mantis." });
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }
}
