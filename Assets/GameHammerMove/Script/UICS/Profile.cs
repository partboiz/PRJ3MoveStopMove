using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Profile : MonoBehaviour
{
    /*#region Singlton:Profile

    public static Profile Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    public List<Avatar> AvatarsList;

    [SerializeField] GameObject AvatarUITemplate;
    [SerializeField] Transform AvatarsScrollView;
    GameObject g;

    public class Avatar
    {
        public Sprite Image;
    }

    void Start()
    {
        GetAvailableAvatars();

    }
    void GetAvailableAvatars()
    {
        for (int i = 0; i < Shop.Instance.ShopItemsList.Count; i++)
        {
            if (Shop.Instance.ShopItemsList[i].IsPurchased)
            {
                AddAvatar(Shop.Instance.ShopItemsList[i].Image);
            }
        }
    }
    public void AddAvatar(Sprite img)
    {
        if (AvatarsList == null)
            AvatarsList = new List<Avatar>();
        Avatar av = new Avatar() { Image = img };
        AvatarsList.Add(av);
        g = Instantiate(AvatarUITemplate, AvatarsScrollView);
        g.transform.GetChild(0).GetComponent<Image>().sprite = av.Image;
    }*/

    public void play()
    {
        gameObject.SetActive(false);
    }
}