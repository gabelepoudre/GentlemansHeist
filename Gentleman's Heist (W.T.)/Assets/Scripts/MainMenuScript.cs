using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame(){
        // Switches to the first Scene here
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame(){
        // Stops the Game -> only works once it's built
        Application.Quit();
    }
}
