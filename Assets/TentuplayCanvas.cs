using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TentuplayCanvas : MonoBehaviour
{
    public GameObject mailBoxAdvice;
    void Start()
    {
        var obj = FindObjectsOfType<MainCamera>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += ShowOrHideMailBox;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ShowOrHideMailBox(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "ShelterScene":
                mailBoxAdvice.SetActive(false);
                break;

            case "Stage1Scene":
                mailBoxAdvice.SetActive(true);
                break;

            case "BossScene":
                mailBoxAdvice.SetActive(false);
                break;
            default:
                break;
        }

    }

    void Update()
    {
        
    }
}
