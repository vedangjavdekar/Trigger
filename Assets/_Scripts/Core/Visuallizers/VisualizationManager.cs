using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VisualizationManager : MonoBehaviour
{
    ObjectPool<GameObject> m_CirclePool = null;
    ObjectPool<GameObject> m_LinePool = null;

    [SerializeField]
    private GameObject m_CirclePrefab;
    [SerializeField]
    private GameObject m_LinePrefab;

    public static VisualizationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != this)
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        if (m_CirclePrefab)
        {
            m_CirclePool = new ObjectPool<GameObject>(() => Instantiate(m_CirclePrefab), (GameObject obj) => obj.SetActive(true), (GameObject obj) => obj.SetActive(false), (GameObject obj) => Destroy(obj), false, 20);
        }

        if(m_LinePrefab)
        {
            m_LinePool = new ObjectPool<GameObject>(() => Instantiate(m_LinePrefab), (GameObject obj) => obj.SetActive(true), (GameObject obj) => obj.SetActive(false), (GameObject obj) => Destroy(obj), false, 20);
        }
    }

    private void OnDisable()
    {
        m_CirclePool.Dispose();
        m_LinePool.Dispose();
    }

    private void OnDestroy()
    {
        m_CirclePool.Dispose();
        m_LinePool.Dispose();
    }

    public GameObject GetCircle()
    {
        if (m_LinePool == null)
        {
            Debug.LogError("Pool Not created! Ensure that prefabs are set");
            return null;
        }
        return m_CirclePool.Get();
    }
    public void ReturnCircle(GameObject circle)
    {
        if (m_CirclePool == null)
        {
            Debug.LogError("Pool Not created! Ensure that prefabs are set");
            return;
        }
        m_CirclePool.Release(circle);
    }
    public GameObject GetLine()
    {
        if (m_LinePool == null)
        {
            Debug.LogError("Pool Not created! Ensure that prefabs are set");
            return null;
        }
        return m_LinePool.Get();
    }
    public void ReturnLine(GameObject circle)
    {
        if (m_LinePool == null)
        {
            Debug.LogError("Pool Not created! Ensure that prefabs are set");
            return;
        }
        m_LinePool.Release(circle);
    }
}
