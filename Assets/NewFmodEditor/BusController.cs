using UnityEngine;
using FmodEditor;


public class BusController : MonoBehaviour
{
    public FmodBus data;

    private void Awake()
    {
        data.Init();
    }

    void OnEnable()
    {
        foreach (BusData bus in data.busData)
        {
            MuteChannel(bus, true);
        }
    }

    public void MuteChannel(BusData data, bool state)
    {
        data.Muted = state;
    }
}
