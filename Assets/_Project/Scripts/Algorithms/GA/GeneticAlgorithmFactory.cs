using Zenject;

namespace _Project.Scripts.Algorithms.GA
{
    public class GeneticAlgorithmFactory
    {
        private DiContainer _container;

        public GeneticAlgorithmFactory(DiContainer container)
        {
            _container = container;
        }

        public GeneticAlgorithm Create()
        {
            return _container.Instantiate<GeneticAlgorithm>();
        }
    }
}