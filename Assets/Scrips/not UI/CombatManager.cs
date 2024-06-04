using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    ResourceManager ResourceManager;
    PlanetManager PlanetManager;
    GameManager GameManager;
    public readonly int[,] defenceBuildupLevel;
    public readonly int[,] attackPower;
    public readonly int maxDefFromBunker;
    public readonly int defScaling;
    public int[,] destroyers;
    public readonly int destroyerTravelCost;
    public readonly int destroyerAttackValue;


    public void Start()
    {
        
    }


    public CombatManager(int playerN, int planetN, int maxDefFromBunker, int defScaling, int destroyerTravelCost, int destroyerAttackValue)
    {
        defenceBuildupLevel = new int[playerN, planetN];
        attackPower = new int[playerN, planetN];
        destroyers = new int[playerN, planetN];

        this.maxDefFromBunker = maxDefFromBunker;
        this.defScaling = defScaling;
        this.destroyerTravelCost = destroyerTravelCost;
        this.destroyerAttackValue = destroyerAttackValue;
    }

    public void editDefenceBuildupLevel(int player, int planet, int amount)
    {
        defenceBuildupLevel[player, planet] += amount;
    }
    public int getDefenceBuildupLevel(int player, int planet)
    {
        return defenceBuildupLevel[player, planet];
    }

    public void updateDefenceBuildup(int player, int planet, bool increaseTrueDecreaseFalse)
    {
        if(increaseTrueDecreaseFalse)
        {
            if (defenceBuildupLevel[player, planet] < maxDefFromBunker)
            {
                defenceBuildupLevel[player, planet] += defScaling;
            }
        }
        else
        {
            if (defenceBuildupLevel[player, planet] > 0)
            {
                defenceBuildupLevel[player, planet] -= defScaling;
            }
        }
    }
    

    public int getDefValue(int player, int planet)
    {
        return defenceBuildupLevel[player, planet] * ResourceManager.getBuildingsAmount(player, planet, (int)BuildingType.bunker);
    }
    public int getAttValue(int player, int planet)
    {
        return destroyers[player, planet] * destroyerAttackValue;
    }

    public void editDestroyerCount(int player, int planet, int amount)
    {
        destroyers[player, planet] += amount;
    }
    public int getDestroyerCount(int player, int planet) { return destroyers[player, planet]; }


    public void sendDestroyer(int player, int fromPlanet, int toPlanet, int amount)
    {
        if(checkIfCanSendDestroyer(player, fromPlanet, toPlanet, amount))
        {
            destroyers[player, fromPlanet] -= amount;
            destroyers[player, toPlanet] += amount;
        }
    }

    public bool checkIfCanSendDestroyer(int player, int fromPlanet, int toPlanet, int amount)
    {
        if(PlanetManager.getSpecificPlanetConnection(fromPlanet, toPlanet))
        {
            if(ResourceManager.checkIfBuildingsExist(player, fromPlanet, (int)BuildingType.logisticsFacility, 1))
            {
                if (destroyers[player, fromPlanet] >= amount)
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

    public void attack(int attackerPlayer, int defenderPlayer, int planet)
    {
        for(int defendingDestroyersLeft = getDestroyerCount(defenderPlayer, planet); defendingDestroyersLeft < 0; defendingDestroyersLeft--)
        {
            if (getDestroyerCount(attackerPlayer, planet) <= 0) 
            {
                break; 
            } 
            else 
            {
                editDestroyerCount(attackerPlayer, planet, -1);
                editDestroyerCount(defenderPlayer, planet, -1);
            }
        }
        if(getDestroyerCount(attackerPlayer, planet) > 0)
        {
            int evaluation = getAttValue(attackerPlayer, planet) - getDefValue(defenderPlayer, planet);
            if (evaluation > 0)
            {
                ResourceManager.overrideBuildingAmount(defenderPlayer, planet, (int)BuildingType.bunker, 0);

                editDestroyerCount(attackerPlayer, planet, -System.Convert.ToInt32((getAttValue(attackerPlayer, planet) - evaluation) / destroyerAttackValue));
            }
            else if (evaluation <= 0)
            {
                editDestroyerCount(attackerPlayer, planet, -getDestroyerCount(attackerPlayer, planet));

                ResourceManager.editBuildingsByAmount(defenderPlayer, planet, (int)BuildingType.bunker, -System.Convert.ToInt32((getDefValue(defenderPlayer, planet) + evaluation) / getDefenceBuildupLevel(defenderPlayer, planet)));
            }
        }
    }

    public void destroySpecificResources(int attackerPlayer, int defenderPlayer, int planet, int resourceType)
    {
        if(getDestroyerCount(attackerPlayer, planet) > 0 && getDestroyerCount(defenderPlayer, planet) == 0)
        {
            ResourceManager.overrideResourceAmount(defenderPlayer, planet, resourceType, 0);
        }
    }
    public void destroySpecificBuildings(int attackerPlayer, int defenderPlayer, int planet, int buildingType)
    {
        if (getDestroyerCount(attackerPlayer, planet) > 0 && getDestroyerCount(defenderPlayer, planet) == 0)
        {
            ResourceManager.overrideBuildingAmount(defenderPlayer, planet, buildingType, 0);
        }
    }
}
