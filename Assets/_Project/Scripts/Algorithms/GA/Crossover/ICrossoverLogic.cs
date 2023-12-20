using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;

namespace _Project.Scripts.Algorithms.GA.Crossover
{
    public interface ICrossoverLogic
    {
        List<Chromosome> Crossover(List<Chromosome> parents, int numberOfOffsprings);
    }
}