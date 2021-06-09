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

    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        
        switch (sceneName)
        {
            case "Stage1Scene":
                MainUI.instance.InitializeArrowCount(50);
                break;

            case "ShelterScene":
                MainUI.instance.InitializeArrowCount(100);
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
