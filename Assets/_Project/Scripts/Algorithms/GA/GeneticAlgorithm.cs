using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Algorithms.GA.Crossover;
using _Project.Scripts.Algorithms.GA.Evaluation;
using _Project.Scripts.Algorithms.GA.Mutation;
using _Project.Scripts.Algorithms.GA.ParentSelection;
using _Project.Scripts.Algorithms.GA.Structs;

namespace _Project.Scripts.Algorithms.GA
{
    // TODO: use total resources, not enemies count
    // TODO: add button to start Game after creating a path
    // TODO: update sprite icons of towers and reorder them
    // TODO: update prices+ update prices for upgrades
    public class GeneticAlgorithm : IWaveCreationAlgorithm
    {
        private readonly int _generationsCount = 50;
        private readonly int _populationSize = 50;
        private float _mutationRate = 0.01f;

        private IChromosomeFactory _chromosomeFactory;
        private IModelFitnessEvaluator _modelFitnessEvaluator;
        private IChromosomeMutator _chromosomeMutator;
        private IParentSelector _parentSelector;
        private ICrossoverLogic _crossoverLogic;

        public GeneticAlgorithm(IChromosomeFactory chromosomeFactory, IModelFitnessEvaluator modelFitnessEvaluator,
            IChromosomeMutator chromosomeMutator, IParentSelector parentSelector, ICrossoverLogic crossoverLogic)
        {
            _chromosomeFactory = chromosomeFactory;
            _modelFitnessEvaluator = modelFitnessEvaluator;
            _chromosomeMutator = chromosomeMutator;
            _parentSelector = parentSelector;
            _crossoverLogic = crossoverLogic;
        }

        private Dictionary<Core.Enemies.EnemyType, float> _resourcesPerEnemyType;
        private float _totalResources;

        public EnemyWave CreateEnemyWave(List<TileStatsGA> tilesStats, int enemiesPerWave)
        {
            Initialize(tilesStats);

            List<Chromosome> population = InitializePopulation(_populationSize, enemiesPerWave);
            var populationFitness = EvaluatePopulation(population);
            var bestIndividual = GetIndividualWithBestFitness(populationFitness);

            for (int i = 0; i < _generationsCount; i++)
            {
                var parents = SelectParents(populationFitness, _populationSize / 2);
                population = Crossover(parents, _populationSize);
                MutateOffsprings(population);
                populationFitness = EvaluatePopulation(population);
                bestIndividual = ChooseNewBestIndividual(populationFitness, bestIndividual);
            }
            
            // Debug.Log("Best fitness: " + bestIndividual.Fitness);

            return new EnemyWave(bestIndividual.Chromosome.EnemyWave);
        }

        private Individual ChooseNewBestIndividual(List<Individual> populationFitness, Individual bestIndividual)
        {
            var bestIndividualOfGeneration = GetIndividualWithBestFitness(populationFitness);
            if (bestIndividualOfGeneration.Fitness > bestIndividual.Fitness)
            {
                bestIndividual = bestIndividualOfGeneration;
            }

            return bestIndividual;
        }

        private void Initialize(List<TileStatsGA> tilesStats)
        {
            _modelFitnessEvaluator.TilesStats = tilesStats;
        }

        private Individual GetIndividualWithBestFitness(List<Individual> population)
        {
            float bestFitness = population.Max(individual => individual.Fitness);
            return population.First(individual => Math.Abs(individual.Fitness - bestFitness) < 0.001f);
        }

        private List<Chromosome> InitializePopulation(int populationSize, int enemiesCount)
        {
            List<Chromosome> population = new List<Chromosome>();

            for (int i = 0; i < populationSize; i++)
            {
                Chromosome newChromosome = _chromosomeFactory.CreateRandom(enemiesCount);
                population.Add(newChromosome);
            }

            return population;
        }

        private Individual EvaluateSingle(Chromosome chromosome)
        {
            float fitness = _modelFitnessEvaluator.Evaluate(chromosome);
            return new Individual { Chromosome = chromosome, Fitness = fitness };
        }

        private List<Individual> EvaluatePopulation(List<Chromosome> population)
        {
            List<Individual> individuals = new List<Individual>();

            foreach (var chromosome in population)
            {
                individuals.Add(EvaluateSingle(chromosome));
            }

            return individuals;
        }

        private List<Chromosome> SelectParents(List<Individual> individuals, int numberOfParents)
        {
            return _parentSelector.SelectParents(individuals, numberOfParents);
        }

        public void Mutate(Chromosome chromosome)
        {
            _chromosomeMutator.Mutate(chromosome, _mutationRate);
        }

        public List<Chromosome> Crossover(List<Chromosome> parents, int numberOfOffsprings)
        {
            return _crossoverLogic.Crossover(parents, numberOfOffsprings);
        }

        private void MutateOffsprings(List<Chromosome> offsprings)
        {
            foreach (var offspring in offsprings)
            {
                Mutate(offspring);
            }
        }

        public struct Individual
        {
            public Chromosome Chromosome;
            public float Fitness;
        }
    }
}