using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopZone : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private Seller seller;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControl playerControl = other.GetComponent<PlayerControl>();
            playerControl.PlayWaveAnimation();
            seller.PlayWaveAnimation();
            button.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(false);
        }
    }
}
