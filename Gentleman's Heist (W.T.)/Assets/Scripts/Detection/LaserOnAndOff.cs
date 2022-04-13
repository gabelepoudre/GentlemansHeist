using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserOnAndOff : MonoBehaviour
{
    public int framesToSwap;
    public Laser laser;

    private int _swapCounter;
    // Start is called before the first frame update
    void Start()
    {
        _swapCounter = framesToSwap;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_swapCounter > 0)
        {
            _swapCounter -= 1;
        }
        else
        {
            laser.Flip();
            _swapCounter = framesToSwap;
        }
    }
}
