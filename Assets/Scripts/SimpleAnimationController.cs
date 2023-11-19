using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
[RequireComponent(typeof(AnimancerComponent))]
public class SimpleAnimationController : MonoBehaviour
{
    [SerializeField] AnimationClip animationClip;
    [SerializeField] float Speed;
    AnimancerComponent animancer;
    private void Awake()
    {
        animancer = GetComponent<AnimancerComponent>();
    }
    void Start()
    {
        animancer.Play(animationClip);
        var state = animancer.Play(animationClip);
        state.Speed = Speed;
    }

}
