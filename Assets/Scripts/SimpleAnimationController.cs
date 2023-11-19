using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
[RequireComponent(typeof(AnimancerComponent))]
public class SimpleAnimationController : MonoBehaviour
{
    [SerializeField] AnimationClip animationClip;
    AnimancerComponent animancer;
    void Start()
    {
        GetComponent<AnimancerComponent>().Play(animationClip);
    }

}
