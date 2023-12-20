using System.Collections.Generic;
using _Project.Scripts.Algorithms.GA.Chromosomes;

namespace _Project.Scripts.Algorithms.GA.Evaluation
{
    public interface IModelFitnessEvaluator
    {
        List<TileStats> TilesStats { get; set; }
        float Evaluate(Chromosome chromosome);
    }
}