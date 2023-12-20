using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;

namespace _Project.Scripts.Algorithms.GA.ParentSelection
{
    public interface IParentSelector
    {
        List<Chromosome> SelectParents(List<GeneticAlgorithm.Individual> individuals, int numberOfParents);
    }
}