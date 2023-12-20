namespace _Project.Scripts.Algorithms.GA.Chromosomes
{
    public interface IChromosomeFactory
    {
        Chromosome CreateRandom(int length);
    }
}