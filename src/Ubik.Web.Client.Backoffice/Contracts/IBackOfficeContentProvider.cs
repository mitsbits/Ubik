namespace Ubik.Web.Client.Backoffice.Contracts
{
    internal interface IBackofficeContentProvider
    {
        IBackofficeContent Current { get; }
    }
}