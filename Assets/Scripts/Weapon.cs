using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using DG.Tweening;
public class Weapon : MonoBehaviour
{
    protected PlayerMovement playerMovement;
    public SpriteRenderer playerRenderer; // Hero的Transform组件
    public Transform weaponParent; // 武器的Transform组件
    public SpriteRenderer weaponRenderer; // 武器的Transform组件
    public float rotationSpeed = 5f; // 旋转速度

    public AnimancerComponent _animancer;
    public AnimationClip shootClip;
    public AnimationClip idleClip;

    public UIController _UI;

    public int magazineSize = 30;
    public int ammo = 30;
    public float reloadDuration;
    public bool reloading = false;
    [ShowInInspector] MMF_Player reloadFeedback;
    [ShowInInspector] MMF_Player reloadInterruptFeedback;


    public LayerMask layerMask;

    public enum WeaponType
    {
        semi, auto, burst
    }

    protected virtual void Awake()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        _UI = GameObject.FindWithTag("UIHUD").GetComponent<UIController>();
        weaponParent = transform.parent;
        if (!weaponRenderer)
            TryGetComponent(out weaponRenderer);
        _animancer = GetComponent<AnimancerComponent>();
        if (!_animancer)
            _animancer = GetComponentInChildren<AnimancerComponent>();
        reloadFeedback = weaponParent.parent.Find("Feedbacks").Find("reloadFeedback").GetComponent<MMF_Player>();
        reloadInterruptFeedback = weaponParent.parent.Find("Feedbacks").Find("reloadInterruptFeedback").GetComponent<MMF_Player>();

    }

    protected void Aiming()
    {
        // 获取鼠标位置
        Vector3 mousePosition = Input.mousePosition;

        // 将鼠标位置转换为世界坐标
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 计算朝向
        Vector2 direction = worldMousePosition - weaponParent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 平滑旋转
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        weaponParent.rotation = Quaternion.Slerp(weaponParent.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 根据旋转角度决定是否翻转Sprite
        Vector2 scale = weaponParent.localScale;
        scale.y = direction.x > 0 ? 1 : -1;
        weaponParent.localScale = scale;

        // 处理武器和角色的遮挡关系
        if (weaponParent.eulerAngles.z > 0 && weaponParent.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
    }
    public void Reload()
    {
        reloadFeedback.GetFeedbackOfType<MMF_Position>("Position").AnimatePositionDuration = reloadDuration;
        reloadFeedback.ComputeCachedTotalDuration();
        reloadFeedback.PlayFeedbacks();

        StartCoroutine(ReloadCoroutine());

    }
    public void ReloadInterrupt()
    {
        if (reloading)
        {
            reloading = false;
            reloadInterruptFeedback.PlayFeedbacks();
            StopCoroutine(ReloadCoroutine());
            DOTween.Kill("reload");
        }
    }

    IEnumerator ReloadCoroutine()
    {
        reloading = true;
        _UI.UIWeaponReload(this);
        yield return new WaitForSeconds(reloadDuration);
        ammo = magazineSize;
        reloading = false;
        _UI.UpdaeWeaponAmmoUI(this);
    }

}
