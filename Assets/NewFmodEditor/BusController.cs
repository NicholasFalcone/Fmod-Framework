using UnityEngine;
using FmodEditor;


public class BusController : MonoBehaviour
{
    public FmodBus data;

    private void Start()
    {
        data.Init();
    }
    
}
