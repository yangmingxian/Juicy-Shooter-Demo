using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using TMPro;
using Animancer;
using DG.Tweening;
public class UIController : MonoBehaviour
{
    public TMP_Text weaponNameText;
    public SpriteRenderer weaponSpriteRenderer;
    public Slider ammoSlider;
    public TMP_Text ammoText;
    [SerializeField] MMF_Player ammoConsumeFeedback;
    public AnimancerComponent _animancer;


    public void UIChangeWeapon(Weapon weapon)
    {
        weaponNameText.text = weapon.name;
        // weaponSpriteRenderer.sprite = weapon.weaponRenderer.sprite;
        PlayIdle(weapon);
        ammoSlider.value = (float)weapon.ammo / weapon.magazineSize;
        ammoText.text = $"{weapon.ammo}/{weapon.magazineSize}";
        UpdaeWeaponAmmoUI(weapon);
    }

    public void UIWeaponShoot(Weapon weapon)
    {
        _animancer.Play(weapon.shootClip);
        var state = _animancer.Play(weapon.shootClip);

        _animancer.Play(weapon.shootClip).Time = 0;
        state.Events.OnEnd = () => PlayIdle(weapon);

        UpdaeWeaponAmmoUI(weapon);
    }

    void PlayIdle(Weapon weapon)
    {
        _animancer.Play(weapon.idleClip);
        var state = _animancer.Play(weapon.idleClip);
        state.Events.OnEnd = () => state.IsPlaying = false;
    }

    public void UpdaeWeaponAmmoUI(Weapon weapon)
    {
        ammoConsumeFeedback.PlayFeedbacks();
        ammoSlider.value = (float)weapon.ammo / weapon.magazineSize;
        ammoText.text = $"{weapon.ammo}/{weapon.magazineSize}";
    }

    public void UIWeaponReload(Weapon weapon)
    {

        ammoSlider.value = 0;
        // ammoText.text = $"0/{weapon.magazineSize}";

        ammoSlider.DOValue(1, weapon.reloadDuration).SetEase(Ease.Linear).SetId("reload");
    }





}
