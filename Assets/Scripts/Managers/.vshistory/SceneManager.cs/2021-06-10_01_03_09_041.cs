using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    SHELTER,
    STAGE
}

public class SceneManager : MonoBehaviour
{
    public SceneType sceneType;

    public const int STAGE1_ARROW_COUNT = 50;
    public const int SHELTER_ARROW_COUNT = 45;

    public GameObject inventoryPanel;
    public GameObject storePanel;

    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        Initialize(sceneName);
    }

    void Initialize(string sceneName)
    {
        switch (sceneName)
        {
            case "ShelterScene":
                MainUI.instance.InitializeArrowCount(SHELTER_ARROW_COUNT);
                GameObject.Find("StoreNpc").GetComponent<Store>().inventoryPanel = inventoryPanel;
                GameObject.Find("StoreNpc").GetComponent<Store>().storePanel = storePanel;

                break;

            case "Stage1Scene":
                MainUI.instance.InitializeArrowCount(STAGE1_ARROW_COUNT);
                break;

            default:
                MainUI.instance.InitializeArrowCount(45);
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
