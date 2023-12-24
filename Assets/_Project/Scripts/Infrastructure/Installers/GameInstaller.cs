using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Algorithms.GA;
using _Project.Scripts.Algorithms.GA.Chromosomes;
using _Project.Scripts.Algorithms.GA.Crossover;
using _Project.Scripts.Algorithms.GA.Evaluation;
using _Project.Scripts.Algorithms.GA.Mutation;
using _Project.Scripts.Algorithms.GA.ParentSelection;
using _Project.Scripts.Algorithms.GA.Structs;
using _Project.Scripts.Core;
using _Project.Scripts.Core.Configs;
using _Project.Scripts.Core.Enemies;
using _Project.Scripts.Core.GridSystem;
using _Project.Scripts.Core.Managers;
using _Project.Scripts.Core.Towers;
using _Project.Scripts.Core.UI;
using _Project.Scripts.Core.WaypointSystem;
using _Project.Scripts.Infrastructure.States;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Zenject;
using EnemyPathCreatorFactory = _Project.Scripts.Core.PathCreation.EnemyPathCreatorFactory;

namespace _Project.Scripts.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        private const string ENEMY_STATS_DATABASE_PATH = "";

        public EnemyFactory enemyFactoryPrefab;
        public int initialShopMoneyAmount;
        public int playerBaseHealth = 5;

        public PlacementSystem placementSystem;
        public SerializedDictionary<EnemyType, float> enemyTypeToPrice;
        
        public GameOverUI GameOverUI;

        public override void InstallBindings()
        {
            BindEnemyPathCreationFactory();
            BindWaypointsHolderFactory();
            BindStatesFactory();
            BindEnemyFactory();

            BindEnemySpawner();
            BindEnemiesAccessor();

            BindPlanningTimer();
            BindScoreCounter();

            BindShop();
            BindGameOverUI();
            BindPlacementSystem();
            BindInputManager();
            BindPlayerBase();
            BindTowersController();

            BindGeneticAlgorithm();
            BindGeneticAlgorithmWaveCreator();

            BindSceneStateMachine();
        }

        private void BindScoreCounter()
        {
            Container.Bind<ScoreCounter>().AsSingle();
        }

        private void BindEnemyPathCreationFactory()
        {
            Container.Bind<EnemyPathCreatorFactory>().AsSingle();
        }

        // private void BindManualPathCreation()
        // {
        //     Container.Bind<ManualPathCreation>().AsSingle();
        // }

        private void BindWaypointsHolderFactory()
        {
            Container.Bind<WaypointsHolderFactory>().AsSingle();
        }

        private void BindGameOverUI()
        {
            Container.Bind<GameOverUI>().FromInstance(GameOverUI).AsSingle();
        }

        private void BindPlanningTimer()
        {
            Container.Bind<PlanningTimer>().AsSingle();
        }

        private void BindEnemiesAccessor()
        {
            Container.Bind<EnemiesAccessor>().AsSingle();
        }

        private void BindGeneticAlgorithmWaveCreator()
        {
            Container.Bind<GeneticAlgorithmWaveCreator>().AsSingle();
        }

        private void BindStatesFactory()
        {
            Container.Bind<StatesFactory>().AsSingle();
        }

        // private void BindEnemyPathCreator()
        // {
        //     Container.Bind<BacktrackingPathCreation>().AsSingle();
        // }

        private void BindSceneStateMachine()
        {
            Container.Bind<SceneStateMachine>().AsSingle();
        }

        private void BindInputManager()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        }

        private void BindEnemySpawner()
        {
            Container.Bind<EnemySpawner>().AsSingle().NonLazy();
        }

        private void BindTowersController()
        {
            Container.BindInterfacesAndSelfTo<TowersController>().AsSingle();
        }

        private void BindGeneticAlgorithm()
        {
            BindGeneticAlgorithmFactory();
            BindChromosomeFactory();
            BindCrossoverLogic();
            BindModelFitnessEvaluator();
            BindChromosomeMutator();
            BindParentSelector();
        }

        private void BindGeneticAlgorithmFactory()
        {
            Container.Bind<GeneticAlgorithmFactory>().AsSingle();
        }

        private void BindParentSelector()
        {
            Container.Bind<IParentSelector>().To<TournamentParentSelector>().AsSingle();
        }

        private void BindChromosomeMutator()
        {
            Container.Bind<IChromosomeMutator>().To<ChromosomeMutator>().AsSingle();
        }

        private void BindModelFitnessEvaluator()
        {
            Container.Bind<IModelFitnessEvaluator>()
                .To<ModelFitnessEvaluator>()
                .AsSingle()
                .WithArguments(GetEnemyStatsDictionary());
        }

        private Dictionary<EnemyType, EnemyStatsGA> GetEnemyStatsDictionary()
        {
            var enemyStatsDatabase = Resources.LoadAll<EnemyStatsConfigSO>(ENEMY_STATS_DATABASE_PATH).First();
            var enemyStatsDictionary =
                enemyStatsDatabase.enemyStats.ToDictionary(el => el.Key, el => ElementSelector(el));
            return enemyStatsDictionary;
        }

        private EnemyStatsGA ElementSelector(KeyValuePair<EnemyType, EnemyStatsSO> el)
        {
            var enemyType = el.Key;
            var enemyStats = el.Value.enemyStats;
            var enemyStatsGA = new EnemyStatsGA
            {
                EnemyType = enemyType,
                MaxHealth = enemyStats.maxHealth,
                Speed = enemyStats.speed,
                ReproductionRate = enemyStats.reproductionRate,
                SpawnedType = EnemyType.Spawned,
                Price = enemyTypeToPrice[enemyType]
            };

            return enemyStatsGA;
        }

        private void BindCrossoverLogic()
        {
            Container.Bind<ICrossoverLogic>().To<SinglePointCrossover>().AsSingle();
        }

        private void BindChromosomeFactory()
        {
            Container.Bind<IChromosomeFactory>().To<ChromosomeFactory>().AsSingle();
        }

        private void BindPlacementSystem()
        {
            Container.Bind<PlacementSystem>()
                .FromInstance(placementSystem).AsSingle();
        }

        private void BindPlayerBase()
        {
            Container.Bind<IPlayerBase>().To<PlayerBase>().AsSingle().WithArguments(playerBaseHealth);
        }

        private void BindShop()
        {
            Container.Bind<Shop>().AsSingle().WithArguments(initialShopMoneyAmount);
        }

        private void BindEnemyFactory()
        {
            Container.Bind<IEnemyFactory>().FromComponentInNewPrefab(enemyFactoryPrefab).AsSingle();
        }
    }
}