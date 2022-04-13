using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeedGolf;
    private int _spinSpeedTrue;

    private int _delayCounter;

    private float _rotateCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        _spinSpeedTrue = (int)(spinSpeedGolf * 60.0);
        _delayCounter = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_delayCounter < _spinSpeedTrue)
        {
            _delayCounter += 1;
            _rotateCounter += (float)(1.0 / _spinSpeedTrue);
            gameObject.transform.rotation = Quaternion.Euler(Vector3.forward * _rotateCounter);
        }
        else
        {
            _delayCounter = 0;
        }
    }
}
