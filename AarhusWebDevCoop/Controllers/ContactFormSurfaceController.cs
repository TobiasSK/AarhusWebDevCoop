using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCoop.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;

namespace AarhusWebDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface

        public ActionResult Index()
        {
            
            return PartialView("ContactForm", new ContactForm());
            
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

            MailMessage message = new MailMessage();
            message.To.Add("tobias.sk@gmail.com");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("tobias.sk@gmail.com", "Tobyderke123");
                smtp.EnableSsl = true;
                // send mails
                smtp.Send(message);
                TempData["success"] = true;
            }

            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
            comment.SetValue("messagename", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            //Save
            Services.ContentService.Save(comment);
           

            return RedirectToCurrentUmbracoPage();
        }

        }

}