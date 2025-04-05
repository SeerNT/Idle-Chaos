
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    #if UNITY_ANDROID
    private AndroidNotificationChannel dailyRewardChannel;

    private void Start()
    {
        // ASK PERMISSION FOR NOTIFICATIONS
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
        }
        // CREATE AND REGISTER NOTIFICATION CHANNEL
        dailyRewardChannel = new AndroidNotificationChannel()
        {
            Id = "2305",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(dailyRewardChannel);

        PrepareDailyRewardNotification();
    }

    public void PrepareDailyRewardNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "Daily Reward is waiting!";
        notification.Text = "1 Hour is left to pick daily reward";
        notification.FireTime = System.DateTime.Now.AddSeconds(82800 - DailyManager.currentTime);

        AndroidNotificationCenter.SendNotification(notification, "2305");
    }
    #endif
}
