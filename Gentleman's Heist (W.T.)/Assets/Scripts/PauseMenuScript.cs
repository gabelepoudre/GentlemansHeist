using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    bool isPaused = false;
    public GameObject pauseMenu;

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            // SceneManager.UnloadSceneAsync("SampleScene");
            SceneManager.LoadScene("MainMenu");
            SaveMaster.ClearTracking();
            Time.timeScale = 1f;
            //temp for testing

        }
    }
    public void ResumeGame(){
        // switches back to players last scene
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame(){
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void BackToMenu(){
        SceneManager.LoadScene("MainMenu");
    }

}
