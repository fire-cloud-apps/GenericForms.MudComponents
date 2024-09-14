using System.Net.Http.Json;
using Blazor.Shared.FormGenerator;
using Blazor.Shared.FormGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace BlazorForms.NoLazyLoad.Pages.Academy;

public partial class StudentProfile : ComponentBase
{

    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        await StudentDetails();
        await GuardianDetails();
        await AttachmentDetails();
        await CourseHistoryDetails();
    }

    #endregion

    #region Student Profile Details 
    FormBuilder[] _formStudentBuilders;
    private async Task StudentDetails()
    {
        #region Student Details
        _formStudentBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/student/student-profile.json");

        //Detail Panel
        var detailPanel = _formStudentBuilders.Where(card => card.Card == "Details").FirstOrDefault();
        if (detailPanel is not null)
        {
            detailPanel.Header.CardAction.ActionTrigger = async () =>
            {
                Console.WriteLine("Triggered the Counter Action");
            };
            
            var nameField = detailPanel.Fields.Where(field => field.FieldName == "FirstName").FirstOrDefault();
            if (nameField is not null)
            {
                nameField.ValidateInput = MaxCharacters;
            }
        }
        
        //Contact Panel
        var contactPanel = _formStudentBuilders.Where(card => card.Card == "Contacts").FirstOrDefault();
        if (contactPanel is not null)
        {
            contactPanel.Header.CardAction.ActionTrigger = async () =>
            {
                Console.WriteLine("Triggered the Counter Action");
            };
            
            var countryField = detailPanel.Fields.Where(field => field.FieldName == "Country").FirstOrDefault();
            
            if (countryField is not null)
            {
                countryField.AutoCompleteFunction = FindCountry;
                Console.WriteLine("Country AutoComplete Attached.");
            }
        }
        #endregion
    }

    #endregion

    #region Guardian Details
    FormBuilder[] _formGuardianBuilders;
    private async Task GuardianDetails()
    {
        _formGuardianBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/student/guardian-details.json");
        //Contact Panel
        var contactPanel = _formGuardianBuilders.Where(card => card.Card == "Contacts").FirstOrDefault();
        if (contactPanel is not null)
        {
            var countryField = contactPanel.Fields.Where(field => field.FieldName == "Country").FirstOrDefault();
            
            if (countryField is not null)
            {
                countryField.AutoCompleteFunction = FindCountry;
                Console.WriteLine("Country AutoComplete Attached.");
            }
        }
    }

    #endregion
    
    #region Attached Documents Details
    FormBuilder[] _formAttachmentsBuilders;
    private GlobalBuilderSettings _globalBuilderSettings = new GlobalBuilderSettings();
    private async Task AttachmentDetails()
    {
        _formAttachmentsBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/student/attachments.json");
        var attachmentPanel = _formAttachmentsBuilders.Where(card => card.Card == "Attachments").FirstOrDefault();
        
        if (attachmentPanel is not null)
        {
            var studentPhoto = attachmentPanel.Fields
                .Where(field => field.FieldName == "StudentPhoto").FirstOrDefault();
            
            if (studentPhoto is not null)
            {
                studentPhoto.FilesChangedFunction = UploadPhotoFile;
                Console.WriteLine("Student Photo Function Attached.");
            }
            var studentFile = attachmentPanel.Fields
                .Where(field => field.FieldName == "StudentFile").FirstOrDefault();
            if (studentFile is not null)
            {
                studentFile.FilesChangedFunction = UploadStudentFile;
                Console.WriteLine("Student File Function Attached.");
            }
        }
    }
    private async Task UploadPhotoFile(IBrowserFile file)
    {
        string key = "StudentPhoto";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }
    private async Task UploadStudentFile(IBrowserFile file)
    {
        string key = "StudentFile";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }
    #endregion
    
    #region Guardian Details
    FormBuilder[] _formCourseHistoryBuilders;
    private async Task CourseHistoryDetails()
    {
        _formCourseHistoryBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/student/course-history.json");
       
    }

    #endregion
    
    #region Validation Methods
    private IEnumerable<string> MaxCharacters(string ch)
    {
        if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
            yield return "Max 25 characters";
    }
    #endregion

    #region Auto Complete

    public IDictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>> _funcAddressList = new Dictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>>();
    private string[] countries =
    {
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
        "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Marshall Islands", "Maryland",
        "Massachusetts", "Michigan", "Minnesota", "Mississippi",
        "Missouri", "Montana", "Nebraska", "Nevada",
        "New Hampshire", "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio",
        "Oklahoma", "Oregon", "Palau", "Pennsylvania", "Puerto Rico",
        "Rhode Island", "South Carolina", "South Dakota", "Tennessee",
        "Texas", "Utah", "Vermont", "Virgin Island", "Virginia",
        "Washington", "West Virginia", "Wisconsin", "Wyoming",
    };
    private async Task<IEnumerable<string>> FindCountry(string value, CancellationToken token)
    {
        Console.WriteLine("Triggred Auto Complete Country.");
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5, token);
        //To Test using API's https://dummyjson.com/docs/products#products-search
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return countries;
        return countries.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    #endregion
    
}