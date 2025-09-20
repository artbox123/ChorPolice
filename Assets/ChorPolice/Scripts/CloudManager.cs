using UnityEngine;

namespace ArtboxGames
{
    public class CloudManager : MonoBehaviour
    {
        GameObject target; //car ref
        SpriteRenderer cloudImg;

        [HideInInspector]
        public VariablesManager vars;

        void OnEnable()
        {
            vars = Resources.Load("VariablesContainer") as VariablesManager;
        }

        // Use this for initialization
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            cloudImg = GetComponent<SpriteRenderer>();
            //in total we have 4 types of cloud , you can change the value depending on cloud
            int r = Random.Range(0, 4);
            if (r == 0)
                cloudImg.sprite = vars.cloud1;
            else if (r == 1)
                cloudImg.sprite = vars.cloud2;
            else if (r == 2)
                cloudImg.sprite = vars.cloud3;
            else if (r == 3)
                cloudImg.sprite = vars.cloud4;
        }

        // Update is called once per frame
        void Update()
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) >= 9 ||
                Mathf.Abs(transform.position.y - target.transform.position.y) >= 12)
            {
                gameObject.SetActive(false);
            }
        }
    }
}