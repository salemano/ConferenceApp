using ConferenceApp.Models;
using Core.Security;
using Core.Services;
using Model.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ConferenceApp.Infrastructure;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using ConferenceApp.Areas.Admin.Models;
using MvcContrib.Pagination;
using System.Web.Security;

namespace ConferenceApp.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private IUserService _userService;
        private ICryptographyService _cryptographyService;
        private IImageService _imageService;
        private IEmailService _emailService;
        public const int ListPageSize = 10;
        public ISessionService _sessionService;

        public UserController(IUserService userService,ICryptographyService cryptographyService,IImageService imageService, IEmailService emailService,ISessionService sessionService)
        {
            _userService = userService;
            _cryptographyService = cryptographyService;
            _imageService = imageService;
            _emailService = emailService;
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(string filterText, bool? showClosed, bool? showArchived, int? page, GridSortOptions sortOptions)
        {
            if (sortOptions.Column == null)
                sortOptions = new GridSortOptions
                {
                    Column = "ActivatedAt",
                    Direction = SortDirection.Ascending
                };

            page = page ?? 1;

            var users = _userService.GetAll();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterText))
            {
                users = from c in users
                           where c.Email.Contains(filterText)
                           select c;
            }

            var list = from p in users
                       select new UserDescriptionModel
                       {
                           Email = p.Email,
                           ActivatedAt = p.ActivatedAt,
                           ActivationToken = p.ActivationToken,
                           IsAdministrator = p.IsAdministrator,
                           DateOfBirth = p.DateOfBirth,
                           FirstName = p.FirstName,
                           Id = p.Id
                       };

            list = list.OrderBy(sortOptions.Column, sortOptions.Direction);

            var vm = new UserListViewModel
            {
                Users = list.AsPagination(page.Value, ListPageSize),
                SortOptions = sortOptions,
                FilterText = filterText,
                Page = page.Value
            };

            return View(vm);

        }

        [Authorize]
        public ActionResult EditUser(int id)
        {
            var user = _userService.GetById(id);

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

        [HttpPost]
        [Authorize]
        public ActionResult EditUser(EditProfileModel model, int id, HttpPostedFileBase image)
        {
            var user = _userService.GetById(id);
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
                .WithSuccessMessage(string.Format("You have successfully updated user's profile."));
        }

        byte[] GetFileContent(HttpPostedFileBase image)
        {
            var imageData = new byte[image.ContentLength];
            image.InputStream.Read(imageData, 0, image.ContentLength);

            return imageData;
        }

        void ValidateEditProfileViewModel(EditProfileModel model)
        {
            var user = _userService.GetByUsername(model.Email);

            if (user != null && user.Id != _userService.CurrentUser.Id)
                ModelState.AddModelError("Email", "User with such email address already exists in the system");
        }

        void PopulateEditProfileViewModel(EditProfileModel model)
        {
            var user = _userService.CurrentUser;

            if (user.PhotoId != null)
                model.PhotoThumbnail = GetThumbnailUrl((byte[])_imageService.GetData(user.PhotoId.Value), "png", 120);
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

        [Authorize]
        public ActionResult Delete(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return RedirectToAction("List", "User").WithWarningMessage(string.Format("Could not find user by id: {0}", id));

            var sessions = (from p in _sessionService.GetAllByUserId(id) select p).ToList();
            foreach (var item in sessions)
            {
                _sessionService.Delete(item.Id);
            }
            _userService.Delete(user);

            return RedirectToAction("List", "User").WithSuccessMessage("User was deleted!");
        }

        [Authorize]
        public ActionResult MakeAdmin(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return RedirectToAction("List", "User").WithWarningMessage(string.Format("Could not find user by id: {0}", id));

            user.IsAdministrator = true;

            _userService.Update(user);

            return RedirectToAction("List", "User").WithSuccessMessage(string.Format("{0},{1} now is in Admin role", user.FullName, user.Email));
        }

        [Authorize]
        public ActionResult ResendActivationEmail(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return RedirectToAction("List", "User").WithWarningMessage(string.Format("Could not find user by id: {0}", id));

            SendActivationConfirmationEmail(user);

            return RedirectToAction("List", "User").WithSuccessMessage(string.Format("Email with activation link has been successfully send to {0} (email: {1})", user.FullName, user.Email));
        }

        [Authorize]
        public ActionResult RequestResetPasswordEmail(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return RedirectToAction("List", "User").WithWarningMessage(string.Format("Could not find user by id: {0}", id));

            SendRequestResetPasswordEmail(user);

            return RedirectToAction("List", "User").WithSuccessMessage(string.Format("Email with password reset link has been successfully sent to {0} (email: {1})", user.FullName, user.Email));
        }

        void SendActivationConfirmationEmail(User user)
        {
            var model = new RegistrationConfirmationModel { ActivationToken = user.ActivationToken.ToString(), Email = user.Email, FullName = user.FullName };
            var emailDesc = new EmailDescription
            {
                Subject = "Activation email",
                To = user.Email,
                Body = Helper.GetEmailBody(Mail.AdminRegistrationConfirmation, model)
            };

            _emailService.SendMessage(emailDesc);
        }

        void SendRequestResetPasswordEmail(User user)
        {
            var model = new RequestResetPasswordModel { ResetPasswordToken = Guid.NewGuid().ToString(), Email = user.Email, FullName = user.FullName };
            var emailDesc = new EmailDescription
            {
                Subject = "Reset password request",
                To = user.Email,
                Body = Helper.GetEmailBody(Mail.AdminRequestResetPassword, model)
            };

            _emailService.SendMessage(emailDesc);
        }
    }
}
