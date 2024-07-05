using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : ITrait
{

    private long foodValue = 0;
    public long FoodValue => foodValue;

    private double foodRate;

    public Food(double foodRate) {
        this.foodRate = foodRate;
    }

    public Food() : this(0.5) {}

    public bool UpgradeTrait()
    {
        foodRate += UnityEngine.Random.Range(0, 0.5f);
        return true;
    }

    public void ConsumeFood(long number) {
        foodValue = Math.Max(0, foodValue - number);
    }

    public IEnumerator UpdateFood() {
        while (true) {
            foodValue = (long) (foodValue * foodRate);
            yield return new WaitForSeconds(2);
        }
    }

    
}
