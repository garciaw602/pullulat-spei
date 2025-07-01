 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Variables públicas para configurar en el Inspector.
    public float speed = 5f;        // Velocidad de movimiento horizontal.
    public float jumpForce = 10f;   // Fuerza del salto.
    private Rigidbody rbPlayer;     // Componente Rigidbody del jugador.
    private bool isGrounded;        // Indica si el jugador está en el suelo.

    // Permite que otros scripts (ej. CameraFollow, Toxico, VoidRespawn) lean si el jugador está en el suelo.
    public bool IsGrounded
    {
        get { return isGrounded; }
    }

    public int score = 0;           // Puntuación del jugador.
    public TextMeshProUGUI scoreText; // Referencia al texto de puntuación en la UI.

    // Referencias a managers externos (componentes del patrón de "Composición").
    // Relación con PlayerAudioManager: Reproduce sonidos (saltar, aterrizar, correr, morir).
    public PlayerAudioManager audioManager;
    // Relación con PlayerParticleManager: Controla efectos de partículas (correr).
    public PlayerParticleManager particleManager;

    [Header("Límites de Movimiento")]
    public Transform leftBoundaryMarker; // Marcador para limitar el movimiento izquierdo.

    private float horizontalInput; // Input de movimiento horizontal.

    void Start() // Se ejecuta al inicio.
    {
        rbPlayer = GetComponent<Rigidbody>(); // Obtiene el Rigidbody.
        UpdateScoreUI(); // Inicializa el UI de puntuación.

        // Comprobaciones para asegurar que los managers y el marcador están asignados.
        if (audioManager == null) Debug.LogError("PlayerAudioManager no asignado.");
        if (particleManager == null) Debug.LogWarning("PlayerParticleManager no asignado.");
        if (leftBoundaryMarker == null) Debug.LogWarning("Left Boundary Marker no asignado.");
    }

    void Update() // Se ejecuta cada frame.
    {
        horizontalInput = Input.GetAxis("Horizontal"); // Captura el input.

        // Lógica de salto.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            if (audioManager != null) audioManager.PlayJumpSound();
        }
    }

    void FixedUpdate() // Se ejecuta a intervalos fijos para la física.
    {
        // Aplica velocidad horizontal al Rigidbody.
        rbPlayer.linearVelocity = new Vector3(horizontalInput * speed, rbPlayer.linearVelocity.y, rbPlayer.linearVelocity.z);

        // Controla sonidos y partículas de correr.
        bool isMovingHorizontal = Mathf.Abs(horizontalInput) > 0.01f;
        if (audioManager != null)
        {
            if (isMovingHorizontal && isGrounded) audioManager.StartRunSound();
            else audioManager.StopRunSound();
        }
        if (particleManager != null)
        {
            if (isMovingHorizontal && isGrounded) particleManager.StartRunParticles();
            else particleManager.StopRunParticles();
        }

        // Gira visualmente el personaje según la dirección.
        if (horizontalInput > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (horizontalInput < 0) transform.localScale = new Vector3(-1f, 1f, 1f);

        // Limita la posición X del jugador.
        if (leftBoundaryMarker != null)
        {
            float currentX = transform.position.x;
            float boundaryX = leftBoundaryMarker.position.x;
            if (currentX < boundaryX)
            {
                transform.position = new Vector3(boundaryX, transform.position.y, transform.position.z);
            }
        }
    }

    void OnCollisionEnter(Collision collision) // Se llama al colisionar con otro objeto.
    {
        // Detecta si toca el "Ground".
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded && audioManager != null) audioManager.PlayLandSound();
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision) // Se llama al dejar de colisionar.
    {
        // Actualiza el estado al salir del "Ground".
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            if (audioManager != null) audioManager.StopRunSound();
        }
    }

    public void UpdateScoreUI() // Actualiza el texto de la puntuación en la UI.
    {
        if (scoreText != null) scoreText.text = "" + score;
    }
}