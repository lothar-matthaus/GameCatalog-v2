using GameCatalog.Repository.Interfaces;

namespace GameCatalog.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGameRepository Game { get; }
        public IGenreRepository Genre { get; }

        public UnitOfWork(IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            Game = gameRepository;
            Genre = genreRepository;
        }
    }
}