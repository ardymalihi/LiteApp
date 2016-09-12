﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApp.Models
{
    public class ModuleSettings
    {
        public bool HasSettings { get; set; }

        public List<ModuleSettingsAction> Actions { get; set; }
    }
}
