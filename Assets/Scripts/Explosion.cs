using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Transform ownerTrans;
    Collider2D[] inExplosionRadius = null;
    [SerializeField] float explosionForceFactor = 5;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] Vector2 explosionForceRange = new(100, 200);



    [Button]
    public void Explode()
    {
        GetComponent<ParticleSystem>().Play();
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D c in inExplosionRadius)
        {
            if (c.transform == ownerTrans)
                continue;
            Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dist = c.transform.position - transform.position;
                if (dist.magnitude > 0)
                {
                    float explosionForce = Mathf.Clamp(explosionForceFactor / dist.magnitude, explosionForceRange.x, explosionForceRange.y);
                    rb.AddForce(dist.normalized * explosionForce, ForceMode2D.Impulse);
                }
            }


        }
    }

}
