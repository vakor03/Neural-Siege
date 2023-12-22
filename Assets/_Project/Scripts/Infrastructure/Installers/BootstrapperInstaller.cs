using Zenject;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class BootstrapperInstaller : MonoInstaller
    {
        // public InputManager inputManagerPrefab;
        public override void InstallBindings()
        {
            BindInputManager();
        }

        private void BindInputManager()
        {
            // Container.Bind<InputManager>()
                // .FromComponentInNewPrefab(inputManagerPrefab).AsSingle();
        }
    }
}