using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebATB.Areas.Admin.Controllers.Models;

namespace WebATB.Areas.Admin.Controllers;
[Area("Admin")]
public class PagesController : Controller
{
  public IActionResult AccountSettings() => View();
  public IActionResult AccountSettingsConnections() => View();
  public IActionResult AccountSettingsNotifications() => View();
  public IActionResult MiscError() => View();
  public IActionResult MiscUnderMaintenance() => View();
}
