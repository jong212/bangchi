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

    protected float Attack_Range = 1.0f; // 공격 범위
    protected float Target_Range = 3.0f; // 추격 범위
    protected bool isAttack;
    protected Transform m_Target; // 추격 범위

    [SerializeField] Transform m_BulletTransform;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();        
    }
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
            value.transform.position = m_BulletTransform.position;
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
