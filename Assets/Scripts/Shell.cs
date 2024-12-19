using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Vector2 speed = new(2, 5);
    public Vector2 ejectOffset = new(-30, 30);

    public float stopTime = 0.5f;
    public float vanishTime = 5f;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public delegate void OnDisableCallback(Shell Instance);
    public OnDisableCallback Disable;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    private void OnEnable()
    {
        spriteRenderer.color = Color.white;
        var angle = Random.Range(ejectOffset.x, ejectOffset.y);
        var dist = Random.Range(speed.x, speed.y);
        rb.linearVelocity = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up * dist;
        StartCoroutine(StopShell());
    }

    IEnumerator StopShell()
    {
        yield return new WaitForSeconds(stopTime);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(10f);
        spriteRenderer.DOColor(Color.clear, vanishTime).SetEase(Ease.Linear).OnComplete(
          () => ShellVanish()
        );
    }

    void ShellVanish()
    {
        ObjectPoolManager.PushObject(gameObject);
    }

}
