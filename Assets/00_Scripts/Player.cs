using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Vector3 startPos;
    Quaternion rot;
    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        rot = transform.rotation;
    }
/*    protected void FindClosetTarget<T>(T[] targets) where T : Component*/

    private void Update()
    {
        // 타겟할 몬스터가 없으면 0 0 0 으로 'Move'
        // 타겟할 몬스터가 없고  0 0 0 이면 'Idle'
        if(m_Target == null)
        {
            FindClosetTarget(Spawner.m_Monster);

            float targetPos = Vector3.Distance(transform.position, startPos);
            if(targetPos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position,startPos,Time.deltaTime);
                transform.LookAt(startPos);
                AnimationChange("isMove");
            } else
            {
                transform.rotation = rot;
                AnimationChange("isIdle");
            }

            return;
        }
        // 타겟할 몬스터가 Dead 상태면 죽은 몬스터를 향해 갈 순 없으니까 
        // 일단 현재 활성화 되어있는 몬스터가 있는지 체크하고 없다면 return 
        // 리턴 이유는 어차피 타겟할 몬스터가 없어서 위에서 move 해서 0 0 0가면 됨
        if (m_Target.GetComponent<Character>().isDead) FindClosetTarget(Spawner.m_Monster);
        if (m_Target == null) return;
            float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        if(targetDistance <= Target_Range && targetDistance > Attack_Range && isAttack == false)
        {
            AnimationChange("isMove");
            transform.LookAt(m_Target);
            transform.position = Vector3.MoveTowards(transform.position,m_Target.position,Time.deltaTime);
        } else if (targetDistance <= Attack_Range && isAttack == false)
        {
            isAttack = true;
            transform.LookAt(m_Target);
            AnimationChange("isAttack");
            Invoke("InitAttack", 1.0f);
        }

    }
}
