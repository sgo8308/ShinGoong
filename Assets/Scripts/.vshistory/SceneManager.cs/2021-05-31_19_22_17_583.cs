﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scene : MonoBehaviour
{
    void goTo(string sceneName)
    {
        SceneManager.Load("ShelterScene");
    }
}