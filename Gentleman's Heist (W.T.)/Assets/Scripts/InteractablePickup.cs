using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simple interaction when a Player interacts with an interactable object.
public class InteractablePickup : MonoBehaviour
{
    //Coin pickup sound
    public AudioSource coinSound;
    //Diamond pickup sound
    public AudioSource diamondSound;
    public AudioSource keySound;
    public AudioSource foodSound;    
    public AudioSource drinkSound;

    public PlayerScript player;
    private bool hasCoffee;
    //Instantiating the sound sources
    void Start() {
        coinSound = gameObject.GetComponent<AudioSource>();
        diamondSound = gameObject.GetComponent<AudioSource>();
        keySound = gameObject.GetComponent<AudioSource>();
        foodSound = gameObject.GetComponent<AudioSource>();
        drinkSound = gameObject.GetComponent<AudioSource>();
    }
    /**Does an interaction with a game object that depends on which game object is being interacted with. 
    Key updates number of keys the player has and plays key audio clip, 
    Diamond updates the number of diamonds a player has and plays a diamond audio clip,
    Coin updates the score of the player and plays a coin audio clip.
    After game object is interacted with, it will be deleted from the scene.
    **/
    public void DoInteraction(string name){
        Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,Camera.main.transform.position.z);
        
        if (name.StartsWith("Key")){
            AudioSource.PlayClipAtPoint(keySound.clip,position, .5f);
            PlayerData.SetKeys();
            Destroy(gameObject);
        }
        else if(name.StartsWith("Diamond")){
            AudioSource.PlayClipAtPoint(diamondSound.clip,position, .5f);
            PlayerData.AddScore(100);
            PlayerData.AddMoney(100);
            PlayerData.SetDiamonds();
            Destroy(gameObject);
        }
        else if(name.StartsWith("Coin")){
            AudioSource.PlayClipAtPoint(coinSound.clip,position, .5f);
            PlayerData.AddScore(20);
            PlayerData.AddMoney(20);
            Destroy(gameObject);
        }
        else if(name.StartsWith("Donut")){
            if (PlayerData.GetMaxHealth() != PlayerData.GetCurrentHealth()){
                AudioSource.PlayClipAtPoint(foodSound.clip,position, .4f);
                PlayerData.HealAmount(50);
                Destroy(gameObject);
            }
        }
        else if(name.StartsWith("Apple")){
             if (PlayerData.GetMaxHealth() != PlayerData.GetCurrentHealth()){
                AudioSource.PlayClipAtPoint(foodSound.clip,position, .4f);
                PlayerData.HealAmount(10);
                Destroy(gameObject);
            }
        }
        else if(name.StartsWith("Coffee")){
            if (!hasCoffee){
                AudioSource.PlayClipAtPoint(drinkSound.clip,position, .4f);
                hasCoffee = true;
                PlayerData.AddSpeed(5.0f); // Temp since who knows if this is speedy
                gameObject.transform.localScale = new Vector3(0,0,0); // this works but is jank
                Invoke(nameof(resetSpeed),5.0f); 
            } 
        }  
    }
    private void resetSpeed(){
        PlayerData.SubSpeed(5.0f);
        hasCoffee = false;
        Destroy(gameObject);
    }
}
