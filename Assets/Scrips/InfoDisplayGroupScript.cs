using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class InfoDisplayGroupScript : MonoBehaviour
{
    //For connecting Unity with the script
    [SerializeField] GameObject upperGroup;
    [SerializeField] GameObject lowerGroup;
    [SerializeField] GameObject CostRoof;
    [SerializeField] TextMeshProUGUI BuildingTypName;

    //These are empty lists for Display-Objects 
    private List<GameObject> ResourceIconDisplay = new List<GameObject>();
    private List<GameObject> ResourceNumberDisplay = new List<GameObject>();

    private int[] cost = new int[4];

    private int BuildingNumber;

    // Start is called before the first frame update
    void Start()
    {
        //subscribes funktions to an event
        GameManager.instance.OnPlanetButtonClick += goToPlanetSpecificView;
        GameManager.instance.OnPlanetBackButtonClick += backFromPlanetSpecificView;
        upperGroup.SetActive(false);
        lowerGroup.SetActive(false);
    }

    public void OnBuildingClick(int BuildingNumber)
    {
        this.BuildingNumber = BuildingNumber;
        BuildingName();
        CostDisplay();
        //InOutPutSingleDisplay();
        
    }
    //is called when one of the button that look like planets is pressed; it is used to display some important information about the planet
    private void goToPlanetSpecificView()
    {
        upperGroup.SetActive(true);
        lowerGroup.SetActive(true);
    }

    //is the opposite of the funktion above
    private void backFromPlanetSpecificView() 
    {
        upperGroup.SetActive(false);
        lowerGroup.SetActive(false);
    }


    //checks what builing name should be displayed
    private void BuildingName()
    {
        switch (BuildingNumber) //Es tut mir innerlich weh das alles zu hardcoden, aber ich hab zu wenig Zeit um einen schönen und funktionierenden Code zu machen
        {
            case 0:
                break;
            case 1:
                BuildingTypName.text = "Smeltery";
                break;
            case 2:
                BuildingTypName.text = "Refinery";
                break;
            case 3:
                BuildingTypName.text = "Processing Plant";
                break;
            case 4:
                BuildingTypName.text = "Factory";
                break;
            case 5:
                BuildingTypName.text = "Bunker";
                break;
            case 6:
                BuildingTypName.text = "Ship Factory";
                break;
            case 7:
                BuildingTypName.text = "Logistics Facility";
                break;
            case 8:
                BuildingTypName.text = "Arms Factory";
                break;
            case 9:
                BuildingTypName.text = "Mining Drill";
                break;
            case 10:
                BuildingTypName.text = "Prospecting Drill";
                break;
            case 11:
                BuildingTypName.text = "Centralized Energy Network";
                break;
        }
    }

    //isn't finished, but should be used to display the input and the output of a single building of any kind
    private void InOutPutSingleDisplay()
    {
        foreach (Transform Parentchild in upperGroup.transform)
        {
            if (Parentchild.tag == "AmountDisplayParent")
            {
                foreach (Transform child in Parentchild.transform)
                {
                    if (child.tag == "AmountDisplay")
                    {
                        ResourceNumberDisplay.Add(child.gameObject);
                    }else if(child.tag == "ResourceTypIcon")
                    {
                        ResourceIconDisplay.Add(child.gameObject);
                    }
                }
            }
        }
        Debug.Log("ResourceNumber " + ResourceNumberDisplay.Count);
    }

    //displays the cost for one building of any kind to be build
    public void CostDisplay()
    {
        foreach(GameObject Display in ResourceNumberDisplay)
        {
            Display.SetActive(true);
        }
        listFillerForDisplay(CostRoof); //calls the funktion below to fill the list with the nessesaire DisplayObjects
        int singleCostAmount = 0;

        for(int i = 0; i < 8; i++)
        {
            if (GameManager.rescourceManager.costs[BuildingNumber-1, i] != 0)//checks the amount of resourcetyps and the specific resource amount needed to build the building, then puts it into a array
            {
                cost[singleCostAmount]  = GameManager.rescourceManager.costs[BuildingNumber-1, i];
                Debug.Log("CostAmountSpesific" + cost[singleCostAmount]);
                singleCostAmount++;
            }
        }
        Debug.Log("CostAmountAll: " + singleCostAmount);
        for (int i = 0; i < singleCostAmount; i++)//finds the displayobject in the list and display in its Text component the cost
        {
            Debug.Log("i: " + i + ", single: " + singleCostAmount);
            Debug.Log(ResourceNumberDisplay.Count);
            TMP_Text Display = ResourceNumberDisplay.Find(go => go.name == ("AmountDisplay("+ (i+1) +")")).GetComponent<TMP_Text>();
            if (Display != null)
            {
                Display.text = cost[i].ToString();
            }
        }
        if(cost.Length > singleCostAmount)//lets the not used display disappear
        {
            int stillLeft2do = 4-(cost.Length - singleCostAmount);
            for(int i = 0;i < stillLeft2do; i++)
            {
                GameObject Display = ResourceNumberDisplay.Find(go => go.name == ("AmountDisplay(" + (4-i) + ")"));
                if (Display != null)
                {
                    Display.SetActive(false);
                }
            }
        }
    }


    //fills the list with the displayObjects; also clears the list form the objects before
    private void listFillerForDisplay(GameObject Parent)
    {
        ResourceIconDisplay.Clear();
        ResourceNumberDisplay.Clear();
        foreach (Transform child in Parent.transform)
        {
            if (child.tag == "AmountDisplay")
            {
                ResourceNumberDisplay.Add(child.gameObject);
            }
            else if (child.tag == "ResourceTypIcon")
            {
                ResourceIconDisplay.Add(child.gameObject);
            }
        }
    }
}
