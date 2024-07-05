using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertility : ITrait
{
    
    private double fertilityValue;
    private double fertilityVariable;

    public double FertilityValue => fertilityValue;

    public Fertility(double initialFertilityVariable) {
        fertilityVariable = initialFertilityVariable;
        UpdateFertility();
    }

    public Fertility() : this(0) {} 

    public bool UpgradeTrait()
    {
        fertilityVariable += 2;
        fertilityValue = GetSigmoid(fertilityVariable);
        return true;
    }

    private void UpdateFertility() {
        fertilityValue = GetSigmoid(fertilityVariable);
    }

    private double GetSigmoid(double val) {
        return 1 / (1 + Math.Exp(-val));
    }

    public IEnumerator DecreaseFertility() {
        while (true) {
            fertilityValue -= 0.25;
            UpdateFertility();
            yield return new WaitForSeconds(10);
        }
    }

}
