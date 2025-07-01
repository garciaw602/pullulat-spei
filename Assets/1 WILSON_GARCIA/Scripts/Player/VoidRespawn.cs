using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class VoidRespawn : MonoBehaviour
{
    // Ya no necesitamos la referencia directa al canvas aqu�. El UIManager la gestiona.
    // public GameObject canvasMuerte; // <--- �ELIMINA ESTA L�NEA!
    public float tiempoReinicio = 2f;       // Tiempo antes de reiniciar la escena
    public PlayerAudioManager playerAudioManager; // Referencia al PlayerAudioManager

    void Start()
    {
        // No hay necesidad de un Awake o Start aqu� si solo manejas el trigger
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si el objeto que entr� en el trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Reproducir el sonido de ca�da/muerte a trav�s del PlayerAudioManager
            if (playerAudioManager != null)
            {
              playerAudioManager.PlayFallSound();
               // playerAudioManager.PlayDeathSound(); si es el mismo sonido
            }
            else
            {
                Debug.LogWarning("PlayerAudioManager no asignado en VoidRespawn. No se reproducir� sonido de ca�da/muerte.");
            }

            // <<-- NUEVO BLOQUE: Llamar al UIManager para mostrar el Canvas de Muerte
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowDeathCanvas();
            }
            else
            {
                Debug.LogWarning("UIManager.Instance no encontrado. Aseg�rate de que el UIManager est� en la escena y configurado.");
            }
            // <<-- FIN NUEVO BLOQUE

            // Desactivar al jugador para evitar m�s interacciones o movimientos
            other.gameObject.SetActive(false);

            // Reiniciar la escena despu�s de un tiempo
            Invoke(nameof(ReiniciarEscena), tiempoReinicio);
        }
    }

    void ReiniciarEscena()
    {
        Time.timeScale = 1f; // Aseg�rate de que el tiempo no est� pausado
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Carga la escena actual
    }
}