using System;
using System.Collections.Generic;
using LiteDB;
using ObscureWare.DocumentDatabase;

namespace Obscureware.Flaksator.Data
{
    internal class LightDictionaryRepositories : IDictionaryRepositories
    {
        private readonly LiteDatabase _db;
        private BaseDictionaryRepository<SongPiece, string> _songPiecesRepository;
        private const string SONG_PIECES_ID = @"SONG_PIECES";

        public LightDictionaryRepositories(LiteDatabase db)
        {
            _db = db;
            _songPiecesRepository = new BaseDictionaryRepository<SongPiece, string>(_db);
        }

        public Dictionary<SongPiece, string> GetConstantSongPieces()
        {
            return _songPiecesRepository.GetDictionary(SONG_PIECES_ID);
        }

        public void SaveConstantSongPieces(Dictionary<SongPiece, string> dictionary)
        {
            _songPiecesRepository.SaveDictionary(SONG_PIECES_ID, dictionary);
        }
    }
}