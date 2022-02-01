namespace GameCatalog.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IGameRepository Game { get; }
        public IGenreRepository Genre { get; }
        public IUserRepository User { get; }
    }
}