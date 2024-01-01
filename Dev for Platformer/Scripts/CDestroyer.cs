using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDestroyer : MonoBehaviour
{
    public float _destroyDelayTime; // 파괴 지연 시간
    public bool _isAutoDestroy; // 자동 파괴 여부

    protected virtual void Start()
    {
        //자동 파괴 되기를 원한다면
        if (_isAutoDestroy) Destroy(); // 파괴시킴
    }

    public virtual void Destroy()
    {
        // 지정된 파괴 시간 뒤에 오브젝트를 파괴함
        Destroy(transform.root.gameObject, _destroyDelayTime); 
    }

}
