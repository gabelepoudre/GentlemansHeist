using UnityEngine;

public class SaveIsActive : SaveScript
{
    
    void Start()
    {
        SaveMaster.KeepTrackOf(this);
    }

    public override void SaveMe()
    {
        SaveMaster.SaveVariables(uid, bools:new bool[1]{gameObject.activeSelf});
    }

    public override void LoadMe()
    {
        var load = SaveMaster.LoadVariables(gameObject, uid, setActiveFalseOnNotFound:true);
        bool loadedProperly = load.Item1;
        var data = load.Item2;
        if (loadedProperly)
        {
            gameObject.SetActive(data.Item2[0]);
        }
    }
}

