using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPara : MonoBehaviour {

    public static int Fire;
    public static int Alertness;
    public static int EndSerchEnemy;

    private void Awake()
    {
        Fire = Animator.StringToHash("Fire");
        Alertness = Animator.StringToHash("Alertness");
        EndSerchEnemy = Animator.StringToHash("EndSerchEnemy");
    }

}
