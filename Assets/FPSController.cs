using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSController : MonoBehaviour
{
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField, Range(0.05f, 1f)] float sampleDuration = 0.05f;
    int frame;
    float duration;
    void Start()
    {
        Application.targetFrameRate = GameController.framerate;
    }
    private void Update()
    {
        if (tMP_Text)
        {
            float frameDuration = Time.unscaledDeltaTime;
            frame += 1;
            duration += frameDuration;
            if (duration >= sampleDuration)
            {
                tMP_Text.text = (frame / duration).ToString("f1");
                frame = 0;
                duration = 0;
            }
        }


    }
}
