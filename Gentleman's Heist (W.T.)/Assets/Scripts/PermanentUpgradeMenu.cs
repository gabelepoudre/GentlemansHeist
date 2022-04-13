using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PermanentUpgradeMenu : MonoBehaviour
{
       [SerializeField] 
    private GameObject BlockSilentText;
    [SerializeField] 
    private GameObject BlockCoffeeText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerData.GetSoundRatio() < 1){
            BlockSilentText.SetActive(true);
        }
        if (PlayerData.GetDefaultSpeed() > 15){
            BlockCoffeeText.SetActive(true);
        }
        //I think this deletes everything here but game breaks if I don't have it in
        SaveMaster.DeleteSave("Paul_Blart");
    }
    
    public void ContinueNextLevel(){
        SceneManager.LoadScene("SampleScene");
        SaveMaster.ClearTracking();
        PlayerData.ClearAllButPermanents();
    }
    public void buySilentShoes(){
        //Probably temp value so sound circle doesn't disappear once upgrading all the time
        // if (PlayerData.GetSoundRatio() >= 1){
        //     PlayerData.AddSilentShoes(0.2f);
        // }
        if (PlayerData.GetSoundRatio() > 1)
        {
            PlayerData.AddSoundBonus(0.2f);
        }
        ContinueNextLevel();
    }
    public void buyCoffee(){
        //Temp since don't want to speedy but who knows how speedy we want
        if (PlayerData.GetDefaultSpeed() < 15){
            PlayerData.PermanentIncreaseSpeed(0.1f);
        }
        ContinueNextLevel();
    }
    public void buyFood(){
        //Max health could probably just keep going up to who cares how much
        PlayerData.PermanentIncreaseMaxHealth(5);
        ContinueNextLevel();
    }

    
}
