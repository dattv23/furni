// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using furni.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;

namespace furni.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        //public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        //{
        //    if (email == null)
        //    {
        //        return RedirectToPage("/Index");
        //    }
        //    returnUrl = returnUrl ?? Url.Content("~/");

        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with email '{email}'.");
        //    }

        //    Email = email;
        //    // Once you add a real email sender, you should remove this code that lets you confirm the account
        //    DisplayConfirmAccountLink = true;
        //    if (DisplayConfirmAccountLink)
        //    {
        //        var userId = await _userManager.GetUserIdAsync(user);
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        EmailConfirmationUrl = Url.Page(
        //            "/Account/ConfirmEmail",
        //            pageHandler: null,
        //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //            protocol: Request.Scheme);
        //    }

        //    return Page();
        //}
        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            // Generate confirmation link
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            // Use your custom SendInvoiceEmail method to send the confirmation email
            var subject = "Confirm your email";
            var htmlContent = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            SendComfirmEmail(email, subject, htmlContent);

            return Page();
        }

        // Make sure this method is accessible, adjust its access modifier and location as necessary
        public void SendComfirmEmail(string toEmail, string subject, string htmlContent)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("tiep.nguyentonyfake@hcmut.edu.vn", "zrddeegmguihuuor");

            // Tạo mail message
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("tiep.nguyentonyfake@hcmut.edu.vn");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = htmlContent;
            mail.IsBodyHtml = true; // Quan trọng nếu bạn gửi HTML

            // Gửi email
            smtpClient.Send(mail);
        }
    }
}
