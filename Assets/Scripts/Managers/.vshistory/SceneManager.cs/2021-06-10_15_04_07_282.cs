using UnityEngine;

public enum SceneType
{
    SHELTER,
    STAGE
}

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public SceneType sceneType;

    public const int STAGE1_ARROW_COUNT = 50;
    public const int SHELTER_ARROW_COUNT = 45;

    public delegate void OnSceneLoad(string sceneName);
    public OnSceneLoad onSceneLoad;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        InitializeArrowCount(sceneName);
        onSceneLoad.Invoke(sceneName);
    }

    void InitializeArrowCount(string sceneName)
    {
        switch (sceneName)
        {
            case "ShelterScene":
                MainUI.instance.InitializeArrowCount(SHELTER_ARROW_COUNT);
                sceneType = SceneType.SHELTER;
                break;

            case "Stage1Scene":
                MainUI.instance.InitializeArrowCount(STAGE1_ARROW_COUNT);
                sceneType = SceneType.STAGE;
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
