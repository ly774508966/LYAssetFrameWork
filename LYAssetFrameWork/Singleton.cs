using UnityEngine;

namespace LYAssetFrameWork
{
    /// <summary>
    /// 单例
    /// </summary>
    /// <typeparam name="T">class类型</typeparam>
    public class Singleton<T>
        where T : class, new()
    {
        private static T m_instance;

        private static readonly object m_lock = new object();

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if (m_instance == null)
                        {
                            m_instance = new T();
                        }
                    }
                }
                return m_instance;
            }
        }
    }

    public class SingletonMono<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T m_instance;

        private static readonly object m_lock = new object();

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        GameObject obj = new GameObject();
                        m_instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                        obj.name = "Singleton_" + m_instance.GetType().Name;
                    }    
                }
                return m_instance;
            }
        }
    }
}
