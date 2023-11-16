using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    [HideInInspector] public Rigidbody2D rb;
    public float speed = 10f;
    public GameObject hitVFX;
    public LayerMask layerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!LyerContains(layerMask, other.gameObject.layer)|| !(other is CapsuleCollider2D))
            return;
        // Debug.Log(other.name);

        if (hitVFX)
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        if (other.tag is "Enemy")
        {
            if (other.transform.TryGetComponent<EnemyStatus>(out var targetStutas))
            {
                if (targetStutas.isDead)
                    return;
                targetStutas.ReveiveDamage(damage);
            }
        }
    }


    bool LyerContains(LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) > 0);
    }



}
