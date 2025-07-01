using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [Header("Partículas de Correr")]
    [Tooltip("Arrastra aquí el sistema de partículas que se activará cuando el jugador corra.")]
    public ParticleSystem runParticles; // Asigna tu sistema de partículas de correr aquí

    private void Start()
    {
        if (runParticles == null)
        {
            Debug.LogWarning("El sistema de partículas de correr no está asignado en el PlayerParticleManager.");
        }
        else
        {
            // Asegúrate de que las partículas no estén reproduciéndose al inicio
            runParticles.Stop();
        }
    }

    // Método para iniciar las partículas de correr
    public void StartRunParticles()
    {
        if (runParticles != null && !runParticles.isPlaying)
        {
            runParticles.Play();
            //Debug.Log("Partículas de correr iniciadas."); // Para depuración
        }
    }

    // Método para detener las partículas de correr
    public void StopRunParticles()
    {
        if (runParticles != null && runParticles.isPlaying)
        {
            runParticles.Stop();
            //Debug.Log("Partículas de correr detenidas."); // Para depuración
        }
    }
}