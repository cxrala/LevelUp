using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Population
{

    private long populationCount;
    private double faithProportion;
    private double faithRate;
    public long PopulationCount => populationCount;
    public double FaithProportion => faithProportion;
    private Fertility fertility;
    private double deathRate = 0.5;
    private Food food;

    public Population(Fertility fertility, Food food, long initialPopulation, double faithProportion, double faithRate) {
        this.fertility = fertility;
        this.food = food;
        populationCount = initialPopulation;
        this.faithProportion = faithProportion;
        this.faithRate = faithRate;
    }

    public static Population GetDefaultPopulation(Fertility fertility, Food food, long initialPopulation) {
        return new Population(fertility, food, initialPopulation, 0, 0);
    }

    public static Population GetProphetPopulation(Fertility fertility, Food food, long initialPopulation) {
        return new Population(fertility, food, initialPopulation, 0.1, 0.01);
    }

    public IEnumerator SimulatePopulationChange() {
        while (true) {
            yield return new WaitForSeconds(3);
            double growth = populationCount * (fertility.FertilityValue - deathRate);
            food.ConsumeFood(populationCount);
            populationCount = Math.Max(0, Math.Min((long) Math.Floor(growth), food.FoodValue) + populationCount);
            if (faithProportion != 0) {
                IncFaith(faithRate);
            }
            Debug.Log(food.FoodValue);
            // Debug.Log(populationCount);
        }
    }

    private double GetInverseSigmoid(double val) {
        return Math.Log(val / (1 - val));
    }

    public long CullPopulation(long number) {
        if (populationCount < number) {
            long temp = populationCount;
            populationCount = 0;
            return temp;
        }
        populationCount -= number;
        return number;
    }

    public void AddPopulation(long amount) {
        populationCount += amount;
    }

    public void IncFaith(double amount) {
        faithProportion = Math.Min(1, faithProportion + amount);
    }

    public void DecFaith(double amount) {
        faithProportion = Math.Max(0, faithProportion - amount);
    }
 
}
