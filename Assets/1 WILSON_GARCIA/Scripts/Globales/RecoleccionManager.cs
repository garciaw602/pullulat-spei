using UnityEngine;
using UnityEngine.SceneManagement;

public class RecoleccionManager : MonoBehaviour
{
    public int plantasRecolectadas = 0;
    public int plantasObjetivo = 5;
    public GameObject mensajeVictoria;
    public float tiempoAntesDeSalir = 3f;
    public string escenaMenu = "MainMenu";

    private bool victoriaMostrada = false;

    public void PlantaRecolectada()
    {
        plantasRecolectadas++;

        if (plantasRecolectadas >= plantasObjetivo && !victoriaMostrada)
        {
            victoriaMostrada = true;

            if (mensajeVictoria != null)
                mensajeVictoria.SetActive(true);

            Invoke(nameof(RegresarAlMenu), tiempoAntesDeSalir);
        }
    }

    void RegresarAlMenu()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}
