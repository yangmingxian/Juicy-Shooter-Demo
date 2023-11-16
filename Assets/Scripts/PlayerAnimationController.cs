using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    AnimancerComponent _Animancer;
    [SerializeField] AnimationClip idleClip;
    [SerializeField] AnimationClip moveClip;

    private void Awake()
    {
        _Animancer = spriteRenderer.GetComponent<AnimancerComponent>();
    }
    private void FixedUpdate()
    {
        FaceToMouse();
    }

    [Button]
    public void PlayIdleLoop()
    {
        _Animancer.Play(idleClip);
        var state = _Animancer.Play(idleClip);
        // _Animancer.Play(idleClip).Time = 0;
    }
    [Button]
    public void PlayMoveLoop()
    {
        _Animancer.Play(moveClip);
        var state = _Animancer.Play(moveClip);
        // _Animancer.Play(moveClip).Time = 0;
    }
    [Button]
    public void PlayClipLoop(AnimationClip clip)
    {
        _Animancer.Play(clip);
        var state = _Animancer.Play(clip);
        _Animancer.Play(clip).Time = 0;
    }

    void FaceToMouse()
    {
        // 获取鼠标位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标位置转换为世界坐标
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 计算角色朝向
        Vector2 direction = worldMousePosition - transform.position;

        // 设置Sprite的Flip
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
