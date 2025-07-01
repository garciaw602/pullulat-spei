using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.score += 1;
            player.UpdateScoreUI();

            // Sonido
            if (player.audioManager != null)
            {
                player.audioManager.PlayCollectSound();
            }

            // Partículas
            if (GameEffectsManager.Instance != null)
            {
                GameEffectsManager.Instance.PlayCollectPlantParticles(transform.position);
            }
            else
            {
                Debug.LogWarning("GameEffectsManager.Instance no encontrado. Asegúrate de que está en la escena y configurado.");
            }

            // 🔥 NUEVO: avisar al RecoleccionManager
            RecoleccionManager manager = FindObjectOfType<RecoleccionManager>();
            if (manager != null)
            {
                manager.PlantaRecolectada(); // ← Aquí está la conexión
            }

            Destroy(gameObject);
        }
    }
}
