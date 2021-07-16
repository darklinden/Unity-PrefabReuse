using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PrefabReuserItemAnimator : PrefabReuserItem
{
    [SerializeField] private Animator animator = null;
    private static int iTrigerReuse = Animator.StringToHash("Reuse");
    public override void OnReuse(PrefabReuser parent)
    {
        base.OnReuse(parent);
        if (animator != null) animator.SetTrigger(iTrigerReuse);
    }
}
