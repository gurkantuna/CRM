using CRM.Business.Abstract;
using CRM.Core.Constants;
using CRM.Core.Infrastructure.Concrete.DotNetCore;
using CRM.Core.Managers.Abstract;
using CRM.Core.ViewModels.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Presentation.Areas.Admin.Controllers {
    [Area(nameof(Strings.Admin))]
    public class AccountController : DbCheckStatusController<CrmDbContext> {
        public AccountController(UserManager<User> userManager,
                                SignInManager<User> signManager,
                                RoleManager<IdentityRole<int>> roleManager,
                                IDbLogger dbLogger,
                                IDbControlManager<CrmDbContext> dbControlManager) : base(dbControlManager) {
            _signManager = signManager;
            _roleManager = roleManager;
            _userManager = userManager;
            _dbLogger = dbLogger;
        }

        private readonly SignInManager<User> _signManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IDbLogger _dbLogger;

        public async Task<IActionResult> Index(UserLoginViewModel userLoginViewModel) {
            if(userLoginViewModel.IsCreated || userLoginViewModel?.UserName == null) {
                userLoginViewModel = new UserLoginViewModel();
                ModelState.Clear();
                _dbLogger.LogToDbByMessage("Login ekranı açıldı", LogLevel.Information);
            }

            try {
                return User.Identity is { IsAuthenticated: true }
                       ? RedirectToAction(nameof(Index), Strings.Home, new { area = string.Empty })
                       : View(userLoginViewModel);
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Login ekranında hata alındı", LogLevel.Error, ex);
            }
            return View(userLoginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginViewModel userLoginViewModel) {
            try {
                if(ModelState.IsValid) {
                    var signInResult = await _signManager.PasswordSignInAsync(userLoginViewModel.UserName!, userLoginViewModel.Password, false, false);
                    if(signInResult.Succeeded) {
                        _dbLogger.LogToDbByMessage("Giriş yapıldı", LogLevel.Information);
                        return RedirectToAction(nameof(Index), Strings.Home, new { area = string.Empty, userLoginViewModel.ReturnUrl });
                    }
                    else {
                        userLoginViewModel.Errors.Add($"Username or password is incorrect!");
                        _dbLogger.LogToDbByMessage("Yanlış bilgilerle giriş denemesi!", LogLevel.Information);
                    }
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Giriş sırasında hata", LogLevel.Critical, ex);
            }
            return View(nameof(Index), userLoginViewModel);
        }

        [Authorize]
        public async Task<IActionResult> SignOut() {
            try {
                if(User.Identity is { IsAuthenticated: true }) {
                    var signOutResult = _signManager.SignOutAsync();
                    if(signOutResult.IsCompletedSuccessfully) {
                        _dbLogger.LogToDbByMessage("Çıkış yapıldı", LogLevel.Information);
                        return RedirectToAction(nameof(Index), new { UserName = User.Identity.Name });
                    }
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Çıkış sırasında hata", LogLevel.Critical, ex);
            }
            return NoContent();
        }

        public IActionResult Register(UserRegisterViewModel userRegisterViewModel) {
            try {
                if(userRegisterViewModel.IsCreated || userRegisterViewModel?.Email == null) {
                    ModelState.Clear();
                    userRegisterViewModel = new UserRegisterViewModel();
                    _dbLogger.LogToDbByMessage("Kayıt ekranı  açıldı", LogLevel.Information);
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Kayıt giriş ekranında hata", LogLevel.Error, ex);
            }
            return View(userRegisterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterViewModel userRegisterViewModel) {
            try {
                if(ModelState.IsValid) {
                    var user = new User {
                        Email = userRegisterViewModel.Email,
                        PasswordHash = userRegisterViewModel.Password,
                        UserName = userRegisterViewModel.UserName,
                    };


                    var result = await _userManager.CreateAsync(user, userRegisterViewModel.Password);

                    if(result.Succeeded) {
                        _dbLogger.LogToDbByMessage($"{user} kullannıcısı oluşturuldu", LogLevel.Information);
                        userRegisterViewModel.IsCreated = true;

                        var role = "Admin";

                        if(!await _roleManager.RoleExistsAsync(role)) {
                            result = await _roleManager.CreateAsync(new IdentityRole<int>(role));
                            _dbLogger.LogToDbByMessage($"{role} rolü oluşturuldu", LogLevel.Information);
                        }

                        if(result.Succeeded) {
                            result = await _userManager.AddToRoleAsync(user, role);
                            if(result.Succeeded) {
                                userRegisterViewModel.IsAddedToRole = true;
                                _dbLogger.LogToDbByMessage($"{user} kullanıcısı {role} rolüne atandı", LogLevel.Information);
                                await _signManager.SignInAsync(user, isPersistent: false);
                                return RedirectToAction(nameof(Index), Strings.Home, new { area = string.Empty });
                            }
                        }
                        else {
                            userRegisterViewModel.Errors = result.Errors.Select(e => e.Description).ToList();
                            return RedirectToAction(nameof(Register), userRegisterViewModel);
                        }
                    }
                    else {
                        userRegisterViewModel.Errors = result.Errors.Select(e => e.Description).ToList();
                        _dbLogger.LogToDbByMessage($"Kullanıcı kaydında verilen yanlış bilgi", LogLevel.Information);
                        return RedirectToAction(nameof(Register),
                            new UserRegisterViewModel {
                                UserName = userRegisterViewModel.UserName,
                                Email = userRegisterViewModel.Email,
                                Password = userRegisterViewModel.Password,
                                ConfirmPassword = userRegisterViewModel.ConfirmPassword,
                                RememberMe = userRegisterViewModel.RememberMe,
                                Role = userRegisterViewModel.Role,
                                Errors = userRegisterViewModel.Errors,
                            });
                    }
                }
                else {
                    return RedirectToAction(nameof(Register), userRegisterViewModel);
                }
            }
            catch(Exception ex) {
                _dbLogger.LogToDbByMessage("Kayıt oluştururken hata", LogLevel.Critical, ex);
            }
            return NoContent();
        }
    }
}
