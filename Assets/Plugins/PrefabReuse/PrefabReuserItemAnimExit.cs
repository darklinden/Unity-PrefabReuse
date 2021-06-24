using UnityEngine;

public class PrefabReuserItemAnimExit : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform trans = animator.gameObject.transform;
        PrefabReuserItem item = trans.GetComponent<PrefabReuserItem>();
        int tryCount = 3;
        while (item == null && trans.parent != null && tryCount > 0)
        {
            trans = trans.parent;
            tryCount--;
            item = trans.GetComponent<PrefabReuserItem>();
        }

        if (item != null) item.UseCompletion();
    }
}
