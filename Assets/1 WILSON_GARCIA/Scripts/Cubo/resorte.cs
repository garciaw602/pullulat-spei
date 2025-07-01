using UnityEngine;

public class Resorte : MonoBehaviour
{
    public float fuerzaImpulso = 10f;
    public Vector3 direccion = Vector3.up;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Anula la velocidad vertical antes de impulsar
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

                // Aplica el impulso en la dirección deseada
                rb.AddForce(direccion.normalized * fuerzaImpulso, ForceMode.Impulse);
            }
        }
    }
}
