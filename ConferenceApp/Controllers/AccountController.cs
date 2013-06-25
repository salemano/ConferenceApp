using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ConferenceApp.Filters;
using ConferenceApp.Models;
using Model.Models;
using Model;
using Core.Services;
using Core.Security;
using ConferenceApp.Infrastructure;
using System.Text.RegularExpressions;
using System.IO;

namespace ConferenceApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserService _userService;
        private ICryptographyService _cryptographyService;
        private IEmailService _emailService;
        private IImageService _imageService { get; set; }

        public AccountController(IUserService userService, ICryptographyService cryptographyService, IEmailService emailService, IImageService imageService)
        {
            _userService = userService;
            _cryptographyService = cryptographyService;
            _emailService = emailService;
            _imageService = imageService;
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var result =  ValidateUser(model.UserName, model.Password);
            if (ModelState.IsValid && result == null)
            {
                _userService.SignIn(model.UserName, model.RememberMe);

                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            SetModelError(result, model);

            return View(model);
        }

        UserValidationResult? ValidateUser(string email, string password)
        {
            var user = _userService.GetByUsername(email);

            if (user == null)   
                return UserValidationResult.InvalidUser;

            // If the user has an activation token, they haven't completed their registration
            if (user.ActivationToken != null)
                return UserValidationResult.NotActivated;

            // If the passwords do not match
            if (user.Password != _cryptographyService.EncryptPassword(password + user.PasswordSalt))
                return UserValidationResult.PasswordMismatch;

            return null;
        }

        void SetModelError(UserValidationResult? result, LoginModel model)
        {
            string errorMessage;
            switch (result)
            {
                case UserValidationResult.InvalidUser:
                    errorMessage = String.Format("Cannot find user with email '{0}'", model.UserName);
                    break;
                case UserValidationResult.PasswordMismatch:
                    errorMessage = "The user name or password provided is incorrect.";
                    break;
                case UserValidationResult.NotActivated:
                    errorMessage =
                        "Your account is currently not activated, please visit your inbox to confirm your email address. If you have not received your confirmation email, please contact a Tau Ora staff member";
                    break;

                default:
                    errorMessage = null;
                    break;
            }

            if (errorMessage != null)
                ModelState.AddModelError("", errorMessage);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult LogOff()
        {
            _userService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        void PopulateEditRegisterViewModel(RegisterModel model)
        {
            if (model.PhotoId != null)
                model.PhotoThumbnail = GetThumbnailUrl((byte[])_imageService.GetData(model.PhotoId.Value), "png", 120);
            else model.PhotoThumbnail = null;
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model,HttpPostedFileBase image)
        {
            if (image != null)
            {
                var photo = _imageService.AddImage(image.FileName, GetFileContent(image), _userService.CurrentUser.Id);
                model.PhotoId = photo.Id;
            }

            PopulateEditRegisterViewModel(model);

            if (!ModelState.IsValid)
                return View(model);

            ValidateRegisterModel(model);

            if (!ModelState.IsValid)
                return View(model);

            var salt = _cryptographyService.GetRandomString(50, false, false);
            var user = new User
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Comment = model.Comment,
                IsAdministrator = false,
                Password = _cryptographyService.EncryptPassword(model.Password + salt),
                PasswordSalt = salt,
                ActivationToken = Guid.NewGuid(),
                PhotoId = model.PhotoId
            };

            _userService.Create(user);

            SendRegistrationConfirmationEmail(user);

            return RedirectToAction("Index", "Home")
                .WithSuccessMessage(string.Format("You have been successfully registered. Please check you email to activate account.")); ;
        }

        void SendRegistrationConfirmationEmail(User user)
        {
            var model = new RegistrationConfirmationModel { ActivationToken = user.ActivationToken.ToString(), Email = user.Email, FullName = user.FullName };
            var emailDesc = new EmailDescription
            {
                Subject = "You have successfully registered",
                To = user.Email,
                Body = Helper.GetEmailBody(Mail.RegistrationConfirmation, model)
            };

            _emailService.SendMessage(emailDesc);
        }

        void SendRequestResetPasswordEmail(User user)
        {
            var model = new RequestResetPasswordModel {  ResetPasswordToken = user.PasswordRecoveryToken.ToString(), Email = user.Email, FullName = user.FullName };
            var emailDesc = new EmailDescription
            {
                Subject = "Reset pas sword request",
                To = user.Email,
                Body = Helper.GetEmailBody(Mail.RequestResetPassword, model)
            };

            _emailService.SendMessage(emailDesc);  
        }

        void ValidateRegisterModel(RegisterModel model)
        {
            var isExists = _userService.GetByUsername(model.Email) != null;

            if (isExists)
                ModelState.AddModelError("Email", "User with such email address already exists in the system");
        }

        [AllowAnonymous]
        public ActionResult Activate(Guid token)
        {
            // check token
            var user = _userService.GetAll().FirstOrDefault(u => u.ActivationToken == token);

            if (user == null)
                return RedirectToAction("Index", "Home")
                    .WithErrorMessage(string.Format("Invalid token supplied"));

            user.ActivationToken = null;
            user.ActivatedAt = DateTime.Now;
            _userService.Update(user);
            // login

            _userService.SignIn(user.Email, true);
            // display success message

            return RedirectToAction("Index", "Home")
                .WithSuccessMessage(string.Format("Your account have been successfully activated"));
        }

        [AllowAnonymous]
        public ActionResult RequestPasswordReset()
        {
            var viewModel = new RequestPasswordResetModel { };

            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RequestPasswordReset(RequestPasswordResetModel viewModel)
        {
            var user = _userService.GetByUsername(viewModel.Email);

            if (user == null)
                ModelState.AddModelError("Email", string.Format("The email {0} is not registered in the system", viewModel.Email));

            if (user != null)
            {
                if (user.ActivationToken != null)
                    ModelState.AddModelError("Email", "The account must be activated first, please click the link in the registration email or contact the administrator");
            }

            if (!ModelState.IsValid)
                return View(viewModel);

            user.PasswordRecoveryToken = Guid.NewGuid();
            _userService.Update(user);

            SendRequestResetPasswordEmail(user);

            return RedirectToAction("Index", "Home")
                .WithSuccessMessage(string.Format("Email with reset password link has been sent. Please check your email."));
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(Guid token)
        {
            var user = _userService.GetAll().FirstOrDefault(u => u.PasswordRecoveryToken == token);

            if (user != null)
            {
                return View(new PasswordResetModel { UserId = user.Id });
            }

            return RedirectToAction("Index", "Home")
                .WithErrorMessage(string.Format("Invalid token supplied"));
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(PasswordResetModel resetModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetAll().FirstOrDefault(u => u.Id == resetModel.UserId);
                user.PasswordSalt = _cryptographyService.GetRandomString(50, false, false);
                user.Password = _cryptographyService.EncryptPassword(resetModel.Password + user.PasswordSalt);
                user.ActivationToken = null;

                _userService.Update(user);

                _userService.SignIn(user.Email, stayLoggedIn: true);

                return RedirectToAction("Index", "Home")
                    .WithSuccessMessage(string.Format("You have successfully set password"));
            }
            return View(resetModel);
        }

        public ActionResult EditProfile()
        {
            var user = _userService.CurrentUser;

            var model = new EditProfileModel
            {
                Comment = user.Comment,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                PhoneNumber = user.PhoneNumber,
                PhotoId = user.PhotoId
            };

            PopulateEditProfileViewModel(model);

            return View(model);
        }

        string GetThumbnailUrl(byte[] myImage, string fileType, int thumbHeight)
        {
            if (Regex.IsMatch(fileType, @"bmp|gif|png|tiff|jpe?g"))
            {
                byte[] arr;
                try
                {
                    using (var ms = new MemoryStream())
                    {

                        System.Drawing.Image originalImage;
                        using (var mst = new MemoryStream(myImage))
                        {
                            originalImage = System.Drawing.Image.FromStream(mst);
                        }

                        var thumbWidth = thumbHeight * (originalImage.Width / originalImage.Height);
                        thumbWidth = thumbWidth == 0 ? 100 : thumbWidth;
                        using (var thumbnail = originalImage.GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
                        {
                            thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            arr = ms.ToArray();
                        }
                    }
                }
                catch { return string.Empty; }

                return string.Format("data:image/{0};base64,", fileType.Replace(".", "")) + Convert.ToBase64String(arr);
            }
            else
            {
                return string.Empty;
            }
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfileModel model, HttpPostedFileBase image)
        {
            var user = _userService.CurrentUser;

            if (image != null)
            {
                var photo = _imageService.AddImage(image.FileName, GetFileContent(image), _userService.CurrentUser.Id);

                user.PhotoId = photo.Id;
                _userService.Update(user);
            }

            PopulateEditProfileViewModel(model);

            ValidateEditProfileViewModel(model);

            if (!ModelState.IsValid)
                return View(model);

            user.Comment = model.Comment;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.MiddleName = model.MiddleName;
            user.PhoneNumber = model.PhoneNumber;

            _userService.Update(user);

            return RedirectToAction("Index", "Home")
                .WithSuccessMessage(string.Format("You have successfully updated your profile.")); ;
        }

        void PopulateEditProfileViewModel(EditProfileModel model)
        {
            var user = _userService.CurrentUser;

            if (user.PhotoId != null)
                model.PhotoThumbnail = GetThumbnailUrl((byte[])_imageService.GetData(user.PhotoId.Value), "png", 120);
            else model.PhotoThumbnail = null;
        }

        void ValidateEditProfileViewModel(EditProfileModel model)
        {
            var user = _userService.GetByUsername(model.Email);

            if (user != null && user.Id != _userService.CurrentUser.Id)
                ModelState.AddModelError("Email", "User with such email address already exists in the system");
        }

        byte[] GetFileContent(HttpPostedFileBase image)
        {
            var imageData = new byte[image.ContentLength];
            image.InputStream.Read(imageData, 0, image.ContentLength);

            return imageData;
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
