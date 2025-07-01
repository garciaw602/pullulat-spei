using UnityEngine;

public class GameEffectsManager : MonoBehaviour
{
    // Esto hace que este manager sea un "Singleton", lo que significa que solo habr� una instancia
    // en toda la escena y ser� f�cilmente accesible desde otros scripts usando GameEffectsManager.Instance
    public static GameEffectsManager Instance { get; private set; }

    [Header("Prefabs de Part�culas")]
    [Tooltip("Arrastra aqu� el prefab del sistema de part�culas para cuando se recoge una planta.")]
    public ParticleSystem collectPlantParticlesPrefab;

    private void Awake()
    {
        // Implementaci�n del Singleton: asegura que solo una instancia de este manager exista.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta nueva.
        }
        else
        {
            Instance = this; // Si no existe, esta es la instancia �nica.
            // Opcional: Si quieres que este manager persista entre escenas:
            // DontDestroyOnLoad(gameObject);
        }

        if (collectPlantParticlesPrefab == null)
        {
            Debug.LogWarning("�Advertencia! 'Collect Plant Particles Prefab' no est� asignado en el GameEffectsManager. Las part�culas de recoger planta no se mostrar�n.");
        }
    }

    /// <summary>
    /// Reproduce el efecto de part�culas de recoger planta en una posici�n espec�fica.
    /// </summary>
    /// <param name="position">La posici�n del mundo donde aparecer�n las part�culas.</param>
    public void PlayCollectPlantParticles(Vector3 position)
    {
        if (collectPlantParticlesPrefab == null)
        {
            // Ya se advirti� en Awake, pero para asegurarse si se llama antes de Awake o si se desasigna.
            Debug.LogWarning("No se puede reproducir el efecto de part�culas de recoger planta: prefab no asignado.");
            return;
        }

        // Instancia el prefab de part�culas en la posici�n dada y sin rotaci�n espec�fica.
        ParticleSystem newParticles = Instantiate(collectPlantParticlesPrefab, position, Quaternion.identity);

        // Aseg�rate de que las part�culas se reproduzcan (aunque Play On Awake est� desmarcado en el prefab)
        newParticles.Play();

        // Destruye el GameObject de las part�culas despu�s de que terminen de reproducirse.
        // Esto es crucial para limpiar la escena y no acumular objetos.
        // sumamos un peque�o buffer para asegurarnos que todas las part�culas terminen.
        float duration = newParticles.main.duration + newParticles.main.startLifetime.constantMax;
        Destroy(newParticles.gameObject, duration);
    }
}