using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecCamMovement : MonoBehaviour
{
    public float spinSpeedGolf;
    public int endDelayFrames;
    private int _endDelayCounter;
    private int _spinSpeedTrue;

    private bool _inverted;

    private int _delayCounter;

    private float _rotateCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        _spinSpeedTrue = (int)(spinSpeedGolf * 60.0);
        _delayCounter = 0;
        _endDelayCounter = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_endDelayCounter > 0)
        {
            _endDelayCounter -= 1;
        }
        else if (_delayCounter < _spinSpeedTrue)
        {
            if (_inverted)
            {
                _delayCounter -= 1;
                _rotateCounter -= (float)(1.0 / _spinSpeedTrue); 
            }
            else
            {
                _delayCounter += 1;
                _rotateCounter += (float)(1.0 / _spinSpeedTrue); 
            }
            
            
            if (_rotateCounter > 45)
            {
                _inverted = true;
                _endDelayCounter = endDelayFrames;
            }
            else if (_rotateCounter < -45)
            {
                _inverted = false;
                _endDelayCounter = endDelayFrames;
            }
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.forward * _rotateCounter);
        }
        else
        {
            _delayCounter = 0;
        }
    }
}
