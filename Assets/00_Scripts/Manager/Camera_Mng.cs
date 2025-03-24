using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Mng : MonoBehaviour
{
    float _Distance = 4.0f;

    [Range(0.0f, 10.0f)]
    [SerializeField] float AddDistance_Value;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, Distance() , Time.deltaTime * 2.0f);
    }

    float Distance()
    {
        Player[] playersArr  = Spawner.m_Players.ToArray();
        float maxDistance = _Distance;

        foreach(Player player in playersArr)
        {
            float tDistance = Vector3.Distance(Vector3.zero, player.transform.position) + AddDistance_Value;
            if(tDistance > maxDistance)
            {
                maxDistance = tDistance;
            }
        }

        return maxDistance;
    }
}
