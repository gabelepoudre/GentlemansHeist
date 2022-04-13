using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEffect : MonoBehaviour
{
    private SpriteRenderer _sprRend;
    private bool _isRed;
    private int _colorSwitchTimer = 10;
    // Start is called before the first frame update
    void Start()
    {
        _sprRend = gameObject.GetComponent<SpriteRenderer>();
        _sprRend.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Alarm.IsOn())
        {
            Debug.Log("Ran hit");
            if (_colorSwitchTimer > 0)
            {
                _colorSwitchTimer -= 1;
            }
            else
            {
                if (_isRed)
                {
                    _sprRend.color = new Color(0.1f, 0.1f, 0.6f, 0.25f);
                    _isRed = false;
                }
                else
                {
                    _sprRend.color = new Color(0.6f, 0.1f, 0.1f, 0.25f);
                    _isRed = true;
                }

                _colorSwitchTimer = 360;
            }
        }
    }
}
