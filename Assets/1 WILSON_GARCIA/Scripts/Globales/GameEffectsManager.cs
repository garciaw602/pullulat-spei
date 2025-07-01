using UnityEngine;

public class GameEffectsManager : MonoBehaviour
{
    // Esto hace que este manager sea un "Singleton", lo que significa que solo habrá una instancia
    // en toda la escena y será fácilmente accesible desde otros scripts usando GameEffectsManager.Instance
    public static GameEffectsManager Instance { get; private set; }

    [Header("Prefabs de Partículas")]
    [Tooltip("Arrastra aquí el prefab del sistema de partículas para cuando se recoge una planta.")]
    public ParticleSystem collectPlantParticlesPrefab;

    private void Awake()
    {
        // Implementación del Singleton: asegura que solo una instancia de este manager exista.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta nueva.
        }
        else
        {
            Instance = this; // Si no existe, esta es la instancia única.
            // Opcional: Si quieres que este manager persista entre escenas:
            // DontDestroyOnLoad(gameObject);
        }

        if (collectPlantParticlesPrefab == null)
        {
            Debug.LogWarning("¡Advertencia! 'Collect Plant Particles Prefab' no está asignado en el GameEffectsManager. Las partículas de recoger planta no se mostrarán.");
        }
    }

    /// <summary>
    /// Reproduce el efecto de partículas de recoger planta en una posición específica.
    /// </summary>
    /// <param name="position">La posición del mundo donde aparecerán las partículas.</param>
    public void PlayCollectPlantParticles(Vector3 position)
    {
        if (collectPlantParticlesPrefab == null)
        {
            // Ya se advirtió en Awake, pero para asegurarse si se llama antes de Awake o si se desasigna.
            Debug.LogWarning("No se puede reproducir el efecto de partículas de recoger planta: prefab no asignado.");
            return;
        }

        // Instancia el prefab de partículas en la posición dada y sin rotación específica.
        ParticleSystem newParticles = Instantiate(collectPlantParticlesPrefab, position, Quaternion.identity);

        // Asegúrate de que las partículas se reproduzcan (aunque Play On Awake esté desmarcado en el prefab)
        newParticles.Play();

        // Destruye el GameObject de las partículas después de que terminen de reproducirse.
        // Esto es crucial para limpiar la escena y no acumular objetos.
        // sumamos un pequeño buffer para asegurarnos que todas las partículas terminen.
        float duration = newParticles.main.duration + newParticles.main.startLifetime.constantMax;
        Destroy(newParticles.gameObject, duration);
    }
}