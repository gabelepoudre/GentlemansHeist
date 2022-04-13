using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonBehaviour : MonoBehaviour
{
    private Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _button = gameObject.GetComponent<Button>();
        if (!SaveMaster.SaveExists("Paul_Blart"))
        {
            var col = _button.colors;
            col.normalColor = Color.gray;
            _button.colors = col;
            _button.interactable = false;
        }
    }
    
}
