using Blazor.Shared.FormGenerator.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Blazor.UI.FormGenerator;

public partial class DynamicMudForm
{

    #region Get Field
    public static Field? GetField(FormBuilder[] formBuilders, string cardName, string fieldName)
    {
        Field? resultField = null;
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {            
            var field = detailPanel.Fields.FirstOrDefault(field => field.FieldName == fieldName);
            if (field is not null)
            {
                 resultField = field;
            }
        }
        return resultField;
    }
    #endregion

    /// <summary>
    /// Attach the validation event on the required field
    /// </summary>
    /// <param name="formBuilders">form builder array of items</param>
    /// <param name="cardName">Name of the card</param>
    /// <param name="fieldName">Name of the field</param>
    /// <param name="validationMethod">Validation method</param>
    /// <code>
    /// e.g. new UrlAttribute() { ErrorMessage = "The URL is not valid" };
    /// e.g. MaxCharacters(string ch)  { ... }
    /// </code>
    /// <returns></returns>
    public static bool AttachTextChangeEvent(FormBuilder[] formBuilders, string cardName, string fieldName, Func<string, Task?> eventMethod)
    {
        bool result = false;
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            var field = detailPanel.Fields.FirstOrDefault(field => field.FieldName == fieldName);
            if (field is not null)
            {
                field.TextChanged_Handler = eventMethod;
                result = true;
            }
        }
        return result;
    }

    #region Validation Event Mapper
    /// <summary>
    /// Attach the validation event on the required field
    /// </summary>
    /// <param name="formBuilders">form builder array of items</param>
    /// <param name="cardName">Name of the card</param>
    /// <param name="fieldName">Name of the field</param>
    /// <param name="validationMethod">Validation method</param>
    /// <code>
    /// e.g. new UrlAttribute() { ErrorMessage = "The URL is not valid" };
    /// e.g. MaxCharacters(string ch)  { ... }
    /// </code>
    /// <returns></returns>
    public static bool AttachValidationEvent(FormBuilder[] formBuilders, string cardName, string fieldName, object validationMethod)
    {
        bool result = false;
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            var field = detailPanel.Fields.FirstOrDefault(field => field.FieldName == fieldName);
            if (field is not null)
            {
                field.Validation.Method = validationMethod;
                result = true;
            }
        }
        return result;
    }

    #endregion

    public static bool AttachCard_EventAction(FormBuilder[] formBuilders, string cardName, 
        Func<Task?> cardAction)
    {
        bool result = false;
        //Identify Card Panel and attach 'Func<Task?>'
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            detailPanel.Header.CardAction.ActionTrigger = cardAction;
            result = true;
        }
        return result;
    }
    
    public static bool AttachSubmitButton_EventAction(FormBuilder[] formBuilders, string cardName, 
        Func<EventArgs, Task?> cardAction)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<EventArgs, Task?> cardAction'
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            detailPanel.Footer.Submit_Handler = cardAction;
            result = true;
        }
        return result;
    }
    
    /// <summary>
    /// Cancel button action attached for butten event.
    /// </summary>
    /// <param name="formBuilders"></param>
    /// <param name="cardName"></param>
    /// <param name="eventAction"></param>
    /// <returns></returns>
    public static bool AttachCancelButton_EventAction(FormBuilder[] formBuilders, string cardName, 
        Func<Task?> eventAction)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<EventArgs, Task?> cardAction'
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            detailPanel.Footer.Cancel_Handler = eventAction;
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="formBuilders"></param>
    /// <param name="cardName"></param>
    /// <param name="fieldName"></param>
    /// <param name="eventAction"></param>
    /// <param name="logger">logger object to log</param>
    /// <returns></returns>
    public static bool AutoComplete_EventAction(
        FormBuilder[] formBuilders, 
        string cardName, 
        string fieldName,
        Func<string, CancellationToken, Task<IEnumerable<string>>> eventAction, ILogger<object>? logger = null)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<string, CancellationToken, Task<IEnumerable<string>>>'
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            var autoCompleteField = detailPanel.Fields.FirstOrDefault(field => field.FieldName == fieldName);
            if (autoCompleteField is not null)
            {
                autoCompleteField.AutoCompleteFunction =  eventAction;
                logger?.LogInformation("{FieldName} - AutoComplete Attached", fieldName);
            }
            result = true;
        }
        return result;
    }
    
    public static bool AttachFileUpload_EventAction(FormBuilder[] formBuilders, string cardName, string fieldName,
        Func<IBrowserFile, Task?> eventAction , ILogger<object> logger = null)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<IBrowserFile, Task?> eventAction '
        var detailPanel = formBuilders.FirstOrDefault(card => card.Card == cardName);
        if (detailPanel is not null)
        {
            var autoCompleteField = detailPanel.Fields.Where(field => field.FieldName == fieldName).FirstOrDefault();
            if (autoCompleteField is not null)
            {
                autoCompleteField.FilesChangedFunction = eventAction;
                logger?.LogInformation("FieldName: {FieldName} - Event: {MethodName} Attached", fieldName, eventAction.Method.Name);
            }
            result = true;
        }
        return result;
    }
}