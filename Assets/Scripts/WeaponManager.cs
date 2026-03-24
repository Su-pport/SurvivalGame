// using UnityEngine;


// public class WeaponManager : MonoBehaviour
// {
//     // 무기 중복 교체 실햏 방지
//     public static bool isChangeWeapon = false; // 공유자원. 클래스 변수 = 정적변수 단, 인스턴스가 파괴되도 유지되기 때문에 메모리가 낭비될 수 있음
//     // 현재 무기와 현재 무기의 애니메이션
//     public static Transform currentWeapon;
//     public static Animator currentWeaponAnim;

//     // 현재 무기의 타입
//     [SerializeField] private string currentWeaponType;

//     // 무기 교체 딜레이
//     [SerializeField] private float changeWeaponDelayTime;
//     // 무기 교체가 완전히 끝난 시점
//     [SerializeField] private float changeWeaponEndDelayTime;

//     // 무기 종류 전부 관리
//     [SerializeField] private Gun[] guns;
//     [SerializeField] private Hand[] hands;

//     // 관리 차원에서 쉽게 무기접근이 가능하도록 만듦
//     private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
//     private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

   
//     // 필요한 컴포넌트
//     [SerializeField] private GunController theGunController;
//     [SerializeField] private HandController theHandController;


//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         for(int i = 0; i < guns.Length; i++)
//         {
//             gunDictionary.Add(guns[i].gunName, guns[i]);
//         }
//         for (int i = 0; i < hands.Length; i++)
//         {
//             gunDictionary.Add(hands[i].handName, hands[i]);
//         }
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (!isChangeWeapon)
//         {
//             if (Input.GetKeyDown(KeyCode.Alpha1))
//             {
//                 // 무기 교체 실행
//             }
//             else if (Input.GetKeyDown(KeyCode.Alpha2))
//             {
                
//             }
//         }
//     }

//     public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
//     {
//         isChangeWeapon = true;
//         currentWeaponAnim.SetTrigger("Weapo_Out");

//         yield return new WaitForSeconds(changeWeaponDelayTime);

//         CancelPreWeaponAction();
//     }

//     private void CancelPreWeaponAction()
//     {
//         switch (currentWeaponType)
//         {
//             case "GUN":
//                 theGunController.CancelFindSight();
//                 break;
//             case "HAND":
//                 break;
//         }
//     }
// }
