using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementarFovSegunVelocidad : MonoBehaviour
{
    public Transform objetoSeguido; // Referencia al objeto cuya velocidad queremos seguir
    public float velocidadMaxima = 1000f; // Velocidad máxima del objeto
    public float fovInicial = 45f; // FOV inicial de la cámara
    public float fovMaximo = 120f; // FOV máximo que queremos alcanzar

    private Camera cam;
    private float fovActual;

    private void Start()
    {
        cam = GetComponent<Camera>();
        fovActual = fovInicial;
        cam.fieldOfView = fovActual;
    }

    private void Update()
    {
        if (objetoSeguido == null)
        {
            Debug.LogWarning("No se ha asignado un objeto para seguir.");
            return;
        }

        // Obtener la velocidad actual del objeto
        float velocidadActual = objetoSeguido.GetComponent<Rigidbody>().velocity.magnitude;

        // Calcular el FOV objetivo en función de la velocidad
        float fovObjetivo = fovInicial + (velocidadActual / velocidadMaxima) * (fovMaximo - fovInicial);

        // Aplicar la interpolación suave para suavizar el cambio de FOV
        fovActual = Mathf.Lerp(fovActual, fovObjetivo, Time.deltaTime * 5f);

        // Establecer el FOV de la cámara
        cam.fieldOfView = fovActual;
    }
}
