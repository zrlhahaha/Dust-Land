
using UnityEngine;

public class CameraShaking : MonoBehaviour {

    //public float currentRecoilAmount;
    //public float targetRecoilAmount;
    //public float damp;
    public CameraShakingDate data;
    public float additionRot_X;
    public float additionRot_Y;
    public float additionPos_Z;
    public float originalPos_Z;
    public float duration;
    public Vector2 randomTarget;
    public float randomTargetSmooth = 0.2f;
    public int randomPosIndex;
    public Vector2[] randomPos;
    
    float timer;
    float divisionOfDuration;
    public Transform target;

    private void Start()
    {
        
        originalPos_Z = target.localPosition.z;
    }


    void FixedUpdate()
    {

        if (data == null)
        {
            return;
        }

        timer += Time.deltaTime;


        if (randomPosIndex >= randomPos.Length)
            return;

        float normalizedTime = timer / duration;
        randomTarget = Vector3.Slerp(randomTarget, randomPos[randomPosIndex], randomTargetSmooth);
        float intensity = data.positionAnimation.Evaluate(normalizedTime);

        target.localRotation = Quaternion.Euler(new Vector3
            (intensity * randomTarget.y, intensity * randomTarget.x, 0));
        target.localPosition = (originalPos_Z + intensity * data.maxPosition) * Vector3.forward;

        if (timer > divisionOfDuration * (randomPosIndex + 1))
            randomPosIndex++;

    }

    public void SetShakingDate(CameraShakingDate newDate,float duration)
    {

        if (newDate == null)
            return;

        data = newDate;

        this.duration = duration;

        divisionOfDuration = duration / data.randomTimes;
        randomPos = new Vector2[data.randomTimes];

    }

    public void Shake()
    {
        if (data == null)
        {
            Debug.LogWarning("cameraShakingData is null");
            return;
        }

        timer = 0;
        randomPosIndex = 0;
        for (int i = 0; i < randomPos.Length; i++)
        {
            //randomPos[i] =  UnityEngine.Random.insideUnitCircle.normalized*data.maxRotation;
        }

    }

    public void CleanShakingDate()
    {
        data = null;
    }

    


}
