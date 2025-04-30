using UnityEngine;

public class EnemyParticleController : ParticleController
{
    public TrailRenderer DeadTrail;
    public EnemyMoving owner;
    protected new float originalSpeed = 5f;

    public void Init(EnemyMoving owner)
    {
        this.owner = owner;
        PlayTrail();
        DeadTrail.emitting = false;
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

    public override void OnDead()
    {
        PauseTrail();
        DeadTrail.emitting = true;
    }
}
