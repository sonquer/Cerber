using System;
using System.IO;
using Cerber.Repository.Models;
using SQLite;

namespace Cerber.Repository
{
    public class CerberDatabaseContext : IRepository
    {
        private readonly SQLiteConnection _SQLiteConnection;

        public CerberDatabaseContext()
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "cerberapp.db3");
            _SQLiteConnection = new SQLiteConnection(databasePath);
            _SQLiteConnection.CreateTable<Token>();
        }

        public Token GetToken()
        {
            if (_SQLiteConnection.Table<Token>().Count() <= 0)
            {
                return null;
            }

            return _SQLiteConnection.Get<Token>(e => true);
        }

        public void UpdateToken(Token token)
        {
            _SQLiteConnection.InsertOrReplace(token, typeof(Token));
        }
    }
}
