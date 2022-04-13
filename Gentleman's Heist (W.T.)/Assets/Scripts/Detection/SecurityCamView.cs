using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamView : MonoBehaviour
{
    public LineRenderer tracer;

    public GameObject camera;
    public int framesUntilCaught;
    public AudioSource musicToStop;
    public AudioSource musicToPlay;
    private bool _counting;
    // private PlayerScript _ps;
    private int _intensity = 0;
    // Start is called before the first frame update
    void Start()
    {

        // If they bought alarm disabler...
        if(PlayerData.getUpgradeAlarmD()){
            camera.SetActive(false);
        }

        tracer.useWorldSpace = true;
        _counting = false;
        musicToStop = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[0];
        musicToPlay = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_counting)
        {
            _intensity += 1;
            if (_intensity > framesUntilCaught)
            {
                _intensity = framesUntilCaught;
                PlayerScript.detected = true;
                musicToStop.Pause();
                musicToPlay.UnPause();
            }
        }
        else
        {
            if (_intensity > 0)
            {
                _intensity -= 2;
            }
        }

        tracer.startColor = new Color(0.5f, 0, 0, _intensity / (float)framesUntilCaught);
        tracer.endColor = new Color(0.5f, 0, 0, _intensity / (float)framesUntilCaught);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _counting = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _counting = false;
            tracer.SetPosition(0, tracer.gameObject.transform.position);
            tracer.SetPosition(1, tracer.gameObject.transform.position);
        }
        
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            tracer.SetPosition(0, tracer.gameObject.transform.position);
            tracer.SetPosition(1, other.gameObject.transform.position);
        }
    }
}
