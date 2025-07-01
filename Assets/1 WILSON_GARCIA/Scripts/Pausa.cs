using UnityEngine;

public class PausaSimple : MonoBehaviour
{
    public GameObject canvasPausa; // Asigna el Canvas de pausa desde el Inspector
    private bool enPausa = false;

    void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.P))
        {
        

            if (!enPausa)
                Pausar();
            else // Si ya está en pausa y presionas 'P' de nuevo, que continúe
                Continuar();
        }
    }

    public void Pausar()
    {
        canvasPausa.SetActive(true);
        Time.timeScale = 0f; // Pausa el tiempo del juego
        enPausa = true;
    }

    public void Continuar()
    {
        canvasPausa.SetActive(false);
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        enPausa = false;
    }
}