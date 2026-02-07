using System;
using CodeBase.Gameplay.Field;
using CodeBase.Gameplay.Field.Config;
using CodeBase.Gameplay.Level;
using VContainer;
using VContainer.Unity;

namespace CodeBase.Infrastructure.Composition
{
    public class GameCompositionRoot : 
        IInitializable,
        IStartable,
        IDisposable
    {
        private readonly IObjectResolver _resolver;

        public GameCompositionRoot(IObjectResolver resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public void Initialize()
        {
            _resolver.Resolve<FieldPresenter>().Attach();   
            _resolver.Resolve<CellView.Pool>().Start();
        }

        public void Start()
        {
            _resolver
                .Resolve<LevelBuilder>()
                .Build(_resolver.Resolve<LevelConfig>());
        }

        public void Dispose()
        {
            _resolver.Resolve<FieldPresenter>().Dispose();
        }
    }
}