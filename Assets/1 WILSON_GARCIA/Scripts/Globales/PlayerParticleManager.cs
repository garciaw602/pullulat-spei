using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [Header("Part�culas de Correr")]
    [Tooltip("Arrastra aqu� el sistema de part�culas que se activar� cuando el jugador corra.")]
    public ParticleSystem runParticles; // Asigna tu sistema de part�culas de correr aqu�

    private void Start()
    {
        if (runParticles == null)
        {
            Debug.LogWarning("El sistema de part�culas de correr no est� asignado en el PlayerParticleManager.");
        }
        else
        {
            // Aseg�rate de que las part�culas no est�n reproduci�ndose al inicio
            runParticles.Stop();
        }
    }

    // M�todo para iniciar las part�culas de correr
    public void StartRunParticles()
    {
        if (runParticles != null && !runParticles.isPlaying)
        {
            runParticles.Play();
            //Debug.Log("Part�culas de correr iniciadas."); // Para depuraci�n
        }
    }

    // M�todo para detener las part�culas de correr
    public void StopRunParticles()
    {
        if (runParticles != null && runParticles.isPlaying)
        {
            runParticles.Stop();
            //Debug.Log("Part�culas de correr detenidas."); // Para depuraci�n
        }
    }
}