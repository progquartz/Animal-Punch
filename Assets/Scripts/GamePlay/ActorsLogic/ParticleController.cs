using UnityEngine;

public abstract class ParticleController : MonoBehaviour
{
    [SerializeField] protected ParticleSystem[] trailParticles;
    protected float originalSpeed = 10f;

    protected abstract void CheckTrailParticles();

    protected void PauseTrail()
    {
        foreach (var particle in trailParticles)
        {
            particle.Pause();
        }
    }

    protected void PlayTrail()
    {
        foreach (var particle in trailParticles)
        {
            particle.Play();
        }
    }

    public abstract void OnDead();

    public void OnTimeToggle(bool isTimeStop)
    {
        if (isTimeStop)
        {
            PauseTrail();
        }
        else
        {
            PlayTrail();
        }
    }
}
