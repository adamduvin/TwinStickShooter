using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public HapticSource hapticA;
    public AudioSource audioA;
    public HapticSource hapticB;
    public AudioSource audioB;
    public HapticClip Achievement_1;
    public HapticClip Achievement_2;
    public HapticClip Achievement_3;
    public Image AchieveButton1;
    public Image AchieveButton2;
    public Image AchieveButton3;
    public Image HighButton;
    public Image MatchedButton;
    public Image LowButton;

    public void Start()
    {
        // Default to Achievement_1
        hapticA.clip = Achievement_1;
        AchieveButton1.color = new Color32(255, 67, 56, 255);
        AchieveButton2.color = new Color32(255, 255, 255, 255);
        AchieveButton3.color = new Color32(255, 255, 255, 255);

        // Default to A > B voice priority
        hapticA.priority = 128;
        hapticB.priority = 255;
        HighButton.color = new Color32(255, 67, 56, 255);
        MatchedButton.color = new Color32(255, 255, 255, 255);
        LowButton.color = new Color32(255, 255, 255, 255);

        Debug.Log("Device supports Lofelt Haptics: " + HapticReceiver.DeviceMeetsMinimumRequirements());
    }

    public void SelectAchievement1Handler()
    {
        hapticA.clip = Achievement_1;
        AchieveButton1.color = new Color32(255, 67, 56, 255);
        AchieveButton2.color = new Color32(255, 255, 255, 255);
        AchieveButton3.color = new Color32(255, 255, 255, 255);
    }

    public void SelectAchievement2Handler()
    {
        hapticA.clip = Achievement_2;
        AchieveButton1.color = new Color32(255, 255, 255, 255);
        AchieveButton2.color = new Color32(255, 67, 56, 255);
        AchieveButton3.color = new Color32(255, 255, 255, 255);

    }

    public void SelectAchievement3Handler()
    {
        hapticA.clip = Achievement_3;
        AchieveButton1.color = new Color32(255, 255, 255, 255);
        AchieveButton2.color = new Color32(255, 255, 255, 255);
        AchieveButton3.color = new Color32(255, 67, 56, 255);
    }

    public void PlayAchievementHandler()
    {
        audioA.Play();
        hapticA.Play();
    }

    public void StopAchievementHandler()
    {
        audioA.Stop();
        hapticA.Stop();
    }

    public void HighPriorityHandler()
    {
        hapticA.priority = 128;
        hapticB.priority = 255;
        HighButton.color = new Color32(255, 67, 56, 255);
        MatchedButton.color = new Color32(255, 255, 255, 255);
        LowButton.color = new Color32(255, 255, 255, 255);
    }

    public void LowPriorityHandler()
    {
        hapticA.priority = 128;
        hapticB.priority = 0;
        HighButton.color = new Color32(255, 255, 255, 255);
        MatchedButton.color = new Color32(255, 255, 255, 255);
        LowButton.color = new Color32(255, 67, 56, 255);
    }

    public void MatchedPriorityHandler()
    {
        hapticA.priority = 128;
        hapticB.priority = 128;
        HighButton.color = new Color32(255, 255, 255, 255);
        MatchedButton.color = new Color32(255, 67, 56, 255);
        LowButton.color = new Color32(255, 255, 255, 255);
    }

    public void PlayStrokeHandler()
    {
        audioB.Play();
        hapticB.Play();
    }

    public void QuitButtonHandler()
    {
        Application.Quit();
    }

}
