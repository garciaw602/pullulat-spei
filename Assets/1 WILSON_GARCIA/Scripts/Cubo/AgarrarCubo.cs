using UnityEngine;

public class AgarrarCubo : MonoBehaviour
{
    public Transform puntoDeSujecion;      // Lugar donde se sujeta el cubo (por ejemplo, encima del jugador)
    public float rangoDeAgarre = 1.5f;     // Qu� tan cerca debe estar el cubo
    private GameObject cuboAgarrado = null;

    void Update()
    {
        // Si presiona W o flecha arriba
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Si ya est� sosteniendo un cubo
            if (cuboAgarrado != null)
            {
                cuboAgarrado.transform.position = puntoDeSujecion.position;
                cuboAgarrado.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
            else
            {
                // Buscar un cubo cercano
                Collider[] colisiones = Physics.OverlapSphere(transform.position, rangoDeAgarre);

                foreach (Collider col in colisiones)
                {
                    if (col.CompareTag("Cubo"))
                    {
                        cuboAgarrado = col.gameObject;
                        // Desactiva gravedad y bloquea movimiento f�sico
                        cuboAgarrado.GetComponent<Rigidbody>().useGravity = false;
                        cuboAgarrado.GetComponent<Rigidbody>().isKinematic = true;
                        break;
                    }
                }
            }
        }
        else if (cuboAgarrado != null) // Si solt� la tecla y hab�a un cubo agarrado
        {
            // Activar f�sica nuevamente y soltar
            Rigidbody rb = cuboAgarrado.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.isKinematic = false;
            cuboAgarrado = null;
        }
    }
}
