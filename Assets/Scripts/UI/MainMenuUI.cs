using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement ;

public class MainMenuUI : UIOpener
{
    public GameObject inventoryPanel;

    public GameObject gameOverPanel;

    GameObject mainMenuPanel;

    GameObject exitStageButton;

    private void Awake()
    {
        var obj = FindObjectsOfType<GameOverUI>();

        if (obj.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();

        mainMenuPanel = transform.Find("MainMenuPanel").gameObject;

        exitStageButton = transform.Find("MainMenuPanel")
                                    .Find("ExitStageButtonHolder").gameObject;
    }

    void Update()
    {
        if (inventoryPanel.activeSelf)
            return;

        if (gameOverPanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenuPanel.activeSelf)
                Close();
            else
                Open();
        }       
    }

    protected override void Open()
    {
        base.Open();

        if (UnityEngine.SceneManagement.SceneManager.
                GetActiveScene().name == "ShelterScene")
        {
            exitStageButton.SetActive(false);
        }
        else
        {
            exitStageButton.SetActive(true);
        }

        mainMenuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    protected override void Close()
    {
        base.Close();
        mainMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
