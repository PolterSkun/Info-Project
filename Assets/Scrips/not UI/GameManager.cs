using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Field to connect Objects from Unity to the skript
    [SerializeField] GameObject MainMenu;
    [SerializeField] public TMP_InputField playerAmountInput;

    //This is used to make GameManager a Singleton; This is useful because there should only be 1 GameManager
    public static GameManager instance;

    //For the Objects of the other classes
    public static ResourceManager rescourceManager;
    public static PlayerManager playerManager;
    public static Logistics logisticsManager;
    public static CombatManager combatManager;
    public static PlanetManager planetManager;
    public static MarketScript marketManager;

    //To keep track of the very important variables
    public int playerAmount;
    public int turnCounter = 1;
    public int subTurnCounter = 1;
    public int currentPlanetNumber;


    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        //Used to make sure there is only one instance of GameManager
        MainMenu.SetActive(true);
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("instance wird gesetzt");
            instance = this;
        }
    }

    //creation of event to "soft"-link our scripts
    public event Action OnPlanetButtonClick;
    public event Action OnPlanetBackButtonClick;
    public event Action OnNextTurn;
    public event Action OnStart;
    //public event Action OnFactoryButtonClick;this could be used later


    //Starst the game; is triggered by the done button in the menu
    public void startGame() // Es tut mir leid für alle die diesen Code lesen und verstehen müssen, aber ich hab ein paar probleme mit dem Code grad und deswegen mach ich ihn nicht einheitlich; ich wünschte ich könnte es 
    {
        startMainObjects(int.Parse(playerAmountInput.text), 5, 5);
        start();
    }


    //does as the name says
    public void startMainObjects(int realPlayerN, int realPlanetN, int planetConnections)
    {
        Debug.Log("PlayerAmount" + realPlayerN); //Bitte wegmachen vor Export
        int playerN = realPlayerN--;
        playerAmount = playerN;
        int planetN = realPlanetN--;
        rescourceManager = new(playerN, planetN);
        playerManager = new(playerN, planetN);
        logisticsManager = new(playerN, planetN, 100, 20);
        combatManager = new(playerN, planetN, 100, 10, 10, 50);
        planetManager = new(playerN, planetN, 0.2f, planetConnections);
        marketManager = new(planetN);
    }


    //is called by the next turn-button
    public void nextTurnButtonClick()
    {
        subTurnCounter++;
        if (subTurnCounter > playerAmount)
        {
            if (subTurnCounter != 1)
            {
                subTurnCounter = 1;
            }
            turnCounter++;
        }
        nextTurn();
    }

    //is called by the buttons in the middle of the main view
    public void getCurrentPlanetNumber(int currentPlanetNumber)
    {
        this.currentPlanetNumber = currentPlanetNumber;
    }

    //everything under this line is a funktion dedicated to their event; they just check if the event is null and if not then they execute the event (they dont kill it)

    public void planetButtonClick()
    {
        if (OnPlanetButtonClick != null)
        {
            OnPlanetButtonClick();

        }
    }
    public void nextTurn()
    {
        if (OnNextTurn != null)
        {
            OnNextTurn();
        }
    }
    public void planetBackButtonClick()
    {
        if (OnPlanetBackButtonClick != null)
        {
            OnPlanetBackButtonClick();
        }
    }

    public void start()
    {
        if (OnStart != null)
        {
            OnStart();
        }
    }
}
