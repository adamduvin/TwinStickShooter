using UnityEngine;

/// <summary>
/// Plays back a haptic clip.
/// </summary>
///
/// Plays back the <c>HapticClip</c> assigned in the <c>clip</c> property when calling
/// <c>Play()</c>. At the moment, playback of a haptic source is not triggered automatically
/// by e.g. proximity between the <c>HapticReceiver</c> and the <c>HapticSource</c>,
/// so you need to call <c>Play()</c> to trigger playback.
///
/// <c>HapticSourceInspector</c> provides a custom editor for <c>HapticSource</c> for the Inspector
/// view. The editor allows you to assign the <c>clip</c> property.
///
/// To make use of <c>HapticSource</c>, you need to place a <c>HapticReceiver</c> component
/// in your scene.
public class HapticSource : MonoBehaviour
{
    HapticReceiver hapticReceiver;

    const int DEFAULT_PRIORITY = 128;

    /// The HapticClip the HapticSource plays.
    public HapticClip clip;

    /// <summary>
    /// The priority of the <c>HapticSource</c>.
    /// </summary>
    ///
    /// This property is set by <c>HapticSourceInspector</c>. 0 is the highest priority and 256
    /// is the lowest priority.
    public int priority = DEFAULT_PRIORITY;

    void Start()
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
        hapticReceiver = hapticReceivers[0];
    }

    /// <summary>
    /// Loads and plays back the haptic clip.
    /// </summary>
    ///
    /// At the moment only one haptic clip at a time can be played.
    /// However, if the incoming haptic has a higher priority than the one currently
    /// being played it will steal playback.
    public void Play()
    {
        if (hapticReceiver != null)
        {
            if (CanPlay())
            {
                hapticReceiver.Load(clip.GetData());
                hapticReceiver.SetPlayingPriority(priority);
                hapticReceiver.Play();
            }
        }
    }

    private bool CanPlay()
    {
        if (!hapticReceiver.IsPlaying() || (hapticReceiver.IsPlaying() && priority <= hapticReceiver.GetPlayingPriority()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Stops playback that was previously started with <c>Start()</c>.
    /// </summary>
    public void Stop()
    {
        if (hapticReceiver != null)
        {
            hapticReceiver.Stop();
        }
    }
}
