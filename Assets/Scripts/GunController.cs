using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = false;

    // 현재 장착된 총
    [SerializeField] private Gun currentGun;

    //연사 속도 계산
    private float currentFireRate;

    //효과음 재생
    private AudioSource audioSource;

    //상태변수
    private bool isReload = false;

    // 원래 priavte이었는데 갑자기 강의에선 퍼블릭
    [HideInInspector] public bool isFineSightMode = false; // false는 기본 true는 정조준


    // 총의 원래 위치
    private Vector3 originPos;
    
    // 레이저 충돌 정보 받아옴
    private RaycastHit hitinfo;

    
    [SerializeField] private Camera theCam;
    private Crosshair theCrosshair;
    [SerializeField] private PlayerController thePlayerController;

    // 피격 이펙트
    [SerializeField] private GameObject hitEffectPrefab;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        theCrosshair = FindFirstObjectByType<Crosshair>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    // 연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초의 역수
        }
    }

    // 발사 시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && isReload == false && !thePlayerController.GetIsRun())
        {
            Fire();
        }
    }

    // 발사 전 계산
    private void Fire()
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    // 발사 후 계산
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 연사 속도 재계산
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Hit();

        // 반동
        StopAllCoroutines();
        StartCoroutine(RetroActioCoroutine());

    }

    private void Hit()
    {
        if(Physics.Raycast(theCam.transform.position
            , theCam.transform.forward + new Vector3(
                    UnityEngine.Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy)
                    , UnityEngine.Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy)
                    , 0)
            , out hitinfo, currentGun.range))
        {
            //맞은 위치의 포면이 바란보는 방향: hitinfo.Normal
            GameObject clone = Instantiate(hitEffectPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(clone, 2f);
        }
    }

    // 사운드 재생
    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    // 재장전 시도
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    // 재장전 취소
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

    // 재장전
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("No more bullets to reload!");
        }

    }

    // 정조준 시도
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    // 정조준 취소
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }

    // 정조준 로직 가동
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeActivateCoroutine());
        }
    }

    // 정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
       
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
            yield return null;
        }
    }

    // 정조준 비활성화
    IEnumerator FineSightDeActivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    // 반동 코루틴
    IEnumerator RetroActioCoroutine()
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);
        Vector3 retroActioRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y, currentGun.retroActionFineSightForce);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            // 반동시작
            while (currentGun.transform.localPosition.z > currentGun.retroActionForce+0.02f) //오차범위
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }

        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
            // 반동시작
            while (currentGun.transform.localPosition.z > currentGun.retroActionFineSightForce+0.02f) //오차범위
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActioRecoilBack, 0.4f);
                yield return null;
            }

            // 원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
    
    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
