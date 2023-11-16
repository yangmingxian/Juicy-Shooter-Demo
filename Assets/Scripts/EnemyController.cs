using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using DG.Tweening;


public class EnemyController : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] SpriteRenderer weaponSprite;


    AnimancerComponent _animancer;
    [SerializeField] AnimationClip idleClip;
    [SerializeField] AnimationClip moveClip;
    [SerializeField] AnimationClip deathClip;



    public Vector2 direction;
    public float speed = 50f;
    public float moveDamping = 0.2f;

    MeleeWeapon meleeWeapon;
    public LayerMask layerMask;

    public float attackRange = 1f;
    public float attackCooldown = 1f;
    public float attackDamage = 10f;

    public bool active = false;

    EnemyStatus status;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animancer = GetComponentInChildren<AnimancerComponent>();
        meleeWeapon = GetComponentInChildren<MeleeWeapon>();

        status = GetComponent<EnemyStatus>();
        meleeWeapon.attackFeedbackPlayer.CooldownDuration = attackCooldown;
        Init();
    }

    private void FixedUpdate()
    {
        if (!active || status.isDead)
            return;

        if (IsPlayerInAttactRange())
        {
            Attack();
        }
        else
        {
            RunToPlayer();
        }
    }

    void Init()
    {
        active = false;
        enemySprite.color = Color.clear;
        weaponSprite.color = Color.clear;
    }

    public void Appear(float time)
    {
        enemySprite.DOColor(Color.white, time);
        weaponSprite.DOColor(Color.white, time);
    }


    public void Idle()
    {
        _animancer.Play(idleClip);
        var state = _animancer.Play(idleClip);
        _animancer.Play(idleClip).Time = 0;
        state.Events.OnEnd = () => state.IsPlaying = false;
    }


    public void RunToPlayer()
    {
        _animancer.Play(moveClip);
        direction = GameObject.FindWithTag("Player").transform.position - transform.position;
        rb.velocity += speed * Time.fixedDeltaTime * direction.normalized;
        rb.velocity *= Mathf.Pow(1 - moveDamping, Time.fixedDeltaTime * 10);
    }

    public void Attack()
    {
        meleeWeapon.damage = attackDamage;
        meleeWeapon.layerMask = layerMask;

        meleeWeapon.attackFeedbackPlayer.PlayFeedbacks();

    }


    public bool IsPlayerInAttactRange()
    {
        return Vector2.Distance(GameObject.FindWithTag("Player").transform.position, transform.position) <= attackRange;
    }

    public MMF_Player deathFeedbackPlayer;
    public Explosion explodsion;

    public void Death()
    {
        _animancer.Play(deathClip);
        var state = _animancer.Play(deathClip);
        _animancer.Play(deathClip).Time = 0;
        state.Events.OnEnd = () => state.IsPlaying = false;

        explodsion.ownerTrans = transform;
        explodsion.Explode();

        deathFeedbackPlayer.PlayFeedbacks();
        transform.GetComponentInChildren<CircleCollider2D>().isTrigger = true;
        transform.GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        rb.mass=20;
        GameController.enemyList.Remove(this);

    }
}
