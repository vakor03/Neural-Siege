using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Algorithms.GA.Structs;

namespace _Project.Scripts.Algorithms.GA.Evaluation
{
    public interface IModelFitnessEvaluator
    {
        List<TileStatsGA> TilesStats { get; set; }
        float Evaluate(Chromosome chromosome);
    }
}