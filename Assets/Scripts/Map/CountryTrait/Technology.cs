using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Technology : ITrait
{

    private double technologyValue;
    public double TechnologyValue => technologyValue;
    private double technologyVariable;
    private double techRadiusFactor;
    private Country coupledCountry;
    private Graph graph;

    public Technology(double initialTechValue, Country coupledCountry, Graph graph) { // is there a way to refactor out this coupling, also wtf aa why am i passing in a graph help
        technologyValue = initialTechValue;
        this.coupledCountry = coupledCountry;
        this.graph = graph;
        technologyVariable = 0;
        techRadiusFactor = 100;
    }

    public bool UpgradeTrait()
    {
        technologyVariable += 0.5;
        techRadiusFactor += 10;
        technologyValue = GetSigmoid(technologyVariable);
        graph.TechnologyUpdated(coupledCountry);
        return true;
    }

    private double GetSigmoid(double val) {
        return 1 / (1 + Math.Exp(-val));
    }

    public double GetTechnologyRadius() {
        // arbitrary radius for now, max of 100km
        return technologyValue * techRadiusFactor;
    }

    public IEnumerator UpdateTechnology() {
        while (true) {
            yield return new WaitForSeconds(2);
            technologyVariable -= 0.1;
            technologyValue = GetSigmoid(technologyVariable);
        }

    }

}

