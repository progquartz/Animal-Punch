using UnityEngine;

public class PlayerParticleController : ParticleController
{
    protected float originalSpeed = 10f;
    public ParticleSystem[] levelUpParticles;
    public TrailRenderer BoostParticle;
    private bool isBoostParticlesActivated = false;
    private float currentBoostTime = 0f;
    private float boostTime = 0.4f;

    void Update()
    {
        if(GameManager.Instance.IsGamePaused) return;
        CheckTrailParticles();
        CheckBoostParticles();

    }

    protected override void CheckTrailParticles()
    {
        float playerSpeed = Player.Instance.Stat.RigidbodySpeed;
        foreach (var particle in trailParticles)
        {
            var main = particle.main;
            main.startLifetimeMultiplier = (playerSpeed / originalSpeed);
        }
    }

    private void CheckBoostParticles()
    {
        if(isBoostParticlesActivated)
        {
            currentBoostTime -= Time.deltaTime;
            if(currentBoostTime < 0f )
            {
                isBoostParticlesActivated = false;
                BoostParticle.emitting = false;
            }
        }
    }

    public override void OnDead()
    {
        throw new System.NotImplementedException();
    }

    public void OnLevelUp()
    {
        foreach (var particle in levelUpParticles)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
                break;
            }
        }
    }

    public void OnBoost()
    {
        if(!isBoostParticlesActivated)
        {
            isBoostParticlesActivated = true;
        }

        currentBoostTime = boostTime;
        BoostParticle.emitting = true;
    }


}
