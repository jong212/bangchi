using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    Animator animator;

    public double HP;
    public double ATK;
    public float ATK_Speed;
    public bool isDead;

    protected float Attack_Range = 3.0f; // 공격 범위
    protected float Target_Range = 6.0f; // 추격 범위
    protected bool isAttack;
    protected Transform m_Target; // 추격 범위

    // 플레이어 블릿 생성 위치 (지팡이 헤드 쪽)
    [SerializeField] Transform m_BulletTransform;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();        
    }
    // Invoke 
    protected void InitAttack() => isAttack = false;

    protected void AnimationChange(string animName)
    {
        if (animName == "isAttack")
        {
            animator.SetTrigger("isAttack");
            return;
        }

        animator.SetBool("isIdle", false);
        animator.SetBool("isMove", false);

        animator.SetBool(animName, true);
    }

    protected virtual void Bullet()
    {
       Base_Mng.Pool.Pooling_Obj("Bullet").Get((value) =>
        {
            if (m_Target == null) { return; } // 이거 오류나서 임시 방편 용으로 null인경우 return 시킴 총알은 있는데 몬스터가 죽은 경우임 추후 처리 예정 Memo
            value.transform.position = m_BulletTransform.position;
            value.GetComponent<Bullet>().Init(m_Target,10,"CH_01");
        });
    }
    protected void FindClosetTarget(List<Monster> targets)
    {
        var monsters = targets;
        Transform closetTarget = null;
        float maxDistance = Target_Range;

        foreach (var monster in monsters)
        {
            float targetDistance = Vector3.Distance(transform.position,monster.transform.position);
            if(targetDistance < maxDistance)
            {
                closetTarget = monster.transform;
                maxDistance = targetDistance;
            }
        }
        m_Target = closetTarget;
        if(m_Target != null) { transform.LookAt(m_Target); }
    }
}
