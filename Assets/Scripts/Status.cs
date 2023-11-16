using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Status : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    public MMF_Player hitFeedbackPlayer;

    protected virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void ReveiveDamage(float damageAmount)
    {
        hitFeedbackPlayer.PlayFeedbacks();
        health -= damageAmount;
    }

}
