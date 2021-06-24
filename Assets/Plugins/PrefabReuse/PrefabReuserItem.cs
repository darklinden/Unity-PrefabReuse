using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


[DisallowMultipleComponent]
public class PrefabReuserItem : MonoBehaviour
{
    public virtual void SetData(object data)
    {

    }

    private PrefabReuser parentReuser = null;
    public virtual void OnReuse(PrefabReuser parent)
    {
        parentReuser = parent;
    }

    public virtual void UseCompletion()
    {
        if (parentReuser != null)
        {
            parentReuser.Recycle(gameObject);
        }
        else
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
