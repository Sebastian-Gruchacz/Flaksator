using System;

namespace Obscureware.Flaksator.Data
{
    public interface IDocumentDatabase : IDisposable
    {
        IStringListsRepository ListResources { get; }


    }
}