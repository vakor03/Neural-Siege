using _Project.Scripts.Algorithms.GA.Chromosomes;

namespace _Project.Scripts.Algorithms.GA.Mutation
{
    public interface IChromosomeMutator
    {
        void Mutate(Chromosome chromosome, float mutationRate);
    }
}