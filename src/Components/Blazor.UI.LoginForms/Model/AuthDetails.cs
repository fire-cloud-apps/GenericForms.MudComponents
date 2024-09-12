using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.UI.LoginForms.Model;

public class AuthDetails
{
    public string Email { get; set; } = "staff@company.com";
    public string Password { get; set; } = "BMWvBPJXZu";

    public bool Remember { get; set; } = false;
}

