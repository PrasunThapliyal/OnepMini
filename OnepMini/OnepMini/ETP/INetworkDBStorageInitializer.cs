namespace OnepMini.ETP
{
    public interface INetworkDBStorageInitializer
    {
        void RunMigrations();
    }
}