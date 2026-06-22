using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    public GameObject[] objects;
    public List<GameObject> pooledObjects;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10; // Number of objects to pool

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        pooledObjects = new List<GameObject>();
        foreach (GameObject obj in objects)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledObj = Instantiate(obj, transform.parent);
                pooledObj.SetActive(false);
                pooledObjects.Add(pooledObj);
            }
        }

    }

    public GameObject GetPooledObject(Vector2 pos, string text)
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                Debug.Log("Position: " + pos);
                obj.SetActive(true);

                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
                Camera mainCamera = Camera.main;
                Vector2 localPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, pos, mainCamera, out localPosition);
                rectTransform.anchoredPosition = localPosition;

                
                obj.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
                return obj;
            }
        }

        foreach (GameObject obj in objects)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject pooledObj = Instantiate(obj, transform.parent);
                pooledObj.SetActive(false);
                pooledObjects.Add(pooledObj);
            }
        }
        return GetPooledObject(pos, text);
    }

    public void ReturnToPool(GameObject obj)
    {
            obj.SetActive(false);
    }


}
