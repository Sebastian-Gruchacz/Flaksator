namespace Obscureware.Flaksator.Data
{
    using System;

    public interface IDocumentDatabase : IDisposable
    {
        IStringListsRepository ListResources { get; }

        IDictionaryRepositories DictionaryResources { get; }
    }
}