using UnityEngine;
using System.Collections; // Necesario para usar Coroutines (para los retrasos)

public class FallingPlatform : MonoBehaviour
{
    [Header("Configuración de Caída")]
    [Tooltip("Tiempo en segundos que el jugador debe estar sobre la plataforma antes de que caiga.")]
    public float fallDelay = 1f;
    [Tooltip("Tiempo en segundos que tarda en reaparecer después de tocar la DeathZone.")]
    public float respawnDelay = 2f;

    private Vector3 originalPosition; // Guarda la posición inicial de la plataforma
    private Rigidbody rb;             // Referencia al componente Rigidbody de la plataforma
    private bool playerOnPlatform = false; // Indica si el jugador está actualmente sobre la plataforma
    private bool hasFallen = false;        // Indica si la plataforma ya se ha caído

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("FallingPlatform requiere un Rigidbody en el GameObject: " + gameObject.name);
            enabled = false; // Desactiva el script si no hay Rigidbody
            return;
        }

        originalPosition = transform.position; // Guarda la posición inicial al inicio del juego
        ResetPlatform(); // Asegura que la plataforma esté en su estado inicial (no cayendo)
    }

    // Este método se llama cuando la plataforma colisiona con otro objeto NO marcado como 'Is Trigger'
    private void OnCollisionEnter(Collision collision)
    {
        // 1. Detectar si el jugador está sobre la plataforma
        // El jugador DEBE tener un Rigidbody y su Collider NO debe ser 'Is Trigger'
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[FallingPlatform] Colisión SÓLIDA con Player: {collision.gameObject.name}. playerOnPlatform: {playerOnPlatform}, hasFallen: {hasFallen}");
            // Solo si el jugador no estaba ya sobre ella y la plataforma no se ha caído
            if (!playerOnPlatform && !hasFallen)
            {
                playerOnPlatform = true;
                Debug.Log("[FallingPlatform] Jugador entró en la plataforma. Iniciando cuenta atrás para la caída.");
                StartCoroutine(FallAfterDelay()); // Inicia la corrutina para la caída
            }
        }
    }

    // Este método se llama cuando la plataforma entra en un Collider marcado como 'Is Trigger'
    private void OnTriggerEnter(Collider other) // Usamos 'Collider other' para triggers
    {
        // 2. Detectar si la plataforma tocó la zona de muerte (que es un Trigger)
        // La DeathZone DEBE tener su Collider marcado como 'Is Trigger'
        if (other.gameObject.CompareTag("DeathZone"))
        {
            Debug.Log($"[FallingPlatform] TRIGGER con DeathZone: {other.gameObject.name}. hasFallen: {hasFallen}");
            // Solo si la plataforma ya se había caído (para evitar reinicios accidentales)
            if (hasFallen)
            {
                Debug.Log("[FallingPlatform] Plataforma tocó DeathZone (Trigger). Iniciando cuenta atrás para reaparición.");
                // Detiene cualquier corrutina de reaparición anterior para evitar duplicados
                StopCoroutine(nameof(RespawnAfterDelay)); // Usa nameof para evitar 'magic strings'
                StartCoroutine(RespawnAfterDelay()); // Inicia la corrutina para reaparecer
            }
            else
            {
                Debug.LogWarning("[FallingPlatform] TRIGGER con DeathZone, pero la plataforma no se había caído aún (hasFallen es false). No se activará la reaparición.");
            }
        }
    }


    // Se llama cuando la plataforma deja de colisionar (sólidamente) con otro objeto
    private void OnCollisionExit(Collision collision)
    {
        // Detectar si el jugador se bajó de la plataforma
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"[FallingPlatform] Jugador salió de la plataforma: {collision.gameObject.name}. playerOnPlatform: {playerOnPlatform}, hasFallen: {hasFallen}");
            // Si el jugador estaba sobre ella y la plataforma no se ha caído aún
            if (playerOnPlatform && !hasFallen)
            {
                playerOnPlatform = false;
                Debug.Log("[FallingPlatform] Jugador salió de la plataforma. Cancelando cuenta atrás para caída.");
                StopCoroutine(nameof(FallAfterDelay)); // Detiene solo la corrutina de caída
            }
        }
    }

    // Corrutina para esperar y luego hacer caer la plataforma
    private IEnumerator FallAfterDelay()
    {
        Debug.Log("[FallingPlatform] FallAfterDelay iniciado. Esperando " + fallDelay + " segundos.");
        yield return new WaitForSeconds(fallDelay); // Espera el tiempo definido en 'fallDelay'

        // Doble verificación: si el jugador sigue en la plataforma y no ha caído aún
        if (playerOnPlatform && !hasFallen)
        {
            Debug.Log("[FallingPlatform] ¡Plataforma cayendo!");
            rb.isKinematic = false; // Desactiva Kinematic para que la física la afecte
            rb.useGravity = true;   // Activa la gravedad
            hasFallen = true;       // Marca la plataforma como caída
            playerOnPlatform = false; // El jugador ya no está "activando" la caída
        }
        else
        {
            Debug.Log("[FallingPlatform] FallAfterDelay no se ejecutó completamente (condición no cumplida). playerOnPlatform=" + playerOnPlatform + ", hasFallen=" + hasFallen);
        }
    }

    // Corrutina para esperar y luego reaparecer la plataforma
    private IEnumerator RespawnAfterDelay()
    {
        Debug.Log("[FallingPlatform] Iniciando RespawnAfterDelay. Collider se desactiva.");
        // Desactiva temporalmente el collider para evitar más colisiones/triggers mientras reaparece
        Collider platformCollider = GetComponent<Collider>();
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }

        yield return new WaitForSeconds(respawnDelay); // Espera el tiempo definido en 'respawnDelay'

        Debug.Log("[FallingPlatform] Plataforma reapareciendo en la posición original.");
        ResetPlatform(); // Llama a la función para restablecer la plataforma

        // Reactiva el collider después de reaparecer
        if (platformCollider != null)
        {
            platformCollider.enabled = true;
        }
        Debug.Log("[FallingPlatform] RespawnAfterDelay finalizado. Collider reactivado.");
    }

    // Función para restablecer la plataforma a su estado inicial
    private void ResetPlatform()
    {
        Debug.Log("[FallingPlatform] Reseteando plataforma a posición original: " + originalPosition);
        rb.isKinematic = true;      // Vuelve a activar Kinematic (detiene el movimiento por física)
        rb.useGravity = false;      // Desactiva la gravedad
        rb.linearVelocity = Vector3.zero; // Resetea cualquier velocidad residual
        rb.angularVelocity = Vector3.zero; // Resetea cualquier rotación residual
        transform.position = originalPosition; // Vuelve a la posición guardada
        hasFallen = false;          // Resetea el estado de caída
        playerOnPlatform = false;   // Asegura que no hay jugador detectado al reaparecer
        StopAllCoroutines(); // Detiene cualquier corrutina en ejecución (fall o respawn) para limpiar el estado
        Debug.Log("[FallingPlatform] Plataforma restablecida. Todas las corrutinas detenidas.");
    }
}