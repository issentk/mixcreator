using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Sqlite;

namespace MixCreator.Model
{
    class SongContext : DbContext, IContext<Song>
    {
        public SongContext(IConfig config)
        {
            Config = config;
        }

        // Config
        private IConfig Config { get; set; }

        // This property defines the table
        public DbSet<Song> SongTable { get; set; }
    
        // This method connects the context with the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = Config.GetDatabaseName() };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        public DbSet<Song> GetTable()
        {
            return SongTable;
        }

        public DatabaseFacade GetDatabase()
        {
            return Database;
        }

        public void Update(Song obj)
        {
            base.Update(obj);
        }

        public void Save()
        {
            SaveChanges();
        }
    }
}
