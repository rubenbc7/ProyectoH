using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pits : MonoBehaviour
{
    public CarControllerPlayer carControllerPlayer;
    public Deform deform;
    public UIGauge uIGaugeNos;
    public UIGauge uIGaugeHealth;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player")
        {
            carControllerPlayer.CurrentNosLeft = carControllerPlayer.MaxNOSCapacity;
            deform.carHealth = 500f;
            uIGaugeNos.ApplyCalculation(carControllerPlayer.CurrentNosLeft);
            uIGaugeHealth.ApplyCalculation(500f);
            Debug.Log("pits");
        }
    }
    
        
    
}
