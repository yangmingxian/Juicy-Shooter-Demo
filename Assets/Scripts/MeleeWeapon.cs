using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MeleeWeapon : MonoBehaviour
{
    public SpriteRenderer weaponRenderer; // 武器的Transform组件
    public Transform weaponParent;
    public SpriteRenderer ownerRenderer; // Hero的Transform组件

    Collider2D col;
    public float damage;
    public float hitForce;
    public LayerMask layerMask;
    public Transform playerTrans;
    public float rotationSpeed = 10f; // 旋转速度

    public MMF_Player attackFeedbackPlayer;


    private void Awake()
    {
        TryGetComponent(out weaponRenderer);
        col = GetComponent<Collider2D>();
        if (!weaponParent)
        {
            weaponParent = transform.parent;
        }
        playerTrans = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        AimingPlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerContains(layerMask, other.gameObject.layer))
        {

            if (other.transform.TryGetComponent<PlayerStatus>(out var player))
            {

                Vector2 difference = (other.transform.position - transform.position).normalized;
                Vector2 force = difference * hitForce;
                other.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);


                player.ReveiveDamage(damage);
                ChangeTrigger(false);
            }
        }
    }

    public void ChangeTrigger(bool status)
    {
        col.enabled = status;
    }


    void AimingPlayer()
    {
        // 计算朝向
        Vector2 direction = playerTrans.transform.position - weaponParent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 平滑旋转
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        weaponParent.rotation = Quaternion.Slerp(weaponParent.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 根据旋转角度决定是否翻转Sprite
        Vector2 scale = weaponParent.localScale;
        scale.y = direction.x > 0 ? 1 : -1;
        // 翻转玩攻击动画FB也要翻转
        attackFeedbackPlayer.GetFeedbackOfType<MMF_Rotation>("Rotation").RemapCurveOne = direction.x > 0 ? 90 : -90;

        weaponParent.localScale = scale;

        // 处理武器和角色的遮挡关系
        if (weaponParent.eulerAngles.z > 0 && weaponParent.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = ownerRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = ownerRenderer.sortingOrder + 1;
        }
    }

    // TODO: Aim nothing and move around


    bool LayerContains(LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) > 0;
    }

}
