using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Animancer;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class SummonFX : MonoBehaviour
{
    public SpriteRenderer circleSpriteRenderer;
    public SpriteRenderer _FXSpriteRenderer;
    public AnimancerComponent summonFXAnimancer;
    [SerializeField] AnimationClip animationClip;

    public float duration;



    private void Awake()
    {
        summonFXAnimancer = GetComponentInChildren<AnimancerComponent>();
    }


    public void PlayFX()
    {
        StartCoroutine(PlayFXCoroutine());
    }


    IEnumerator PlayFXCoroutine()
    {
        circleSpriteRenderer.DOColor(Color.white, 0.4f);
        yield return new WaitForSeconds(0.2f);

        summonFXAnimancer.Play(animationClip);
        var state = summonFXAnimancer.Play(animationClip);
        state.Time = 0;
        // state.Events.OnEnd = () => Destroy(animancer.gameObject);
    }
}
