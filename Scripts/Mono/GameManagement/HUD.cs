using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [Header("Perception Indicator")]
    public List<PerceptionIndicator> perceptionIndicator= new List<PerceptionIndicator>();
    public PerceptionIndicator perceptionIndicatorPrefab;
    public Transform perceptionIndicatorContainer;
    public Color grayStateColor;
    public Color RedStateColor;

    [Header("Hit Sign")]
    public Image hitSign;
    public float hitSignAlphaDropSpeed = 1f;
    public float hitSignScaleFactor = 1f;
    public float hitSignScalePowFactor = 1f;

    public static HUD _instance;

    private void Awake()
    {
        if (_instance != null)
            Debug.LogWarning("Another HUD instance exist");

        _instance = this;
    }

    private void Update()
    {
        UpdatePerceptionIndicator();
        UpdateHitSign();

    }

    void UpdatePerceptionIndicator()
    {
        List<PerceptionSystem> perceptions = Battle._instance.botsPerception;

        int offset = perceptionIndicator.Count - perceptions.Count;

        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                int index = perceptionIndicator.Count - 1;
                perceptionIndicator[index].ReturnPool();
                perceptionIndicator.RemoveAt(index);
            }
        }
        else if (offset < 0)
        {
            offset = -offset;
            for (int i = 0; i < offset; i++)
            {
                PerceptionIndicator pi = ResourceManager._instance.GetPoolMember(perceptionIndicatorPrefab);
                pi.transform.SetParent(perceptionIndicatorContainer);
                pi.transform.Reset();
                perceptionIndicator.Add(pi);
            }
        }

        Transform mainCamera = Player._instance.mainCamera.transform;
        for (int i = 0; i < perceptions.Count; i++)
        {
            Color col = perceptions[i].alertState == AlertState.Grey ? grayStateColor : RedStateColor;
            float amount = perceptions[i].alertState == AlertState.Grey ? perceptions[i].alertness : 1;
            perceptionIndicator[i].SetTarget(perceptions[i].transform.position, mainCamera, amount,col);
        }
    }

    void UpdateHitSign()
    {
        Color col = hitSign.color;
        col.a = Mathf.Clamp01(col.a - hitSignAlphaDropSpeed * Time.deltaTime);
        hitSign.color = col;
    }

    public void ShowHitSign(float damege)
    {
        hitSign.transform.localScale = Vector3.one *  Mathf.Pow(damege * hitSignScaleFactor * 0.01f,hitSignScalePowFactor);
        Color col = hitSign.color;
        col.a = 1;
        hitSign.color = col;
    }

}
