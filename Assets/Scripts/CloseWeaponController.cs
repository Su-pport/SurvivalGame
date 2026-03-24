using UnityEngine;
using System.Collections;

// 미완성 클래스 = 추상 클래스
public abstract class CloseWeponController : MonoBehaviour
{
    // 현재 장착된 Hand형 타입 무기
    //
    [SerializeField] protected CloseWeapon currentHand;
    
    //공격중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo; // 닿은녀석의 정보를 가져와서 그 스크립트에서 체력을 깎거나...

    
    // Update is called once per frame
    void Update()
    {
       TryAttack(); 
    }

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1")) // 좌클릭 총알발사
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());//코루틴 실행
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack"); // 상태변수 Attack 트리거 발동

        yield return new WaitForSeconds(currentHand.attackDelayA); // 팔을 뻗는 시간 대기
        isSwing = true; // 팔 뻗는중

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB); // 팔 접는 시간
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay-currentHand.attackDelayA-currentHand.attackDelayB); // 전체 딜레이에서 팔을 뻗고 접는 시간을 빼고 나머지 시간을 기다려서 전체 딜레이만 대기할 수 있도록
        isAttack = false;
    }

    //abstract 미완성, 추상코루틴 자식클래스가 완성시킴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    //public void HandChange(CloseWeapon _hand)
}
