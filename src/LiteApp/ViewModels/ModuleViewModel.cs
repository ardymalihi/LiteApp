﻿using LiteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApp.ViewModels
{
    public class ModuleViewModel
    {
        public App App { get; set; }

        public Page CurrentPage { get; set; }

        public IModule CurrentModule { get; set; }
    }
}
