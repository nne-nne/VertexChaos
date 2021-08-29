using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image mainMenu;
    public Image pauseMenu;
    public Image endMenu;

    public Button playMainButton;
    public Button quitMainButton;
    public Button continuePauseButton;
    public Button quitPauseButton;
    public Button retryEndButton;
    public Button quitEndButton;
    
    private enum ActiveMenu
    {
        Main,
        Pause,
        End,
        None
    }

    private ActiveMenu activeMenu = ActiveMenu.None;

    // Start is called before the first frame update
    private void Start()
    {
        playMainButton.onClick.AddListener(SwitchNoMenu);
        quitMainButton.onClick.AddListener(QuitGame);
        continuePauseButton.onClick.AddListener(SwitchNoMenu);
        quitPauseButton.onClick.AddListener(QuitGame);
        retryEndButton.onClick.AddListener(Retry);
        quitEndButton.onClick.AddListener(QuitGame);
        SwitchMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        MenuEventBroker.PauseMenuSwitch += SwitchPauseMenu;
        MenuEventBroker.EndMenuSwitch += SwitchEndMenu;
        MenuEventBroker.PlayerKilled += SwitchEndMenu;
        MenuEventBroker.Retry += ReloadLevel;

        mainMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        endMenu.gameObject.SetActive(false);
    }

    private void SwitchMainMenu()
    {
        if (activeMenu == ActiveMenu.None)
        {
            Time.timeScale = 0f;
            mainMenu.gameObject.SetActive(true);
            activeMenu = ActiveMenu.Main;
        }
        else if (activeMenu == ActiveMenu.Main)
        {
            SwitchNoMenu();
        }
    }
    
    private void SwitchPauseMenu()
    {
        if (activeMenu == ActiveMenu.None)
        {
            Time.timeScale = 0f;
            pauseMenu.gameObject.SetActive(true);
            activeMenu = ActiveMenu.Pause;
        }
        else if (activeMenu == ActiveMenu.Pause)
        {
            SwitchNoMenu();
        }
    }
    
    private void SwitchEndMenu()
    {
        if (activeMenu == ActiveMenu.None)
        {
            endMenu.gameObject.SetActive(true);
            activeMenu = ActiveMenu.End;
        }
        else if (activeMenu == ActiveMenu.End)
        {
            SwitchNoMenu();
        }
    }

    private void SwitchNoMenu()
    {
        if (activeMenu != ActiveMenu.None)
        {
            mainMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
            endMenu.gameObject.SetActive(false);
            activeMenu = ActiveMenu.None;
            Time.timeScale = 1f;
        }
    }

    private void Retry()
    {
        SwitchNoMenu();
        MenuEventBroker.CallRetry();
    }

    private void QuitGame()
    {
        SwitchNoMenu();
        Application.Quit();
    }

    private void ReloadLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
