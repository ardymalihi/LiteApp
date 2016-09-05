﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteApp.Services;
using LiteApp.ViewModels;
using LiteApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Schema;

namespace LiteApp.Controllers
{
    public class AppController : Controller
    {
        private IAppService _appService;

        public AppController(IAppService appService)
        {
            _appService = appService;
        }
        public IActionResult Index(string page)
        {
            var pageModel = _appService.GetCurrentPage("/" + page);
            var appViewModel = new AppViewModel
            {
                App = _appService.App,
                CurrentPage = pageModel ?? GetNotFoundPage()
            };

            if (User.Identity.IsAuthenticated)
            {
                appViewModel.UserProfile = new UserProfileViewModel
                {
                    Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                    EmailAddress = User.Claims.FirstOrDefault(c => c.Type == "emailaddress")?.Value,
                    ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value,
                    Country = User.Claims.FirstOrDefault(c => c.Type == "country")?.Value
                };
            }

            return View(appViewModel);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Admin()
        {
            var jsonSchemaGenerator = new JsonSchemaGenerator();
            var myType = typeof(App);
            var schemaGenerator = jsonSchemaGenerator.Generate(myType);
            schemaGenerator.Title = myType.Name;
            var schema = schemaGenerator.ToString();

            var adminViewModel = new AdminViewModel
            {
                App = _appService.App,
                Schema = schema
            };

            return View(adminViewModel);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Save(App request)
        {
            _appService.Save(request);
            return Ok();
        }

        public IActionResult Error()
        {
            return View();
        }

        private Page GetNotFoundPage()
        {
            return new Models.Page
            {
                Route = this.Request.Path.Value,
                Rows = new List<Models.Row> {
                    new Models.Row {
                        ClassName = "row",
                        Cols = new List<Models.Col> {
                            new Models.Col {
                                ClassName = "col-md-12",
                                Modules = new List<Models.Module> {
                                    new Module {
                                        Content = _appService.App.NotFoundHtml
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}