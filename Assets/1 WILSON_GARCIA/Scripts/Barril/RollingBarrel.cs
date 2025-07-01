using UnityEngine;

public class RollingBarrel : MonoBehaviour
{
    [Header("Movimiento")]
    public float rollSpeed = 5f;            // Velocidad a la que el barril rodar�
    public float rollTorque = 50f;          // Fuerza de rotaci�n para que el barril ruede visiblemente
    private Rigidbody rb;

    [Header("Desaparici�n")]
    public float despawnDistance = 1000f;   // Distancia de desplazamiento para desaparecer (establece este valor en el Inspector)
    private float spawnXPosition;           // Guardaremos la posici�n X inicial

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("RollingBarrel requiere un Rigidbody en el GameObject: " + gameObject.name);
        }

        spawnXPosition = transform.position.x; // Guardar la posici�n X inicial al spawnear
    }

    private void FixedUpdate() // Usar FixedUpdate para la f�sica
    {
        if (rb != null) // Solo necesitamos el Rigidbody para el movimiento
        {
            // La direcci�n de movimiento ahora es fija hacia la izquierda (negativo en el eje X global)
            Vector3 direction = Vector3.left; // Es lo mismo que new Vector3(-1, 0, 0)

            // Aplicar fuerza para mover el barril
            rb.AddForce(direction * rollSpeed, ForceMode.Force);

            // Aplicar torque para que ruede visiblemente
            // Calcula el eje de rotaci�n perpendicular a la direcci�n de movimiento (Vector3.left) y a Vector3.up.
            // Vector3.Cross(Vector3.up, Vector3.left) produce Vector3.back (0, 0, -1), lo cual es correcto para rodar.
            Vector3 rollAxis = Vector3.Cross(Vector3.up, direction);
            rb.AddTorque(rollAxis * rollTorque, ForceMode.Force);
        }
        else
        {
            // Debug.LogWarning("Rigidbody es nulo en FixedUpdate de RollingBarrel."); // Descomentar para depuraci�n si es necesario
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Condici�n de eliminaci�n: si cae en la zona de muerte (vac�o)
        // Aseg�rate de que tu DeathBarrier tenga la etiqueta "DeathZone"
        if (other.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }
}