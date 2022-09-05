using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleBurst : MonoBehaviour
{
    public ParticleSystem particles;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //var em = particles.emission;
            //var dur = particles.duration;

            ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();
            emitOverride.startLifetime = 10f;
            particles.Emit(emitOverride, 20);

            // em.enabled = true;
            //particles.Play();
        }
    }
}