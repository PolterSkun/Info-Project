using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Logistics : MonoBehaviour
{
    PlanetManager PlanetManager;
    ResourceManager ResourceManager;
    public readonly int[,] ships;
    public int[,,] travelingResources;
    public readonly int capacity;
    public readonly int travelCost;
    public int load;
    public int playerN;
    public int planetN;

    public Logistics(int playerN, int planetN, int shipCapacity, int travelCostsOfShip)
    {
        ships = new int[playerN, 1]; //zweites ist ob die genutzt werden oder nicht
        travelingResources = new int[playerN, planetN, Enum.GetNames(typeof(ResourceType)).Length -1];
        this.playerN = playerN;
        this.planetN = planetN;
        capacity = shipCapacity;
        travelCost = travelCostsOfShip;
    }
    
    public void editShipCount(int player, int amount)
    {
        ships[player, 1] += amount;
    }

    public int getShipCount(int player, int empty1full0)
    {
        return ships[player, empty1full0];
    }

    public int[,,] getTravelingResourcesArray() { return travelingResources; }

    public int[,] getTravelingResourcesOfPlayer(int player)
    {
        int[,] travelingResourcesOfPlayer = new int[travelingResources.GetLength(1), travelingResources.GetLength(2)];
        for(int planet = 0; planet < ships.GetLength(1); planet++)
        {
            for(int resourceType = 0; resourceType < ships.GetLength(2); resourceType++)
            {
                travelingResourcesOfPlayer[planet, resourceType] = travelingResources[player, planet, resourceType];
            }
        }
        return travelingResourcesOfPlayer;
    }

    public void updateDeliveries()
    {
        for(int player = 0; player <= playerN; player++)
        {
            for(int planet = 0; planet <= planetN; planet++)
            {
                for(int resourceType = 0; resourceType < Enum.GetNames(typeof(ResourceType)).Length; resourceType++)
                {
                    ResourceManager.editResourceByAmount(player, planet, resourceType, travelingResources[player, planet, resourceType]);
                }
            }
            ships[player, 1] += ships[player, 0];
            ships[player, 0] = 0;
        }
    }

    

    public void sendShip(int player, int fromPlanet, int toPlanet, int[] load)
    {
        if (checkIfCanSendShip(player, fromPlanet, toPlanet, load))
        {
            if (ships[player, 1] > 0)
            {
                foreach(int resourceType in load)
                {
                    travelingResources[player, toPlanet, resourceType] = +load[resourceType];
                }
                ships[player, 1]--;
                ships[player, 0]++;
            }
        }
    }

    public bool checkIfCanSendShip(int player, int fromPlanet, int toPlanet, int[] load)
    {
        bool hasAllResources = true;
        if (PlanetManager.getSpecificPlanetConnection(fromPlanet, toPlanet))
        {
            if(load.Sum() <= capacity)
            {
                for(int i = 0; i < Enum.GetNames(typeof(ResourceType)).Length; i++)
                {
                    if (ResourceManager.checkIfResourcesExist(player, fromPlanet, i, load[i]) == false)
                    {
                        hasAllResources = false;
                    }
                }
                if(hasAllResources)
                {
                    if (ResourceManager.checkIfBuildingsExist(player, fromPlanet, (int)BuildingType.logisticsFacility, 1))
                    {
                        return true;
                    }
                    else if (ResourceManager.checkIfResourcesExist(player, fromPlanet, (int)ResourceType.fuel, travelCost))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false; 
            }
        }
        else
        {
            return false;
        }
    }
}