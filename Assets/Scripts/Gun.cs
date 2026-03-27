using JetBrains.Annotations;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public string gunName; // 총의 이름
    public float range; // 총의 사거리
    public float accuracy; // 총의 정확도
    public float fireRate; // 총의 발사 속도
    public float reloadTime; // 총의 재장전 시간

    public int damage; // 총의 데미지

    public int reloadBulletCount; // 총의 재장전 탄약 수
    public int currentBulletCount; // 현재 총의 탄약 수
    public int maxBulletcount; // 총의 최대 탄약 수
    public int carryBulletCount; // 현재 소유하고있는 총알 개수

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준 반동 세기

    public Vector3 fineSightOriginPos;

    public Animator anim;

    public ParticleSystem muzzleFlash; // 총구 화염 효과

    public AudioClip fireSound;

}
