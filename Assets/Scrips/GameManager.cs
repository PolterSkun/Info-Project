using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject MainMenu;
    [SerializeField] public TMP_InputField playerAmountInput;

    public static GameManager instance;

    public static ResourceManager rescourceManager;
    public static PlayerManager playerManager;
    public static Logistics logisticsManager;
    public static CombatManager combatManager;
    public static PlanetManager planetManager;
    public static MarketScript marketManager;

    public int playerAmount;
    public int turnCounter = 1;
    public int subTurnCounter = 1;
    public int currentPlanetNumber;


    //public static DisplayManager displayManager = new();

    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
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

    // Start is called before the first frame update   
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public event Action OnPlanetButtonClick;
    public event Action OnPlanetBackButtonClick;
    public event Action OnNextTurn;
    public event Action OnStart;
    //public event Action OnFactoryButtonClick;

    public void startGame() // Es tut mir leid für alle die diesen Code lesen und verstehen müssen, aber ich hab ein paar probleme mit dem Code grad und deswegen mach ich ihn nicht einheitlich; ich wünschte ich könnte es 
    {
        startMainObjects(int.Parse(playerAmountInput.text), 5, 5);
        start();
    }

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

    public void getCurrentPlanetNumber(int currentPlanetNumber)
    {
        this.currentPlanetNumber = currentPlanetNumber;
    }

    //ALLLES UNTER DEISER ZEILE SIND FUNKTIONEN FÜR EVENTS

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

    /*public void FactoryButtonClick()
    {
        if(OnFactoryButtonClick != null)
        {
            OnFactoryButtonClick();
        }
    }*/

}
