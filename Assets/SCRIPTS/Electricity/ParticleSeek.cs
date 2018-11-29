using UnityEngine;

[ExecuteInEditMode]
public class ParticleSeek : MonoBehaviour
{

    public Transform target;
    public float force = 10.0f;

    new ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles;
    private ParticleSystem.MainModule particleSystemMainModule;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = particleSystem.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (particles == null || particles.Length < particleSystemMainModule.maxParticles)
        {
            //particles = new ParticleSystem.Particle[particleSystemMainModule.maxParticles];
        }
        particles = new ParticleSystem.Particle[particleSystemMainModule.maxParticles];

        //ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];

        particleSystem.GetParticles(particles);
        float forceDeltaTime = force * Time.deltaTime;
        Vector3 targetTransformedPosition = target.position;

        if (particleSystemMainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
        {
            targetTransformedPosition = transform.InverseTransformPoint(target.position);
        }

        for (int i = 0; i < particles.Length; i++)
        {
            Vector3 directionToTarget = Vector3.Normalize(targetTransformedPosition - particles[i].position);

            Vector3 seekforce = directionToTarget * forceDeltaTime;

            particles[i].velocity += seekforce;
        }
        particleSystem.SetParticles(particles, particles.Length);
    }
}
