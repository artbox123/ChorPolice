using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ArtboxGames
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public Text coinCountText, timeText, gameOverScore, gameOverBestScore,
            gameOverStarPoints, gameOverTimePoints, totalStarCount; //ref to text

        public GameObject menuPanel, gameMenu, gameOverMenu, joystick, shopMenu; //main menu

        private bool gameStarted = false;
        private float heldTime = 0.0f, timePoints = 0;

        public bool GameStarted
        {
            get { return gameStarted; }
            set { gameStarted = value; }
        }

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            //set the basic values at the game start
            GameManager.instance.currentScore = 0;
            GameManager.instance.currentStars = 0;
            GameManager.instance.isGameOver = false;

            //Debug.Log("music : "+)
            //sound button
            if (GameManager.instance.isMusicOn == true)
            {
                AudioListener.volume = 1;
                SoundManager.instance.mainMenuUI.soundImage.sprite = vars.soundOnImg;
            }
            else
            {
                AudioListener.volume = 0;
                SoundManager.instance.mainMenuUI.soundImage.sprite = vars.soundOffImg;
            }

            //vibrate button
            if (GameManager.instance.isVibrateOn == true)
            {
                SoundManager.instance.mainMenuUI.vibrateImage.sprite = vars.soundOnImg;
            }
            else
            {
                SoundManager.instance.mainMenuUI.vibrateImage.sprite = vars.soundOffImg;
            }

            if (GameManager.instance.restart)
            {
                PlayBtn();
                GameManager.instance.restart = false;
                if (GameManager.instance.isMusicOn == true)
                    AudioListener.volume = 1;
            }

            totalStarCount.text = "" + GameManager.instance.coins;
            coinCountText.text = "" + GameManager.instance.currentStars;
            timeText.text = "" + GameManager.instance.currentScore;
        }

        // Update is called once per frame
        void Update()
        {
            //we check if game is started
            if (gameStarted == true && GameManager.instance.isGameOver == false)
            {   //with every second we increase score by 1
                heldTime += Time.deltaTime;
                if (heldTime >= 1)
                {
                    GameManager.instance.currentScore += (int)heldTime;
                    timeText.text = "" + GameManager.instance.currentScore;
                    timePoints += (int)heldTime;
                    heldTime -= (int)heldTime;
                }

                if (GameManager.instance.currentScore > GameManager.instance.highScore)
                {
                    GameManager.instance.highScore = GameManager.instance.currentScore;
                    GameManager.instance.Save();
                }
            }

            if (gameStarted == false)
            {
                totalStarCount.text = "" + GameManager.instance.coins;
            }
        }

        public void PlayBtn()
        {
            SoundManager.instance.ButtonPress();
            GameManager.instance.isGameOver = false;

            menuPanel.SetActive(false);
            shopMenu.SetActive(false);
            gameMenu.SetActive(true);
            gameStarted = true;
            joystick.SetActive(true);
            CarSpawner.instance.SpawnPlane();
        }

        public void MenuBtn()
        {
            SoundManager.instance.ButtonPress();
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        public void ShareBtn()
        {
            SoundManager.instance.ButtonPress();
            new NativeShare().SetSubject("Share").SetText("I realy enjoy this game, download from play store : " + vars.rateButtonUrl).Share();
        }

        public void Restart()
        {
            SoundManager.instance.ButtonPress();
            GameManager.instance.restart = true;
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        public void MoreStarBtn()
        {
            SoundManager.instance.ButtonPress();
            //show reward ads here
        }

        public void LeaderboardBtn()
        {
            SoundManager.instance.ButtonPress();
            FindAnyObjectByType<Leadersboard>().ShowLeadersboard();
        }

        public void RateUsBtn()
        {
            SoundManager.instance.ButtonPress();
            Application.OpenURL(vars.rateButtonUrl);
        }

        public void SoundBtn()
        {
            SoundManager.instance.ButtonPress();

            if (GameManager.instance.isMusicOn == true)
            {
                GameManager.instance.isMusicOn = false;
                AudioListener.volume = 0;
                SoundManager.instance.mainMenuUI.soundImage.sprite = vars.soundOffImg;
                GameManager.instance.Save();
            }
            else
            {
                GameManager.instance.isMusicOn = true;
                AudioListener.volume = 1;
                SoundManager.instance.mainMenuUI.soundImage.sprite = vars.soundOnImg;
                GameManager.instance.Save();
            }
        }

        public void VibrateBtn()
        {
            SoundManager.instance.ButtonPress();

            if (GameManager.instance.isVibrateOn == true)
            {
                GameManager.instance.isVibrateOn = false;
                SoundManager.instance.mainMenuUI.vibrateImage.sprite = vars.soundOffImg;
                GameManager.instance.Save();
            }
            else
            {
                GameManager.instance.isVibrateOn = true;
                SoundManager.instance.mainMenuUI.vibrateImage.sprite = vars.soundOnImg;
                GameManager.instance.Save();
            }
        }

        public void GameOver()
        {
            if (gameOverMenu.activeSelf)
                return;

            gameStarted = false;
            joystick.SetActive(false);

            gameOverScore.text = "" + GameManager.instance.currentScore;
            gameOverBestScore.text = "Best " + GameManager.instance.highScore;

            FindAnyObjectByType<Leadersboard>().ReportScore(GameManager.instance.highScore);
            if (GameManager.instance.currentScore > GameManager.instance.highScore)
            {
                GameManager.instance.highScore = GameManager.instance.currentScore;
                GameManager.instance.Save();
            }

            gameOverTimePoints.text = "+" + timePoints;
            gameOverStarPoints.text = "+" + (2 * GameManager.instance.currentStars);

            gameOverMenu.SetActive(true);
            AdsManager.Instance.ShowInterstitial();
        }
    }
}