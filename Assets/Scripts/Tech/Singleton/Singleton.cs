using UnityEngine;

namespace Tech.Singleton
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }
		#region Unity Methods
		protected virtual void Awake()
		{
			if (Instance != null)
			{
				//Destroy(gameObject);
				return;
			}
			Instance = this as T;
		}
		protected virtual void OnApplicationQuit()
		{
			Instance = null;
			Destroy(gameObject);
		}
		#endregion
	}

	public class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	}
}
