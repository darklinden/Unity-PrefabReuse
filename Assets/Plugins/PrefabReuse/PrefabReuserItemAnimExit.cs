using UnityEngine;

public class PrefabReuserItemAnimExit : StateMachineBehaviour
{
    [SerializeField] private int TryCount = 3;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform trans = animator.gameObject.transform;
        PrefabReuserItem item = trans.GetComponent<PrefabReuserItem>();
        int tryCount = TryCount;
        while (item == null && trans.parent != null && tryCount > 0)
        {
            trans = trans.parent;
            tryCount--;
            item = trans.GetComponent<PrefabReuserItem>();
        }

        if (item != null) item.UseCompletion();
    }
}
