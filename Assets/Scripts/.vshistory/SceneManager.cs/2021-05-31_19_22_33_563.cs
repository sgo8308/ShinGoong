﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    void goTo(string sceneName)
    {
        SceneManager.Load("ShelterScene");
    }
}
