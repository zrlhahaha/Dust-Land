using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerceptionIndicator : PoolMember {

    public float amount { get; private set; }

    public Image indicator_a;
    public Image indicator_b;

    public void SetTarget(Vector3 pos_world,Transform camera, float amount,Color color)
    {
        Vector3 pos_camera = camera.InverseTransformPoint(pos_world);
        pos_camera.y = 0;
        float angle = -Vector3.Angle(pos_camera, Vector3.forward) * Mathf.Sign(pos_camera.x);

        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        this.amount = amount;
        indicator_a.fillAmount = indicator_b.fillAmount = amount * 0.5f;
        indicator_a.color = indicator_b.color = color;
    }

}
