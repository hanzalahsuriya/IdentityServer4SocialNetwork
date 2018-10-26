using SocialNetwork.OAuthId.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SocialNetwork.OAuthId.Extensions
{
    public static class EmailSenderExtensions
    {
        public async static Task SendConfirmationEmail(this IEmailSender emailSender, string email, string link)
        {
            string subject = "Confirm your email address.";
            string message = $"Please confirm your account by clicking on this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>";

            await emailSender.SendEmailAsync(email, subject, message);
        }
    }
}
