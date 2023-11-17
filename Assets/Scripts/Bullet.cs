using System.Collections;
using System.Collections.Generic;
using Animancer;
using Lean.Pool;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Vector3 initFirePos;
    public float damage;
    [HideInInspector] public Rigidbody2D rb;
    public float speed = 10f;
    public GameObject hitVFX;
    public AnimationClip explodeAnimClip;

    public float hitForce;
    public LayerMask layerMask;

    // public delegate void OnDisableCallback(Bullet Instance);
    // public OnDisableCallback Disable;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!LayerContains(layerMask, other.gameObject.layer) || !(other is CapsuleCollider2D))
            return;
        // Debug.Log(other.name);

        if (hitVFX)
        {
            var explode = Instantiate(hitVFX, transform.position, Quaternion.identity);
            if (explode.TryGetComponent<AnimancerComponent>(out var animancer))
            {
                var state = animancer.Play(explodeAnimClip);
                state.Events.OnEnd = () => Destroy(explode.gameObject);
            }
        }

        // Destroy(gameObject);
        // Disable?.Invoke(this);
        LeanPool.Despawn(gameObject);

        if (other.tag is "Enemy")
        {
            Vector2 difference = (other.transform.position - initFirePos).normalized;
            Vector2 force = difference * hitForce;
            other.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);

            if (other.transform.TryGetComponent<EnemyStatus>(out var targetStutas))
            {
                if (targetStutas.isDead)
                    return;
                targetStutas.ReveiveDamage(damage);
            }
        }
    }

    bool LayerContains(LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) > 0);
    }





}
