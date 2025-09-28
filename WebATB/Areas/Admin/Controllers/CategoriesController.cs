using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebATB.Data;
using WebATB.Models.Category;

namespace WebATB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController(AppATBDbContext db, IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            var categories = db.Categories.ProjectTo<CategoryItemModel>(mapper.ConfigurationProvider).ToList();
            return View(categories);
        }
    }
}
