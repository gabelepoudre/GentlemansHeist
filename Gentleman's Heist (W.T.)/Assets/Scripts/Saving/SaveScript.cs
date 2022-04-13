using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    /*
     * THIS VALUE CAN NEVER CONTAIN __ AND SHOULD ALWAYS BE UNIQUE TO EVERY OBJECT
     */
    public string uid;
    // private bool neverBeenLoaded = true;
    // private int var1;
    // private int var2;
    
    void Start()
    {
        SaveMaster.KeepTrackOf(this);
        if (uid == null)
        {
            Debug.Log("No uid set for savefile. Using: " + this.gameObject.name + this.gameObject.tag);
            uid = this.gameObject.name + this.gameObject.tag;
        }
    }

    public virtual void SaveMe()
    {
        // SaveMaster.SaveVariables(
        //     uid,
        //     ints:new int[2]{var1, var2},
        //     bools:new bool[1]{false},
        //     floats:new float[2]{this.gameObject.transform.position.x, this.gameObject.transform.position.y},
        //     strings:new string[1]{this.gameObject.GetComponent<PlayerHandling>().name});
    }

    public virtual void LoadMe()
    {
        // var load = SaveMaster.LoadVariables(this.gameObject, uid, setActiveFalseOnNotFound:true);
        // bool successfulLoad = load.Item1;
        // var data = load.Item2;
        // int[] ints = data.Item1;
        // bool[] bools = data.Item2;
        // float[] floats = data.Item3;
        // string[] strings = data.Item4;
        //
        // if (successfulLoad)
        // {
        //     var1 = ints[0];
        //     var2 = ints[1];
        //
        //     neverBeenLoaded = bools[0];
        //
        //     Vector3 loadedPosition = new Vector3(floats[0], floats[1], 0f);
        //     this.gameObject.transform.position = loadedPosition;
        //
        //     this.gameObject.GetComponent<PlayerHandling>().name = strings[0];  
        // }
    }
}
