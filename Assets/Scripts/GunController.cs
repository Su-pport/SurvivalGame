using System;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField] private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초의 역수
        }
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate; // 총의 발사 속도로 발사 속도 설정
        Shoot();
    }

    private void Shoot()
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
        Debug.Log("Bang!");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
