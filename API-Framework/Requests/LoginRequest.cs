﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Framework.Requests
{
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Passoword { get; set; }
    }
}