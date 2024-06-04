using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public readonly int[] netWorth;
    
    public readonly bool[,,] productionEnabled;


    public PlayerManager(int playerN, int planetN)
    {
        netWorth = new int[playerN];
        
        productionEnabled = new bool[playerN, planetN, BuildingType.GetNames(typeof(BuildingType)).Length -1];
    }

    public void editNetWorth(int player, int amount)
    {
        if(checkIfCanPay(player, amount))
        {
            netWorth[player] = netWorth[player] + amount;
        }
    }
    public bool checkIfCanPay(int player, int amount)
    {
        if (netWorth[player] >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int getNetWorth(int player)
    {
        return netWorth[player];
    }
    
    

    public void enableBuilding(int player, int planet, int buildingType)
    {
        productionEnabled[player, planet, buildingType] = true;
    }
    public void disableBuilding(int player, int planet, int buildingType)
    {
        productionEnabled[player, planet, buildingType] = false;
    }
    public bool checkIfBuildingEnabled(int player, int planet, int buildingType)
    {
        return productionEnabled[player, planet, buildingType];
    }



}
