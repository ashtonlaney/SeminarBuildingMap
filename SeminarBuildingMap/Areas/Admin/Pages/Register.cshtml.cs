using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SeminarBuildingMap.Areas.Identity.Data;

namespace SeminarBuildingMap.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin,Manager")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<SeminarBuildingMapUser> _signInManager;
        private readonly UserManager<SeminarBuildingMapUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<SeminarBuildingMapUser> userManager,
            SignInManager<SeminarBuildingMapUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            //RoleManager<IdentityRole> roleManager <- add this to parameter list to reenable manual registration
            //_roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<String> AvailableRoles { get; set; }
        [BindProperty]
        public string SelectedRole { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            AvailableRoles = new List<String>();
            if (User.IsInRole("Admin"))
            {
                AvailableRoles.Add("Manager");
                AvailableRoles.Add("Faculty");
            }
            else if (User.IsInRole("Manager"))
            {
                AvailableRoles.Add("Faculty");
            } 
            //this below is how we manually add roles/users probably not needed since we have the proper registration created
            // var result = await _roleManager.CreateAsync(new IdentityRole("Manager"));
            //result = await _roleManager.CreateAsync(new IdentityRole("User"));
            //SeminarBuildingMapUser user = await _userManager.FindByEmailAsync("ashtonlaney72@gmail.com");
            //await _userManager.AddToRoleAsync(user, "Admin");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid && ((User.IsInRole("Admin") && SelectedRole != "Admin" ) || (User.IsInRole("Manager") && SelectedRole == "Faculty")))
            {
                var user = new SeminarBuildingMapUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, SeminarBuildingMap.Areas.Identity.Data.Password.Generate(12, 3));
                await _userManager.AddToRoleAsync(user, SelectedRole);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirm", new { email = Input.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
