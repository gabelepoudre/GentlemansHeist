using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#region SaveMasterExplainer
    /*
    * Almost definitely overcomplicated general save system!
    *
    * Fundamental Idea: A class that extends the SaveScript class implements methods:
    * | public override void SaveMe()
    * | public override void LoadMe()
    * should be able to save and load variables so long as it's UID is set to something unique,
    * and those methods implement the calls:
    * | SaveMaster.SaveVariables(uid)
    * | SaveMaster.LoadVariables(gameObject, uid)
    * respectively. Both of these variables have very important optional commands and returns.
    * so you should make sure to read their descriptions somewhere within.
    * It is also very important that a SaveScript extension implements:
    * | SaveMaster.KeepTrackOf(this)
    * so that the system knows you are an object who wishes to be saved or loaded.
    * 
    * It should be noted that SaveScript's Start() implements an example attempt to save
    * despite uid being null, using an object's name and tag as a UID
    *
    * So long as these conditions are met, the class's chosen variables are added to a local storage space when this
    * class has it's SaveAll() called, which is then converted to a SaveData class and saved as binary.
    * SaveData (and subsequently, this class) implements a lookUp and Key table to decide who's information belongs to who.
    */
#endregion
public static class SaveMaster
{
    /*TODO: create an update method for the LSS for if a script calls save variables personally*/
    
    private static List<SaveScript> _toTrack = new List<SaveScript>();
    
    // Local Storage Space (LSS)
    private static List<string> _lookUpTable = new List<string>();
    private static List<int> _intList = new List<int>();
    private static List<bool> _boolList = new List<bool>();
    private static List<float> _floatList = new List<float>();
    private static List<string> _stringList = new List<string>();
    private static string PATH = Application.persistentDataPath + "/";
    public static bool needsLoad = false;
    public static bool needsSave = false;

    public static void DeleteSave(string saveName)
    {
        File.Delete($"{PATH}{saveName}.smbf");
    }
    public static void ClearTracking()
    {
        _toTrack.Clear();
    }
    public static void FlipNeedsLoad()
    {
        needsLoad = !needsLoad;
    }

    public static void FlipNeedsSave()
    {
        needsSave = !needsSave;
    }
    public static bool SaveExists(string saveName)
    {
        return File.Exists($"{PATH}{saveName}.smbf");
    }
    /*
     * Function that adds given variables to the LSS
     * Normally called as a part of SaveAll(), theoretically can be called for use as a global variable space
     * 
     * use (within SaveMe) SaveVariables(uid, [type1]s:new type1[numVariables]{var1, var2, ...}, [type2]s:new type2[1]{newVar1})
     */
    #region LongSaveVariablesExplainer
    /*
     * || USAGE! ||
     * | parameters ints, bools, floats, and strings are optional variables. They are arrays of the only values
     * | that are allowed to be serialized(saved). Pretty much every important variable you could need to save can
     * | be saved using these 4 optional params (need they be converted or not)
     * || ||
     * 
     * || simple example (within SaveMe() function) ||
     * | 
     * | SaveMaster.SaveVariables(uid, bools: new bool[1]{gameObject.activeSelf})
     * | 
     * || ^ will save if the object this is attached to is active ||
     *
     * || more complex example (within SaveMe() function) ||
     * |
     * | float x = gameObject.transform.position.x;
     * | float y = gameObject.transform.position.y;
     * | SaveMaster.SaveVariables(uid, floats:new float[2]{x, y});
     * |
     * || ^ will save the position of the object this is attached to ||
     */
    #endregion
    public static void SaveVariables(string uid, int[] ints = null, bool[] bools = null, float[] floats = null,
        string[] strings = null)
    {
        // Simultaneously add the generated lookUp entry to the table and save all the passed data to LSS 
        _lookUpTable.Add(GenerateLookUpEntryAndAddToList(uid, ints, bools, floats, strings));
    }

    /*
     * Function that returns given variables from the LSS
     * Normally called as a part of LoadAll(), theoretically can be called for use as a global variable space
     *
     * tl;dr:
     * use (within LoadMe()) var x = LoadVariables(gameObject, uid) 
     * returns tuple (successfulLoad, (ints, bools, floats, strings))
     * use successfulLoad for extra failed to load logic
     * this return should be organized in the same way you saved
     * ie if you save bools:new Bool[2]{x, y} then you load one from y = LoadVariables(...).Item2.Item2[1] 
     */
    #region LongLoadVariablesExplainer
    /*
     * || USAGE! ||
     * | If parameters you sent to SaveVariables have been saved, this function will return them to you
     * | a reference to the gameObject in addition to the uid is passed as it's inefficient to search and
     * | find this reference within the _toTrack variable given a uid.
     * | It's needed for the two optional arguments: destroyOnNotFound and setActiveFalseOnNotFound.
     * | To explain the purpose of these arguments, consider this theoretical gameplay:
     * |  >Player enters preboss room
     * |  >Player saves, in fear
     * |  >Player enters boss arena, boss is added to the current scene dynamically
     * |  >player loads back, in fear
     * |  >SaveMaster is asked by the boss to load his position
     * |  >This fails, the boss has not been saved. This does not result in a crash, it's a pretty common occurance
     * |  >Boss maintains his position while the scene loads as the preboss room
     * |  >Boss has teleported into the preboss room
     * | To prevent this, the developer of the boss's SaveScript can write (within LoadMe()):
     * |  LoadVariables(gameObject, uid, destroyOnNotFound: true
     * | This change means that in that same scenario, that instance of the boss can be destroyed and a new one can
     * | be dynamically created. 
     * || ||
     * 
     * || simple example (within LoadMe() function) ||
     * | 
     * | var load = SaveMaster.LoadVariables(gameObject, uid, setActiveFalseOnNotFound:true);
     * | bool loadedProperly = load.Item1;
     * | var data = load.Item2;
     * | if (loadedProperly)
     * | {
     * |    gameObject.SetActive(data.Item2[0]);
     * | }
     * | 
     * || ^ will load if the object this is attached to was active ||
     *
     * || more complex example (within LoadMe() function) ||
     * |
     * | var load = SaveMaster.LoadVariables(gameObject, uid);
     * | bool loadedProperly = load.Item1;
     * | var data = load.Item2;
     * | if (loadedProperly)
     * | {
     * |    Vector3 newPos = new Vector3(data.Item3[0], data.Item3[1], 0);
     * |    gameObject.transform.position = newPos;
     * |
     * | }
     * | else
     * | {
     * | Debug.Log("failed to load properly");
     * | }
     * |
     * || ^ will load the old position of the object this is attached to ||
     */
    #endregion
    public static (bool, (int[], bool[], float[], string[])) LoadVariables(GameObject thisGameObject, string uid, bool destroyOnNotFound=false, bool setActiveFalseOnNotFound=false)
    {
        // default to doesn't exist, could be a seperate bool lol
        string key = "Not Found";
        
        // check each entry for one with a matching uid
        foreach (string entry in _lookUpTable)
        {
            (string, string ) entryParsed = ParseLookUpEntry(entry);
            if (entryParsed.Item1 == uid)
            {
                key = entryParsed.Item2;
            }
        }

        // Logic for failure to find
        // TODO: Maybe add an option argument ignoreWarnings to disable else's Debug.Log() 
        if (key == "Not Found")
        {
            if (destroyOnNotFound)
            {
                Object.Destroy(thisGameObject);
            }
            else if (setActiveFalseOnNotFound)
            {
                thisGameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Attempted to Load an Object not contained in save file. You can destroy object "
                          + thisGameObject.name + " by adding \"destroyOnNotFound:true\" to your LoadVariables SaveFile." +
                          "Additionally, you can disable an object on a failed load using \"setActiveFalseOnNotFound:true\"");
            }
        }
        
        else
        {
            // Parse the found key to get the actual indexes out
            (List<int>, List<int>, List<int>, List<int>) idxs = ParseKey(key);
            
            // get the lists to store the actual value from the indexes
            List<int> intvals = new List<int>();
            List<bool> boolvals = new List<bool>();
            List<float> floatvals = new List<float>();
            List<string> stringvals = new List<string>();
            if (idxs.Item1 != null)
            {
                foreach (int idx in idxs.Item1)
                {
                    intvals.Add(_intList[idx]);
                }
            }
            if (idxs.Item2 != null)
            {
                foreach (int idx in idxs.Item2)
                {
                    boolvals.Add(_boolList[idx]);
                }
            }
            if (idxs.Item3 != null)
            {
                foreach (int idx in idxs.Item3)
                {
                    floatvals.Add(_floatList[idx]);
                }
            }
            if (idxs.Item4 != null)
            {
                foreach (int idx in idxs.Item4)
                {
                    stringvals.Add(_stringList[idx]);
                }
            }
            // return them as arrays for consistency
            return (true, (intvals.ToArray(), boolvals.ToArray(), floatvals.ToArray(), stringvals.ToArray()));
        }

        // wasnt found. 
        return (false, (null, null, null, null));
    }
    
    /*
     * Calls the SaveMe() functions of all the scripts who used KeepTrackOf(this)
     * takes the name of the save as an argument to allow for multiple saves and loads
     */
    public static void SaveAll(string saveName)
    {
        // clear the LSS to avoid bugs
        _lookUpTable.Clear();
        _intList.Clear();
        _boolList.Clear();
        _floatList.Clear();
        _stringList.Clear();

        Debug.Log($"Saver looking for people to save, found {_toTrack.Count}");
        foreach (SaveScript script in _toTrack)
        {
            script.SaveMe();
            Debug.Log(script.name);
        }

        SaveData save = new SaveData(_lookUpTable, _intList, _boolList, _floatList, _stringList);
        //actually save this to binary
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".smbf";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        binaryFormatter.Serialize(stream, save);
        stream.Close();

    }

    /*
     * Calls the LoadMe of all Tracked scripts
     * takes saveName, the filename of the save to be used
     */
    public static void LoadAll(string saveName)
    {
        // clear the LSS to avoid bugs
        _lookUpTable.Clear();
        _intList.Clear();
        _boolList.Clear();
        _floatList.Clear();
        _stringList.Clear();
        
        string path = Application.persistentDataPath + "/" + saveName + ".smbf";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            SaveData newData = binaryFormatter.Deserialize(stream) as SaveData;

            // apply the loaded data to the LSS 
            ConvertSaveData(newData);

            foreach (SaveScript script in _toTrack)
            {
                script.LoadMe();
            }
            stream.Close();
        }
        else
        {
            Debug.LogError("File Not Found");
        }
    }

    /*
     * Given an instance of SaveData, reads the save data into the LSS
     */
    private static void ConvertSaveData(SaveData sd)
    {
        // for every value in sd, if it isn't null iterate through it
        if (sd.lookUpTable != null)
        {
            foreach (string entry in sd.lookUpTable)
            {
                _lookUpTable.Add(entry);
            }
        }

        if (sd.ints != null)
        {
            foreach (int x in sd.ints)
            {
                _intList.Add(x);
            }
        }

        if (sd.bools != null)
        {
            foreach (bool b in sd.bools)
            {
                _boolList.Add(b);
            }
        }

        if (sd.floats != null)
        {
            foreach (float f in sd.floats)
            {
                _floatList.Add(f);
            }
        }

        if (sd.strings != null)
        {
            foreach (string s in sd.strings)
            {
                _stringList.Add(s);
            }
        }
    }
    
    /*
     * Given a lookup entry formatted uid__key, returns (uid, key)
     */
    public static (string, string) ParseLookUpEntry(string entry)
    {
        // basic 'if there has been a __: add to key, else: add to uid 
        bool readingKey = false; 
        string key = "";
        string uid = "";
        for (int i = 0; i < entry.Length; i++)
        {
            if (readingKey)
            {
                if (entry[i] != '_')
                {
                    key += entry[i];
                }
            }
            else
            {
                if (entry[i] == '_' && i < entry.Length-1)
                {
                    if (entry[i + 1] == '_')
                    {
                        readingKey = true;
                    }
                    else
                    {
                        uid += entry[i];
                    }
                }
                else
                {
                    uid += entry[i];
                }
            }
        }

        return (uid, key);
    }

    
    
    /*
     * basic function that returns an array list of ints between 2 (full inclusive)
     * helps function ParseIntRange
     */
    private static List<int> IntRangeFill(int lower, int upper)
    {
        if (lower > upper)
        {
            // getting this error? You probably are asking the save system to save an empty array. 
            Debug.LogError("Lower bound greater than Upper: (" + lower + "/" + upper + ")");
            return null;
        }
        
        List<int> range = new List<int>();
        range.Add(lower);
        int cur = lower;

        while (cur != upper)
        {
            cur += 1;
            range.Add(cur);
        }

        return range;

    }

    /*
     * returns list of ints given the key's version of a int range (ex: 0t5)
     * helps ParseKey
     */
    private static List<int> ParseIntRange(string range)
    {
        int upper = -1;
        int lower = -1;
        string buffer = "";
        for (int x = 0; x < range.Length; x++)
        {
            if(range[x] != 't')
            {
                buffer += range[x];
            }
            else
            {
                lower = Convert.ToInt32(buffer);
                buffer = "";
            }
        }

        upper = Convert.ToInt32(buffer);
        if (upper == lower)
        {
            List<int> same = new List<int>();
            same.Add(upper);
            return same;
        }
        return IntRangeFill(lower, upper);
    }

    
    /*
     * Important function to deconstruct meaning from the keys
     * returns list of lists of integers correlating to data owned by the key
     */
    private static (List<int>, List<int>, List<int>, List<int>) ParseKey(string key)
    {
        List<int> int_idxs = new List<int>();
        List<int> bool_idxs = new List<int>();
        List<int> float_idxs = new List<int>();
        List<int> string_idxs = new List<int>();
        string buffer = "";
        int counter = 0;
        while (counter < key.Length)
        {
            switch (key[counter])
            {
                case 'I':
                    counter += 1;
                    while (key[counter] != 'B')
                    {
                        buffer += key[counter];
                        counter += 1;
                    }
                    if (buffer[0] == 'n')
                    {
                        int_idxs = null;
                    }
                    else
                    {
                        int_idxs= ParseIntRange(buffer);
                    }
                    buffer = "";
                    break;
                
                case 'B':
                    counter += 1;
                    while (key[counter] != 'F')
                    {
                        buffer += key[counter];
                        counter += 1;
                    }

                    if (buffer[0] == 'n')
                    {
                        bool_idxs = null;
                    }
                    else
                    {
                        bool_idxs= ParseIntRange(buffer);
                    }
                    buffer = "";
                    break;
                case 'F':
                    counter += 1;
                    while (key[counter] != 'S')
                    {
                        buffer += key[counter];
                        counter += 1;
                    }
                    if (buffer[0] == 'n')
                    {
                        float_idxs = null;
                    }
                    else
                    {
                        float_idxs= ParseIntRange(buffer);
                    }
                    buffer = "";
                    break;
                case 'S':
                    counter += 1;
                    while (counter < key.Length)
                    {
                        buffer += key[counter];
                        counter += 1;
                    }
                    if (buffer[0] == 'n')
                    {
                        string_idxs = null;
                    }
                    else
                    {
                        string_idxs= ParseIntRange(buffer);
                    }
                    buffer = "";
                    break;
                default:
                    counter += 1;
                    break;
            }
            
        }

        return (int_idxs, bool_idxs, float_idxs, string_idxs);
    } 

    /*
     * Add a given script to the list to check for SaveMe and LoadMe
     */
    public static void KeepTrackOf(SaveScript referenceToThisScript)
    {
        _toTrack.Add(referenceToThisScript);
    }
    
    /*
     * Generates the Lookup entry (uid__key) while adding the data to the proper lists
     */
    private static string GenerateLookUpEntryAndAddToList(string uid, int[] ints, bool[] bools, float[] floats, string[] strings)
    {

        // Integer Component of Key 
        string lookUpEntry = uid + "__I";
        if (ints != null)
        {
            lookUpEntry += (_intList.Count).ToString() + "t";
            foreach (int x in ints)
            {
                _intList.Add(x);
            }

            lookUpEntry += (_intList.Count - 1).ToString();
        }
        else
        {
            lookUpEntry += "n";
        }
        
        // Boolean Component of Key
        lookUpEntry +=  "B";
        if (bools != null)
        {
            lookUpEntry += (_boolList.Count).ToString() + "t";
            foreach (bool b in bools)
            {
                _boolList.Add(b);
            }
            lookUpEntry += (_boolList.Count - 1).ToString();
        }
        else
        {
            lookUpEntry += "n";
        }
        
        // Float Component of Key
        lookUpEntry +=  "F";
        if (floats != null)
        {
            lookUpEntry += (_floatList.Count).ToString() + "t";
            foreach (float f in floats)
            {
                _floatList.Add(f);
            }
            lookUpEntry += (_floatList.Count - 1).ToString();
        }
        else
        {
            lookUpEntry += "n";
        }
        
        
        // String Component of Key
        lookUpEntry +=  "S";
        if (strings != null)
        {
            lookUpEntry += (_stringList.Count).ToString() + "t";
            foreach (string s in strings)
            {
                _stringList.Add(s);
            }
            lookUpEntry += (_stringList.Count - 1).ToString();
        }
        // no idx 
        else
        {
            lookUpEntry += "n";
        }

        return lookUpEntry;

    }
}
  