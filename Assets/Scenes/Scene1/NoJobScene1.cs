using UnityEngine;
using System.Collections.Generic;

public class NoJobScene1 : MonoBehaviour
{
    public float amplitude = 1f; // Altura m�xima del movimiento
    public float frequency = 1f; // Velocidad del movimiento
    private Vector3 startPosition; // Posici�n inicial del objeto

    public Transform target; // El GameObject al que mirar�
    public float minDistance = 5f; // Distancia m�nima antes de apartarse
    public float avoidSpeed = 3f; // Velocidad a la que se aparta

    private void OnEnable()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimiento sinusoidal
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Calcular direcci�n hacia el objetivo
        Vector3 directionToTarget = target.position - transform.position;

        // Mirar al objetivo
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;

        //distancia
        float distanceToTarget = directionToTarget.magnitude;
        // Apartarse si est� muy cerca
        if (distanceToTarget < minDistance)
        {
            Vector3 avoidDirection = -directionToTarget.normalized;
            transform.position += avoidDirection * avoidSpeed * Time.deltaTime;
        }
    }
}
