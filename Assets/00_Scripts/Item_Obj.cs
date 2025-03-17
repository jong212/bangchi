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
        // 목표 위치까지의 거리 계산
        float targetDistance = Vector3.Distance(transform.position, pos);

        // 포물선 운동 속도 계산
        float projectileVelocity = Mathf.Sqrt((targetDistance * gravity) / Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad));

        // x축 속도 y축 속도 
        float vX = projectileVelocity * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float vY = projectileVelocity * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // 총 비행 시간
        float flightDuration = targetDistance / vX;

        // 아이템을 목표 위치 방향으로 회전
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        float time = 0.0f;
        while (time < flightDuration)
        {
            Debug.Log("중력 * 시간:" +gravity * time);
            // 포물선 운동 적용
            transform.Translate(0, (vY - (gravity * time)) * Time.deltaTime, vX * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
