using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicEffectsHelper : MonoBehaviour
{
    public static GraphicEffectsHelper Instance;

    #region Inspector Variables

    [Range(0.0f, 1.0f)]
    [SerializeField] float slowmoFactor = 0.7f;
    [Range(0.0f, 1.0f)]
    [SerializeField] float slowmoTime = 0.1f;
    [SerializeField] ParticleSystem particlePrefab;

    #endregion

    void Awake()
    {
        Instance = this;
    }

    //Slowmo variables

    //Slows down time by a set amount for a set period of time
    //Used for collision impact
    public void Slowmo()
    {
        //Debug.Log ("SLOWMO ENGAGED");
        StartCoroutine(CoSlowmo());
    }


    //Slow down time for some time
    IEnumerator CoSlowmo()
    {
        //Slow down time by a predefine factor
        Time.timeScale = slowmoFactor;
        //Debug.Log ("SLOWMO ENGAGED");

        //Wait a set amount of time before going back to normal
        yield return new WaitForSeconds(slowmoTime);

        //Debug.Log ("SLOWMO Done");
        //Go back to normal
        Time.timeScale = 1.0F;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
    }

    public void EmitParticles(Vector3 position, Quaternion rotation)
    {
        ParticleSystem newParticleSystem = Instantiate(
                    particlePrefab,
                    position,
                    Quaternion.identity
                ) as ParticleSystem;

        // Make sure it will be destroyed
        Destroy(
            newParticleSystem.gameObject,
            newParticleSystem.startLifetime
        );

        // Instantiate(newParticleSystem, position);
    }
}
