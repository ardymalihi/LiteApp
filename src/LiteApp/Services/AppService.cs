﻿using LiteApp.Data;
using LiteApp.Models;
using System.Linq;
using System;

namespace LiteApp.Services
{
    public class AppService: IAppService
    {
        private IAppData _appData;
        private App _app;

        public App App
        {
            get
            {
                return _app;
            }
        }

        public AppService(IAppData appData)
        {
            _appData = appData;

            _app = _appData.Load();
        }

        public void Save(App app)
        {
            _app = app;

            _appData.Save(_app);
        }

        public Page GetCurrentPage(string route)
        {
            var compareValue = route.ToLower().TrimEnd(new char[] { ' ', '?' }).TrimStart('/');

            if (string.IsNullOrWhiteSpace(compareValue))
            {
                compareValue = "home";
            }
            return _app.Pages.FirstOrDefault(o => o.Name.ToLower() == compareValue);
        }

        public Module GetModule(string moduleId)
        {
            return _app.Pages
                .SelectMany(p => p.Rows)
                .SelectMany(r => r.Cols)
                .SelectMany(c => c.Modules)
                .FirstOrDefault(m => m.Id == moduleId);
        }
    }
}
