using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrisonMenuScript : MonoBehaviour
{

    void Start()
    {
        SaveMaster.DeleteSave("Paul_Blart");
    }
    
    public void EscapePrison(){
        // Starts player at beginning
        PlayerData.SetLevel(0);
        // Loads SampleScene
        SceneManager.LoadScene("SampleScene");
        SaveMaster.ClearTracking();
    }
    public void QuitGame(){
        // Stops the Game -> only works once it's built
        SceneManager.LoadScene("MainMenu");
        SaveMaster.ClearTracking();
        PlayerData.ClearAll();
    }

}
