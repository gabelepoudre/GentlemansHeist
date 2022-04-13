using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveFloor : MonoBehaviour
{
    Game game;

    public Sprite newSprite;

    private bool canUseStairs = false;
    private bool _triggered = false;
    public static bool IsUnlocking = false;
    private bool hasChanged = false;
    private float _originalGuardSpeed;
    private HealthBar progBar;
    private Canvas test;

    void Start() {
        game = FindObjectOfType<Game>();
        progBar = GetComponentInChildren<HealthBar>();
        progBar.setMaxHealth(500);
        progBar.setHealth(0);
        gameObject.GetComponentInChildren<HealthBar>().gameObject.transform.localScale = new Vector3(0,0,0);
        _originalGuardSpeed = GuardData.getGuardSpeed();
    }

    void Update() {
        if (PlayerData.GetKeys() >= 1) {
            // this.GetComponent<SpriteRenderer>().sprite = newSprite;
            canUseStairs = true;
        }

        if (IsUnlocking)
        {
            GuardData.setGuardSpeed(GuardData.getGuardSpeed() + 0.25f);
        }
        else
        {
            if (hasChanged)
            {
                GuardData.setGuardSpeed(_originalGuardSpeed);
                hasChanged = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (canUseStairs && progBar.slider.value/progBar.slider.maxValue > 0.99f && _triggered == false)
            {
                _triggered = true;
                GuardData.setGuardSpeed(_originalGuardSpeed);
                this.GetComponent<SpriteRenderer>().sprite = newSprite;
                PlayerData.ChangeKeys(PlayerData.GetKeys()-1);
                PlayerData.NextLevel();
                PlayerData.RemoveSilentShoes();
                game.NextFloor();
                SceneManager.LoadScene("UpgradeMenu");
                SaveMaster.ClearTracking();
            }
            else if (canUseStairs)
            {
                IsUnlocking = true;
                if (gameObject.GetComponentInChildren<HealthBar>().gameObject.transform.localScale.x<1)
                {
                    gameObject.GetComponentInChildren<HealthBar>().gameObject.transform.localScale = new Vector3(3, 3, 1);
                }

                if (progBar.slider.value + PlayerData.GetKeys() > progBar.slider.maxValue)
                {
                    progBar.slider.value = progBar.slider.maxValue;
                }
                else
                {
                    progBar.slider.value += PlayerData.GetKeys();
                }
            }
            else {
                Debug.Log("No keys collected");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        IsUnlocking = false;
        hasChanged = true;
        if (gameObject.GetComponentInChildren<HealthBar>().gameObject.transform.localScale.x > 1)
        {
            gameObject.GetComponentInChildren<HealthBar>().gameObject.transform.localScale = new Vector3(0,0,0);
        }
    }
}