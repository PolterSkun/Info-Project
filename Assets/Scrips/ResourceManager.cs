using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;

enum ResourceType
{
    ore,
    rareOre,
    lightMetals,
    heavyMetals,
    organics,
    fuel,
    heavyMachinery,
    aiCore
}



enum BuildingType
{
    smeltery, refinery, processingPlant, factory, bunker, shipFactory, logisticsFacility, armsFactory, miningDrill, prospectingDrill, centralizedEnergyNetwork
}



public class ResourceManager : MonoBehaviour
{
    PlanetManager PlanetManager;
    PlayerManager PlayerManager;
    Logistics Logistics;
    CombatManager CombatManager;
    

    public readonly int players;
    public readonly int planets;
    public readonly int[,,] buildings;
    public readonly int[,,] resources;
    
    //public readonly int[,,] privateModifiers;
    

    

    public ResourceManager(int playersn, int planetsn)

    {
        players = playersn;
        planets = planetsn;
        //privateModifiers = new int[players, planets, 2];
        buildings = new int[players, planets, 11]; //4 players, 6 planets, 11 types of buildings
        resources = new int[players, planets, 8]; //4 players, 6 planets, 7 resource types 0.ore 1.rareOre 2.lightMetals 3.heavyMetals 4.oraganics 5.fuel 6.heavyMachinery 7. aiCores
        
        
        
    }

    /*
    Buildings: 
    0. Smeltery (Intake 10 ore 1 fuel -> 10 light metals),
     cost: 10 heavy metals 10 lightMetals
    1. Refinery (Intake 1 ore 5 rare ore 1 fuel -> 5 heavy metals),
     cost: 20 light metals 10 heavy metals 1 heavy machinery
    2. Processing Plant (Intake 14 organics -> 7 fuel),
     cost: 10 light metals 1 heavy machienry
    3. Factory (Intake 10 light metals 10 heavy metals 5 fuel -> 1 heavy machinery),
     cost: 20 light metals 5 heavy machinery
    4. Bunker (Intake 4 fuel -> Defence value),
     cost: 10 heavy machinery 20 heavy metals
    5. Ship-factory (Intake 20 heavy machinery 20 fuel 1 aiCore -> ship),
     cost: 50 heavy machinery 50 heavy metals 50 light metals
    6. Logistics facility (Intake 10 fuel -> can transport things from this planet to the next)
     cost: 20 light metals
    7. Arms-factory (Intake 50 fuel 7 heavy machinery -> 1 destroyer),
     cost: 20 heavy machinery 10 heavy metals
    8. Mining Drill (Intake 4 fuel -> the resources mined)
     cost: 10 light metals 1 heavy machinery
    9. Prospecting Drill (4 fuel -> +20% mine output modifier)
     cost: 50 light metals 50 heavy metals 25 heavy machinery 1 ai core
    10. Centralized Energy Network (20 fuel -> +20% factory, refinery, smelter, processing plant, arms factory output
     cost: 50 light metals 100 heavy metals 40 heavy machiners 1 ai core

    Six planets: Primus, Dus, Tertia, Fori, Quin, Sextan
    */

    
    public int[,] costs = //type, costs: 0.ore 1.rareOre 2.lightMetals 3.heavyMetals 4.oraganics 5.fuel 6.heavyMachinery 7.aiCores
        {/*0smeltery*/{0, 0, 10, 10, 0, 0, 0, 0},
        /*1refinery*/{ 0, 0, 10, 0, 0, 0, 1, 0},
        /*2processingPlant*/{0, 0, 10, 0, 0, 0, 1, 0},
        /*3factory*/{0, 0, 20, 0, 0, 0, 5, 0},
        /*4bunker*/{0, 0, 0, 20, 0, 0, 10, 0},
        /*5ship-factory*/{0, 0, 50, 50, 0, 0, 50, 0},
        /*6logistics-facility*/{0, 0, 20, 20, 0, 0, 0, 0},
        /*7arms-factory*/{0, 0, 0, 20, 0, 0, 20, 0},
        /*8miningDrill*/{0, 0, 10, 0, 0, 0, 1, 0},
        /*9prospectingDrill*/{0, 0, 50, 50, 0, 0, 25, 1},
        /*10 centralizedEnergyNetwork*/{0, 0, 50, 100, 0, 0, 40, 1} };
    public int[,] consumption = {
        {10,  0,  0,  0,  0,  1,  0,  0 },//smeltery
        { 1,  5,  0,  0,  0,  1,  0,  0 },//refinery
        { 0,  0,  0,  0, 14,  0,  0,  0 },//processing plant
        { 0,  0, 10, 10,  0,  5,  0,  0 },//factory
        { 0,  0,  5,  0,  0, 10,  0,  0 },//bunker
        { 0,  0,  0,  0,  0, 20, 20,  1 },//shipFactory
        { 0,  0,  0,  0,  0, 10,  0,  0 },//logisticsFacility
        { 0,  0,  0,  0,  0, 50,  7,  0 },//arms factory
        { 0,  0,  0,  0,  0,  4,  0,  0 },//mining drill
        { 0,  0,  0,  0,  0,  4,  0,  0 },//prospecting drill
        { 0,  0,  0,  0,  0, 20,  0,  0 }//centralized energy network
    };
        
    int turn;

    

    



    public void updateProduction()
    {
        for (int playerNumber = 0; playerNumber < buildings.GetLength(0); playerNumber++)
        {
            for(int planetNumber = 0; planetNumber < buildings.GetLength(1); planetNumber++)
            {
                //mining drill check, 4 fuel -> dependable ore (multiplier 10 for ore and 7.5 for rare ore)
                if(checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.miningDrill)  &&  PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.miningDrill)) 
                {
                    resources[playerNumber, planetNumber, (int)ResourceType.ore] =
                        System.Convert.ToInt32(
                            getResourceAmount(playerNumber, planetNumber, (int)ResourceType.ore)
                            + 10
                            * PlanetManager.getPlanetDetail(planetNumber, (int)PlanetModifier.oreFrequency)
                            * PlanetManager.getLocalGeneralMiningModifier(playerNumber, planetNumber));

                    resources[playerNumber, planetNumber, (int)ResourceType.rareOre] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.rareOre]
                            + 7.5
                            * PlanetManager.getPlanetDetail(planetNumber, (int)PlanetModifier.rareOreFrequency)
                            * PlanetManager.getLocalGeneralMiningModifier(playerNumber, planetNumber));

                    resources[playerNumber, planetNumber, (int)ResourceType.organics] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.organics]
                            + 10
                            * PlanetManager.getPlanetDetail(planetNumber, (int)PlanetModifier.organicsFrequency)
                            * PlanetManager.getLocalGeneralMiningModifier(playerNumber, planetNumber));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.miningDrill);
                }
                //smelter 10ore 1fuel -> 10lightmetals
                if(checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.smeltery)  &&  PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.smeltery))
                {
                    resources[playerNumber, planetNumber, (int)ResourceType.lightMetals] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.lightMetals]
                            + 10
                            * getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.smeltery)
                            * PlanetManager.getLocalProductionModifier(playerNumber, planetNumber));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.smeltery);
                }
                //refinery 1ore 5rareOre 1fuel -> 5 heavyMetals
                if (checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.refinery) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.refinery))
                {
                    resources[playerNumber, planetNumber, (int)ResourceType.heavyMetals] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.heavyMetals]
                            + 5
                            * getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.refinery)
                            * PlanetManager.getLocalProductionModifier(playerNumber, planetNumber));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.refinery);
                }
                //processing plant 14organics -> 7 fuel
                if(checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.processingPlant) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.processingPlant))
                {
                    resources[playerNumber, planetNumber, (int)ResourceType.fuel] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.fuel]
                            + 7
                            * getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.processingPlant)
                            * PlanetManager.getLocalProductionModifier(playerNumber, planetNumber));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.processingPlant);
                }
                //factory 10HeavyMetals 10lightMetals 5 fuel -> 1heavyMachinery
                if (checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.factory) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.factory))
                {
                    resources[playerNumber, planetNumber, (int)ResourceType.heavyMachinery] =
                        System.Convert.ToInt32(
                            resources[playerNumber, planetNumber, (int)ResourceType.heavyMachinery]
                            + 1
                            * getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.factory)
                            * PlanetManager.getLocalProductionModifier(playerNumber, planetNumber));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.factory);
                }
                //bunker 4 fuel -> 100 combat value
                if (checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.bunker) /*&& PlayerManager.checkIfProductionEnabled(playerNumber, planetNumber, (int)BuildingType.bunker)*/)
                {
                    CombatManager.updateDefenceBuildup(playerNumber, planetNumber, true);

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.bunker);
                }
                else
                {
                    CombatManager.updateDefenceBuildup(playerNumber, planetNumber, false);
                }
                //ship factory 20heavyMachinery 20fuel 1aiCore -> 1 ship
                if (checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.shipFactory) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.shipFactory))
                {
                    Logistics.editShipCount(playerNumber, getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.shipFactory));

                    
                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.shipFactory);
                }
                //arms factory 50fuel 7 heavy machinery
                if(checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.armsFactory) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.armsFactory)) 
                {
                    //ADD DESTROYER HERE TO COMBAT

                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.armsFactory);
                }
                //prospecting drill 4 fuel -> +20% local mining output
                if(checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.prospectingDrill) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.prospectingDrill))
                {
                    PlanetManager.clearPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.mining);
                    PlanetManager.editPrivatePlanetModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.mining, getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.prospectingDrill));

                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.prospectingDrill);
                }
                else
                {
                    PlanetManager.clearPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.mining);
                }
                //centralized energy network 20 fuel -> +20% production output
                if (checkIfBuildingCanProduce(playerNumber, planetNumber, (int)BuildingType.centralizedEnergyNetwork) && PlayerManager.checkIfBuildingEnabled(playerNumber, planetNumber, (int)BuildingType.centralizedEnergyNetwork))
                {
                    PlanetManager.clearPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.production);
                    PlanetManager.editPrivatePlanetModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.production, getBuildingsAmount(playerNumber, planetNumber, (int)BuildingType.centralizedEnergyNetwork));

                    updateBuildingConsumption(playerNumber, planetNumber, (int)BuildingType.centralizedEnergyNetwork);
                }
                else
                {
                    PlanetManager.clearPrivateModifier(playerNumber, planetNumber, (int)PrivatePlanetModifier.production);
                }
            }
        }
    }

    public bool checkIfBuildingCanProduce(int playerNumber, int planetNumber, int buildingType)
    {
        int trueCount = 0;
        for (int i = 0; i <= resources.GetLength(2); i++)
        {
            if(checkIfResourcesExist(playerNumber, planetNumber, i, consumption[buildingType, i]))
            {
                trueCount++;
            }
        }
        if(trueCount == resources.GetLength(2))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    //consumes the resources for inserted building
    public void updateBuildingConsumption(int playerNumber, int planetNumber, int buildingType)
    {
        for (int i = 0; i <= consumption.GetLength(1); i++)
        {
            resources[playerNumber, planetNumber, i] = resources[playerNumber, planetNumber, i] - consumption[buildingType, i] * getBuildingsAmount(playerNumber, planetNumber, buildingType);
        }
    }

    

    

    void build(int player, int planet, int buildingType)
    {
        bool allGood = true;
        for (int i = 0; i <= costs.GetLength(1); i++)
        {
            if (costs[buildingType, i] > resources[player, planet, i]) allGood = false;
        }
        if (allGood)
        {
            buildings[player, planet, buildingType]++;
            for (int i = 0; i <= costs.GetLength(1); i++)
            {
                resources[player, planet, i] = resources[player, planet, i] - costs[buildingType, i];
            }
        }
    }

    public int[,] getCostsArray()
    {
        return costs;
    }

    

    public bool checkIfResourcesExist(int player, int planet, int resourceType, int amount)
    {
        if (resources[player, planet, resourceType] >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool checkIfBuildingsExist(int player, int planet, int buildingType, int amount)
    {
        if (buildings[player, planet, buildingType] >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int getResourceAmount(int player, int planet, int resourceType)
    {
        return resources[player, planet, resourceType];
    }

    public int getBuildingsAmount(int player, int planet, int buildingType)
    {
        return buildings[player, planet, buildingType];
    }

    public int[,,] getResourcesArray()
    {
        return resources;
    }

    public int[,,] getBuildingsArray()
    {
        return buildings;
    }

    public void editResourceByAmount(int player, int planet, int resourceType, int amount)
    {
        resources[player, planet, resourceType] = resources[player, planet, resourceType] + amount;
    }

    public void editBuildingsByAmount(int player, int planet, int buildingType, int amount)
    {
        buildings[player, planet, buildingType] = buildings[player, planet, buildingType] + amount;
    }

    public void overrideResourceAmount(int player, int planet, int resourceType, int newAmount)
    {
        resources[player, planet, resourceType] = newAmount;
    }
    public void overrideBuildingAmount(int player, int planet, int buildingType, int  newAmount)
    {
        resources[player, planet, buildingType] = newAmount;
    }


    public void updateTurn(int newTurn)
    {
        turn = newTurn;
    }

    
}
