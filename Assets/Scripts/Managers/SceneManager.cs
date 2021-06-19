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
    public const int SHELTER_ARROW_COUNT = 45;

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

        SoundManager.instance.MutePlayerSound();
        SoundManager.instance.StopNonPlayerSound();
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
                Inventory.instance.InitializeArrowCount(45);
                break;
        }
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
