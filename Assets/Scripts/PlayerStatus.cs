using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
public class PlayerStatus : Status
{
    [SerializeField] MMProgressBar healthBar;
    public delegate void PlayerEvent();


    public static event PlayerEvent PlayerDie;

    protected override void Awake()
    {
        base.Awake();
        healthBar.UpdateBar01(health / maxHealth);
    }

    public override void ReveiveDamage(float damageAmount)
    {
        hitFeedbackPlayer.PlayFeedbacks();
        health -= damageAmount;
        healthBar.UpdateBar01(health / maxHealth);
        if (health <= 0)
        {
            health = 0;
            PlayerDie?.Invoke();
        }
    }

}
