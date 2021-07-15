using UnityEngine;
using UnityEngine.SceneManagement;

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
    public const int SHELTER_ARROW_COUNT = 50;

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

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeArrowCount;
    }

    public void GoTo(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        if (SoundManager.instance == null)
            return;

        SoundManager.instance.MutePlayerSound();
        SoundManager.instance.MutePlayerRunningSound();
    }

    void InitializeArrowCount(Scene scene, LoadSceneMode sceneMode)
    {
        switch (scene.name)
        {
            case "ShelterScene":
                Inventory.instance.InitializeArrowCount(SHELTER_ARROW_COUNT);
                sceneType = SceneType.SHELTER;
                break;

            case "Stage1Scene":
                Inventory.instance.InitializeArrowCount(STAGE1_ARROW_COUNT);
                sceneType = SceneType.STAGE;
                break;

            default:
                Inventory.instance.InitializeArrowCount(50);
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("exit game2");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
