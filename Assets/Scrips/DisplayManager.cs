using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DisplayManager : MonoBehaviour
{

    [SerializeField] GameObject factoryDispaly;

    // Start is called before the first frame update
    void Start()
    {
        factoryDispaly.SetActive(false);
        Debug.Log(GameManager.rescourceManager.getBuildingsAmount(1,1,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void isInPlanetSpecificView()
    {
        

    }

}
