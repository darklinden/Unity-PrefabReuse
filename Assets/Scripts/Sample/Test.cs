using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] PrefabReuser reuser = null;
    public void zzBtnClicked()
    {
        reuser.Show(new Vector3(reuser.RunningCount * 2, 0, 0));
    }
}
