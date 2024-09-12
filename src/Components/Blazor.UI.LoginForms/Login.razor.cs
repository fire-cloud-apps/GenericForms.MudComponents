using Blazor.UI.LoginForms.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Linq;

namespace Blazor.UI.LoginForms;
public partial class Login
{
    #region Parameters
    [Parameter]
    public AuthDetails Auth { get; set; } = new AuthDetails();
    /// <summary>
    /// Logo Alignment to position the logo
    /// </summary>
    [Parameter]
    public AlignItems LogoAlign { get; set; } = AlignItems.Stretch;

    /// <summary>
    /// Class Name to achieve Gradient Style.
    /// </summary>
    /// <remarks>
    /// https://mycolor.space/?hex=%23845EC2&sub=1
    /// https://www.cssportal.com/css-text-gradient-generator/
    /// </remarks>
    [Parameter]
    public string TextGradient { get; set; } = "gradient";
    [Parameter]
    public string ForgetPasswordLink { get; set; }
    [Parameter]
    public string RegisterLink { get; set; }
    [Parameter]
    public string LogoIcon { get; set; } = Icons.Custom.Brands.GitHub;

    #endregion 

    #region Password Visibility Style
    bool PasswordVisibility;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
    #endregion

    #region SignIn Event Click

    [Parameter]
    public EventCallback<AuthDetails> OnClick { get; set; }

    private async Task HandleClickAsync(EventArgs args, AuthDetails authDetails)
    {
        // Check if OnClick has a delegate (method assigned)
        if (OnClick.Equals(EventCallback.Empty))
        {
            return; // No method assigned, do nothing
        }
        Console.WriteLine("Component Login Invoked.");
        await OnClick.InvokeAsync(authDetails);
    }
    #endregion
}