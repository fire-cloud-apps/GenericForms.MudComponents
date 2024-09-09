using MudBlazor;

namespace Blazor.UI.FormGenerator.Builders;

public class FormProperty
{
    public bool EnableCard { get; set; } = false;
    public CardAvatar CardHeaderAvatar { get; set; }
    public CardAction CardHeaderAction { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public bool TitleDivider { get; set; } = false;
    public CardSettings CardSettings { get; set; } = new CardSettings();

}

public class FooterSettings
{
    public bool EnableFooter { get; set; } = true;
    public bool Cancel { get; set; } = true;
    public string CancelText { get; set; } = "Cancel";
    public bool Submit { get; set; } = true;
    public string SubmitText { get; set; } = "Submit";
}
public class CardSettings
{
    public string CustomClass { get; set; } = "ma-4";
    public int Elevation { get; set; } = 0;
    public bool Outlined { get; set; } = true;
    public FooterSettings FooterSettings { get; set; } = new FooterSettings();
}
public class CardAvatar : BaseAction
{
    public Variant Variant { get; set; }
}
public class CardAction : BaseAction
{
    public Func<Task?> CardHeaderTrigger { get; set; }
}

public class BaseAction
{
    public bool IsEnable { get; set; }
    public string Icons { get; set; }
    public Color Color { get; set; }
    
}