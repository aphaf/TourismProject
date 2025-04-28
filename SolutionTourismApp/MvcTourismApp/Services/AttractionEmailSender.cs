using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MvcTourismApp.Services
{
    //email sender code is included but not implemented
    /*
    public class AttractionEmailSender : IAttractionEmailSender
    {
        private IEmailSender _emailSender;
        private IHttpContextAccessor _contextAccessor;

        public AttractionEmailSender(IEmailSender emailSender, IHttpContextAccessor contextAccessor)
        {
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
        }

        public void SendAddAttractionEmail(string controllerAndMethodNames, string email, string subject, string message)
        {
            string callbackUrl = @"https://" + _contextAccessor.HttpContext.Request.Host.Value + "/" + controllerAndMethodNames;

            _emailSender.SendEmailAsync(email, subject, message + $"by clicking <a href='{callbackUrl}'>here</a>").Wait();

        }
    }
    */
}
