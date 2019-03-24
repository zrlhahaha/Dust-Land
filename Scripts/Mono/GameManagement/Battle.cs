using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public Vector3 positon;
    public float expireTime;
    public float range;

    public Sound(Vector3 pos,float range, float expireTime)
    {
        positon = pos;
        this.range = range;
        this.expireTime = expireTime;
    }
}



public class Battle : MonoBehaviour {

    public static Battle _instance;

    public List<Sound> sounds = new List<Sound>();
    public List<PerceptionSystem> botsPerception = new List<PerceptionSystem>();
    private void Awake()
    {
        if (_instance != null)
            Debug.LogWarning("Another Battle instance exist");

        _instance = this;
    }

    private void Update()
    {
        for (int i = sounds.Count - 1; i >=0; i--)
            if (sounds[i].expireTime < Time.time)
                sounds.RemoveAt(i);

    }

    public void AddSound(Vector3 pos)
    {
        sounds.Add(new Sound(pos,100, Time.time + 4));
    }


    public bool GetNearestSound(Vector3 pos, out Vector3 nearestSound)
    {
        if (sounds.Count == 0)
        {
            nearestSound = Vector3.zero;
            return false;
        }

        nearestSound = new Vector3();
        bool dirty = false;
        float nearestSqrDist = float.MaxValue;
        for (int i = 0; i < sounds.Count; i++)
        {
            float sqrDist = (sounds[i].positon - pos).sqrMagnitude;
            if (sqrDist > sounds[i].range * sounds[i].range)
                continue;

            if (sqrDist < nearestSqrDist)
            {
                dirty = true;
                nearestSound = sounds[i].positon;
                nearestSqrDist = sqrDist;
            }
        }

        return dirty;
    }

    public void BotDestroyed(PerceptionSystem ps)
    {
        botsPerception.Remove(ps);
    }
}
