namespace Obscureware.Flaksator.Data
{
    using System.Collections.Generic;

    using LiteDB;

    using ObscureWare.DocumentDatabase;

    internal class LightDictionaryRepositories : IDictionaryRepositories
    {
        private readonly LiteDatabase _db;
        private readonly BaseDictionaryRepository<SongPiece, string> _songPiecesRepository;
        private const string SONG_PIECES_ID = @"SONG_PIECES";

        public LightDictionaryRepositories(LiteDatabase db)
        {
            this._db = db;
            this._songPiecesRepository = new BaseDictionaryRepository<SongPiece, string>(this._db);
        }

        public Dictionary<SongPiece, string> GetConstantSongPieces()
        {
            return this._songPiecesRepository.GetDictionary(SONG_PIECES_ID);
        }

        public void SaveConstantSongPieces(Dictionary<SongPiece, string> dictionary)
        {
            this._songPiecesRepository.SaveDictionary(SONG_PIECES_ID, dictionary);
        }
    }
}