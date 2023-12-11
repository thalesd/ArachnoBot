using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Player : Singleton<Player>
    {
        public float speed;

        [SerializeField] public Vector2 dir;

        public GameObject firstArachnoBot;
        public GameObject finishLine;

        public GameObject possessedObject;
        private GameObject _defaultControllable;

        //private ControllableArachnoBot arachnoBotControllableScript;
        new void Awake()
        {
            base.Awake();

            Instance._defaultControllable = firstArachnoBot;
            Instance.possessedObject = Instance._defaultControllable;

            Instance.dir = new Vector2(0, 0);
        }

        private void Start()
        {
            Instance.possessedObject.GetComponent<IControllableEntity>().Possess();
        }

        // Update is called once per frame
        void Update()
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                finishLine = GameObject.Find("ToBeContinued");
            }

            if (!finishLine.GetComponent<FinishLine>().stageFinished)
            {
                if (Instance.possessedObject != null)
                {
                    if (Input.GetButtonDown("Action"))
                    {
                        Instance.possessedObject.GetComponent<IControllableEntity>().Action();
                    }

                    if (Input.GetButtonDown("Cancel"))
                    {
                        if(Instance.possessedObject != Instance._defaultControllable)
                        {
                            Instance.possessedObject.GetComponent<IControllableEntity>().StopPossessing();
                            Instance.possessedObject = Instance._defaultControllable;
                            Instance.possessedObject.GetComponent<ControllableArachnoBot>().PlaySound(5);
                            Instance.possessedObject.GetComponent<IControllableEntity>().Possess();
                        }
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        Instance.possessedObject.GetComponent<ControllableArachnoBot>().Jump();
                    }

                    if (Input.GetButtonDown("Interact"))
                    {
                        Instance.possessedObject.GetComponent<IControllableEntity>().Interact();
                    }
                    Instance.dir.x = Input.GetAxisRaw("Horizontal");
                    Instance.dir.y = Input.GetAxisRaw("Vertical");
                }
            }
        }
        private void FixedUpdate()
        {
            if (!finishLine.GetComponent<FinishLine>().stageFinished)
            {
                Instance.possessedObject.GetComponent<IControllableEntity>().Move(Instance.dir, Instance.speed, Time.fixedDeltaTime);
            }
        }
    }
}
