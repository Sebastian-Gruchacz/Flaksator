namespace Obscureware.Flaksator.Data
{
    using System.Collections.Generic;

    public interface IDictionaryRepositories
    {
        Dictionary<SongPiece, string> GetConstantSongPieces();


        void SaveConstantSongPieces(Dictionary<SongPiece, string> dictionary);
    }
}