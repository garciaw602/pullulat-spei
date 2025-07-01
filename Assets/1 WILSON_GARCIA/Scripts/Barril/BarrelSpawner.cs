using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    // Prefab del barril que se va a instanciar
    public GameObject barrelPrefab;

    // ¡AHORA UN ARRAY! Arrastra aquí todos tus GameObjects vacíos de puntos de spawn
    public Transform[] spawnPoints;

    // Tiempo en segundos entre cada aparición de barril
    public float spawnInterval = 5f;

    // Retraso inicial antes de que el primer barril aparezca
    public float initialDelay = 1f;

    // Si es verdadero, la rotación en Y será aleatoria
    public bool randomYRotation = false;

    // Si no es aleatoria, esta será la rotación fija en grados para el eje Y
    public float fixedYRotation = 0f;

    void Start()
    {
        // Llama repetidamente al método SpawnBarrel() después de un retraso inicial
        InvokeRepeating(nameof(SpawnBarrel), initialDelay, spawnInterval);

        // Verificaciones para evitar errores
        if (barrelPrefab == null)
        {
            Debug.LogError("Error: Barrel Prefab no asignado. ¡Asígnalo en el Inspector!");
        }

        // Verifica si hay puntos de spawn asignados
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Error: No hay puntos de Spawn asignados. ¡Crea y arrastra GameObjects vacíos a la lista 'Spawn Points'!");
            // Desactiva el spawner para evitar errores continuos si no hay puntos
            enabled = false;
            return; // Sal del método Start
        }
    }

    void SpawnBarrel()
    {
        // Solo spawnea si el prefab y hay al menos un punto de aparición
        if (barrelPrefab != null && spawnPoints != null && spawnPoints.Length > 0)
        {
            // --- Elige un punto de spawn aleatorio del array ---
            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Inicia con la rotación del punto de spawn elegido
            Quaternion rotationToApply = chosenSpawnPoint.rotation;

            // Decide si aplicar rotación aleatoria o fija en el eje Y
            if (randomYRotation)
            {
                // Añade una rotación aleatoria de 0 a 360 grados en el eje Y
                rotationToApply *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            }
            else
            {
                // Añade la rotación fija especificada en el eje Y
                rotationToApply *= Quaternion.Euler(0f, fixedYRotation, 0f);
            }

            // Instancia el barril en la posición y rotación calculadas del punto elegido
            GameObject newBarrel = Instantiate(barrelPrefab, chosenSpawnPoint.position, rotationToApply);
            // Aquí puedes añadir más lógica para el barril recién creado (ej. darle velocidad)
        }
    }
}