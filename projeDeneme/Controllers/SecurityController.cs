using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projeDeneme.Identity;
using projeDeneme.Models.Security;

namespace projeDeneme.Controllers
{
    public class SecurityController : Controller
    {
        private UserManager<AppIdentityUser> _userManager;
        private SignInManager<AppIdentityUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SecurityController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager,
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }


            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(String.Empty, "Email Doğrulaması Yapmanız Gerekmektedir");
                    return View(loginViewModel);
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Kullanıcıya Ulaşılamadı Kayıt olduğunuzdan Eminmisiniz?");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);
            //ilk false benihatırla butonu ikinci false iseStarupclasında yazılan kullanıcıyının sifreyi hatalı girme ve beklemesini aktif ediyor
            if (result.Succeeded)
            {
                return RedirectToAction("Index2", "Home");
            }


            ModelState.AddModelError(String.Empty, "Kullanıcı Şifre Hatalı");
            return View(loginViewModel);


        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccesDenied()
        {
            return View();
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, IFormFile file)
        {
            //File Upload ile resim alınıyor
            string path_to_image = "";

            if (registerViewModel.userImage != "" || registerViewModel.userImage.Length != 0)
            {
                //yolu aldık

                string path_Root = _hostingEnvironment.WebRootPath;

                path_to_image = path_Root + "\\wwwroot\\UserFile\\Images\\Images" + file.FileName;
                //dosyayı hedefe kopyalayıp yolu db ye gönderceğiz

                using (var stream = new FileStream(path_to_image, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }
                path_to_image = "\\UserFile\\Images\\Images" + file.FileName;
                //tolga hocaya sorulacak buişlem bu şekildemi yapılıyor!!!!

            };
            //register form oluşturma alanı
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                userImage = path_to_image
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var cofirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmeEmail", "Security", new { userId = user.Id, code = cofirmationCode.Result });
                return RedirectToAction("Index", "Home");
            }
            return View(registerViewModel);




            //Email Gönderilecek Kod Blogu;           



        }
        public async Task<IActionResult> ConfirmeEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                throw new ApplicationException("Kullanıcı biligilerine ulaşılamadı");

            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("chosePlan", "Security");
            }


            return RedirectToAction("Index", "Home");

        }
        public IActionResult chosePlan()
        {
             return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View();
            }

            var cofirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callBackUrl = Url.Action("ResetPassword", "Security", new { userId = user.Id, code = cofirmationCode });

            return RedirectToAction("ForgotPasswordEmailSent");

            ////Email Gönderilecek Kod Blogu;  callBackUrl

        }
        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }
        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                throw new ApplicationException("Bir Sorunla Karşılaşıldı ");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                throw new ApplicationException("Kullanıcı Bulunamadı");
            }
            var result = await _userManager.ResetPasswordAsync(user,
                resetPasswordViewModel.Code, resetPasswordViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            return View();

        }
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
        public IActionResult UserList()
        {
            var model = _userManager.Users;
            return View(model);
        }




    }
}