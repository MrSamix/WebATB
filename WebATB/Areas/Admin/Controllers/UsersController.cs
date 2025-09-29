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
    //public async Task<IActionResult> Index()
    //{
    //    var users = await db.Users.AsNoTracking().ToListAsync();

    //    // Fetch roles sequentially to avoid concurrent DbContext operations
    //    var model = new List<UserItemVm>(users.Count);
    //    foreach (var u in users)
    //    {
    //        var roles = await userManager.GetRolesAsync(u);
    //        model.Add(new UserItemVm
    //        {
    //            Id = u.Id,
    //            Email = u.Email,
    //            UserName = u.UserName,
    //            Roles = roles.ToList(),
    //            Image = u.Image,
    //            FirstName = u.FirstName,
    //            LastName = u.LastName
    //        });
    //    }

    //    return View(model);
    //}
    public async Task<IActionResult> Index()
    {
        var users = await db.Users
            .ProjectTo<UserItemVm>(mapper.ConfigurationProvider)
            .ToListAsync();

        return View(users);
    }
    //public IActionResult Delete(int id)
    //{
    //    var user = db.Users.Find(id);
    //    if (user != null)
    //    {
    //        db.Users.Remove(user);
    //        db.SaveChanges();
    //    }
    //    return RedirectToAction("Index");
    //}
    public IActionResult Delete(int id)
    {
        var user = db.Users.Find(id);
        if (user != null)
        {
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(1000);
            db.Users.Update(user);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
