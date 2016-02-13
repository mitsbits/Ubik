namespace Ubik.Web.Components.Contracts
{
    public interface IHtmlMeta
    {
        string Content { get; }

        string HttpEquiv { get; }

        bool IsHttpEquiv { get; }

        bool IsOpenGraph { get; }

        string TypeIdentifier { get; }

        string Name { get; }

        string Scheme { get; }

        bool ShouldRender { get; }
    }
}