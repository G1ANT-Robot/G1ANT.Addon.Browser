using G1ANT.Browser.Driver.Actions;
using G1ANT.Browser.Driver.Data;

namespace G1ANT.Browser.Driver.Interfaces
{
    public interface IBrowserDriver
    {
        BrowserTab ActivateTab(ActivateTabAction action);
        void Click(ClickAction action);
        void CloseTab(CloseTabAction action);
        BrowserTab GetActiveTab(GetActiveTabAction action);
        string GetAttribute(GetAttributeAction action);
        string GetHtml(GetHtmlAction action);
        string GetOuterHtml(GetOuterHtmlAction action);
        string GetInnerHtml(GetInnerHtmlAction action);
        string GetText(GetTextAction action);
        BrowserTab NewTab(NewTabAction action);
        BrowserTab FindTab(FindTabAction action);
        BrowserTab Open(OpenAction action);
        BrowserTab Refresh(RefreshAction action);
        void SetAttribute(SetAttributeAction action);
        BrowserTab SetUrl(SetUrlAction action);
        void TypeText(TypeTextAction action);
        void PressKey(PressKeyAction action);
    }
}
