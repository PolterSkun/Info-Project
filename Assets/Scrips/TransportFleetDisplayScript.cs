using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportFleetDisplayScript : MonoBehaviour
{
    [SerializeField] GameObject OverGroup;
    [SerializeField] GameObject TransportDisplay;
    [SerializeField] GameObject FleetDisplay;

    private bool transportLastDisplayed = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnPlanetButtonClick += goToPlanetSpecificView;
        GameManager.instance.OnPlanetBackButtonClick += backFromPlanetSpecificView;
        TransportDisplay.SetActive(true);
        FleetDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void goToPlanetSpecificView()
    {
       OverGroup.SetActive(false);
    }
    private void backFromPlanetSpecificView()
    {
        OverGroup.SetActive(true);
        if(transportLastDisplayed)
        {
            TransportDisplay.SetActive(true);
            FleetDisplay.SetActive(false);
        }
        else
        {
            FleetDisplay.SetActive(true);
            TransportDisplay.SetActive(false);
        }
        
    }
    public void TransportLastDisplayed(bool transportLastDisplay)
    {
        transportLastDisplayed = transportLastDisplay;
        if(transportLastDisplayed)
        {
            TransportDisplay.SetActive(true);
            FleetDisplay.SetActive(false);
        }
        else
        {
            TransportDisplay.SetActive(false);
            FleetDisplay.SetActive(true);
        }
    }
}
