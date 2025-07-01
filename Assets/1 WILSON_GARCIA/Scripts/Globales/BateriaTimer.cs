using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BateriaTimer : MonoBehaviour
{
    public float tiempoLimite = 60f;
    public Image imagenBateria;

    public GameObject canvasMuerte; // Asigna el Canvas de Muerte aquí

    private float tiempoRestante;
    private bool yaMurio = false;

    void Start()
    {
        tiempoRestante = tiempoLimite;
    }

    void Update()
    {
        if (yaMurio) return;

        tiempoRestante -= Time.deltaTime;

        float porcentaje = Mathf.Clamp01(tiempoRestante / tiempoLimite);
        imagenBateria.fillAmount = porcentaje;

        if (tiempoRestante <= 0f)
        {
            yaMurio = true;
            canvasMuerte.SetActive(true); // Mostrar el Canvas de Muerte
            StartCoroutine(ReiniciarEscena()); // Esperar y reiniciar
        }
    }

    IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos como en tu otro script
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}