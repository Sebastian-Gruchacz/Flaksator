using System.Collections.Generic;

namespace Obscureware.Flaksator.Data
{
    public interface IDictionaryRepositories
    {
        Dictionary<SongPiece, string> GetConstantSongPieces();


        void SaveConstantSongPieces(Dictionary<SongPiece, string> dictionary);
    }
}