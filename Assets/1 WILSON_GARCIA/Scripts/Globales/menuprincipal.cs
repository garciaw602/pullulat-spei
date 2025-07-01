using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public string nombreEscenaJuego = "Nivel1";

    public GameObject panelControles; // ← Añadir este campo

    public void NuevoJuego()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void MostrarControles()
    {
        panelControles.SetActive(true); // Muestra el panel de controles
    }

    public void VolverAlMenu()
    {
        panelControles.SetActive(false); // Oculta el panel
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}
