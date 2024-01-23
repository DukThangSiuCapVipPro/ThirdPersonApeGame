using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using NotificationSamples;
using TruongNT;
using UnityEngine;
using UnityEngine.Android;

public class LocalNotificationManager : SingletonMonoBehaviour<LocalNotificationManager>
{
    #region Inspector

    [SerializeField]private GameNotificationsManager manager;
    #endregion

    #region Variables
    private string ChannelId = "fruit_noti";
    #endregion

    #region Unity Methods

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        var channel = new GameNotificationChannel(ChannelId, "Watermelon Match: Fruit Merge", "Watermelon Match Notifications");
        StartCoroutine(manager.Initialize(channel));
    }


    private void Start()
    {
       
        /*
         * No launch in 24 hours
         * When the player hasn’t launched the game for 24 hours or more
         */
        
        if (PlayerPrefs.HasKey("24h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("24h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("48h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("48h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("72h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("72h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("96h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("96h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("120h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("120h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("144h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("144h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }
        
        if (PlayerPrefs.HasKey("168h_remind"))
        {
            int notiID = PlayerPrefs.GetInt("168h_remind");
            if(notiID!=-1)
                manager.CancelNotification(notiID);
        }


        StartCoroutine(Tools.Delay(10, () =>
        {
            DateTime nextTimeNoti = DateTime.Now + new TimeSpan(24, 0, 0);
            int notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("24h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(48, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("48h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(72, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("72h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(96, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("96h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(120, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("120h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(144, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("144h_remind", notiID);

            notiID = -1;
            nextTimeNoti = DateTime.Now + new TimeSpan(168, 0, 0);
            notiID = CreateNotification("Watermelon Match: Fruit Merge",
                "Your crush is wating for you! Let's back with us!", nextTimeNoti);
            if (notiID != -1)
                PlayerPrefs.SetInt("168h_remind", notiID);
        }));
    }


    private void OnEnable()
    {
        if (manager != null)
        {
            manager.LocalNotificationDelivered += OnDelivered;
            manager.LocalNotificationExpired += OnExpired;
        }
    }

    private void OnDisable()
    {
        if (manager != null)
        {
            manager.LocalNotificationDelivered -= OnDelivered;
            manager.LocalNotificationExpired -= OnExpired;
        }
    }
    #endregion

    #region Public Methods

    public void FoodReady(int plotID,TimeSpan delayTime)
    {
#if UNITY_ANDROID || UNITY_IOS
        DateTime timeNoti =  DateTime.Now + delayTime;
        int notiID = CreateNotification("Foods ready to collect!", "Your farm is ready to harvest, it's time to continue for new crops!", timeNoti);
        if(notiID!=-1)
            PlayerPrefs.SetInt($"food_ready_{plotID}",notiID);
#endif
    }
    #endregion

    #region Private Methods
    private int CreateNotification(string title, string body,DateTime deliveryTime)
    {
#if UNITY_ANDROID || UNITY_IOS
        IGameNotification notification = manager.CreateNotification();
        if (notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = deliveryTime;
            manager.ScheduleNotification(notification);
            Debug.Log($"Init daily notification: {notification.Id} - {deliveryTime.ToString(CultureInfo.InvariantCulture)}");
            if (notification.Id != null)
                return (int)notification.Id;
        }
#endif
        return -1;

    }
    
    private void OnDelivered(PendingNotification deliveredNotification)
    {
        Debug.Log($"Notification with title: \n{deliveredNotification.Notification.Title}");
    }

    private void OnExpired(PendingNotification obj)
    {
        Debug.Log($"Notification with title \"{obj.Notification.Title}\" expired and was not displayed.");
    }
    
    #endregion
}
