using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebATB.Areas.Admin.Models;
using WebATB.Data;
using WebATB.Data.Entities.Identity;

namespace WebATB.Areas.Admin.Controllers;
[Area("Admin")]
public class UsersController(AppATBDbContext db, IMapper mapper, UserManager<UserEntity> userManager) : Controller
{

    public async Task<IActionResult> Index()
    {
        var users = await db.Users
            .ProjectTo<UserItemVm>(mapper.ConfigurationProvider)
            .ToListAsync();

        return View(users);
    }

    public async Task<IActionResult> Ban(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            // logout user
            await userManager.UpdateSecurityStampAsync(user);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Unban(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            await userManager.SetLockoutEndDateAsync(user, null);
            // logout user
            await userManager.UpdateSecurityStampAsync(user);
        }
        return RedirectToAction("Index");
    }

    public IActionResult SetAdmin(int id)
    {
        var user = db.Users.Include(p => p.UserRoles).FirstOrDefault(p => p.Id == id);
        var role = db.Roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");

        if (user != null && role != null)
        {
            user.UserRoles.Clear();
            user.UserRoles.Add(new UserRoleEntity
            {
                RoleId = role.Id,
                UserId = user.Id
            });
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult SetUser(int id)
    {
        var user = db.Users.Include(p => p.UserRoles).FirstOrDefault(p => p.Id == id);
        var role = db.Roles.FirstOrDefault(r => r.NormalizedName == "USER");

        if (user != null && role != null)
        {
            user.UserRoles.Clear();
            user.UserRoles.Add(new UserRoleEntity
            {
                RoleId = role.Id,
                UserId = user.Id
            });
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
