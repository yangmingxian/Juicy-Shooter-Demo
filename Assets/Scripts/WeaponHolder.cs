using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponHolder : MonoBehaviour
{
    public Transform weaponParentTrans;
    public Weapon curWeapon;
    public List<Weapon> weapons;
    public UIController _UI;
    int index = 0;
    private void Awake()
    {
        _UI = GameObject.FindWithTag("UIHUD").GetComponent<UIController>();

        foreach (Transform tr in weaponParentTrans)
        {
            Debug.Log(tr.name);
            if (tr.TryGetComponent<Weapon>(out var wp))
            {
                weapons.Add(wp);
                wp.gameObject.SetActive(false);
            }
        }
        if (weapons.Count > 0)
        {
            curWeapon = weapons[0];
            index = 0;
            curWeapon.gameObject.SetActive(true);
            _UI.UIChangeWeapon(curWeapon);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            curWeapon.ReloadInterrupt();
            SwitchWeapon();
        }
    }

    public void SwitchWeapon()
    {
        if (weapons.Count >= 1)
        {
            curWeapon.gameObject.SetActive(false);
            index += 1;
            if (index >= weapons.Count)
            {
                index = 0;
            }
            curWeapon = weapons[index];
            curWeapon.gameObject.SetActive(true);
            _UI.UIChangeWeapon(curWeapon);
        }

    }





}
