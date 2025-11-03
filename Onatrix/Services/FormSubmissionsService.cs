using Azure;
using Azure.Communication.Email;
using Onatrix.ViewModels;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Services;
using EmailMessage = Azure.Communication.Email.EmailMessage;

namespace Onatrix.Services;

public class FormSubmissionsService(IContentService contentService, IConfiguration configuration)
{
    private readonly IContentService _contentService = contentService;
    private readonly IConfiguration _configuration = configuration;


    public bool SaveCallbackRequest(CallbackFormViewModel model)
    {
        try {           
            var container = _contentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "formSubmissions");
            if (container == null)
                return false;

            var requestName = $"{model.Name} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var request = _contentService.Create(requestName, container, "callbackRequest");
            request.SetValue("callbackRequestName", model.Name);
            request.SetValue("callbackRequestEmail", model.Email);
            request.SetValue("callbackRequestPhone", model.Phone);
            request.SetValue("callbackRequestOption", model.SelectedOption);

            var saveResult = _contentService.Save(request);
            return saveResult.Success;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> SendConfirmationEmailAsync(string toEmail, string name)
    {
        var connectionString = _configuration["AzureCommunication:EmailConnectionString"];
        var client = new EmailClient(connectionString);

        var emailContent = new EmailContent("Tack för din förfrågan")
        {
            PlainText = $"Hej {name}, tack för att du kontaktade oss. Vi hör av oss snart!",
            Html = $"<strong>Hej {name}</strong><br/>Tack för att du kontaktade oss. Vi hör av oss snart!"
        };

        var senderAddress = _configuration["AzureCommunication:SenderAddress"];
        var emailMessage = new EmailMessage(senderAddress, toEmail, emailContent);

        try
        {
            var response = await client.SendAsync(WaitUntil.Completed, emailMessage);
            return response.Value.Status == EmailSendStatus.Succeeded;
        }
        catch (Exception ex)
        {
            
            return false;
        }
    }
}
