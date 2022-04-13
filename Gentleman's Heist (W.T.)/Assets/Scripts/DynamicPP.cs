using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DynamicPP : MonoBehaviour
{
    // Start is called before the first frame update
    private PostProcessVolume ppv;
    private int last;
    private AudioSource[] music;
    void Start()
    {
        ppv = gameObject.GetComponent<PostProcessVolume>();
        last = PlayerData.GetCurrentHealth();
        music = gameObject.GetComponents<AudioSource>();
        music[1].pitch = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (last != PlayerData.GetCurrentHealth())
        {
            // Debug.Log("DynPP noticed change");
            if ((float) PlayerData.GetCurrentHealth() / PlayerData.GetMaxHealth() < 0.1)
            {
                ppv.profile.GetSetting<ColorGrading>().saturation.Override(-100);
                ppv.profile.GetSetting<ColorGrading>().gamma.Override(new Vector4(10.0f, 10.0f, 10.0f, -0.6f));
                music[1].pitch = 0.7f;
                PlayerData.SetSpeed(PlayerData.GetSpeed()*0.7f);
            }
            else if ((float) PlayerData.GetCurrentHealth() / PlayerData.GetMaxHealth() < 0.2)
            {
                ppv.profile.GetSetting<ColorGrading>().saturation.Override(-90);
                ppv.profile.GetSetting<ColorGrading>().gamma.Override(new Vector4(10.0f, 10.0f, 10.0f, -0.5f));
                music[1].pitch = 0.8f;
                PlayerData.SetSpeed(PlayerData.GetSpeed()*0.8f);
            }
            else if ((float) PlayerData.GetCurrentHealth() / PlayerData.GetMaxHealth() < 0.3)
            {
                ppv.profile.GetSetting<ColorGrading>().saturation.Override(-80);
                ppv.profile.GetSetting<ColorGrading>().gamma.Override(new Vector4(10.0f, 10.0f, 10.0f, -0.4f));
                music[1].pitch = 0.85f;
                PlayerData.SetSpeed(PlayerData.GetSpeed()*0.9f);
            }
            else if ((float) PlayerData.GetCurrentHealth() / PlayerData.GetMaxHealth() < 0.4)
            {
                ppv.profile.GetSetting<ColorGrading>().saturation.Override(-70);
                ppv.profile.GetSetting<ColorGrading>().gamma.Override(new Vector4(10.0f, 10.0f, 10.0f, -0.3f));
                music[1].pitch = 0.9f;
            }
            else if ((float) PlayerData.GetCurrentHealth() / PlayerData.GetMaxHealth() < 0.5)
            {
                ppv.profile.GetSetting<ColorGrading>().saturation.Override(-60);
                ppv.profile.GetSetting<ColorGrading>().gamma.Override(new Vector4(10.0f, 10.0f, 10.0f, -0.2f));

            }
            else
            {
                ppv.profile.GetSetting<Vignette>().intensity.Override((float)((1.25 - PlayerData.GetCurrentHealth()/(float)PlayerData.GetMaxHealth())));
                ppv.profile.GetSetting<Vignette>().intensity.overrideState = true;
            }
        }

        last = PlayerData.GetCurrentHealth();
    }
}
