using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Obj : MonoBehaviour
{
    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;

    public void Init(Vector3 pos)
    {
        transform.position = pos;
        Vector3 TargetPos = new Vector3(pos.x + (Random.insideUnitSphere.x * 2.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 2.0f));
        StartCoroutine(SimulateProjectile(TargetPos));
    }

    IEnumerator SimulateProjectile(Vector3 pos)
    {
        // ��ǥ ��ġ������ �Ÿ� ���
        float targetDistance = Vector3.Distance(transform.position, pos);

        // ������ � �ӵ� ���
        float projectileVelocity = Mathf.Sqrt((targetDistance * gravity) / Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad));

        // x�� �ӵ� y�� �ӵ� 
        float vX = projectileVelocity * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vY = projectileVelocity * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // �� ���� �ð�
        float flightDuration = targetDistance / vX;

        // �������� ��ǥ ��ġ �������� ȸ��
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;
        while (time < flightDuration)
        {
            Debug.Log("�߷� * �ð�:" +gravity * time);
            // ������ � ����
            transform.Translate(0, (vY - (gravity * time)) * Time.deltaTime, vX * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
