using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    void goTo(string sceneName)
    {
        SceneManager.LoadScene("ShelterScene");
    }
}
