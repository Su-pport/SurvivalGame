using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{

    // 필요한 컴포넌트
    [SerializeField] private GunController theGunController;
    private Gun currentGun;

    // 필요하면 HUD 호출, 비활성화 등
    [SerializeField] private GameObject goBulletHUD;

    // 총알 개수 텍스트에 반영
    [SerializeField] private TextMeshProUGUI[] textBulllet;



    // Update is called once per frame
    void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        textBulllet[0].text = currentGun.carryBulletCount.ToString();
        textBulllet[1].text = currentGun.reloadBulletCount.ToString();
        textBulllet[2].text = currentGun.currentBulletCount.ToString();
    }
}
