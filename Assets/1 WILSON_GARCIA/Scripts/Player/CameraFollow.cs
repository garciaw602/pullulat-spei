using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variables públicas para configurar en el Inspector.
    public Transform objetivo;              // Objeto a seguir (Player).
    public float suavizado = 5f;            // Suavidad general del seguimiento.

    // Ángulos de rotación fijos para la cámara.
    [Header("Ángulos de Rotación Fijos")]
    [Range(-180f, 180f)]
    public float anguloHorizontal = 0f;     // Rotación en Y de la cámara.
    [Range(-90f, 90f)]
    public float anguloVertical = 30f;      // Inclinación en X de la cámara.

    public float offsetY = 2f;              // Altura de la cámara respecto al objetivo.
    public float distanciaZ = 10f;          // Distancia de la cámara al objetivo.

    // Efectos de Cámara al Saltar.
    [Header("Efectos de Salto de Cámara")]
    public float jumpRotationAngleX = -10f; // Ángulo extra en X al saltar.
    public float jumpRotationXSmoothing = 3f; // Suavizado para rotación en X.

    public float jumpRotationAngleY = 0f;   // Ángulo extra en Y al saltar.
    public float jumpRotationYSmoothing = 3f; // Suavizado para rotación en Y.

    public float jumpAddDistanceZ = 3f;     // Distancia adicional en Z al saltar.
    public float jumpDistanceSmoothing = 3f; // Suavizado para cambio de distancia en Z.

    private PlayerController playerController; // Referencia al PlayerController.
    private float originalAnguloHorizontal;   // Almacena ángulo Y original.
    private float originalAnguloVertical;     // Almacena ángulo X original.
    private float originalDistanciaZ;         // Almacena distancia Z original.

    void Start() // Se ejecuta al inicio.
    {
        if (objetivo == null) return;

        // Obtiene el componente PlayerController del objetivo.
        // Relación con PlayerController: Necesita saber si el jugador está en el suelo.
        playerController = objetivo.GetComponent<PlayerController>();

        // Guarda valores iniciales.
        originalAnguloHorizontal = anguloHorizontal;
        originalAnguloVertical = anguloVertical;
        originalDistanciaZ = distanciaZ;

        // Posiciona y rota la cámara instantáneamente al inicio.
        Vector3 puntoDeReferenciaInicial = objetivo.position + new Vector3(0f, offsetY, 0f);
        Quaternion rotacionDeseadaInicial = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
        Vector3 posicionCalculadaInicial = puntoDeReferenciaInicial - (rotacionDeseadaInicial * Vector3.forward * distanciaZ);

        transform.position = posicionCalculadaInicial;
        transform.rotation = rotacionDeseadaInicial;
    }

    void LateUpdate() // Se ejecuta después de todos los movimientos.
    {
        if (objetivo == null) return;

        // Define los valores objetivo para ángulos y distancia.
        float targetAnguloX = originalAnguloVertical;
        float targetAnguloY = originalAnguloHorizontal;
        float targetDistanciaZ = originalDistanciaZ;

        // Aplica efectos de salto si el jugador no está en el suelo.
        if (playerController != null && !playerController.IsGrounded)
        {
            targetAnguloX = originalAnguloVertical + jumpRotationAngleX;
            targetAnguloY = originalAnguloHorizontal + jumpRotationAngleY;
            targetDistanciaZ = originalDistanciaZ + jumpAddDistanceZ;
        }

        // Suaviza la cámara hacia los valores objetivo.
        anguloVertical = Mathf.Lerp(anguloVertical, targetAnguloX, jumpRotationXSmoothing * Time.deltaTime);
        anguloHorizontal = Mathf.Lerp(anguloHorizontal, targetAnguloY, jumpRotationYSmoothing * Time.deltaTime);
        distanciaZ = Mathf.Lerp(distanciaZ, targetDistanciaZ, jumpDistanceSmoothing * Time.deltaTime);

        // Calcula la posición y rotación final de la cámara.
        Vector3 puntoDeReferencia = objetivo.position + new Vector3(0f, offsetY, 0f);
        Quaternion rotacionDeseada = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
        Vector3 posicionCalculada = puntoDeReferencia - (rotacionDeseada * Vector3.forward * distanciaZ);

        // Aplica el seguimiento suave.
        transform.position = Vector3.Lerp(transform.position, posicionCalculada, suavizado * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, suavizado * Time.deltaTime);
    }
}