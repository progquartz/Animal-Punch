using UnityEngine;

public class PlayerParticleController : ParticleController
{
    protected float originalSpeed = 10f;
    [Header("레벨업")]
    public ParticleSystem[] levelUpParticles;
    [Header("부스터")]
    public TrailRenderer BoostParticle;
    [Header("피버모드")]
    public ParticleSystem[] feverParticles;
    

    private bool isBoostParticlesActivated = false;
    private float currentBoostTime = 0f;
    private float boostTime = 0.4f;

    private bool prevFeverState = false;

    void Update()
    {
        if(GameManager.Instance.IsGamePaused) return;
        CheckTrailParticles();
        CheckBoostParticles();
        CheckFeverParticles();
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

    private void CheckFeverParticles()
    {
        if(prevFeverState != Player.Instance.feverHandler.isFever)
        {
            prevFeverState = Player.Instance.feverHandler.isFever;

            foreach(var particle in feverParticles)
            {
                if(prevFeverState)
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
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
