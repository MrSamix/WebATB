using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Claims;
using WebATB.Data.Entities.Identity;
using WebATB.Interfaces;
using WebATB.Models.Account;

namespace WebATB.Controllers
{
    [Area("Default")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        IImageService service;
        IMapper mapper;
        public AccountController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<RoleEntity> roleManager, IImageService service, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.service = service;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = null;
                if (!model.UsernameOrEmail.Contains('@'))
                {
                    result = await _signInManager.PasswordSignInAsync(model.UsernameOrEmail, model.Password, model.RememberMe, false);
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                    if (user != null)
                    {
                        result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    }
                }
                if (result != null && result.Succeeded)
                {
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильний логін та/або пароль");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imageStr = model.Image is not null ? await service.SaveImageAsync(model.Image, "AvatarDir") : null;
                //UserEntity user = new UserEntity
                //{
                //    Email = model.Email,
                //    NormalizedEmail = model.Email,
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    Image = imageStr,
                //    UserName = model.UserName
                //};
                var user = mapper.Map<UserEntity>(model);

                user.Image = imageStr;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Ensure default role "User" exists
                    const string roleName = "User";
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var createRoleResult = await _roleManager.CreateAsync(new RoleEntity
                        {
                            Name = roleName,
                            NormalizedName = roleName.ToUpperInvariant()
                        });
                        if (!createRoleResult.Succeeded)
                        {
                            foreach (var error in createRoleResult.Errors)
                                ModelState.AddModelError(string.Empty, error.Description);
                            return View(model);
                        }
                    }

                    // Add user to default role
                    var addToRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!addToRoleResult.Succeeded)
                    {
                        foreach (var error in addToRoleResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                        return View(model);
                    }

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }
    }
}
