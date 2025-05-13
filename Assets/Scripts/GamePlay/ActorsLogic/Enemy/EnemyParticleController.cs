using UnityEngine;

public class EnemyParticleController : ParticleController
{
    public TrailRenderer DeadTrail;
    public ParticleSystem[] HitParticles;
    public ParticleSystem[] CriticalHitParticles;

    public GameObject TwoLegTrail;
    public GameObject FourLegTrail;

    public EnemyMoving owner;
    protected float originalSpeed = 5f;


    public void Init(EnemyMoving owner)
    {
        this.owner = owner;
        CheckModelType();
        PlayTrail();

        DeadTrail.emitting = false;
    }
    
    private void CheckModelType()
    {
        switch(owner.targetEnemyDataSO.ModelType)
        {
            case EnemyModelType.FOURLEGS:
                TwoLegTrail.SetActive(false);
                FourLegTrail.SetActive(true);
                break;
            case EnemyModelType.TWOLEGS:
                TwoLegTrail.SetActive(true);
                FourLegTrail.SetActive(false);
                break;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsTimeStop) return;
        CheckTrailParticles();
    }

    protected override void CheckTrailParticles()
    {
        float enemySpeed = owner.EnemyRB.linearVelocity.magnitude;
        foreach (var particle in trailParticles)
        {
            var main = particle.main;
            main.startLifetimeMultiplier = (enemySpeed / originalSpeed);
        }
    }

    public void OnHit()
    {
        foreach(var particle in HitParticles)
        {
            particle.Pause();
            particle.Play();
        }
    }

    public void OnCriticalHit()
    {
        foreach(var particle in CriticalHitParticles)
        {
            particle.Pause();
            particle.Play();
        }
    }
    

    public override void OnDead()
    {
        PauseTrail();
        DeadTrail.emitting = true;
    }
}
