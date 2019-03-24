using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public enum Team
{
    TeamA = 9,
    TeamB = 10,
    TeamC = 11,
}

public class Property : MonoBehaviour ,IHitFX{


    public float maxHp;
    public float hp;
    public HitEffect hitEffect = HitEffect.Defualt;
    public FXContainer deathFX;
    public Team team = Team.TeamA;

    [Header("Dependence")]
    public Collider TeamTagCollider;
    public Rigidbody rootRigid;
    public PerceptionSystem perception;
    private void Start()
    {
        hp = maxHp;
        SetTeam(team);
    }

    public void TakeDamege(float damege,Property source)
    {
        if (source.team == team)
            return;

        hp -= damege;

        if (perception != null)
            perception.Msg_TakeDamege(source);

        if (source == Player._instance.playerProperty)
            HUD._instance.ShowHitSign(damege);

        if (hp <= 0)
            Destroy();
    }

    public void Destroy()
    {
        deathFX.Play(transform.position, transform.rotation);

        DetachOnDestroy[] detachables = GetComponentsInChildren<DetachOnDestroy>();
        foreach (var go in detachables)
        {
            Detachable detachable = go.Detach();

            if (rootRigid != null)
                foreach (var rigid in detachable.rigids)
                {
                    rigid.velocity = rootRigid.velocity + Vector3.up * 10;
                    rigid.angularVelocity = rootRigid.angularVelocity;
                }
        }

        Destroy(gameObject);
    }

    public HitEffect GetHitFXType()
    {
        return hitEffect;
        
    }

    public void SetTeam(Team newTeamTag)
    {
        team = newTeamTag;
        TeamTagCollider.gameObject.layer = (int)newTeamTag;
    }
}
