using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_Obj : MonoBehaviour
{
    [SerializeField] Transform itemTextRect;
    [SerializeField] TextMeshProUGUI itemText;


    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;

    bool isCheck = false; 

    void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity;

        itemTextRect.gameObject.SetActive(true);
        itemTextRect.parent = Base_Canvas.instance.HolderLayer(2);

        itemText.text = "Test ITem";
    }

    private void Update()
    {
        if (isCheck == false) return;

        itemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    public void Init(Vector3 pos)
    {
        isCheck = false;
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
            // ������ � ����
            transform.Translate(0, (vY - (gravity * time)) * Time.deltaTime, vX * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}
