using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketSell : MonoBehaviour
{
    private int[] itemCosts = new int[16] { 12, 10, 14, 8, 6, 4, 16, 2, 3, 32, 27, 55, 12, 12, 12, 12 };
    private Button[] itemButtons;

    [SerializeField] private GameObject buyScreen;
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject buttonsScreen;
    [SerializeField] private Text coins;
    [SerializeField] private GameObject openShopButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Seller seller;

    private Vector3 cameraOffset = new Vector3(-0.5f, 0.37f, -1.15f);
    void Start()
    {
        itemButtons = buttonsScreen.GetComponentsInChildren<Button>();
    }

    private void AddListenersToButtons()
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            itemButtons[index].onClick.AddListener(() => Sell(index));
        }
    }

    private void RemoveListenersFromButtons()
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            itemButtons[index].onClick.RemoveAllListeners();
        }
    }

    void Sell(int itemIndex)
    {
        bool operatinSuccess = player.Sell(itemIndex, itemCosts[itemIndex]);
        if(operatinSuccess)
        {
            UpdateCoinsText();
            seller.PlayWaveInteract();
            UdateItem(itemIndex);
        }      
    }

    private void UdateItem(int itemIndex)
    {
        Text itemCountText = itemButtons[itemIndex].transform.GetChild(2).GetComponent<Text>();
        int itemsCount = int.Parse(itemCountText.text) - 1;
        if (itemsCount == 0)
        {
            itemButtons[itemIndex].interactable = false;
            
        }
        itemCountText.text = itemsCount.ToString();
    }

    private void UpdateItemsNum(int[] itemsCount)
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            Text itemCountText = itemButtons[i].transform.GetChild(2).GetComponent<Text>();
            if(itemsCount[i] > 0)
            {
                itemButtons[i].interactable = true;
                itemCountText.text = itemsCount[i].ToString();
            }
            itemCountText.gameObject.SetActive(true);
        }
    }

    private void ResetAllButtons()
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            Text itemCountText = itemButtons[i].transform.GetChild(2).GetComponent<Text>();
            itemButtons[i].interactable = false;
            itemCountText.text = "0";
            itemCountText.gameObject.SetActive(false);
        }
    }

    public void SellScren(bool isOpen)
    {
        player.IsInShopSet(isOpen);
        if (isOpen)
        {
            AddListenersToButtons();
            UpdateItemsNum(player.GetItemsCount());
            UpdateCoinsText();
            closeButton.onClick.AddListener(() => SellScren(false));
            MoveCameraToShop();
        }
        else
        {
            RemoveListenersFromButtons();
            ResetAllButtons();
            player.MoveCameraBack();
            closeButton.onClick.RemoveAllListeners();
        }

        buyScreen.SetActive(isOpen);
        openShopButton.SetActive(!isOpen);
    }

    private void UpdateCoinsText()
    {
        int playerCoins = player.GetCoins();
        coins.text = playerCoins.ToString();
    }
    
    private void MoveCameraToShop()
    {
        Transform cameraTransform = Camera.main.GetComponent<Transform>();
        cameraTransform.parent = transform;
        cameraTransform.DOLocalMove(cameraOffset, 0.5f);
        cameraTransform.DOLocalRotate(new Vector3(0, -90, 0), 0.5f);
    }
}
