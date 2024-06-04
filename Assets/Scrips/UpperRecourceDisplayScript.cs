using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpperRecourceDisplayScript : MonoBehaviour
{

    [SerializeField] GameObject UpperRecourceDisplay;

    private int resource;

    private List<GameObject> ResourceNumberDisplayParents = new List<GameObject>();
    private List<GameObject> ResourceNumberDisplay = new List<GameObject>();
    

    // Start is called before the first frame update
    //is called for displaying alle the resource that are held by the player; this is in the wrong place
    void Start()
    {
        GameManager.instance.OnPlanetButtonClick += onPlantButtonClick;
        GameManager.instance.OnPlanetBackButtonClick += OnPlanetBackButtonClick;
        foreach (Transform Parentchild in UpperRecourceDisplay.transform)
        {
            ResourceNumberDisplayParents.Add(Parentchild.gameObject);
            if (Parentchild.tag == "AmountDisplayParent")
            {
                foreach (Transform child in Parentchild.transform)
                {
                    if (child.tag == "AmountDisplay")
                    {
                        ResourceNumberDisplay.Add(child.gameObject);
                    }
                }
            }
        }
        Debug.Log("ResourceNumber " + ResourceNumberDisplay.Count);
        foreach(GameObject Parent in ResourceNumberDisplayParents)
        {
            Parent.SetActive(false);
        }
        //UpperRecourceDisplay.SetActive(false);
    }


    //displays the resouceamount on that planet
    private void onPlantButtonClick()
    {
        
        for (int i = 0; i < ResourceNumberDisplay.Count; i++)
        {
            resource = GameManager.rescourceManager.getResourceAmount(GameManager.instance.subTurnCounter, GameManager.instance.currentPlanetNumber, i);
            //Debug.Log("Recource " + i + " ist: " + resource);
            TMP_Text Display = ResourceNumberDisplay.Find(go => go.name == "RecourceAmount" + i).GetComponent<TMP_Text>();//Find(go => go. ...): finds anything after the "." after the second "go"
            if (Display != null)
            {
                Display.text = resource.ToString();
            }
        }
        foreach(GameObject Parent in ResourceNumberDisplayParents) 
        {
            Parent.SetActive(true); 
        }
    }
    //used to not display the displays
    private void OnPlanetBackButtonClick()
    {
        foreach(GameObject Parent in ResourceNumberDisplayParents)
        {
            Parent.SetActive(false);
        }
    }
}
