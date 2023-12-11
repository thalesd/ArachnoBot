using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this.gameObject.GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }
    }
}
