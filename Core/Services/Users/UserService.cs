using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Model;
using Model.Models;

namespace Core.Services
{
    public class UserService: IUserService
    {
        private readonly ConferenceContext _context;

        public UserService(ConferenceContext context)
        {
            _context = context;
        }

        public void SignIn(String userName, bool stayLoggedIn)
        {
            FormsAuthentication.SetAuthCookie(userName, stayLoggedIn);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        //public UserSignInValidationResult ValidateUser(string email, string password, bool onlyCurrentCompetition)
        //{
        //    var result = new UserSignInValidationResult { UserCompetitionType = null };

        //    if (onlyCurrentCompetition)
        //        result.User = _userService.GetByEmailAndCompetitionId(email, _competitionService.CurrentCompetition.Id);
        //    else
        //        result.User = _userService.GetByEmail(email, false, user1 => user1.Competition,
        //                                              user1 => user1.Competition.CompetitionUrls);

        //    result.UserValidationResult = Validate(result.User, password, onlyCurrentCompetition);

        //    return result;
        //}

        //UserValidationResult? Validate(User user, string password, bool onlyCurrentCompetition)
        //{
        //    if (user == null)
        //        return UserValidationResult.InvalidUser;

        //    // Is the user part of the current competition
        //    if (onlyCurrentCompetition && user.CompetitionId != null &&
        //        user.CompetitionId != _competitionService.CurrentCompetition.Id)
        //        return UserValidationResult.NotInCurrentCompetition;

        //    // If the user has an activation token, they haven't completed their registration
        //    if (user.ActivationToken != null)
        //        return UserValidationResult.NotActivated;

        //    // If the user is locked out of TOA
        //    if (user.Locked)
        //        return UserValidationResult.Locked;

        //    // If the passwords do not match
        //    if (user.Password != _cryptographyService.EncryptPassword(password + user.PasswordSalt))
        //        return UserValidationResult.PasswordMismatch;

        //    return null;
        //}

    }
}
