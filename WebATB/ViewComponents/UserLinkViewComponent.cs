using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebATB.Data.Entities.Identity;
using WebATB.Models.Account;

namespace WebATB.ViewComponents
{
    public class UserLinkViewComponent(UserManager<UserEntity> userManager,
        IMapper mapper) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity?.Name;
            var model = new UserLinkViewModel();

            if (userName != null)
            {
                var user = userManager.FindByNameAsync(userName).Result; // first & last == name
                if (user != null) { 
                    model = mapper.Map<UserLinkViewModel>(user);
                }
            }
            return View(model);
        }
    }
}
