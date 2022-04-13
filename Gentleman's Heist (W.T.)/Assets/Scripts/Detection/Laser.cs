using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public AudioSource musicToStop;
    public AudioSource musicToPlay;
    // Start is called before the first frame update
    private bool _active = true;

    private bool _disabled = false;

    protected Transform __diodeTransform;
    protected LineRenderer __laserLR; 
    void Start()
    {
        // If they bought alarm disabler...
        if(PlayerData.getUpgradeAlarmD()){
            _disabled = true;
        }

        Physics2D.queriesHitTriggers = false;
        __laserLR = gameObject.GetComponentInChildren<Grabber>().gameObject.GetComponent<LineRenderer>();
        __diodeTransform = gameObject.GetComponentInChildren<Grabber2>().transform;
        __laserLR.useWorldSpace = true;
        musicToStop = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[0];
        musicToPlay = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1];
    }

    // Update is called once per fram

    private void Update()
    {
        if(!_disabled){
        if (!__laserLR.gameObject.activeSelf && _active)
        {
            __laserLR.gameObject.SetActive(true);
        }
        RaycastHit2D set = Physics2D.Raycast(this.gameObject.transform.position, gameObject.transform.up);
        
        if (set.collider != null && _active)
        {

            if (set.collider.gameObject.CompareTag("Player"))
            {
                PlayerScript.detected = true;
                musicToStop.Pause();
                musicToPlay.UnPause();
            }
            
            Vector3[] points = new Vector3[2];
            points[0] = __diodeTransform.position;
            points[1] = set.point;
            __laserLR.SetPositions(points);
        }
        else
        {
            if (__laserLR.gameObject.activeSelf)
            {
                __laserLR.gameObject.SetActive(false);
            }
        }
        }
    }


    public void On()
    {
        _active = true;
    }

    public void Off()
    {
        _active = false;
    }

    public void Flip()
    {
        _active = !_active;
    }

    public bool IsActive()
    {
        return _active;
    }
    
}
