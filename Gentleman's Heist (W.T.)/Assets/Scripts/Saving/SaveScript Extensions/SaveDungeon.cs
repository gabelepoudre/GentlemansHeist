using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDungeon : SaveScript
{
    private int _delayTimer;
    private int _seed;
    public override void SaveMe()
    {
        SaveMaster.SaveVariables(
            uid:this.uid,
            ints:new int[]{gameObject.GetComponent<GenerateDungeon>().seed});
        Debug.Log($"Dungeon Saved. For reference, seed = {gameObject.GetComponent<GenerateDungeon>().seed}");
    }

    void Start()
    {
        SaveMaster.KeepTrackOf(this);
        _delayTimer = -1;
    }

    //delayed to (try) to force PlayerData to be loaded before generating the dungeon
    private void FixedUpdate()
    {
        if (_delayTimer > 0)
        {
            _delayTimer -= 1;
        }
        else
        {
            if (_delayTimer == 0)
            {
                gameObject.GetComponent<GenerateDungeon>().seed = _seed;
                gameObject.GetComponent<Game>().LoadDungeon(_seed);
                Debug.Log("Tried to generate identical room");
                SaveMaster.needsLoad = false;
                _delayTimer = -1;
                Debug.Log($"Dungeon Loaded. For reference, seed = {_seed}");
            }
        }
    }

    public override void LoadMe()
    {
        var load = SaveMaster.LoadVariables(gameObject, uid);
        bool loadedProperly = load.Item1;
        var data = load.Item2;
        if (loadedProperly)
        {
            _seed = data.Item1[0];
            _delayTimer = 10;
        }
        else
        {
            Debug.Log("failed to load properly");
        }
    }
}
