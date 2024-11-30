using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    #region Singleton: Shop

    public static Shop Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    [System.Serializable]
    class ShopItem
    {
        public Sprite Image;            
        public int Price;               
        public bool IsPurchased = false;
        public GameObject AssociatedGameObject;
        public bool IsEquipped = false; 
    }

    [SerializeField] List<ShopItem> ShopItemsList;
    [SerializeField] Animator NoCoinsAnim;

    GameObject ItemTemplate;
    GameObject g;
    [SerializeField] Transform ShopScrollView;
    Button buyBtn;
    Button equipBtn;

    private void Start()
    {
        ItemTemplate = ShopScrollView.GetChild(0).gameObject;
        int len = ShopItemsList.Count;
        for (int i = 0; i < len; i++)
        {
            g = Instantiate(ItemTemplate, ShopScrollView);
            g.transform.GetChild(0).GetComponent<Image>().sprite = ShopItemsList[i].Image;
            g.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = ShopItemsList[i].Price.ToString();
            buyBtn = g.transform.GetChild(2).GetComponent<Button>();
            buyBtn.interactable = !ShopItemsList[i].IsPurchased;
            buyBtn.AddEventListener(i, OnShopItemBuyClicked);
            equipBtn = g.transform.GetChild(3).GetComponent<Button>();
            equipBtn.interactable = ShopItemsList[i].IsPurchased;
            equipBtn.AddEventListener(i, OnShopItemEquipClicked);
        }
        Destroy(ItemTemplate);
        Game.Instance.UpdateAllCoinsUIText();
    }

    void OnShopItemBuyClicked(int itemIndex)
    {
        if (Game.Instance.HasEnoughCoins(ShopItemsList[itemIndex].Price))
        {
            Game.Instance.UseCoins(ShopItemsList[itemIndex].Price);
            ShopItemsList[itemIndex].IsPurchased = true;
            buyBtn = ShopScrollView.GetChild(itemIndex).GetChild(2).GetComponent<Button>();
            buyBtn.interactable = false;
            buyBtn.transform.GetChild(0).GetComponent<Text>().text = "PURCHASED";
            equipBtn = ShopScrollView.GetChild(itemIndex).GetChild(3).GetComponent<Button>();
            equipBtn.interactable = true;

            Game.Instance.UpdateAllCoinsUIText();
        }
        else
        {
            NoCoinsAnim.SetTrigger("NoCoins");
        }
    }

    void OnShopItemEquipClicked(int itemIndex)
    {
        if (ShopItemsList[itemIndex].IsPurchased)
        {   
            if (ShopItemsList[itemIndex].IsEquipped)
            {
                ShopItemsList[itemIndex].AssociatedGameObject.SetActive(false);
                ShopItemsList[itemIndex].IsEquipped = false;
                UpdateEquipButtonText(itemIndex, "Equip");
            }
            else
            {
                /*UnequipAll();*/
                ShopItemsList[itemIndex].AssociatedGameObject.SetActive(true);
                ShopItemsList[itemIndex].IsEquipped = true;
                UpdateEquipButtonText(itemIndex, "Unequip" +
                    "");
            }
        }
        else
        {
            Debug.LogWarning("item chua mua");
        }
    }

    void UnequipAll()
    {
        for (int i = 0; i < ShopItemsList.Count; i++)
        {
            if (ShopItemsList[i].IsEquipped)
            {
                ShopItemsList[i].AssociatedGameObject.SetActive(false);
                ShopItemsList[i].IsEquipped = false;
                UpdateEquipButtonText(i, "huy trang bi truoc do");
            }
        }
    }

    void UpdateEquipButtonText(int itemIndex, string text)
    {
        Transform item = ShopScrollView.GetChild(itemIndex);
        Button equipBtn = item.GetChild(3).GetComponent<Button>();
        equipBtn.transform.GetChild(0).GetComponent<Text>().text = text;
    }

    public void openShop()
    {
        gameObject.SetActive(true);
    }

    public void closeShop()
    {
        gameObject.SetActive(false);
    }
}
