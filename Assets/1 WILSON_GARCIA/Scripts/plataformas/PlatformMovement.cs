using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Referencias de Posici�n")]
    public Transform startPoint; // Objeto vac�o que marca el punto de inicio (su coordenada X)
    public Transform endPoint;   // Objeto vac�o que marca el punto final (su coordenada X)

    [Header("Configuraci�n de Movimiento")]
    public float moveSpeed = 2f;          // Velocidad a la que se mover� la plataforma
    public bool startsAtEndPoint = false; // Si la plataforma debe empezar en el 'endPoint'

    private Vector3 currentTargetPosition; // El punto al que la plataforma se dirige actualmente
    private Rigidbody rb;                  // Referencia al Rigidbody de la plataforma
    private float fixedY;                  // Mantendr� la coordenada Y original de la plataforma
    private float fixedZ;                  // Mantendr� la coordenada Z original de la plataforma

    private Vector3 actualStartPos;        // Posici�n de inicio procesada (con Y y Z de la plataforma)
    private Vector3 actualEndPos;          // Posici�n de fin procesada (con Y y Z de la plataforma)

    private bool movingTowardsEnd;         // 'true' si va hacia 'endPoint', 'false' si va hacia 'startPoint'

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlatformMovement requiere un Rigidbody.");
            enabled = false; // Desactiva el script si no hay Rigidbody
            return;
        }

        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("�Asigna 'Start Point' y 'End Point' en el Inspector!");
            enabled = false;
            return;
        }

        // Guarda las coordenadas Y y Z iniciales para que la plataforma solo se mueva en X
        fixedY = transform.position.y;
        fixedZ = transform.position.z;

        // Crea los puntos de inicio y fin con la Y y Z fijas de la plataforma
        actualStartPos = new Vector3(startPoint.position.x, fixedY, fixedZ);
        actualEndPos = new Vector3(endPoint.position.x, fixedY, fixedZ);

        // Verifica si los puntos de inicio y fin son el mismo
        if (actualStartPos.x == actualEndPos.x)
        {
            Debug.LogWarning("Los puntos de inicio y fin tienen la misma X. La plataforma no se mover�.");
            enabled = false;
            return;
        }

        // Configura la posici�n inicial de la plataforma y el primer objetivo
        if (startsAtEndPoint)
        {
            transform.position = actualEndPos;
            currentTargetPosition = actualStartPos;
            movingTowardsEnd = false; // Inicialmente se mueve hacia el inicio
        }
        else
        {
            transform.position = actualStartPos;
            currentTargetPosition = actualEndPos;
            movingTowardsEnd = true; // Inicialmente se mueve hacia el final
        }

   }

    private void FixedUpdate() // Usar FixedUpdate para operaciones de f�sica
    {
        // Mueve la plataforma hacia el objetivo actual a la velocidad definida
        Vector3 newPosition = Vector3.MoveTowards(rb.position, currentTargetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // --- L�gica para cambiar de direcci�n ---
        // Obtiene la posici�n X actual de la plataforma y la X del objetivo actual
        float currentX = rb.position.x;
        float targetX = currentTargetPosition.x;
        bool hasCrossedTarget = false; // Bandera para saber si ya pasamos el objetivo

        // Verifica si la plataforma ha llegado o pasado su objetivo en el eje X
        // Esto depende de la direcci�n del movimiento actual.
        if (movingTowardsEnd) // Si la plataforma va hacia el 'endPoint'
        {
            if (actualEndPos.x > actualStartPos.x) // Si el 'endPoint' est� a la derecha del 'startPoint'
            {
                if (currentX >= targetX - 0.05f) // Si la X actual es mayor o igual a la X objetivo (con peque�a tolerancia)
                {
                    hasCrossedTarget = true;
                }
            }
            else // Si el 'endPoint' est� a la izquierda del 'startPoint'
            {
                if (currentX <= targetX + 0.05f) // Si la X actual es menor o igual a la X objetivo (con peque�a tolerancia)
                {
                    hasCrossedTarget = true;
                }
            }
        }
        else // Si la plataforma va hacia el 'startPoint'
        {
            if (actualStartPos.x < actualEndPos.x) // Si el 'startPoint' est� a la izquierda del 'endPoint'
            {
                if (currentX <= targetX + 0.05f)
                {
                    hasCrossedTarget = true;
                }
            }
            else // Si el 'startPoint' est� a la derecha del 'endPoint'
            {
                if (currentX >= targetX - 0.05f)
                {
                    hasCrossedTarget = true;
                }
            }
        }

        // Si la plataforma ha cruzado el objetivo, invertimos la direcci�n y establecemos el nuevo objetivo
        if (hasCrossedTarget)
        {
            movingTowardsEnd = !movingTowardsEnd; // Invierte la direcci�n (true a false, false a true)

            // El nuevo objetivo ser� 'endPoint' si ahora va hacia el final, o 'startPoint' si va hacia el inicio
            currentTargetPosition = movingTowardsEnd ? actualEndPos : actualStartPos;
        }
    }
}