using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    void goTo(string sceneName)
    {
        SceneManager.LoadScene("ShelterScene");
    }
}
