using _Project.Scripts.Core.Managers;
using Zenject;

namespace _Project.Scripts.Core.GridSystem
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