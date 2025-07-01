using UnityEngine;

public class ParallaxController2 : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform[] layerSegments; // Array para los segmentos de la capa (3 para repetici�n)
        public float parallaxSpeed;      // Velocidad de movimiento de esta capa
        [HideInInspector] public float segmentWidth; // Ancho de un solo sprite en unidades de Unity
    }

    public ParallaxLayer[] parallaxLayers; // Array de todos los grupos de capas parallax

    public Transform targetToFollow;    // Objeto a seguir (c�mara o jugador)
    private Vector3 lastTargetPosition; // Posici�n del objetivo en el frame anterior

    void Start()
    {
        // Si no hay objetivo asignado, intenta usar la c�mara principal
        if (targetToFollow == null)
        {
            targetToFollow = Camera.main.transform;
            if (targetToFollow == null)
            {
                Debug.LogError("ParallaxController: No se asign� objetivo y no se encontr� la c�mara principal. Desactivando script.");
                enabled = false;
                return;
            }
        }

        // Inicializa la �ltima posici�n del objetivo
        lastTargetPosition = targetToFollow.position;

        // Configura el ancho y posiciona los segmentos de cada capa
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            ParallaxLayer currentLayer = parallaxLayers[i];
            if (currentLayer.layerSegments.Length > 0 && currentLayer.layerSegments[0] != null)
            {
                currentLayer.segmentWidth = GetSpriteWidth(currentLayer.layerSegments[0]);

                // Posiciona los segmentos uno al lado del otro perfectamente
                for (int j = 1; j < currentLayer.layerSegments.Length; j++)
                {
                    currentLayer.layerSegments[j].position = new Vector3(
                        currentLayer.layerSegments[j - 1].position.x + currentLayer.segmentWidth,
                        currentLayer.layerSegments[j].position.y,
                        currentLayer.layerSegments[j].position.z
                    );
                }
            }
            else
            {
                Debug.LogWarning($"ParallaxController: La capa {i} no tiene segmentos asignados o el primer segmento es nulo. No se puede calcular el ancho.");
                currentLayer.segmentWidth = 0f;
            }
        }
    }

    void LateUpdate()
    {
        // Calcula el movimiento del objetivo desde el �ltimo frame
        Vector3 deltaMovement = targetToFollow.position - lastTargetPosition;

        // Itera sobre cada grupo de capas
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            ParallaxLayer currentLayer = parallaxLayers[i];

            // Calcula el movimiento parallax para esta capa
            float parallaxX = deltaMovement.x * currentLayer.parallaxSpeed;

            // Mueve cada segmento del grupo
            foreach (Transform segment in currentLayer.layerSegments)
            {
                segment.position = new Vector3(segment.position.x + parallaxX, segment.position.y, segment.position.z);
            }

            // --- L�gica de Repetici�n Infinita con m�ltiples segmentos ---
            // Reposiciona los segmentos cuando salen de la vista
            foreach (Transform segment in currentLayer.layerSegments)
            {
                // Si la c�mara se mueve a la derecha y el segmento est� fuera a la izquierda
                if (parallaxX > 0 && segment.position.x < targetToFollow.position.x - (currentLayer.segmentWidth * 1.5f))
                {
                    // Encuentra el segmento m�s a la derecha y reposiciona este despu�s de �l
                    Transform rightmostSegment = GetRightmostSegment(currentLayer.layerSegments);
                    segment.position = new Vector3(rightmostSegment.position.x + currentLayer.segmentWidth, segment.position.y, segment.position.z);
                }
                // Si la c�mara se mueve a la izquierda y el segmento est� fuera a la derecha
                else if (parallaxX < 0 && segment.position.x > targetToFollow.position.x + (currentLayer.segmentWidth * 1.5f))
                {
                    // Encuentra el segmento m�s a la izquierda y reposiciona este antes de �l
                    float leftmostX = float.MaxValue;
                    foreach (Transform otherSegment in currentLayer.layerSegments)
                    {
                        if (otherSegment.position.x < leftmostX)
                        {
                            leftmostX = otherSegment.position.x;
                        }
                    }
                    segment.position = new Vector3(leftmostX - currentLayer.segmentWidth, segment.position.y, segment.position.z);
                }
            }
        }

        // Actualiza la �ltima posici�n del objetivo para el siguiente frame
        lastTargetPosition = targetToFollow.position;
    }

    // Helper para encontrar el segmento m�s a la derecha en un array de Transform
    private Transform GetRightmostSegment(Transform[] segments)
    {
        Transform rightmost = segments[0];
        for (int i = 1; i < segments.Length; i++)
        {
            if (segments[i].position.x > rightmost.position.x)
            {
                rightmost = segments[i];
            }
        }
        return rightmost;
    }

    // Helper para encontrar el segmento m�s a la izquierda en un array de Transform
    private Transform GetLeftmostSegment(Transform[] segments)
    {
        Transform leftmost = segments[0];
        for (int i = 1; i < segments.Length; i++)
        {
            if (segments[i].position.x < leftmost.position.x)
            {
                leftmost = segments[i];
            }
        }
        return leftmost;
    }

    // M�todo de ayuda para obtener el ancho de un sprite en unidades de Unity
    private float GetSpriteWidth(Transform segmentTransform)
    {
        SpriteRenderer spriteRenderer = segmentTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            return spriteRenderer.sprite.bounds.size.x;
        }
        Debug.LogWarning($"ParallaxController: No se encontr� SpriteRenderer o sprite en {segmentTransform.name}. No se puede calcular el ancho.");
        return 0f;
    }
}