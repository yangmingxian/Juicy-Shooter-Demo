using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class CameraController : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera;

    public Transform aimTrans;
    public float aimDuration;
    public Ease ease;

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire2"))
        {
            aimTrans.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            aimTrans.localPosition = Vector3.zero;
        }
    }

}
