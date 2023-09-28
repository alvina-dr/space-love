using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    private bool started = false;
    private void Update()
    {
        if (started == false && vfx.aliveParticleCount > 0) started = true;
        if (vfx.aliveParticleCount == 0 && started) Destroy(gameObject);
    }
}
