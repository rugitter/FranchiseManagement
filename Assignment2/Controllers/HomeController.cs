using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;
using Microsoft.AspNetCore.Authorization;
using Assignment2.Data;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = Constants.OwnerRole)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Constants.FranchiseeRole + "," + Constants.OwnerRole)]
        public IActionResult About()
        {
            ViewData["Message"] = "Your Franchisee role - only page.";

            return View();
        }

        [Authorize(Roles = Constants.CustomerRole + "," + Constants.OwnerRole)]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your Customer role - only page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
