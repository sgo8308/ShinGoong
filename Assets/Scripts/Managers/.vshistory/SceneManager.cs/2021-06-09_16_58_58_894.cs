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

    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        
        switch (sceneName)
        {
            case "ShelterScene":
                MainUI.instance.InitializeArrowCount(SHELTER_ARROW_COUNT);
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
