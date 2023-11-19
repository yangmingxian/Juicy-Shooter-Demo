using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSetting : MonoBehaviour
{
    Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void UpdateStatus()
    {
        if (!toggle)
            return;
        if (toggle.isOn)
        {
            GameController.vSyncCount = 1;
            GameController.UpdateFramerate();
        }
        else
        {
            GameController.vSyncCount = 0;
            GameController.UpdateFramerate();

        }
    }
}
