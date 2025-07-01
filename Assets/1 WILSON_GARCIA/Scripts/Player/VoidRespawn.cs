using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class VoidRespawn : MonoBehaviour
{
    // Ya no necesitamos la referencia directa al canvas aquí. El UIManager la gestiona.
    // public GameObject canvasMuerte; // <--- ¡ELIMINA ESTA LÍNEA!
    public float tiempoReinicio = 2f;       // Tiempo antes de reiniciar la escena
    public PlayerAudioManager playerAudioManager; // Referencia al PlayerAudioManager

    void Start()
    {
        // No hay necesidad de un Awake o Start aquí si solo manejas el trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que entró en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Reproducir el sonido de caída/muerte a través del PlayerAudioManager
            if (playerAudioManager != null)
            {
              playerAudioManager.PlayFallSound();
               // playerAudioManager.PlayDeathSound(); si es el mismo sonido
            }
            else
            {
                Debug.LogWarning("PlayerAudioManager no asignado en VoidRespawn. No se reproducirá sonido de caída/muerte.");
            }

            // <<-- NUEVO BLOQUE: Llamar al UIManager para mostrar el Canvas de Muerte
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowDeathCanvas();
            }
            else
            {
                Debug.LogWarning("UIManager.Instance no encontrado. Asegúrate de que el UIManager está en la escena y configurado.");
            }
            // <<-- FIN NUEVO BLOQUE

            // Desactivar al jugador para evitar más interacciones o movimientos
            other.gameObject.SetActive(false);

            // Reiniciar la escena después de un tiempo
            Invoke(nameof(ReiniciarEscena), tiempoReinicio);
        }
    }

    void ReiniciarEscena()
    {
        Time.timeScale = 1f; // Asegúrate de que el tiempo no esté pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Carga la escena actual
    }
}