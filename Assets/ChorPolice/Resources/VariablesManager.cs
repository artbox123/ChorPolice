using UnityEngine;
using System.Collections.Generic;

namespace ArtboxGames
{
    [System.Serializable]
    public class ShopItem
    {
        public Sprite carSprite;
        public int carPrice;
    }

    public class VariablesManager : ScriptableObject
    {

        [SerializeField]
        public List<ShopItem> cars = new List<ShopItem>();

        [SerializeField]
        public Sprite soundOnImg, soundOffImg;

        [SerializeField]
        public Sprite speedPowerUp, shieldPowerUp, cloud1, cloud2, cloud3, cloud4, enemySign, starSign, starSprite, pickUpsSign;

        [SerializeField]
        public Texture2D explosionEffect;

        [SerializeField]
        public AudioClip buttonSound, enemyCarSound, enemyExplosionSound, carExplosionSound, carSound,
            pickUpSound;

        // Standart Vars
        [SerializeField]
        public string adMobInterstitialID, admobRewardID, adMobBannerID, admobAppOpenID, rateButtonUrl, leaderBoardID;
    }
}