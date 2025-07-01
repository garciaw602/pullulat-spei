using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variables p�blicas para configurar en el Inspector.
    public Transform objetivo;              // Objeto a seguir (Player).
    public float suavizado = 5f;            // Suavidad general del seguimiento.

    // �ngulos de rotaci�n fijos para la c�mara.
    [Header("�ngulos de Rotaci�n Fijos")]
    [Range(-180f, 180f)]
    public float anguloHorizontal = 0f;     // Rotaci�n en Y de la c�mara.
    [Range(-90f, 90f)]
    public float anguloVertical = 30f;      // Inclinaci�n en X de la c�mara.

    public float offsetY = 2f;              // Altura de la c�mara respecto al objetivo.
    public float distanciaZ = 10f;          // Distancia de la c�mara al objetivo.

    // Efectos de C�mara al Saltar.
    [Header("Efectos de Salto de C�mara")]
    public float jumpRotationAngleX = -10f; // �ngulo extra en X al saltar.
    public float jumpRotationXSmoothing = 3f; // Suavizado para rotaci�n en X.

    public float jumpRotationAngleY = 0f;   // �ngulo extra en Y al saltar.
    public float jumpRotationYSmoothing = 3f; // Suavizado para rotaci�n en Y.

    public float jumpAddDistanceZ = 3f;     // Distancia adicional en Z al saltar.
    public float jumpDistanceSmoothing = 3f; // Suavizado para cambio de distancia en Z.

    private PlayerController playerController; // Referencia al PlayerController.
    private float originalAnguloHorizontal;   // Almacena �ngulo Y original.
    private float originalAnguloVertical;     // Almacena �ngulo X original.
    private float originalDistanciaZ;         // Almacena distancia Z original.

    void Start() // Se ejecuta al inicio.
    {
        if (objetivo == null) return;

        // Obtiene el componente PlayerController del objetivo.
        // Relaci�n con PlayerController: Necesita saber si el jugador est� en el suelo.
        playerController = objetivo.GetComponent<PlayerController>();

        // Guarda valores iniciales.
        originalAnguloHorizontal = anguloHorizontal;
        originalAnguloVertical = anguloVertical;
        originalDistanciaZ = distanciaZ;

        // Posiciona y rota la c�mara instant�neamente al inicio.
        Vector3 puntoDeReferenciaInicial = objetivo.position + new Vector3(0f, offsetY, 0f);
        Quaternion rotacionDeseadaInicial = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
        Vector3 posicionCalculadaInicial = puntoDeReferenciaInicial - (rotacionDeseadaInicial * Vector3.forward * distanciaZ);

        transform.position = posicionCalculadaInicial;
        transform.rotation = rotacionDeseadaInicial;
    }

    void LateUpdate() // Se ejecuta despu�s de todos los movimientos.
    {
        if (objetivo == null) return;

        // Define los valores objetivo para �ngulos y distancia.
        float targetAnguloX = originalAnguloVertical;
        float targetAnguloY = originalAnguloHorizontal;
        float targetDistanciaZ = originalDistanciaZ;

        // Aplica efectos de salto si el jugador no est� en el suelo.
        if (playerController != null && !playerController.IsGrounded)
        {
            targetAnguloX = originalAnguloVertical + jumpRotationAngleX;
            targetAnguloY = originalAnguloHorizontal + jumpRotationAngleY;
            targetDistanciaZ = originalDistanciaZ + jumpAddDistanceZ;
        }

        // Suaviza la c�mara hacia los valores objetivo.
        anguloVertical = Mathf.Lerp(anguloVertical, targetAnguloX, jumpRotationXSmoothing * Time.deltaTime);
        anguloHorizontal = Mathf.Lerp(anguloHorizontal, targetAnguloY, jumpRotationYSmoothing * Time.deltaTime);
        distanciaZ = Mathf.Lerp(distanciaZ, targetDistanciaZ, jumpDistanceSmoothing * Time.deltaTime);

        // Calcula la posici�n y rotaci�n final de la c�mara.
        Vector3 puntoDeReferencia = objetivo.position + new Vector3(0f, offsetY, 0f);
        Quaternion rotacionDeseada = Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
        Vector3 posicionCalculada = puntoDeReferencia - (rotacionDeseada * Vector3.forward * distanciaZ);

        // Aplica el seguimiento suave.
        transform.position = Vector3.Lerp(transform.position, posicionCalculada, suavizado * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, suavizado * Time.deltaTime);
    }
}