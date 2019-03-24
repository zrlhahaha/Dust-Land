using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitFX {

    HitEffect GetHitFXType();

}

public class Obstacle : MonoBehaviour, IHitFX
{
    public HitEffect hitFxType;
    public HitEffect GetHitFXType()
    {
        return hitFxType;
    }
}
