// 26/06/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float speed = 5f;

    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float colorChangeSpeed = 2f;

    private Renderer cubeRenderer;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        if (cubeRenderer == null)
        {
            Debug.LogError("El CubeController necesita un componente Renderer adjunto al GameObject para cambiar de color.", this);
            enabled = false;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
     
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;

        transform.Translate(movement);

        if (cubeRenderer != null)
        {
            float t = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
            cubeRenderer.material.color = Color.Lerp(color1, color2, t);
        }
    }
}