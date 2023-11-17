using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using UnityEngine.Pool;
using Lean.Pool;
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

    public float hitForce;


    [SerializeField] Shell shellPrefab;
    [SerializeField] Transform shellEjectTrans;

    // Pool<Bullet> bulletPool;
    // Pool<Shell> shellPool;

    ObjectPool<Bullet> bulletPool;
    ObjectPool<Shell> shellPool;

    Transform bulletContainer;
    Transform shellContainer;


    [SerializeField] MMF_Player shootFeedbackPlayer;

    protected override void Awake()
    {
        base.Awake();
        if (transform.Find("ShootFeedback"))
        {
            shootFeedbackPlayer = transform.Find("ShootFeedback").GetComponent<MMF_Player>();
        }
        bulletContainer = GameObject.FindWithTag("Pool").transform.Find("BulletPool").transform;
        shellContainer = GameObject.FindWithTag("Pool").transform.Find("ShellPool").transform;

        // bulletPool = Pool.Create(bulletPrefab, 0, bulletContainer);
        // shellPool = Pool.Create(shellPrefab, 0, shellContainer);

        // bulletPool = new ObjectPool<Bullet>(CreatePooledObject, OnTakeFromPool, OnReturnToPool, OnDestroyObject, true, 10, 1000);
        // shellPool = new ObjectPool<Shell>(CreatePooledShell, OnTakeShellFromPool, OnReturnShellToPool, OnDestroyShell, true, 10, 1000);
    }
    // void ReturnBulletToPool(Bullet instance)
    // {
    //     bulletPool.Take(instance);
    // }
    // void ReturnShellToPool(Shell instance)
    // {
    //     shellPool.Take(instance);
    // }

    // #region bullet Pool
    // public Bullet CreatePooledObject()
    // {
    //     var instance = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    //     instance.Disable += ReturnObjectToPool;
    //     instance.gameObject.SetActive(false);
    //     Debug.Log("池为空,新建对象" + instance.gameObject.name);
    //     return instance;
    // }

    // void ReturnObjectToPool(Bullet instance)
    // {
    //     bulletPool.Release(instance);
    // }

    // void OnTakeFromPool(Bullet instance)
    // {
    //     instance.gameObject.transform.position = firePoint.position;
    //     instance.gameObject.SetActive(true);

    //     instance.Invoke("DestroySelf", 2f);
    //     Debug.Log(instance.gameObject.name + "出池");
    // }
    // void OnReturnToPool(Bullet instance)
    // {
    //     instance.gameObject.SetActive(false);
    //     // gameObject.GetComponent<Bullet>().rb.velocity = Vector2.zero;
    //     Debug.Log(instance.gameObject.name + "进池");
    // }
    // void OnDestroyObject(Bullet instance)
    // {
    //     Debug.Log("池已满," + gameObject.name + "被销毁");
    //     Destroy(instance.gameObject);
    // }

    // #endregion

    // #region  shell Pool
    // public Shell CreatePooledShell()
    // {
    //     var instance = Instantiate(shellPrefab, shellEjectTrans.position, shellEjectTrans.rotation);
    //     instance.Disable += ReturnShellToPool;
    //     instance.gameObject.SetActive(false);
    //     Debug.Log("池为空,新建对象" + instance.gameObject.name);
    //     return instance;
    // }

    // void ReturnShellToPool(Shell instance)
    // {
    //     shellPool.Release(instance);
    // }

    // void OnTakeShellFromPool(Shell instance)
    // {
    //     instance.gameObject.transform.SetPositionAndRotation(shellEjectTrans.position, shellEjectTrans.rotation);
    //     instance.gameObject.SetActive(true);
    //     Debug.Log(instance.gameObject.name + "出池");
    // }
    // void OnReturnShellToPool(Shell instance)
    // {
    //     instance.gameObject.SetActive(false);
    //     Debug.Log(instance.gameObject.name + "进池");
    // }
    // void OnDestroyShell(Shell instance)
    // {
    //     Debug.Log("池已满," + gameObject.name + "被销毁");
    //     Destroy(instance.gameObject);
    // }
    // #endregion

    private void Update()
    {
        if (PlayerStatus.isDead)
            return;
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
        if (PlayerStatus.isDead)
            return;
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
            // var bulletObj = Instantiate(bulletPrefab.gameObject, firePoint.position, Quaternion.identity);
            // var bulletObj = bulletPool.Get();
            // var bullet = bulletObj.GetComponent<Bullet>();
            // var bullet = bulletPool.Get();
            // bullet.Disable += ReturnBulletToPool;

            var bullet = LeanPool.Spawn(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.SetParent(bulletContainer);
            LeanPool.Despawn(bullet.gameObject, 2);

            bullet.initFirePos = firePoint.position;
            var spread = Random.Range(-spreadAngle, spreadAngle);
            bullet.damage = damage;

            bullet.hitForce = hitForce;
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
        var shell = LeanPool.Spawn(shellPrefab, shellEjectTrans.position, shellEjectTrans.rotation);
        shell.transform.SetParent(shellContainer);
        // var shell = shellPool.Get();
        // shell.transform.SetPositionAndRotation(shellEjectTrans.position, shellEjectTrans.rotation);
        // shell.Disable += ReturnShellToPool;
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
