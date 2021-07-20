using UnityEngine;
using System;

#if (UNITY_ANDROID && !UNITY_EDITOR)
using System.Text;
#elif (UNITY_IOS && !UNITY_EDITOR)
using System.Runtime.InteropServices;
using UnityEngine.iOS;
#endif

/// <summary>
/// Plays haptic clips.
/// </summary>
///
/// <c>HapticReceiver</c> receives and plays back haptics from <c>HapticSource</c> components.
/// It also provides an API to play haptics directly, but the usual way to play haptics
/// is by using the <c>HapticSource</c> component.
///
/// Your scene should have exactly one <c>HapticReceiver</c> component, similar to how a scene
/// should have exactly one <c>AudioListener</c>.
///
/// Internally, <c>HapticReceiver</c> makes use of the iOS framework or the Android library from
/// the Lofelt SDK, which are added as plugins. On other platforms like a PC or Mac,
/// <c>HapticReceiver</c> does nothing.
///
/// At the moment only one haptic clip at a time can be loaded and played back.
public class HapticReceiver : MonoBehaviour
{

    // Priority of the haptic clip playing/played
    private int playingPriority;
    // Date of playback start time
    System.DateTime playStartTime;
    // Date of playback end time
    // It can be calculated upon playback with start time + haptic clip duration
    // or it is set when Stop() is called
    System.DateTime playEndTime;
    // Duration of the loaded haptic clip
    float hapticClipLoadedDuration;

    // Flag indicating if the device supports Lofelt Haptics minimum requirements
    bool deviceMeetsMinimumRequirements;

#if (UNITY_ANDROID && !UNITY_EDITOR)
    AndroidJavaObject lofeltHaptics;
    static int MinimumSupportedAndroidVersion = 26;
#elif (UNITY_IOS && !UNITY_EDITOR)
    // imports of iOS Framework bindings
    // All of the following functions that are dealing with raw pointers have the unsafe keyword.


    [DllImport("__Internal")]
    private static extern bool lofeltHapticsDeviceMeetsMinimumRequirementsBinding();

    [DllImport("__Internal")]
    private static extern IntPtr lofeltHapticsInitBinding();

    // Use Marshalling to convert string into a pointer to a null-terminated char array.
    [DllImport("__Internal")]
    private static extern bool lofeltHapticsLoadBinding(IntPtr controller, [MarshalAs(UnmanagedType.LPStr)] string data);

    [DllImport("__Internal")]
    private static extern bool lofeltHapticsPlayBinding(IntPtr controller);

    [DllImport("__Internal")]
    private static extern bool lofeltHapticsStopBinding(IntPtr controller);

    [DllImport("__Internal")]
    private static extern float lofeltHapticsGetClipDurationBinding(IntPtr controller);

    [DllImport("__Internal")]
    private static extern bool lofeltHapticsReleaseBinding(IntPtr controller);

    IntPtr controller;

    static int MinimumSupportedIOSVersion = 13;

#endif

    /// <summary>
    /// Initializes the component by creating an instance of the <c>LofeltHaptics</c>
    /// class from the Lofelt SDK.
    /// </summary>
    void Start()
    {

        if (IsVersionSupported())
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var context = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                lofeltHaptics = new AndroidJavaObject("com.lofelt.haptics.LofeltHaptics", context);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
                controller = lofeltHapticsInitBinding();
#endif
        }

        deviceMeetsMinimumRequirements = DeviceMeetsMinimumPlatformRequirements();
        // Set default start values
        playingPriority = 0;
        hapticClipLoadedDuration = 0.0f;
        playStartTime = System.DateTime.Now;
        playEndTime = System.DateTime.Now;
    }
    /// <summary>
    /// Indicates if the device meets the requirements to play Lofelt Haptics.
    /// In a nutshell, the requirements are the following:
    /// - iOS: iPhone 8 and newer, and iOS 13 and newer will return True; otherwise False.
    /// - Android: Android API level < 26 will return False; API level >= 26
    /// -          will return True if the Vibrator API has amplitude control.
    /// </summary>
    /// <returns>
    /// Returns `True`if the device supports the minimum requirements; false otherwise.
    /// Throws excpetion if `HapticReceiver` is not a component of any game object.
    /// </returns>
    public static bool DeviceMeetsMinimumRequirements()
    {
        var hapticReceivers = FindObjectsOfType(typeof(HapticReceiver)) as HapticReceiver[];

        if (hapticReceivers.Length == 0)
        {
            throw new System.Exception("Unable to find HapticReceiver component");
        }
        if (hapticReceivers.Length > 1)
        {
            throw new System.Exception("There is more than one HapticReceiver component in the scene. Please ensure there is always exactly one HapticReceiver component in the scene.");
        }

        HapticReceiver hapticReceiver = hapticReceivers[0];

        return hapticReceiver.deviceMeetsMinimumRequirements;
    }

    /// <summary>
    /// Checks if the device OS version supports Lofelt Haptics playback.
    /// </summary>
    /// <returns>
    /// Returns `True` if the device OS version is above or equal the minimum supported version for
    /// Lofelt Haptics playback, and `False` otherwise.
    /// </returns>
    bool IsVersionSupported()
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
        var buildClass = AndroidJNI.FindClass("android/os/Build$VERSION");
        var buildFieldId = AndroidJNI.GetStaticFieldID(buildClass, "SDK_INT", "I");
        int androidSDKLevel = AndroidJNI.GetStaticIntField(buildClass, buildFieldId);

        if (androidSDKLevel < MinimumSupportedAndroidVersion)
        {
            return false;
        }
        else
        {
            return true;
        }
#elif (UNITY_IOS && !UNITY_EDITOR)
        int iOSVersion = 0;
        string versionString = Device.systemVersion;
        string[] versionArray = versionString.Split('.');

        if(int.TryParse(versionArray[0], out iOSVersion)) {
            if(iOSVersion < MinimumSupportedIOSVersion) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
#elif (UNITY_EDITOR)
        return true;
#endif
    }

    /// <summary>
    /// Calls platform specific functions to check if the device supports the minimum requirements
    /// of the platform.
    /// </summary>
    /// <returns>Returns `True`if the device supports the minimum requirements; `False` otherwise.</returns>
    bool DeviceMeetsMinimumPlatformRequirements()
    {
        if (IsVersionSupported())
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            // lofeltHaptics.Call("deviceMeets")
            bool deviceMeetsRequirements = lofeltHaptics.Call<bool>("deviceMeetsMinimumRequirements");
            return deviceMeetsRequirements;
#elif (UNITY_IOS && !UNITY_EDITOR)
                if(lofeltHapticsDeviceMeetsMinimumRequirementsBinding()) {
                    return true;
                } else {
                    return false;
                }

#elif (UNITY_EDITOR)
            return true;
#endif
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Loads a haptic clip for later playback.
    /// </summary>
    /// <param name="data">The haptic clip, as a JSON string of the <c>.haptic</c> file content</param>
    public void Load(string data)
    {
        if (deviceMeetsMinimumRequirements)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            lofeltHaptics.Call("load", Encoding.UTF8.GetBytes(data));
            hapticClipLoadedDuration = lofeltHaptics.Call<float>("getClipDuration");
#elif (UNITY_IOS && !UNITY_EDITOR)
                lofeltHapticsLoadBinding(controller, data);
                hapticClipLoadedDuration = lofeltHapticsGetClipDurationBinding(controller);
#endif
        }
    }

    /// <summary>
    /// Plays the haptic clip that was previously loaded with <c>Load()</c>.
    /// </summary>
    public void Play()
    {
        if (deviceMeetsMinimumRequirements)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            lofeltHaptics.Call("play");
#elif (UNITY_IOS && !UNITY_EDITOR)
                lofeltHapticsPlayBinding(controller);
#endif
        }
        playStartTime = System.DateTime.Now;
        playEndTime = playStartTime.AddSeconds(hapticClipLoadedDuration);
    }

    /// <summary>
    /// Stops playback of the haptic clip that was previously started with <c>Play()</c>.
    /// </summary>
    public void Stop()
    {
        if (deviceMeetsMinimumRequirements)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            lofeltHaptics.Call("stop");
#elif (UNITY_IOS && !UNITY_EDITOR)
                lofeltHapticsStopBinding(controller);
#endif
        }
        playEndTime = System.DateTime.Now;
    }

    /// <summary>
    /// Checks if a loaded haptic clip is playing
    /// </summary>
    /// <returns>Returns `true` if a haptic clip is playing, `false` otherwise </returns>
    public bool IsPlaying()
    {
        if (System.DateTime.Now < playEndTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///  Sets playing priority for the haptic clip triggered to be played
    /// </summary>
    /// <param name="priority"></param>
    public void SetPlayingPriority(int priority)
    {
        playingPriority = priority;
    }

    /// <summary>
    ///  Returns the playing priority of the haptic clip playing
    /// </summary>
    /// <returns>The priority value of the current playing haptic clip</returns>
    public int GetPlayingPriority()
    {
        return playingPriority;
    }

    /// <summary>
    /// Destroys the instance of the <c>LofeltHaptics</c> class from the SDK.
    /// </summary>
    void OnDestroy()
    {
        if (deviceMeetsMinimumRequirements)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            lofeltHaptics.Dispose();
            lofeltHaptics = null;
#elif (UNITY_IOS && !UNITY_EDITOR)
                lofeltHapticsReleaseBinding(controller);
#endif
        }
    }

    /// <summary>
    /// Manages LofeltHaptics when the application focus changes.
    ///
    /// It will stop haptics once the app loses focus.
    /// On iOS, it will also release the haptics controller when the app loses
    /// focus. It will initialize it again once the app gains focus
    /// </summary>
    /// <param name="hasFocus"></param>
    void OnApplicationFocus(bool hasFocus)
    {
        if (deviceMeetsMinimumRequirements)
        {
            if (hasFocus)
            {
#if (UNITY_IOS && !UNITY_EDITOR)
                    controller = lofeltHapticsInitBinding();
#endif
            }
            else
            {
                Stop();
#if (UNITY_IOS && !UNITY_EDITOR)
                    lofeltHapticsReleaseBinding(controller);
#endif
            }
        }
    }
}
