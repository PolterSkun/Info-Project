using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnCounterDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI playerXTrun;
    [SerializeField] TextMeshProUGUI turnCounter;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnNextTurn += nextTurn;
        GameManager.instance.OnStart += firstTurnSetUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void firstTurnSetUp()
    {
        playerXTrun.SetText("Player 1");
        turnCounter.SetText("Turn 1");
    }

    public void nextTurn()
    {
        playerXTrun.SetText("Player "+GameManager.instance.subTurnCounter.ToString());
        turnCounter.SetText("Turn "+GameManager.instance.turnCounter.ToString());
    }
}
