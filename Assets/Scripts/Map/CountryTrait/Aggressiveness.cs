using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggressiveness : ITrait
{

    private double aggressivenessValue;
    public double AggressivenessValue => aggressivenessValue;

    // used to calculate the next aggressiveness value
    private double incRate;
    private double incValue;

    public Aggressiveness(double initialAggressiveness, double incRate) {
        if (initialAggressiveness < 0 || incRate < 0) throw new ArgumentException("Both rates must be >= 0.");
        aggressivenessValue = initialAggressiveness;
        this.incRate = incRate;
        incValue = incRate;
    }

    public Aggressiveness(double initialAggressiveness) : this(initialAggressiveness, 0.2) {}
    public bool UpgradeTrait() {
        incValue += incRate;
        aggressivenessValue = Math.Tanh(incValue);
        return true;
    }

    public bool RollWar(Country attacker, Country defender) {
        // rolls for war between two countries

        // calculate dependent probabilities
        double probDepFaith = Math.Abs(attacker.Population.FaithProportion - defender.Population.FaithProportion); // closer to 0 if similar faiths.

        double probNotIntimidated = 1;
        if (attacker.Aggressiveness < defender.Aggressiveness) {
            probNotIntimidated = 1 - (defender.Aggressiveness - attacker.Aggressiveness);
        }
        double probWar = probDepFaith * probNotIntimidated * attacker.Aggressiveness;

        // roll for war based on probabilities
        double probabilityRoll = UnityEngine.Random.Range(0f, 1f);
        return probabilityRoll < probWar;
    }

    public IEnumerator InitiateWar(Country attacker, Country defender) {

        long numAttackers = attacker.Population.PopulationCount / 4;
        long numDefenders = defender.Population.PopulationCount / 4;
        attacker.Population.CullPopulation(numAttackers);
        defender.Population.CullPopulation(numDefenders);

        double attackerAdvantage = attacker.Technology * attacker.Aggressiveness;
        double defenderAdvantage = defender.Technology * attacker.Aggressiveness;
        double probAttackerWins = Math.Clamp(0.5 + attackerAdvantage - defenderAdvantage, 0, 1);
        while (numAttackers != 0 && numDefenders != 0) {
            if (UnityEngine.Random.Range(0f, 1f) < probAttackerWins) {
                numAttackers -= UnityEngine.Random.Range(0, (int) numAttackers + 1);
            } else {
                numDefenders -= UnityEngine.Random.Range(0, (int) numAttackers + 1);
            }
            yield return new WaitForSeconds(3);
        }

        attacker.CompleteWar(numAttackers, numAttackers > numDefenders);
        defender.Population.AddPopulation(numDefenders);
    }
}
