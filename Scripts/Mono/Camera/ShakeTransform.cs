using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTransform : MonoBehaviour {

    [System.Serializable]
    public class ShakeEvent
    {
        public Vector2 offset_x,offset_y,offset_z;
        public float timer;
        public ShakeData data;
        public Vector3 noise;
        public ShakeTarget target { get { return data.target; } }

        public ShakeEvent()
        {
            offset_x = Utility.RandomUV() * 32;
            offset_y = Utility.RandomUV() * 32;
            offset_z = Utility.RandomUV() * 32;
            data = (ShakeData)ScriptableObject.CreateInstance(typeof(ShakeData));
        }

        public ShakeEvent(ShakeTarget target, float intensity, float duration, float sampleDelta)
        {
            offset_x = Utility.RandomUV() * 32;
            offset_y = Utility.RandomUV() * 32;
            offset_z = Utility.RandomUV() * 32;
            data = (ShakeData)ScriptableObject.CreateInstance(typeof(ShakeData));
            data.target = target;
            data.intensity = intensity;
            data.duration = duration;
            data.sampleDelta = sampleDelta;
        }

        public ShakeEvent(ShakeData shakeData)
        {
            offset_x = Utility.RandomUV() * 32;
            offset_y = Utility.RandomUV() * 32;
            offset_z = Utility.RandomUV() * 32;
            data = shakeData;
        }

        public void Update()
        {
            timer += Time.deltaTime;
            float delta = timer * data.sampleDelta;
            Vector3 col = new Vector3();
            col.x = Mathf.PerlinNoise(offset_x.x + delta, offset_x.y);
            col.y = Mathf.PerlinNoise(offset_y.x + delta, offset_y.y);
            col.z = Mathf.PerlinNoise(offset_z.x + delta, offset_z.y);

            float atten = data.attenuation.Evaluate(timer / data.duration);
            noise = (col * 2 - Vector3.one) * data.intensity * atten;
        }

        public bool IsExpired()
        {
            return timer > data.duration;
        }
    }

    public List<ShakeEvent> events = new List<ShakeEvent>();

    private void Update()
    {
        Vector3 position = new Vector3();
        Vector3 rotation = new Vector3();
        for (int i = events.Count - 1; i>=0 ; i--)
        {
            ShakeEvent shakeEvent = events[i];
            shakeEvent.Update();

            if (shakeEvent.target == ShakeTarget.Position)
                position += shakeEvent.noise;
            else if (shakeEvent.target == ShakeTarget.Rotation)
                rotation += shakeEvent.noise;

            if (shakeEvent.IsExpired())
                events.RemoveAt(i);
        }

        transform.localPosition = position;
        transform.localEulerAngles = rotation;
    }

    public void AddShakeEvent()
    {
        events.Add(new ShakeEvent());
    }

    public void AddShakeEvent(ShakeData shakeData)
    {
        events.Add(new ShakeEvent(shakeData));
    }

}
