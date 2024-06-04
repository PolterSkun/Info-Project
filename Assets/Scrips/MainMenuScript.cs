using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playerNumberInputPanel;
    [SerializeField] private GameObject isSurePanel;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject exeption;
    [SerializeField] public TMP_InputField inputFieldPlayerAmount;
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioSource source;

    private bool isFirstTimeMenu;
    private bool isSure;
    private int playerAmount;


    private void Awake()
    {
        playerNumberInputPanel.SetActive(false);
        isSurePanel.SetActive(false);
        exeption.SetActive(false);
        resumeButton.SetActive(false);
        mainMenuPanel.SetActive(true);
        startButton.SetActive(true);
        mainMenu.SetActive(true);
        source.PlayOneShot(mainTheme);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void start()
    {
        playerNumberInputPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

    }
    public void WhenApplied()
    {
        mainMenuPanel.SetActive(false);
        int playerAmountNew = int.Parse(inputFieldPlayerAmount.text);
        if (playerAmountNew <=4 && playerAmountNew >0) 
        {
            playerAmount = int.Parse(inputFieldPlayerAmount.text);
            mainMenu.SetActive(false);
            isFirstTimeMenu = false;
        }
        else
        {
            exeption.SetActive(true);
        }
    }

    public void closeApplied()
    {
        mainMenuPanel.SetActive(true);
        playerNumberInputPanel.SetActive(false);
    }

    public void ExitGame()
    {
        mainMenu.SetActive(true);
        if (!isFirstTimeMenu) 
        { 
            startButton.SetActive(false); 
        }
        isSurePanel.SetActive(true);        
    }

    public void TrueExitGame(bool isSure)
    {
        this.isSure = isSure;
            if (isSure)
            {
                Application.Quit();
            }
            else
            {
                startButton.SetActive(true);
                isSurePanel.SetActive(false);
            }
    }
    public void MainMenuOpen()
    {
        startButton.SetActive(false);
        exeption.SetActive(false);
        playerNumberInputPanel.SetActive(false);
        isSurePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        resumeButton.SetActive(true);
        mainMenu.SetActive(true);
    }
    public void MainMenuClose()
    {
        mainMenu.SetActive(false);
           
    }
}
