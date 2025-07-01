using UnityEngine;
using UnityEngine.SceneManagement;

public class Toxico : MonoBehaviour
{
    public GameObject canvasMuerte;         // Asigna aquí el Canvas de Muerte
    public float tiempoReinicio = 2f;       // Tiempo antes de reiniciar la escena

    private void OnTriggerEnter(Collider other)
    {
        // Intentar obtener el PlayerController del objeto que colisionó
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            // Reproducir el sonido de muerte a través del PlayerAudioManager del jugador
            if (player.audioManager != null)
            {
              player.audioManager.PlayDeathSound(); // Llamamos al nuevo método
            }
            else
            {
                Debug.LogWarning("PlayerAudioManager not found on PlayerController, cannot play death sound.");
            }

            // Mostrar el Canvas de Muerte
            if (canvasMuerte != null)
            {
                canvasMuerte.SetActive(true);
            }

            // Desactivar al jugador
            other.gameObject.SetActive(false);

            // Esperar y reiniciar escena
            // Considera ajustar 'tiempoReinicio' para que el sonido de muerte termine antes de reiniciar.
            Invoke(nameof(ReiniciarEscena), tiempoReinicio);
        }
    }

    void ReiniciarEscena()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}