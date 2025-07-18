using UnityEngine;

public class PlayParticleWhenDestroy : MonoBehaviour {
    public ParticleSystem PrefParticle;

    void OnDisable() {
        ParticleSystem particle = Instantiate(PrefParticle, this.transform.position, PrefParticle.transform.rotation);
        particle.Play();

        float duration = particle.main.duration + particle.main.startLifetime.constantMax;
        Destroy(particle.gameObject, duration);
    }
}
