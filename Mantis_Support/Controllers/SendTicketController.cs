using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;
using Newtonsoft.Json;
using Mantis.Support.Module.Dnn.Mantis_Support.Models;

public class SendTicketController : DnnController
{
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
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", "aS-X0G5twCHlaJjdsnq0ruiugKLl5kkY");

            var response = await client.PostAsync("https://fr.uni-corvinus.hu/mantis/api/rest/issues", data);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                // Handle success
                return Json(new { success = true, message = "Message sent!" });
            }
            else
            {
                // Handle failure
                return Json(new { success = false, message = "Failed to send message." });
            }
        }
    }
}
