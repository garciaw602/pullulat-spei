using UnityEngine;

public class InstructionTrigger : MonoBehaviour
{
    // La variable 'hasTriggered' YA NO ES NECESARIA si quieres que el panel se muestre siempre al entrar.
    // private bool hasTriggered = false; // <-- �ELIMINA ESTA L�NEA!

    // Esta variable la mantenemos para decidir si el panel se debe ocultar al salir.
    public bool hideOnExit = true;

    private void OnTriggerEnter(Collider other)
    {
        // Aseg�rate de que el objeto que entra en el trigger es el jugador
        // Y aqu� eliminamos la condici�n '&& !hasTriggered'
        if (other.CompareTag("Player")) // <-- �MODIFICADO AQU�!
        {
            // Llama al UIManager para mostrar el panel de instrucciones
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowInstructionsPanel();
                // Ya no marcamos hasTriggered = true; porque queremos que se active siempre.
                // Opcional: Si quieres desactivar el GameObject del trigger despu�s de la primera vez
                // para que no vuelva a detectar, descomenta la l�nea de abajo.
                // Si quieres que funcione SIEMPRE al entrar y salir, d�jala comentada.
                // gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("InstructionTrigger: UIManager.Instance no encontrado. No se puede mostrar el panel de instrucciones.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Comprobamos si el objeto que sali� del trigger es el jugador
        if (other.CompareTag("Player") && hideOnExit)
        {
            // Llama al UIManager para ocultar el panel de instrucciones
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideInstructionsPanel();
                // No hay necesidad de reiniciar hasTriggered aqu� porque ya la quitamos.
            }
            else
            {
                Debug.LogWarning("InstructionTrigger: UIManager.Instance no encontrado. No se puede ocultar el panel de instrucciones.");
            }
        }
    }
}