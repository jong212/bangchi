using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_Obj : MonoBehaviour
{
    [SerializeField] Transform itemTextRect;
    [SerializeField] TextMeshProUGUI itemText;

    [SerializeField] GameObject[] raritysArr;
    [SerializeField] ParticleSystem lootParticle;
    

    [SerializeField] float firingAngle = 45.0f;
    [SerializeField] float gravity = 9.8f;
    Rarity rarity;


    bool isCheck = false; 

    void RarityCheck()
    {
        isCheck = true;

        transform.rotation = Quaternion.identity;

        raritysArr[(int)rarity].SetActive(true);

        itemTextRect.gameObject.SetActive(true);
        itemTextRect.parent = Base_Canvas.instance.HolderLayer(2);

        itemText.text = Utils.String_color_Rarity(rarity) + "Test" + "</color>";
        StartCoroutine(LootItem());
    }
    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        for (int i = 0; i < raritysArr.Length; i++) { raritysArr[i].SetActive(false); }

        itemTextRect.transform.parent = this.transform;
        itemTextRect.gameObject.SetActive(false);

        lootParticle.gameObject.SetActive(true);
        lootParticle.Play();
        yield return new WaitForSeconds(.5f);

        lootParticle.gameObject.SetActive(false);
        Base_Mng.Pool.m_pool_Dictionary["Item_Obj"].Return(this.gameObject);
    }
        

    private void Update()
    {
        if (lootParticle == null)
        {
            Debug.Log("null");
        }
        else
        {
            Debug.Log("Not null");
        }
        if (isCheck == false) return;

        itemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }
    public void Init(Vector3 pos)
    {

        
        rarity = (Rarity)Random.Range(0,3); // 캐스팅
        isCheck = false;
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
            // 포물선 운동 적용
            transform.Translate(0, (vY - (gravity * time)) * Time.deltaTime, vX * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}
