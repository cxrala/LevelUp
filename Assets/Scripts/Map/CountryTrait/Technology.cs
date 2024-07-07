using System;

public class Technology : ITrait
{

    private double technologyValue;
    public double TechnologyValue => technologyValue;

    public Technology(double initialTechValue) {
        technologyValue = initialTechValue;
    }

    public bool UpgradeTrait()
    {
        double incRate = 1.5f;
        if (technologyValue * incRate > 1) return false;
        technologyValue = Math.Max(technologyValue * 1.5, 1f);
        // TODO: ping function to connect new countries
        return true;
    }

    public double GetTechnologyRadius() {
        // arbitrary radius for now, max of 100km
        return technologyValue * 100;
    }

}

