using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Paneles de UI")]
    [Tooltip("Arrastra aqu� el GameObject de tu Canvas de Muerte (el que contiene el mensaje de Game Over).")]
    public GameObject deathCanvas;
    [Tooltip("Arrastra aqu� el GameObject del Panel/Texto de Instrucciones inicial.")]
    public GameObject instructionsPanel; // <-- �NUEVA VARIABLE!

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        // Comprobaci�n y configuraci�n del Canvas de Muerte
        if (deathCanvas == null)
        {
            GameObject foundDeathCanvas = GameObject.FindGameObjectWithTag("DeathCanvas");
            if (foundDeathCanvas != null)
            {
                deathCanvas = foundDeathCanvas;
                Debug.Log("UIManager: Canvas de Muerte encontrado din�micamente por Tag: " + foundDeathCanvas.name);
            }
            else
            {
                Debug.LogWarning("UIManager: Canvas de Muerte NO asignado en Inspector y NO encontrado por etiqueta 'DeathCanvas'.");
            }
        }
        if (deathCanvas != null)
        {
            deathCanvas.SetActive(false); // Asegurarse de que el Canvas de Muerte est� inicialmente oculto
        }

        // Comprobaci�n y configuraci�n del Panel de Instrucciones
        if (instructionsPanel == null) // <-- NUEVO BLOQUE
        {
            // Opcional: Si quieres que el panel de instrucciones tambi�n pueda ser encontrado por Tag
            // GameObject foundInstructionsPanel = GameObject.FindGameObjectWithTag("InstructionsPanel");
            // if (foundInstructionsPanel != null)
            // {
            //     instructionsPanel = foundInstructionsPanel;
            //     Debug.Log("UIManager: Panel de Instrucciones encontrado din�micamente por Tag.");
            // }
            // else
            // {
            //     Debug.LogWarning("UIManager: Panel de Instrucciones NO asignado en Inspector y NO encontrado por etiqueta 'InstructionsPanel'.");
            // }
        }
        if (instructionsPanel != null) // <-- NUEVO BLOQUE
        {
            instructionsPanel.SetActive(false); // Asegurarse de que el Panel de Instrucciones est� inicialmente oculto
        }
    }

    public void ShowDeathCanvas()
    {
        if (deathCanvas != null)
        {
            deathCanvas.SetActive(true);
            Debug.Log("UIManager: Canvas de Muerte mostrado.");
        }
        else
        {
            Debug.LogWarning("UIManager: No se pudo mostrar el Canvas de Muerte: referencia nula.");
        }
    }

    public void HideDeathCanvas()
    {
        if (deathCanvas != null)
        {
            deathCanvas.SetActive(false);
            Debug.Log("UIManager: Canvas de Muerte ocultado.");
        }
    }

    /// <summary>
    /// Muestra el panel de instrucciones.
    /// </summary>
    public void ShowInstructionsPanel() // <-- �NUEVO M�TODO!
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
            Debug.Log("UIManager: Panel de Instrucciones mostrado.");
        }
        else
        {
            Debug.LogWarning("UIManager: No se pudo mostrar el Panel de Instrucciones: referencia nula.");
        }
    }

    /// <summary>
    /// Oculta el panel de instrucciones.
    /// </summary>
    public void HideInstructionsPanel() // <-- �NUEVO M�TODO!
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
            Debug.Log("UIManager: Panel de Instrucciones ocultado.");
        }
    }
}