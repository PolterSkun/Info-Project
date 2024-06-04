
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

enum PlanetName
{
    Primus, Dus, Tertia, Fori, Quin, Sextan
}

enum PlanetModifier
{
    oreFrequency, rareOreFrequency, organicsFrequency, miningModifier, productionModifier, orbitalModifier
}

enum PrivatePlanetModifier
{
    mining, production, orbital
}

public class PlanetManager : MonoBehaviour
{
    ResourceManager ResourceManager;
    
    public readonly double[,] planetDetails;
    public readonly bool[,] planetConnections;
    private readonly int players;
    private readonly int planets;
    public double[,,] privateModifiers;
    public double privateModifierCoefficient;

    public PlanetManager(int playersN, int planetsN, double privateModifierCoefficient, int numberOfPlanetConnections)
    {
        planetDetails = new double[planetsN, 6];  //0.oreFrequency 1.rareOreFrequency 2.organicsFrequency 3. minesModifier 4. productionModifier 5. orbitalModifier
        planetConnections = new bool[planetsN, planetsN];
        players = playersN;
        planets = planetsN;
        privateModifiers = new double[players, planets, 2];
        this.privateModifierCoefficient = privateModifierCoefficient;

        generatePlanets();
        generatePlanetConnections(numberOfPlanetConnections-1);
    }

    public void generatePlanetConnections(int numberOfConnectionsStartsWith0)
    {
        
        for (int i = 0; i < planetConnections.GetLength(0); i++)
        {
            
            int[] randoms = new int[2];
            do {
                randoms[0] = Random.Range(0, 5);
                randoms[1] = Random.Range(0, 5);
            } while (randoms[0] != randoms[1] && randoms[0] != i && randoms[1] != i);
            planetConnections[i, i] = false;
            planetConnections[i, randoms[0]] = true;
            planetConnections[i, randoms[1]] = true;
            planetConnections[randoms[0], i] = true;
            planetConnections[randoms[1], i] = true;
        }
    }

    public bool getSpecificPlanetConnection(int planet1, int planet2)
    {
        return planetConnections[planet1, planet2];
    }

    
    public bool[] getConnectionsOfPlanet(int planet)
    {
        bool[] connectionsOfSpecificPlanet = new bool[planetConnections.GetLength(1)];
        for(int i = 0; i <= planetConnections.GetLength(1); i++)
        {
            connectionsOfSpecificPlanet[i] = planetConnections[planet, i];
        }
        return connectionsOfSpecificPlanet;
    }
    public bool[,] getPlanetConnectionsArray()
    {
        return planetConnections;
    }


    public double getPlanetDetail(int planet, int modifier)
    {
        return planetDetails[planet, modifier];
    }
    public void overridePlanetDetail(int planet, int modifier, double newValue)
    {
        planetDetails[planet, modifier] = newValue;
    }

    public double[,] getPlanetDetailsArray()
    {
        return planetDetails;
    }


    public void generatePlanets()
    {
        for (int i = 0; i < planetDetails.GetLength(0); i++)
        {
            if (Random.Range(0, 1) <= 0.90) planetDetails[i, 0] = Random.Range(0.5f, 4); //ore
            if (Random.Range(0, 1) <= 0.75) planetDetails[i, 1] = Random.Range(0.25f, 2); //rare ore 
            if (Random.Range(0, 1) <= 0.5) planetDetails[i, 2] = Random.Range(1, 10); // organics
            if (Random.Range(0, 1) <= 0.33) planetDetails[i, 3] = Random.Range(0.66f, 1.5f); //miningModifier
            if (Random.Range(0, 1) <= 0.33) planetDetails[i, 4] = Random.Range(0.66f, (float)1.5); //productionModifier
            if (Random.Range(0, 1) <= 0.33) planetDetails[i, 5] = Random.Range((float)0.5, (float)1.33); //orbitalModifier (remember its different)
        }
    }


    public void clearPrivateModifier(int player, int planet, int modifierType)
    {
        privateModifiers[player, planet, modifierType] = 0;
    }

    public void editPrivatePlanetModifier(int player, int planet, int modifierType, int times)
    {
        privateModifiers[player, planet, modifierType] += privateModifierCoefficient * times;
    }

    public double getPrivateModifier(int player, int planet, int modifier) 
    {
        return privateModifiers[player, planet, modifier];
    }

    public double getLocalGeneralMiningModifier(int playerNumber, int planetNumber)
    {
        return
            getPlanetDetail(planetNumber, (int)PlanetModifier.miningModifier)
            * ResourceManager.getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.miningDrill)
            * getPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.mining);
    }
    public double getLocalProductionModifier(int playerNumber, int planetNumber)
    {
        return
            getPlanetDetail(planetNumber, (int)PlanetModifier.productionModifier)
            * getPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.production);
    }
}
