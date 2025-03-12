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

    protected float Attack_Range = 3.0f; // ���� ����
    protected float Target_Range = 6.0f; // �߰� ����
    protected bool isAttack;
    protected Transform m_Target; // �߰� ����

    // �÷��̾� �� ���� ��ġ (������ ��� ��)
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
