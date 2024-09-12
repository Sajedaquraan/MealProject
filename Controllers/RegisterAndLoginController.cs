using MealProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace MealProject.Controllers
{
    public class RegisterAndLoginController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public RegisterAndLoginController(ModelContext context, IWebHostEnvironment environment)
        {
            _context=context;
            _environment=environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Userid,Username,Useremail,Userpassword,Userimage,ImageFile,Googleid,Registerdate")] Customer customer, string Useremail, string Userpassword)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var emailExists = await _context.Customers.AnyAsync(u => u.Useremail == Useremail);
                if (emailExists)
                {
                    TempData["ErrorMessage"] = "Email is already registered. ";
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(customer);
                }

                if (customer.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + customer.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await customer.ImageFile.CopyToAsync(fileStream);
                    }

                    customer.Userimage = fileName;
                }

                var verificationCode = Guid.NewGuid().ToString().Substring(0, 6);
                HttpContext.Session.SetString("VerificationCode", verificationCode);
                HttpContext.Session.SetString("Email", Useremail);

                _context.Add(customer);
                await _context.SaveChangesAsync();

                Userlogin login = new Userlogin
                {
                    Email = Useremail,
                    Password = Userpassword,
                    Roleid = 2,
                    Userid = customer.Userid
                };
                _context.Add(login);
                await _context.SaveChangesAsync();

                SendVerificationEmail(customer.Useremail, verificationCode);

                return RedirectToAction("VerifyEmail", new { email = customer.Useremail });
            }
            return View(customer);
        }

        private void SendVerificationEmail(string email, string verificationCode)
        {
            var fromAddress = new MailAddress("sajedaalquraan1@gmail.com", "Meal");
            var toAddress = new MailAddress(email);
            const string fromPassword = "izdw sras niqv jnnh";
            const string subject = "Email Verification";
            string body = $"Your verification code is {verificationCode}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }



        [HttpGet]
        public IActionResult VerifyEmail(string email)
        {
            var model = new VerifyEmailViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyEmail(VerifyEmailViewModel model)
        {
            var storedCode = HttpContext.Session.GetString("VerificationCode");

            if (storedCode == null || storedCode != model.VerificationCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
                return View(model);
            }

            HttpContext.Session.Remove("VerificationCode");
            HttpContext.Session.Remove("Email");

            TempData["SuccessMessage"] = "Successfully registered! You can now log in to your profile.";

            return RedirectToAction("Login");
        }



        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login([Bind("Email")] Userlogin userlogin, string returnUrl = null)
        {
            var auth = _context.Userlogins
                .Where(x => x.Email == userlogin.Email)
                .SingleOrDefault();

            if (auth != null)
            {
                // Generate a random 6-digit verification code
                var verificationCode = new Random().Next(100000, 999999).ToString();

                // Store the code and user information in the session
                HttpContext.Session.SetString("VerificationCode", verificationCode);
                HttpContext.Session.SetString("UserEmail", auth.Email);
                HttpContext.Session.SetInt32("UserId", (int)auth.Userid);

                // Send the verification code via email
                SendVerificationEmail1(auth.Email, verificationCode);

                // Redirect to verification page
                return RedirectToAction("VerifyCode1", new { returnUrl });
            }
            else
            {
                TempData["ErrorMessage"] = "Email not found!";
            }

            return View();
        }

        private void SendVerificationEmail1(string email, string verificationCode)
        {
            var fromAddress = new MailAddress("sajedaalquraan1@gmail.com", "Meal");
            var toAddress = new MailAddress(email);
            const string fromPassword = "izdw sras niqv jnnh"; // Ensure this is securely stored
            const string subject = "Email Verification";
            string body = $"Your verification code is {verificationCode}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        [HttpGet]
        public IActionResult VerifyCode1(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode1(string code, string returnUrl = null)
        {
            var storedCode = HttpContext.Session.GetString("VerificationCode");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (storedCode == code)
            {
                // Clear the verification code from session
                HttpContext.Session.Remove("VerificationCode");

                // Log the user in by setting the session for the user
                HttpContext.Session.SetInt32("CustomerID", userId.Value);

                // Query the database for the user's RoleId
                using (var db = new ModelContext())
                {
                    var userLogin = db.Userlogins.SingleOrDefault(u => u.Userid == userId.Value);
                    if (userLogin != null)
                    {
                        var roleId = userLogin.Roleid;

                        // Redirect based on RoleId
                        if (roleId == 1)
                        {
                            // Redirect to Admin page
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (roleId == 2)
                        {
                            // Redirect to Home page
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }

                // If a returnUrl is provided and valid, redirect to that URL
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    // Default redirect to Home page
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid verification code!";
                return RedirectToAction("VerifyCode1");
            }
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var user = _context.Userlogins.SingleOrDefault(x => x.Email == Email);
            if (user != null)
            {
                // Generate a random code
                var resetCode = new Random().Next(100000, 999999).ToString();

                // Store the code in session
                HttpContext.Session.SetString("ResetCode", resetCode);
                HttpContext.Session.SetString("ResetEmail", Email);

                // Send the code via email
                await SendResetCodeEmail(Email, resetCode);

                return RedirectToAction("VerifyCode");
            }
            ModelState.AddModelError("", "Email not found.");
            return View();
        }

        // Method to send email (implement your email sending logic here)
        private async Task SendResetCodeEmail(string email, string resetCode)
        {
            // Example using SmtpClient (replace with your email provider's details)
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sajedaalquraan1@gmail.com", "izdw sras niqv jnnh"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("Meal@gmail.com"),
                Subject = "Password Reset Code",
                Body = $"Your password reset code is: {resetCode}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(string ResetCode)
        {
            var sessionCode = HttpContext.Session.GetString("ResetCode");

            if (sessionCode == ResetCode)
            {
                return RedirectToAction("ResetPassword");
            }

            ModelState.AddModelError("", "Invalid code. Please try again.");
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string NewPassword, string ConfirmPassword)
        {
            if (NewPassword == ConfirmPassword)
            {
                var email = HttpContext.Session.GetString("ResetEmail");

                if (!string.IsNullOrEmpty(email))
                {
                    var user = _context.Userlogins.SingleOrDefault(x => x.Email == email);
                    var customer = _context.Customers.SingleOrDefault(x => x.Useremail == email);
                    if (user != null)
                    {
                        // Update the password
                        user.Password = NewPassword;
                        customer.Userpassword = NewPassword;
                        _context.SaveChanges();

                        // Clear session
                        HttpContext.Session.Remove("ResetCode");
                        HttpContext.Session.Remove("ResetEmail");

                        return RedirectToAction("Login");
                    }
                }
            }

            ModelState.AddModelError("", "Passwords do not match or there was an issue.");
            return View();
        }


        public async Task Login1()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties

                {
                    RedirectUri = Url.Action("GoogleResponse1")
                });

        }

        public async Task<ActionResult> GoogleResponse1()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var emailClaim = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var nameClaim = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var profilePicture = result.Principal.FindFirst("picture")?.Value;

            if (emailClaim != null)
            {
                // Check if the user already exists in the Userlogin table
                var existingUserLogin = _context.Userlogins.FirstOrDefault(u => u.Email == emailClaim);

                if (existingUserLogin == null)
                {
                    // If the user doesn't exist, create a new Customer and Userlogin

                    // Create new Customer entry
                    var newCustomer = new Customer
                    {
                        Username = nameClaim,
                        Useremail = emailClaim,
                        Googleid = googleId != null ? Convert.ToDecimal(googleId) : (decimal?)null,
                        Userimage = profilePicture,
                        Registerdate = DateTime.Now
                    };

                    // Save new customer to the database
                    _context.Customers.Add(newCustomer);
                    await _context.SaveChangesAsync();

                    // Create new Userlogin entry associated with the newly created Customer
                    var newUserLogin = new Userlogin
                    {
                        Email = emailClaim,
                        Userid = newCustomer.Userid, // Associate the Customer with this Userlogin
                        Roleid = 2, // Assuming default role ID; change according to your roles setup
                    };

                    // Save the new Userlogin to the database
                    _context.Userlogins.Add(newUserLogin);
                    await _context.SaveChangesAsync();

                    // Store Userloginid in session
                    HttpContext.Session.SetInt32("CustomerID", (int)newUserLogin.Userloginid);
                }
                else
                {
                    // User already exists, so store the Userloginid in session
                    HttpContext.Session.SetInt32("CustomerID", (int)existingUserLogin.Userloginid);
                }
            }

            // Redirect to the home page after login
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session on logout
            return RedirectToAction("Login");
        }
    }
}
