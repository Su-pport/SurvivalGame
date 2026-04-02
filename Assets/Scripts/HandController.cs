using UnityEngine;
using System.Collections;

public class HandController : CloseWeponController
{

    public static bool isActivate = false;

    void Update()
    {
        if(isActivate)
            TryAttack(); 
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public void CloseWeaponChange(CloseWeapon _currentCloseWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentCloseWeapon = _currentCloseWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        isActivate = true;
    }

        // public override void CloseWeaponChange(CloseWeapon _closeWeapon)
        // {
        //     base.CloseWeaponChange(_closeWeapon); // 완성본 먼저 실행
        //     isActivate = true; // 하고 나머지 실행
        // }
    }
