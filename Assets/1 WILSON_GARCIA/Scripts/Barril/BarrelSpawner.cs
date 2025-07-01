using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    // Prefab del barril que se va a instanciar
    public GameObject barrelPrefab;

    // �AHORA UN ARRAY! Arrastra aqu� todos tus GameObjects vac�os de puntos de spawn
    public Transform[] spawnPoints;

    // Tiempo en segundos entre cada aparici�n de barril
    public float spawnInterval = 5f;

    // Retraso inicial antes de que el primer barril aparezca
    public float initialDelay = 1f;

    // Si es verdadero, la rotaci�n en Y ser� aleatoria
    public bool randomYRotation = false;

    // Si no es aleatoria, esta ser� la rotaci�n fija en grados para el eje Y
    public float fixedYRotation = 0f;

    void Start()
    {
        // Llama repetidamente al m�todo SpawnBarrel() despu�s de un retraso inicial
        InvokeRepeating(nameof(SpawnBarrel), initialDelay, spawnInterval);

        // Verificaciones para evitar errores
        if (barrelPrefab == null)
        {
            Debug.LogError("Error: Barrel Prefab no asignado. �As�gnalo en el Inspector!");
        }

        // Verifica si hay puntos de spawn asignados
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Error: No hay puntos de Spawn asignados. �Crea y arrastra GameObjects vac�os a la lista 'Spawn Points'!");
            // Desactiva el spawner para evitar errores continuos si no hay puntos
            enabled = false;
            return; // Sal del m�todo Start
        }
    }

    void SpawnBarrel()
    {
        // Solo spawnea si el prefab y hay al menos un punto de aparici�n
        if (barrelPrefab != null && spawnPoints != null && spawnPoints.Length > 0)
        {
            // --- Elige un punto de spawn aleatorio del array ---
            Transform chosenSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Inicia con la rotaci�n del punto de spawn elegido
            Quaternion rotationToApply = chosenSpawnPoint.rotation;

            // Decide si aplicar rotaci�n aleatoria o fija en el eje Y
            if (randomYRotation)
            {
                // A�ade una rotaci�n aleatoria de 0 a 360 grados en el eje Y
                rotationToApply *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            }
            else
            {
                // A�ade la rotaci�n fija especificada en el eje Y
                rotationToApply *= Quaternion.Euler(0f, fixedYRotation, 0f);
            }

            // Instancia el barril en la posici�n y rotaci�n calculadas del punto elegido
            GameObject newBarrel = Instantiate(barrelPrefab, chosenSpawnPoint.position, rotationToApply);
            // Aqu� puedes a�adir m�s l�gica para el barril reci�n creado (ej. darle velocidad)
        }
    }
}