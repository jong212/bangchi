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
        // Ÿ���� ���Ͱ� ������ 0 0 0 ���� 'Move'
        // Ÿ���� ���Ͱ� ����  0 0 0 �̸� 'Idle'
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
        // Ÿ���� ���Ͱ� Dead ���¸� ���� ���͸� ���� �� �� �����ϱ� 
        // �ϴ� ���� Ȱ��ȭ �Ǿ��ִ� ���Ͱ� �ִ��� üũ�ϰ� ���ٸ� return 
        // ���� ������ ������ Ÿ���� ���Ͱ� ��� ������ move �ؼ� 0 0 0���� ��
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
