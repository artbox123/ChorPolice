using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
/// <summary>
/// This script helps in saving and loading data in device
/// </summary>

namespace ArtboxGames
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private GameData data;

        //data which is not stored on device but refered while game is on
        public bool isGameOver = false, restart = false;
        public int currentScore, currentStars;
        public int gamesPlayed;

        //data to store on device
        public bool isGameStartedFirstTime;
        public bool isMusicOn;
        public bool isVibrateOn;
        public int highScore;
        public bool showRate;
        public bool[] skinUnlocked;
        public int selectedSkin;
        public int coins; //to buy new skins
                          //ref to the background music
                          //private AudioSource audio;

        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        void Awake()
        {
            instance = this;
            InitializeVariables();
        }

        //we initialize variables here
        void InitializeVariables()
        {
            //first we load any data is avialable
            Load();
            if (data != null)
            {
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
            }
            else
            {
                isGameStartedFirstTime = true;
            }
            if (isGameStartedFirstTime)
            {
                //when game is started for 1st time on device we set the initial values
                isGameStartedFirstTime = false;

                highScore = 0;

                coins = 0;

                skinUnlocked = new bool[vars.cars.Count];
                skinUnlocked[0] = true;
                for (int i = 1; i < skinUnlocked.Length; i++)
                {
                    skinUnlocked[i] = false;
                }
                selectedSkin = 0;

                isMusicOn = true;
                isVibrateOn = true;
                showRate = true;
                data = new GameData();

                //storing data
                data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                data.setIsMusicOn(isMusicOn);
                data.setIsVibrateOn(isVibrateOn);
                data.setHiScore(highScore);
                data.setShowRate(showRate);
                data.setSkinUnlocked(skinUnlocked);
                data.setStars(coins);
                data.setSelectedSkin(selectedSkin);

                Save();
                Load();
            }
            else
            {
                //getting data
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
                isMusicOn = data.getIsMusicOn();
                isVibrateOn = data.getIsVibrateOn();
                highScore = data.getHiScore();
                showRate = data.getShowRate();
                coins = data.getStars();
                selectedSkin = data.getSelectedSkin();
                skinUnlocked = data.getSkinUnlocked();
            }
        }

        void Update()
        {//here we control the background music
         //if (isGameOver == false && audio.isPlaying == false)
         //{
         //    audio.Play();
         //}
         //else if (isGameOver == true)
         //{
         //    audio.Stop();
         //}
        }

        //method to save data
        public void Save()
        {
            FileStream file = null;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                //Debug.Log("path : " + Application.persistentDataPath);
                file = File.Create(Application.persistentDataPath + "/ChorPolice.dat");
                if (data != null)
                {
                    data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                    data.setHiScore(highScore);
                    data.setIsMusicOn(isMusicOn);
                    data.setIsVibrateOn(isVibrateOn);
                    data.setShowRate(showRate);
                    data.setSkinUnlocked(skinUnlocked);
                    data.setStars(coins);
                    data.setSelectedSkin(selectedSkin);
                    bf.Serialize(file, data);
                }
            }
            catch (Exception e)
            { }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        //method to load data
        public void Load()
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/ChorPolice.dat", FileMode.Open);//here we get saved file
                data = (GameData)bf.Deserialize(file);
            }
            catch (Exception e) { }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        //for resetting the gameManager

        public void ResetGameManager()
        {
            //when game is started for 1st time on device we set the initial values
            isGameStartedFirstTime = false;

            highScore = 0;

            coins = 0;

            skinUnlocked = new bool[vars.cars.Count];

            skinUnlocked[0] = true;

            for (int i = 1; i < skinUnlocked.Length; i++)
            {
                skinUnlocked[i] = false;
            }

            isMusicOn = true;
            isVibrateOn = true;
            showRate = true;
            data = new GameData();

            //storing data
            data.setIsGameStartedFirstTime(isGameStartedFirstTime);
            data.setIsMusicOn(isMusicOn);
            data.setIsVibrateOn(isVibrateOn);
            data.setHiScore(highScore);
            data.setShowRate(showRate);
            data.setSkinUnlocked(skinUnlocked);
            data.setStars(coins);
            data.setSelectedSkin(selectedSkin);
            Save();
            Load();

            Debug.Log("GameManager Reset");
        }

        public void OnSetting()
        {
            SoundManager.instance.ButtonPress();
        }
    }

    [Serializable]
    class GameData
    {
        private bool isGameStartedFirstTime;
        private bool isMusicOn;
        private bool isVibrateOn;
        private int hiScore;
        private bool showRate;
        private bool[] skinUnlocked;
        private int selectedSkin;
        private int stars; //to buy new skins

        //is game started 1st time
        public void setIsGameStartedFirstTime(bool isGameStartedFirstTime)
        {
            this.isGameStartedFirstTime = isGameStartedFirstTime;
        }

        public bool getIsGameStartedFirstTime()
        {
            return isGameStartedFirstTime;
        }

        //rate
        public void setShowRate(bool showRate)
        {
            this.showRate = showRate;
        }

        public bool getShowRate()
        {
            return showRate;
        }

        //music
        public void setIsMusicOn(bool isMusicOn)
        {
            this.isMusicOn = isMusicOn;
        }

        //vibrate
        public void setIsVibrateOn(bool isVibrateOn)
        {
            this.isVibrateOn = isVibrateOn;
        }

        public bool getIsMusicOn()
        {
            return isMusicOn;
        }

        public bool getIsVibrateOn()
        {
            return isVibrateOn;
        }

        //hi score 
        public void setHiScore(int hiScore)
        {
            this.hiScore = hiScore;
        }

        public int getHiScore()
        {
            return hiScore;
        }

        //stars
        public void setStars(int stars)
        {
            this.stars = stars;
        }

        public int getStars()
        {
            return this.stars;
        }

        //skin unlocked
        public void setSkinUnlocked(bool[] skinUnlocked)
        {
            this.skinUnlocked = skinUnlocked;
        }

        public bool[] getSkinUnlocked()
        {
            return this.skinUnlocked;
        }

        //selectedSkin
        public void setSelectedSkin(int selectedSkin)
        {
            this.selectedSkin = selectedSkin;
        }

        public int getSelectedSkin()
        {
            return this.selectedSkin;
        }
    }
}