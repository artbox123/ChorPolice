using UnityEngine;
using UnityEngine.UI;

namespace ArtboxGames
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager instance;

        public Transform container;
        public Text starText;
        public GameObject watchVideo;
        public GameObject info;
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            UpdateShopItems();
        }

        //method called by select button to select the car
        public void SelectPlane(int planeIndex)
        {
            SoundManager.instance.ButtonPress();
            if (GameManager.instance.skinUnlocked[planeIndex] == true)
            {
                GameManager.instance.selectedSkin = planeIndex;
                GameManager.instance.Save();
            }
            else if (GameManager.instance.coins >= vars.cars[planeIndex].carPrice)
            {
                GameManager.instance.coins -= vars.cars[planeIndex].carPrice;
                GameManager.instance.skinUnlocked[planeIndex] = true;
                GameManager.instance.selectedSkin = planeIndex;
                GameManager.instance.Save();
                starText.text = "" + GameManager.instance.coins;
            }
            else if (GameManager.instance.coins < vars.cars[planeIndex].carPrice)
            {
                watchVideo.SetActive(true);
            }
            //then update the shop
            UpdateShopItems();
        }

        //method which controls the movement and scrolling and spawning image prefabs 
        public void UpdateShopItems()
        {
            for (int i = 0; i < container.childCount; i++)
            {
                GameObject car = container.GetChild(i).gameObject;
                GameObject coin = car.transform.Find("Price/Coin").gameObject;
                Text cost = car.transform.Find("Price/Text").GetComponent<Text>();

                if (GameManager.instance.skinUnlocked[i] == true)
                {
                    coin.SetActive(false);
                    cost.text = "Use";
                    if (GameManager.instance.selectedSkin == i)
                    {
                        cost.text = "Selected";
                    }
                }
                else
                {
                    cost.text = " " + vars.cars[i].carPrice.ToString();
                }
            }
        }

        public void WatchVideo()
        {
            SoundManager.instance.ButtonPress();
            if (AdsManager.Instance.ShowRewardVideo())
            {
                watchVideo.SetActive(false);
            }
            else
            {
                info.SetActive(true);
            }
        }
    }
}