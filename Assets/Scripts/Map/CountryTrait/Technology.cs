using System;
using UnityEngine.EventSystems;

public class Technology : ITrait
{

    private double technologyValue;
    public double TechnologyValue => technologyValue;
    private Country coupledCountry;
    private Graph graph;

    public Technology(double initialTechValue, Country coupledCountry, Graph graph) { // is there a way to refactor out this coupling, also wtf aa why am i passing in a graph help
        technologyValue = initialTechValue;
        this.coupledCountry = coupledCountry;
        this.graph = graph;
    }

    public bool UpgradeTrait()
    {
        double incRate = 1.5f;
        if (technologyValue * incRate > 1) return false;
        technologyValue = Math.Max(technologyValue * 1.5, 1f);
        graph.TechnologyUpdated(coupledCountry);
        return true;
    }

    public double GetTechnologyRadius() {
        // arbitrary radius for now, max of 100km
        return technologyValue * 100;
    }

}

