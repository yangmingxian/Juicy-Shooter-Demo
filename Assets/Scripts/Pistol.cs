using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;

public class Pistol : Weapon
{
    public WeaponType weaponType = WeaponType.semi;

    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform firePoint;

    public float bulletsNum = 1;
    public float attackSpeed;
    public float damage;
    public float CritDamage;
    public float CritRate;

    public int burstNum = 3;
    public float burstDelay = 0.05f;


    public float spreadAngle;

    public float recoil = 10;


    [SerializeField] GameObject shellPrefab;
    [SerializeField] Transform shellEjectTrans;



    [SerializeField] MMF_Player shootFeedbackPlayer;

    protected override void Awake()
    {
        base.Awake();
        if (transform.Find("ShootFeedback"))
        {
            shootFeedbackPlayer = transform.Find("ShootFeedback").GetComponent<MMF_Player>();
        }
    }

    private void Update()
    {
        switch (weaponType)
        {
            case WeaponType.semi:
                if (Input.GetButtonDown("Fire1") && reloading == false && ammo > 0)
                {
                    ShootOnce();
                }
                break;
            case WeaponType.auto:
                if (Input.GetButton("Fire1") && reloading == false && ammo > 0)
                {
                    ShootAuto();
                }
                break;
            case WeaponType.burst:
                if (Input.GetButtonDown("Fire1") && reloading == false && ammo > 0)
                {
                    ShootBurst(burstNum, burstDelay);
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.R) && reloading == false && ammo < magazineSize)
        {
            Reload();
        }
        if (Input.GetButton("Fire1") && reloading == false && ammo <= 0)
        {
            Reload();
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

    }
    void FixedUpdate()
    {
        Aiming();
    }

    public void ShootOnce()
    {
        if (ammo <= 0)
        {
            Reload();
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        // 将鼠标位置转换为世界坐标
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        // 计算朝向
        Vector2 direction = worldMousePosition - firePoint.transform.position;

        for (int i = 0; i < bulletsNum; i++)
        {
            var bulletObj = Instantiate(bulletPrefab.gameObject, firePoint.position, Quaternion.identity);
            var bullet = bulletObj.GetComponent<Bullet>();
            var spread = Random.Range(-spreadAngle, spreadAngle);
            bullet.damage = damage;
            bullet.layerMask = layerMask;

            bullet.transform.right = Quaternion.AngleAxis(spread, Vector3.forward) * direction;

            bullet.rb.velocity = bullet.transform.right * bullet.speed;
        }
        Vector2 RecoilDir = playerMovement.transform.position - worldMousePosition;
        ApplyRecoil(RecoilDir);

        ammo -= 1;
        if (ammo <= 0)
        {
            ammo = 0;
            Reload();
        }
        _UI.UIWeaponShoot(this);
        PlayShootAnim();
        shootFeedbackPlayer.PlayFeedbacks();
        EjectShell();
    }

    float timer;
    public void ShootAuto()
    {
        if (timer <= 0)
        {
            ShootOnce();
            timer = 1 / attackSpeed;
        }
    }

    public void ShootBurst(int num, float delay)
    {
        StartCoroutine(ShootBurstCoroutine(num, delay));
    }

    private IEnumerator ShootBurstCoroutine(int num, float delay)
    {
        for (int i = 0; i < num; i++)
        {
            ShootOnce();
            yield return new WaitForSeconds(delay);
        }
    }

    void ApplyRecoil(Vector3 dir)
    {
        playerMovement.rb.AddForce(dir * recoil, ForceMode2D.Impulse);
    }

    void EjectShell()
    {
        Instantiate(shellPrefab, shellEjectTrans.position, shellEjectTrans.rotation);
    }

    public void PlayShootAnim()
    {
        _animancer.Play(shootClip);
        var state = _animancer.Play(shootClip);

        _animancer.Play(shootClip).Time = 0;
        state.Events.OnEnd = PlayIdle;
    }
    void PlayIdle()
    {
        _animancer.Play(idleClip);
        var state = _animancer.Play(idleClip);

        state.Events.OnEnd = () => state.IsPlaying = false;
    }

}
