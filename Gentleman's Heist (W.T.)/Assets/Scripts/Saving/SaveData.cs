using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string[] lookUpTable;
    public int[] ints;
    public bool[] bools;
    public float[] floats;
    public string[] strings;

    public SaveData(List<string> nlookUpTable, List<int> intList, List<bool> boolList, List<float> floatList, List<string> stringList)
    {
        lookUpTable = nlookUpTable.ToArray();
        ints = intList.ToArray();
        bools = boolList.ToArray();
        floats = floatList.ToArray();
        strings = stringList.ToArray();
    }

}
