using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using elraee.Models;
using elraee.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace elraee.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<applicationUser> userManager;
        private readonly SignInManager<applicationUser> signInManager;
       
        public AccountController(UserManager<applicationUser> userManager,
                                 SignInManager<applicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

     /*   [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult RegisterAdmin()
        {
            return View("RegisterNewAdmin");
        }
      
        
        [HttpPost]
        public async Task<IActionResult> SaveRegister 
            (RegisterUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping
                applicationUser appUser = new applicationUser();
                appUser.Email = UserViewModel.Email;
                appUser.UserName = UserViewModel.UserName;
                appUser.PasswordHash = UserViewModel.Password;
                appUser.PhoneNumber = UserViewModel.phoneNumber;

                //save database
                IdentityResult result =
                    await userManager.CreateAsync(appUser, UserViewModel.Password);
                if (result.Succeeded)
                {
                   
                    //Cookie
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Car");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", UserViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterNewAdmin
            (RegisterUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping
                applicationUser appUser = new applicationUser();
                appUser.Email = UserViewModel.Email;
                appUser.UserName = UserViewModel.UserName;
                appUser.PasswordHash = UserViewModel.Password;
                appUser.PhoneNumber = UserViewModel.phoneNumber;

                //save database
                IdentityResult result =
                    await userManager.CreateAsync(appUser, UserViewModel.Password);
                if (result.Succeeded)
                {
                    //assign to role
                    await userManager.AddToRoleAsync(appUser, "Admin");
                    //Cookie
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Car");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", UserViewModel);
        }*/

        public IActionResult Login()
        {
            return View("Login");
            
        }


        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid == true)
            {
                //check found 
                applicationUser appUser =
                    await userManager.FindByNameAsync(userViewModel.Name);
                if (appUser != null)
                {
                    bool found =
                         await userManager.CheckPasswordAsync(appUser, userViewModel.Password);
                    if (found == true)
                    {
                        List<Claim> Claims = new List<Claim>();
                        Claims.Add(new Claim("UserEmail", appUser.Email));

                        await signInManager.SignInWithClaimsAsync(appUser, userViewModel.RememberMe, Claims);
                        //await signInManager.SignInAsync(appUser, userViewModel.RememberMe);
                        return RedirectToAction("Index", "category");
                    }

                }
                ModelState.AddModelError("", "Username OR PAssword wrong");
                //create cookie
            }
            return View("Login", userViewModel);
        }

        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }
    }
}
