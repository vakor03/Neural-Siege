using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using UnityEngine;

namespace _Project.Scripts.Algorithms.GA.ParentSelection
{
    public class TournamentParentSelector : IParentSelector
    {
        public List<Chromosome> SelectParents(List<GeneticAlgorithm.Individual> individuals, int numberOfParents)
        {
            List<Chromosome> parents = new List<Chromosome>();

            for (int i = 0; i < numberOfParents; i++)
            {   
                var firstParent = individuals[Random.Range(0, individuals.Count)];
                var secondParent = individuals[Random.Range(0, individuals.Count)];
                
                if (firstParent.Fitness > secondParent.Fitness)
                {
                    parents.Add(firstParent.Chromosome);
                }
                else
                {
                    parents.Add(secondParent.Chromosome);
                }
            }

            return parents;
        }
    }
}