using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ImpulseTimer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] public Color couleur1 = Color.white;
    [SerializeField] public Color couleur2 = Color.red;

    [SerializeField] private AnimationCurve ColorlerpCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve NumberOfParticleCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public int burstCycleCount = 1;
    [SerializeField] private int minParticles = 25;
    [SerializeField] private int maxParticles = 75;

    public float repulserTime = 5f;
    private float repulserTimer;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        repulserTimer = 0f;
    }

    private void Update()
    {
        repulserTimer += Time.deltaTime;
        if (repulserTimer > repulserTime)
        {
            repulserTimer = 0f;
        }
        float t = repulserTimer / repulserTime;
        float curvedT = ColorlerpCurve.Evaluate(t);
        
        Color currentColor = Color.Lerp(couleur1, couleur2, curvedT);
        float particleCurveValue = NumberOfParticleCurve.Evaluate(t);
        int particleCount = Mathf.RoundToInt(Mathf.Lerp(minParticles, maxParticles, particleCurveValue));

        var main = particleSystem.main;
        main.startColor = currentColor;

        var emission = particleSystem.emission;
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[1];
        bursts[0] = new ParticleSystem.Burst(0f, (short)particleCount, (short)particleCount, (short)burstCycleCount, 1f);
        emission.SetBursts(bursts);

        if (repulserTimer >= repulserTime)
        {
            Destroy(gameObject);
        }
    }
}