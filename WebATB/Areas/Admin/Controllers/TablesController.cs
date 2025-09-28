using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebATB.Areas.Admin.Controllers.Models;

namespace WebATB.Areas.Admin.Controllers;
[Area("Admin")]
public class TablesController : Controller
{
  public IActionResult Basic() => View();
}
