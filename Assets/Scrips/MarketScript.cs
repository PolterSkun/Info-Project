
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketScript : MonoBehaviour
{
    ResourceManager ResourceManager;
    PlayerManager PlayerManager;

    int[] marketPrices = new int[System.Enum.GetNames(typeof(ResourceType)).Length -1];
    int portalPlanet;

    public MarketScript(int planetN)
    {
        generatePortalPlanet(planetN);
    }

    public void generatePortalPlanet(int planetN)
    {
        portalPlanet = Random.Range(0, planetN);
    }
    

    public void randomizeMarketPrices()
    {
        marketPrices[(int)ResourceType.ore] = Random.Range(20, 30);
        marketPrices[(int)ResourceType.rareOre] = Random.Range(100, 120);
        marketPrices[(int)ResourceType.lightMetals] = Random.Range(60, 100);
        marketPrices[(int)ResourceType.heavyMetals] = Random.Range(120, 200);
        marketPrices[(int)ResourceType.organics] = Random.Range(10, 15);
        marketPrices[(int)ResourceType.fuel] = Random.Range(50, 75);
        marketPrices[(int)ResourceType.heavyMachinery] = Random.Range(300, 400);
        marketPrices[(int)ResourceType.aiCore] = Random.Range(300, 1000);
    }

    

    public int getMarketPrice(int resourceType)
    {
        return marketPrices[resourceType];
    }

    public void buyFromMarket(int player, int resourceType, int amount)
    {
        if(checkIfCanBuyFromMarket(player, resourceType, amount))
        {
            PlayerManager.editNetWorth(player, -amount * marketPrices[resourceType]);
            ResourceManager.editResourceByAmount(player, portalPlanet, resourceType, amount);
        }
    }

    public void sellToMarket(int player, int resourceType, int amount)
    {
        if(ResourceManager.checkIfResourcesExist(player, portalPlanet, resourceType, amount))
        {
            ResourceManager.editResourceByAmount(player, portalPlanet, resourceType, -amount);
            PlayerManager.editNetWorth(player, amount * marketPrices[resourceType]);
        }
    }

    public bool checkIfCanBuyFromMarket(int player, int resourceType, int amount)
    {
        if(PlayerManager.getNetWorth(player) >= amount * marketPrices[resourceType])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void tradeResources(int fromPlayer, int toPlayer, int planet, int resourceType, int amount)
    {
        if (ResourceManager.checkIfResourcesExist(fromPlayer, planet, resourceType, amount))
        {
            ResourceManager.editResourceByAmount(fromPlayer, planet, resourceType, -amount);
            ResourceManager.editResourceByAmount(toPlayer, planet, resourceType, amount);
        }
    }

    public void tradeBuildings(int fromPlayer, int toPlayer, int planet, int buildingType, int amount)
    {
        if (ResourceManager.getBuildingsAmount(fromPlayer, planet, buildingType) > amount)
        {
            ResourceManager.editBuildingsByAmount(fromPlayer, planet, buildingType, -amount);
            ResourceManager.editBuildingsByAmount(toPlayer, planet, buildingType, amount);
        }
    }
    
    public void tradeMoney(int fromPlayer, int toPlayer, int amount)
    {
        if(PlayerManager.checkIfCanPay(fromPlayer, amount))
        {
            PlayerManager.editNetWorth(fromPlayer, -amount);
            PlayerManager.editNetWorth(toPlayer, amount);
        }
    }
}
