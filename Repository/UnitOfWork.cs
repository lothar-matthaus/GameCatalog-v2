using GameCatalog.Repository.Interfaces;

namespace GameCatalog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGameRepository Game { get; }
        public IGenreRepository Genre { get; }
        public IUserRepository User { get; }

        public UnitOfWork(IGameRepository gameRepository, IGenreRepository genreRepository, IUserRepository userRepository)
        {
            Game = gameRepository;
            Genre = genreRepository;
            User = userRepository;
        }
    }
}