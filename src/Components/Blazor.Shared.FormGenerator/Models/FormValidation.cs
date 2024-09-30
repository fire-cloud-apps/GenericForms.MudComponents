namespace Blazor.Shared.FormGenerator.Models;

public class FormValidation
{
    public bool IsRequired { get; set; }
    public string RequiredError { get; set; }
    
    /// <summary>
    /// The function used to detect problems with the input. Eg. you can pass it as
    /// new EmailAddressAttribute() { ErrorMessage = "The email address is invalid" };
    ///  new UrlAttribute() { ErrorMessage = "The URL is invalid" };
    /// More custom methods.
    public object? Method { get; set; }
}