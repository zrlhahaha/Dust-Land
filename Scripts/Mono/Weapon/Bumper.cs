using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : WeaponBase {
    public float damege;
    public float cameraShakeIntensityFacotr = 0.1f;
    public Collider bumperCollider;

    public override void OnImpactOtherEntity(Collision collision, Property other)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.thisCollider == bumperCollider)
            {
                float atten = Mathf.Pow( collision.relativeVelocity.magnitude*0.1f,2f);
                other.TakeDamege(atten * damege, owner);

                ShakeData sd = ScriptableObject.Instantiate<ShakeData>(cameraShakingData);
                sd.intensity = atten * cameraShakeIntensityFacotr;
                print(atten * damege + "_" + sd.intensity);

                if (base.cameraShakingData!= null)
                    Player._instance.cameraShake.AddShakeEvent(sd);
                return;
            }
        }
    }

}
