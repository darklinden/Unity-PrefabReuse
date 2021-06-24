using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DisallowMultipleComponent]
public class PrefabReuser : MonoBehaviour
{
    [SerializeField] bool ShowOnEnable = false;
    [SerializeField] private AssetReference PrefabRef;
    [SerializeField] private Transform ShowParent;

    private List<GameObject> _runningItems = new List<GameObject>();
    private Queue<GameObject> _itemPool = new Queue<GameObject>();

    public int RunningCount { get { return _runningItems.Count; } }

    private void InstantiatePrefab(Action<GameObject> loaded)
    {
        if (PrefabRef == null)
        {
            D.Error("PrefabReuser No PrefabRef", gameObject.name);
            return;
        }

        PrefabRef.InstantiateAsync().Completed += (AsyncOperationHandle<GameObject> obj) =>
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                loaded?.Invoke(obj.Result);
            }
            else
            {
                loaded?.Invoke(null);
            }
        };
    }

    private void SetupGameObject<T>(GameObject go, Vector3 position, System.Object data, Action<T> completion) where T : PrefabReuserItem
    {
        // D.Log("SetupGameObject", position);
        go.transform.parent = ShowParent != null ? ShowParent : transform;
        go.transform.position = position;
        go.SetActive(true);
        _runningItems.Add(go);

        var comp = go.GetComponent<T>();
        if (comp != null)
        {
            if (data != null) comp.SetData(data);
            comp.OnReuse(this);
            if (completion != null) completion.Invoke(comp);
        }
    }

    public void Show<T>(Vector3 position, System.Object data = null, Action<T> completion = null) where T : PrefabReuserItem
    {
        GameObject pgo = _itemPool.Count > 0 ? _itemPool.Dequeue() : null;

        if (pgo)
        {
            SetupGameObject(pgo, position, data, completion);
            return;
        }

        InstantiatePrefab((GameObject go) =>
        {
            if (go != null) SetupGameObject(go, position, data, completion);
        });
    }

    public void Show(Vector3 position, System.Object data = null)
    {
        Show<PrefabReuserItem>(position, data);
    }

    public void Show()
    {
        Show<PrefabReuserItem>(Vector3.zero);
    }

    public void Recycle(GameObject go)
    {
        for (var i = _runningItems.Count - 1; i >= 0; i--)
        {
            if (_runningItems[i] == go)
            {
                _runningItems[i].SetActive(false);
                _itemPool.Enqueue(_runningItems[i]);
                _runningItems.RemoveAt(i);
                break;
            }
        }
    }

    public void HideAll()
    {
        if (_runningItems.Count > 0)
        {
            for (var i = _runningItems.Count - 1; i >= 0; i--)
            {
                _runningItems[i].SetActive(false);
                _itemPool.Enqueue(_runningItems[i]);
                _runningItems.RemoveAt(i);
            }
        }
    }

    private void OnEnable()
    {
        if (ShowOnEnable) Show();
    }

    private void OnDestroy()
    {
        HideAll();
        while (_itemPool.Count > 0)
        {
            var item = _itemPool.Dequeue();
            Addressables.ReleaseInstance(item);
        }
    }
}