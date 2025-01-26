using UnityEngine;
using System.Collections.Generic;

public class DemoController : MonoBehaviour
{
    public List<Transform> objList;
    List<Transform> transformList;
    
    bool activateUpdate = false;

    public float amplitude = 1f; // Altura máxima del movimiento
    public float frequency = 1f; // Velocidad del movimiento
    List<Vector3> startPostions;

    public Transform target; // El GameObject al que mirará
    public float minDistance = 5f; // Distancia mínima antes de apartarse
    public float avoidSpeed = 7f; // Velocidad a la que se aparta

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformList = new List<Transform>();
        startPostions = new List<Vector3>();
        for (int i = 0; i < objList.Count; i++)
        {
            for (int j = 0; j < objList[i].childCount; j++)
            {
                transformList.Add(objList[i].GetChild(j));
                startPostions.Add(transformList[transformList.Count - 1].position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!activateUpdate) return;

        for (int i = 0; i < transformList.Count; i++)
        {
            //// Movimiento sinusoidal en el eje Y
            float newY = startPostions[i].y + Mathf.Sin(Time.time * frequency) * amplitude;
            transformList[i].position = new Vector3(transformList[i].position.x, newY, transformList[i].position.z);

            // Calcular dirección hacia el objetivo
            Vector3 directionToTarget = target.position - transformList[i].position;
            float distanceToTarget = directionToTarget.magnitude;

            // Mirar al objetivo (interpolado para suavidad)
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transformList[i].rotation = targetRotation;

            // Apartarse si está muy cerca
            if (distanceToTarget < minDistance)
            {
                // Dirección opuesta al objetivo
                Vector3 avoidDirection = -directionToTarget.normalized;

                // Moverse en la dirección opuesta
                transformList[i].position += avoidDirection * avoidSpeed * Time.deltaTime;
            }
        }
    }
    public void ActivateNoJob()
    {
        for (int i = 0; i < transformList.Count; i++)
        {
            transformList[i].GetComponent<NoJobScene1>().enabled = true;
        }
    }
    public void DesactivateNoJob()
    {
        for (int i = 0; i < transformList.Count; i++)
        {
            transformList[i].GetComponent<NoJobScene1>().enabled = false;
        }
    }
    public void ActivateGroupNoJob()
    {
        activateUpdate = true;
    }
    public void DesactivateGroupNoJob()
    {
        activateUpdate = false;
    }
    public void ActivateJob()
    {
        GetComponent<JobScript1>().enabled = true;
    }
    public void DesactivateJob()
    {
        GetComponent<JobScript1>().enabled = false;
    }
}
