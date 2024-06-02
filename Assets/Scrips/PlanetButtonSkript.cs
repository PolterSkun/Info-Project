using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static Unity.VisualScripting.Metadata;

public class PlanetSkriptButton : MonoBehaviour
{
    [SerializeField] private AudioClip pressed;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject buildingDisplayGroup;
    [SerializeField] private GameObject infoDisplayGroup;
    [SerializeField] private GameObject backToOverViewButton;
    [SerializeField] private GameObject planetGroup;

    
    GameObject currentPlanet;
    List<GameObject> planets = new List<GameObject>();

    private void Awake()
    {
        GameManager.instance.OnPlanetButtonClick += GoToPlanetSpecific;
        GameManager.instance.OnPlanetBackButtonClick += BackToOverView;
        foreach (Transform child in planetGroup.transform)
        {
            if (child.tag == "Planet")
            {
                planets.Add(child.gameObject);
            }
        }
        backToOverViewButton.SetActive(false);

    }

    public void OnPlanetClick(int planetNumber)
    {
        currentPlanet = planets.Find(go => go.name == "Planet" + planetNumber);
        GameManager.instance.planetButtonClick();
    }

    private void GoToPlanetSpecific()
    {
        buttonGroup.SetActive(false);
        currentPlanet.SetActive(true);
        buildingDisplayGroup.SetActive(true);
        infoDisplayGroup.SetActive(true);
        //source.PlayOneShot(pressed);
        backToOverViewButton.SetActive(true );
        Debug.Log(currentPlanet);
    }

    public void OnBackToOverViewClick()
    {
        GameManager.instance.planetBackButtonClick();
    }


    private void BackToOverView()
    {
        backToOverViewButton.SetActive(false);
        currentPlanet.SetActive(false);
        buildingDisplayGroup.SetActive(false );
        infoDisplayGroup.SetActive(false);
        buttonGroup.SetActive(true);
    }


}

