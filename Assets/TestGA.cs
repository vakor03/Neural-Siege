using System;
using _Project.Scripts.Algorithms.GA;
using UnityEngine;
using Zenject;

public class TestGA : MonoBehaviour
{
    private GeneticAlgorithmFactory _geneticAlgorithmFactory;

    [Inject]
    private void Construct(GeneticAlgorithmFactory geneticAlgorithmFactory)
    {
        _geneticAlgorithmFactory = geneticAlgorithmFactory;
    }

    private void Start()
    {
        _geneticAlgorithmFactory.Create();
    }
}