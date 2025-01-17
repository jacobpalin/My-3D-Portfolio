#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using System.Runtime.InteropServices;
using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsMobile(); // Returns 1 (true) for mobile, 0 (false) for desktop
#endif

    public bool CheckIfMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    return IsMobile() == 1;
#else
        return false; // Default for non-WebGL builds
#endif
    }
}