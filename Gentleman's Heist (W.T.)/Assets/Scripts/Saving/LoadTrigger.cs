using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    private int _waitTimer;
    void Start()
    {
        _waitTimer = 30;
    }

    private void FixedUpdate()
    {
        if (_waitTimer > 0)
        {
            _waitTimer -= 1;
        }
        else
        {
            if (SaveMaster.needsLoad)
            {
                SaveMaster.LoadAll("Paul_Blart");
                Debug.Log("A state was loaded!");
                SaveMaster.needsLoad = false;
            }

            if (SaveMaster.needsSave)
            {
                SaveMaster.SaveAll("Paul_Blart");
                Debug.Log("A state was saved!");
                SaveMaster.FlipNeedsSave();
            }
        }
    }
}
