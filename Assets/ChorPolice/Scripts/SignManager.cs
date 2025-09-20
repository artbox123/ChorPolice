using UnityEngine;

namespace ArtboxGames
{
    public class SignManager : MonoBehaviour
    {
        private SpriteRenderer signImg;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        void Start()
        {
            signImg = GetComponent<SpriteRenderer>();
            if (gameObject.name == "carSign(Clone)")
            {
                signImg.sprite = vars.enemySign;
            }
            else if (gameObject.name == "pointSign(Clone)")
            {
                signImg.sprite = vars.starSign;
            }
            else if (gameObject.name == "pickUpSign(Clone)")
            {
                signImg.sprite = vars.pickUpsSign;
            }
        }

        //method called when sign is to be shown
        public void SetPos(float xPos, float yPos)
        {   //position of sign is set
            transform.position = new Vector3(xPos, yPos, 10f);
        }
    }
}