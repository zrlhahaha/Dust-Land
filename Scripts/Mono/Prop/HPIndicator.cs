using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Property))]
public class HPIndicator : MonoBehaviour {

    public Property property;
    public GameObject yellowIndicator;
    public GameObject redIndicator;

    [Range(0,1)]
    public float yellow = 0.6f;
    [Range(0,1)]
    public float red = 0.25f;
    [Range(1,5)]
    public float maxSize = 3;
    float yellowHP,redHP;
    float smoothSize_red;
    float smoothSize_yellow;
    
    
    private void Awake()
    {
        yellowHP  = yellow * property.maxHp;
        redHP = red * property.maxHp;
    }

    private void Update()
    {
        float hp = property.hp;
        if (hp < redHP)
        {
            if(redIndicator!=null)
                redIndicator.SetActive(true);

            if (hp < yellowHP)
                yellowIndicator.SetActive(true);
        }
        else if (hp < yellowHP)
        {
            if (redIndicator != null)
                redIndicator.SetActive(false);

            if (yellowIndicator != null)
                yellowIndicator.SetActive(true);
        }
        else
        {
            if (redIndicator != null)
                redIndicator.SetActive(false);

            if (yellowIndicator != null)
                yellowIndicator.SetActive(false);
        }

        float size_red = (Mathf.InverseLerp(redHP, 0, hp) * (maxSize - 1) + 1);
        float size_yellow = (Mathf.InverseLerp(yellowHP, 0, hp) * (maxSize - 1) + 1);
        smoothSize_red = Mathf.Lerp(smoothSize_red, size_red, 1 * Time.deltaTime);
        smoothSize_yellow = Mathf.Lerp(smoothSize_yellow, size_yellow, 1 * Time.deltaTime);

        if (redIndicator!=null)
            redIndicator.transform.localScale =  smoothSize_red * Vector3.one;

        if(yellowIndicator!=null)
            yellowIndicator.transform.localScale =  smoothSize_yellow * Vector3.one;
    }

}
