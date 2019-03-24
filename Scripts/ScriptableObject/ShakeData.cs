using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakeTarget
{
    Position = 0,
    Rotation = 1,
    Both = 3,
}

[CreateAssetMenu(fileName = "New ShakeData",menuName = "ScriptableObject/ShakeDate")]
public class ShakeData : ScriptableObject {

    public ShakeTarget target = ShakeTarget.Position;
    public float intensity = 1f;
    public float sampleDelta = 1f;
    public float duration = 1f;
    public AnimationCurve attenuation = new AnimationCurve
        (
            new Keyframe(0.0f, 0.0f,Mathf.Tan(Mathf.Deg2Rad * 0), Mathf.Tan(Mathf.Deg2Rad * 85)), 
            new Keyframe(0.2f, 1.0f, Mathf.Tan(Mathf.Deg2Rad * 0), Mathf.Tan(Mathf.Deg2Rad * 0)),
            new Keyframe(1.0f,0.0f)
        );

}
