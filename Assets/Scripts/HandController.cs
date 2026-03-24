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

    // public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    // {
    //     base.CloseWeaponChange(_closeWeapon); // 완성본 먼저 실행
    //     isActivate = true; // 하고 나머지 실행
    // }
}
