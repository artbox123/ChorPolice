using UnityEngine;
using UnityEngine.UI;

namespace ArtboxGames
{
    [System.Serializable]
    public class MainMenuUI
    {
        public Image soundImage, vibrateImage;
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        private AudioSource audioSource;
        private AudioClip buttonClick;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            buttonClick = vars.buttonSound;
        }

        public MainMenuUI mainMenuUI;

        public void ButtonPress()
        {
            audioSource.PlayOneShot(buttonClick);
        }
    }
}