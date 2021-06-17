using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MiniMapCamera : MonoBehaviour
{
    Camera miniMapCamera;
    CinemachineVirtualCamera miniMapVCam;
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
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += AssignCameraConfiner;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        miniMapCamera = GetComponent<Camera>();
        miniMapVCam = transform.Find("MiniMapVCam").GetComponent<CinemachineVirtualCamera>();
        miniMap = GameObject.Find("MainUI").transform
                              .Find("MiniMap").gameObject;

        miniMapTransform = miniMap.GetComponent<RectTransform>();
        minimapImage = miniMap.GetComponent<RawImage>();

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += InitializeMiniMap;
        


        shelterRenderTexture = new RenderTexture(684, 312, 0); ;
        stage1RenderTexture = new RenderTexture(1092, 528, 0);
        bossRenderTexture = new RenderTexture(732, 336, 0);

        shelterImageSize = new Vector2(684, 312);
        stage1ImageSize = new Vector2(1092, 528);
        bossImageSize = new Vector2(732, 336);
    }

    void AssignCameraConfiner(Scene scene, LoadSceneMode mode)
    {
        CinemachineConfiner cameraConfiner = transform.Find("MiniMapVCam").GetComponent<CinemachineConfiner>();
        GameObject camConfiner = GameObject.Find("CamConfiner");
        cameraConfiner.m_BoundingShape2D = camConfiner.GetComponent<PolygonCollider2D>();
    }

    void InitializeMiniMap(Scene scene, LoadSceneMode mode)
    {
        miniMapCamera.targetTexture.Release();

        switch (scene.name)
        {
            case "ShelterScene":
                miniMapCamera.targetTexture = shelterRenderTexture;
                minimapImage.texture = shelterRenderTexture;

                miniMapVCam.m_Lens.OrthographicSize = 16;

                miniMapTransform.sizeDelta = shelterImageSize;
                break;

            case "Stage1Scene":
                miniMapCamera.targetTexture = stage1RenderTexture;
                minimapImage.texture = stage1RenderTexture;

                miniMapVCam.m_Lens.OrthographicSize = 39;

                miniMapTransform.sizeDelta = stage1ImageSize;
                break;

            case "BossScene":
                miniMapCamera.targetTexture = bossRenderTexture;
                minimapImage.texture = bossRenderTexture;

                miniMapVCam.m_Lens.OrthographicSize = 30;

                miniMapTransform.sizeDelta = bossImageSize;
                break;
            default:
                break;
        }
    }
}
