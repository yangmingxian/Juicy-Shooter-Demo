using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : Status
{
    public bool isDead;

    protected override void Awake()
    {
        base.Awake();
        isDead = false;
    }

    public override void ReveiveDamage(float damageAmount)
    {
        hitFeedbackPlayer.PlayFeedbacks();
        health -= damageAmount;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
            GetComponent<EnemyController>().Death();
        }
    }

}
