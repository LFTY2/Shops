using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketBuy : MonoBehaviour
{
    private int[] itemCosts = new int[16] { 10, 8, 12, 7, 5, 3, 15, 1, 2, 30, 25, 50, 10, 10, 10, 10 };
    private Button[] itemButtons;
    private List<int> sellCurrent = new List<int>();

    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject buyScreen;
    [SerializeField] private GameObject buttonsScreen;
    [SerializeField] private Text coins;
    [SerializeField] private GameObject changeItemsButton;
    [SerializeField] private GameObject openShopButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Seller seller;
    
    private Vector3 cameraOffset = new Vector3(-20.4f, 10.6f, 1.3f);

    void Start()
    {
        itemButtons = buttonsScreen.GetComponentsInChildren<Button>();              
    }

    void AddListenersToButtons()
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            int index = i;
            itemButtons[index].onClick.AddListener(() => Buy(index));
        }
    }

    void RemoveListenersFromButtons()
    {
        int length = itemButtons.Length - 1;
        for (int i = 0; i < length; i++)
        {
            itemButtons[i].onClick.RemoveAllListeners();
        }
    }

    void Buy(int itemIndex)
    {
        bool oprationSuccess = player.Buy(itemIndex, itemCosts[itemIndex]);
        if(oprationSuccess)
        {
            UpdateCoinsText();
            seller.PlayWaveInteract();
        }
        
    }

    public void ChangeSellItems()
    {
        DisableCurrentSellItems();
        sellCurrent.Clear();
        for (int i = 0; i < 4; i++)
        {
            int randNum = Random.Range(0, 16);
            if(sellCurrent.Contains(randNum))
            {
                i--;
                continue;
            }
            else
            {
                sellCurrent.Add(randNum);
                ChangeItemState(randNum, true);
            }
        }
    }
    void DisableCurrentSellItems()
    {
        int curentSellNum = sellCurrent.Count;
        for (int i = 0; i < curentSellNum; i++)
        {
            ChangeItemState(sellCurrent[i], false);
        }
    }

    void ChangeItemState(int itemNum, bool isShow)
    {
        itemButtons[itemNum].interactable = isShow;

        Text cost = itemButtons[itemNum].transform.GetChild(0).GetComponent<Text>(); 
        Transform coin = itemButtons[itemNum].transform.GetChild(1);

        cost.text = itemCosts[itemNum].ToString();

        cost.gameObject.SetActive(isShow);        
        coin.gameObject.SetActive(isShow); 
    }

    public void BuyScren(bool isOpen)
    {
        player.IsInShopSet(isOpen);
        if (isOpen)
        {
            AddListenersToButtons();
            ChangeSellItems();
            UpdateCoinsText();
            MoveCameraToShop();
            closeButton.onClick.AddListener(() => BuyScren(false));
        }
        else
        {
            RemoveListenersFromButtons();
            DisableCurrentSellItems();
            player.MoveCameraBack();
            closeButton.onClick.RemoveAllListeners();
        }
        changeItemsButton.SetActive(isOpen);        
        buyScreen.SetActive(isOpen);
        openShopButton.SetActive(!isOpen);
    }

    void UpdateCoinsText()
    {
        int playerCoins = player.GetCoins();
        coins.text = playerCoins.ToString();
    }

    private void MoveCameraToShop()
    {
        Transform cameraTransform = Camera.main.GetComponent<Transform>();
        cameraTransform.parent = transform;
        cameraTransform.DOLocalMove(cameraOffset, 0.5f);
        cameraTransform.DOLocalRotate(new Vector3(0, 90, 0), 0.5f);
    }
}
