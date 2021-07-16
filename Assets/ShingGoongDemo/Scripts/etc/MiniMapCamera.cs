using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 미니맵 카메라에 붙어있는 스크립트.
/// </summary>
public class MiniMapCamera : MonoBehaviour
{
    Camera miniMapCamera;
    GameObject miniMap;
    RectTransform miniMapTransform;
    RawImage minimapImage;

    RenderTexture shelterRenderTexture;
    RenderTexture stage1RenderTexture;
    RenderTexture bossRenderTexture;

    Vector2 shelterImageSize;
    Vector2 stage1ImageSize;
    Vector2 bossImageSize;

    private void Awake()
    {
        var obj = FindObjectsOfType<MiniMapCamera>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += FindPosition;
            FindPosition();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        miniMapCamera = GetComponent<Camera>();
        miniMap = GameObject.Find("MainUI").transform
                              .Find("MiniMap").gameObject;

        miniMapTransform = miniMap.GetComponent<RectTransform>();
        minimapImage = miniMap.GetComponent<RawImage>();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeMiniMap;
        
        shelterRenderTexture = new RenderTexture(684, 312, 0); ;
        stage1RenderTexture = new RenderTexture(910, 440, 0);
        bossRenderTexture = new RenderTexture(732, 336, 0);

        shelterImageSize = new Vector2(684, 312);
        stage1ImageSize = new Vector2(910, 440);
        bossImageSize = new Vector2(732, 336);
    }

    void InitializeMiniMap(Scene scene, LoadSceneMode mode)
    {
        miniMapCamera.targetTexture.Release();

        switch (scene.name)
        {
            case "ShelterScene":
                miniMapCamera.targetTexture = shelterRenderTexture;
                minimapImage.texture = shelterRenderTexture;

                miniMapCamera.orthographicSize = 16;

                miniMapTransform.sizeDelta = shelterImageSize;
                break;

            case "Stage1Scene":
                miniMapCamera.targetTexture = stage1RenderTexture;
                minimapImage.texture = stage1RenderTexture;

                miniMapCamera.orthographicSize = 39;
                miniMapTransform.sizeDelta = stage1ImageSize;
                break;

            case "BossScene":
                miniMapCamera.targetTexture = bossRenderTexture;
                minimapImage.texture = bossRenderTexture;

                miniMapCamera.orthographicSize = 30;

                miniMapTransform.sizeDelta = bossImageSize;
                break;
            default:
                break;
        }
    }

    public void FindPosition(Scene scene, LoadSceneMode mode)
    {
        transform.position = GameObject.Find("MiniMapCameraPosition").transform.position;
    }

    void FindPosition()
    {
        transform.position = GameObject.Find("MiniMapCameraPosition").transform.position;
    }
}
