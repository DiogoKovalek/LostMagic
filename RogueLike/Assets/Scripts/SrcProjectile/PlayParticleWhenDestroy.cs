using UnityEngine;

public class PlayParticleWhenDestroy : MonoBehaviour {
    public ParticleSystem PrefParticle;
    private bool isQuitting = false;

    void OnApplicationQuit() {
        isQuitting = true;
    }
    void OnDestroy() {
        if (isQuitting) return;
        ParticleSystem particle = Instantiate(PrefParticle, this.transform.position, PrefParticle.transform.rotation);
        particle.Play();

        float duration = particle.main.duration + particle.main.startLifetime.constantMax;
        Destroy(particle.gameObject, duration);
    }
}
