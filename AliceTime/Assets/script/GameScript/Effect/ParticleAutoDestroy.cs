using UnityEngine;
using System.Collections;

public delegate void Particle_Handler();

public class ParticleAutoDestroy : MonoBehaviour {
	public event Particle_Handler EventAction;

	private ParticleSystem particle;

    private float lifecounter = 0f;

	void Awake ()
	{
		particle = GetComponent<ParticleSystem>();
	}

	void Update ()
	{
		if (particle != null)
		{
            lifecounter += Time.deltaTime;
            if (lifecounter >= particle.startLifetime)
            {
                EventAction();
                Destroy(this.gameObject);
            }
		}
	}
}
