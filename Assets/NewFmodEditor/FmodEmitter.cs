using UnityEngine;
using FmodEditor;

public class FmodEmitter : MonoBehaviour
{
    public FmodEvent fmodEvent;

    private void Awake()
    {
        if(fmodEvent != null)
            fmodEvent.InitFmodEvent();
    }


    private void Start()
    {
        fmodEvent.PlayAudio();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            fmodEvent.ChangeParameter(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            fmodEvent.ChangeParameter(0, 0);
        }
    }

    private void OnDisable()
    {
        fmodEvent.ReleaseEvent();
    }


    private void OnDrawGizmos()
    {
        if(fmodEvent != null && fmodEvent.MaxDistance > 0)
        Gizmos.color = new Color(1,1,1,0.5f);

        Gizmos.DrawSphere(transform.position, fmodEvent.MaxDistance);
    }
}
