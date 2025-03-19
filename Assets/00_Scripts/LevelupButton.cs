using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelupButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] Image expSlider;
    [SerializeField] TextMeshProUGUI expText,atkText,goldText,hpText,getExpText;
    [SerializeField] DOTweenAnimation dotween;
    bool isPush = false;
    float timer = 0.0f;
    Coroutine _coroutine;
    private void Update()
    {
        if (isPush)
        {
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                dotween.DORestart();
                Debug.Log("연속터치");
            }
        }
    } 

   /* void ExpUp()
    {
        transform.DORewind();
        transform.DOPunchScale(new Vector3(.2f, .2f), 0.25f);
    }*/
    public void OnPointerDown(PointerEventData eventData)
    {
        /*ExpUp();*/

        dotween.DORestart();
        _coroutine = StartCoroutine(PushCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("스탑터치");
        isPush = false;
        if(_coroutine != null)
        {
            Debug.Log("1초 이내로 손 뗌"); 
            StopCoroutine(_coroutine);
        }
        timer = 0.0f;
    }
    IEnumerator PushCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}
