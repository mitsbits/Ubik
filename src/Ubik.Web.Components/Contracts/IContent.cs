namespace Ubik.Web.Components.Contracts
{
    public interface IContent
    {
        ITextualInfo Textual { get; }
        IHtmlHeadInfo HtmlHeadInfo { get; }
    }
}