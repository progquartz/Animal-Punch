using UnityEngine;

public class PlayerParticleController : ParticleController
{
    protected new float originalSpeed = 10f;
    public ParticleSystem[] levelUpParticles;

    void Update()
    {
        if(GameManager.Instance.IsTimeStop) return;
        CheckTrailParticles();

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


}
