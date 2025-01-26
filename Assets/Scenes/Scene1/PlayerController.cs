using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento

    void Update()
    {
        // Obtén las entradas del teclado
        float horizontal = Input.GetAxis("Horizontal"); // A y D
        float vertical = Input.GetAxis("Vertical");     // W y S
        float upDown = 0f;

        // Subir con Espacio y bajar con Ctrl
        if (Input.GetKey(KeyCode.Space))
        {
            upDown = 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            upDown = -1f;
        }

        // Calcula el movimiento
        Vector3 moveDirection = new Vector3(horizontal, upDown, vertical);

        // Aplica el movimiento al transform
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
