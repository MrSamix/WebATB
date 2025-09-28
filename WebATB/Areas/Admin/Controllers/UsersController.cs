using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebATB.Areas.Admin.Models;
using WebATB.Data;
using WebATB.Data.Entities.Identity;

namespace WebATB.Areas.Admin.Controllers;
[Area("Admin")]
public class UsersController(AppATBDbContext db, UserManager<UserEntity> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await db.Users.AsNoTracking().ToListAsync();

        // Fetch roles sequentially to avoid concurrent DbContext operations
        var model = new List<UserListItemVm>(users.Count);
        foreach (var u in users)
        {
            var roles = await userManager.GetRolesAsync(u);
            model.Add(new UserListItemVm
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                Roles = roles.ToList(),
                Image = u.Image,
                FirstName = u.FirstName,
                LastName = u.LastName
            });
        }

        return View(model);
    }
    public IActionResult Delete(int id)
    {
        var user = db.Users.Find(id);
        if (user != null)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
