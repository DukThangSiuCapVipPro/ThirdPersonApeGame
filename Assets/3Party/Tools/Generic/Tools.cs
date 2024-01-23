using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using DG.Tweening;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TruongNT
{
    public static class Tools
    {

        public static bool openUrl(string packageName)
        {
#if UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject pManager = unityActivity.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaObject intent = null;
            try
            {
                intent = pManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
                unityActivity.Call("startActivity", intent);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to Opeen App: " + e.Message);
                //Open with Browser
                string link = "https://play.google.com/store/apps/details?id=" + packageName + "&hl=en";

                Application.OpenURL(link);
                return false;
            }
#endif
            return false;
        }
        public static string UpperFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public static List<List<T>> Split<T>(this List<T> source, int chunkSize)
        {
            if (source == null)
                source = new List<T>();
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static Vector2Int CalculatePartPos(int index, int bgSize)
        {
            int x = 0;
            int y = 0;
            switch (bgSize)
            {
                case 1:
                    x = 0;
                    y = 0;
                    break;
                case 2:
                    if (index == 0)
                    {
                        x = 0;
                        y = 0;
                    }
                    else if (index == 1)
                    {
                        x = -1;
                        y = 0;
                    }

                    break;
                case 4:
                    if (index == 0)
                    {
                        x = 0;
                        y = 0;
                    }
                    else if (index == 1)
                    {
                        x = -1;
                        y = 0;
                    }
                    else if (index == 2)
                    {
                        x = 0;
                        y = -1;
                    }
                    else if (index == 3)
                    {
                        x = -1;
                        y = -1;
                    }

                    break;
                case 9:
                    if (index == 0)
                    {
                        x = 0;
                        y = 0;
                    }
                    else if (index == 1)
                    {
                        x = -1;
                        y = 0;
                    }
                    else if (index == 2)
                    {
                        x = -2;
                        y = 0;
                    }
                    else if (index == 3)
                    {
                        x = 0;
                        y = -1;
                    }
                    else if (index == 4)
                    {
                        x = -1;
                        y = -1;
                    }
                    else if (index == 5)
                    {
                        x = -2;
                        y = -1;
                    }
                    else if (index == 6)
                    {
                        x = 0;
                        y = -2;
                    }
                    else if (index == 7)
                    {
                        x = -1;
                        y = -2;
                    }
                    else if (index == 8)
                    {
                        x = -2;
                        y = -2;
                    }

                    break;
            }

            return new Vector2Int(x, y);
        }

        public static int CalculatePartIndex(int x, int y, int bgSize)
        {
            int index = 0;
            switch (bgSize)
            {
                case 1:
                    if (x == 0 && y == 0) index = 0;
                    break;
                case 2:
                    if (x == 0 && y == 0) index = 0;
                    else if (x == -1 && y == 0) index = 1;
                    break;
                case 4:
                    if (x == 0 && y == 0) index = 0;
                    else if (x == -1 && y == 0) index = 1;
                    else if (x == 0 && y == -1) index = 2;
                    else if (x == -1 && y == -1) index = 3;
                    break;
                case 9:
                    if (x == 0 && y == 0) index = 0;
                    else if (x == -1 && y == 0) index = 1;
                    else if (x == -2 && y == 0) index = 2;
                    else if (x == 0 && y == -1) index = 3;
                    else if (x == -1 && y == -1) index = 4;
                    else if (x == -2 && y == -1) index = 5;
                    else if (x == 0 && y == -2) index = 6;
                    else if (x == -1 && y == -2) index = 7;
                    else if (x == -2 && y == -2) index = 8;
                    break;
            }

            return index;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static float RoundDown(float value, float digits)
        {
            float x = Mathf.Pow(10, digits);
            float rs = (int)(value * x);
            rs /= x;
            return rs;
        }

        public static float RoundUp(float value, float digits)
        {
            float x = Mathf.Pow(10, digits);
            double rs = Math.Ceiling(value * x);
            rs /= x;
            return (float)rs;
        }

        public static float RoundDown(double value, float digits)
        {
            float x = Mathf.Pow(10, digits);
            double rs = Math.Round(value * x - 0.5f);
            rs /= x;
            return (float)rs;
        }

        public static float RoundUp(double value, float digits)
        {
            float x = Mathf.Pow(10, digits);
            double rs = Math.Ceiling((float)value * x);
            rs /= x;
            return (float)rs;
        }

        public static string ToGoldString(this int value)
        {
            string d = value.ToString();
            int c = d.Length % 3 == 0 ? d.Length / 3 : d.Length / 3 + 1;
            List<string> strs = new List<string>();
            for (int i = 1; i <= c; i++)
            {
                strs.Add(SubLastString(d, 3 * i));
            }
            string res = "";
            for (int i = strs.Count - 1; i >= 0; i--)
            {
                if (i != strs.Count - 1)
                    res += $",{strs[i]}";
                else
                    res += $"{strs[i]}";
            }
            return res;
        }

        public static string ToStringValue(this int value)
        {
            string d = "";
            if (value < 0)
                d = "-";
            value = Mathf.Abs(value);
            string strValue = "";
            int Tdiv = (value / 1000000000);
            int Tmov = (value % 1000000000);
            int MMMdiv = (Tmov / 100000000);
            int MMdiv = (Tmov / 10000000);
            int Mdiv = (Tmov / 1000000);
            int Mmov = (Tmov % 1000000);
            int KKKdiv = (Mmov / 100000);
            int KKdiv = (Mmov / 10000);
            int Kdiv = (Mmov / 1000);
            int Kmov = (Mmov % 1000);
            int Hdiv = (Kmov / 100);
            int Hmov = (Kmov % 100);
            if (Tdiv >= 1)
            {
                if (MMMdiv > 0)
                    strValue = $"{Tdiv}.{MMMdiv}B";
                else
                    strValue = $"{Tdiv}B";
            }
            else if (Mdiv >= 1)
            {
                if (KKKdiv > 0)
                    strValue = $"{Mdiv}.{KKKdiv}M";
                else
                    strValue = $"{Mdiv}M";
            }
            else if (Kdiv >= 1)
            {
                if (Hdiv > 0)
                    strValue = $"{Kdiv}.{Hdiv}K";
                else
                    strValue = $"{Kdiv}K";
            }
            else
            {
                strValue = $"{value}";
            }

            return d + strValue;
        }

        public static string ToStringValue(this float float_value)
        {
            string d = "";
            if (float_value < 0)
                d = "-";
            float_value = Mathf.Abs(float_value);
            string strValue = "";
            //int value = (int)float_value;
            int Tdiv = ((int)float_value / 1000000000);
            int Tmov = ((int)float_value % 1000000000);
            int MMMdiv = (Tmov / 100000000);
            int MMdiv = (Tmov / 10000000);
            int Mdiv = (Tmov / 1000000);
            int Mmov = (Tmov % 1000000);
            int KKKdiv = (Mmov / 100000);
            int KKdiv = (Mmov / 10000);
            int Kdiv = (Mmov / 1000);
            int Kmov = (Mmov % 1000);
            int Hdiv = (Kmov / 100);
            int Hmov = (Kmov % 100);
            if (Tdiv >= 1)
            {
                if (MMMdiv > 0)
                    strValue = $"{Tdiv}.{MMMdiv}B";
                else
                    strValue = $"{Tdiv}B";
            }
            else if (Mdiv >= 1)
            {
                if (KKKdiv > 0)
                    strValue = $"{Mdiv}.{KKKdiv}M";
                else
                    strValue = $"{Mdiv}M";
            }
            else if (Kdiv >= 1)
            {
                if (Hdiv > 0)
                    strValue = $"{Kdiv}.{Hdiv}K";
                else
                    strValue = $"{Kdiv}K";
            }
            else
            {
                strValue = $"{float_value}";
            }

            return d + strValue;
        }

        public static string ToStringValue(this double value)
        {
            string d = "";
            if (value < 0)
                d = "-";
            value = Math.Abs(value);
            string strValue = "";
            int Tdiv = (int)(value / 1000000000);
            int Tmov = (int)(value % 1000000000);
            int MMMdiv = (Tmov / 100000000);
            int MMdiv = (Tmov / 10000000);
            int Mdiv = (Tmov / 1000000);
            int Mmov = (Tmov % 1000000);
            int KKKdiv = (Mmov / 100000);
            int KKdiv = (Mmov / 10000);
            int Kdiv = (Mmov / 1000);
            int Kmov = (Mmov % 1000);
            int Hdiv = (Kmov / 100);
            int Hmov = (Kmov % 100);
            if (Tdiv >= 1)
            {
                if (MMMdiv > 0)
                    strValue = $"{Tdiv}.{MMMdiv}B";
                else
                    strValue = $"{Tdiv}B";
            }
            else if (Mdiv >= 1)
            {
                if (KKKdiv > 0)
                    strValue = $"{Mdiv}.{KKKdiv}M";
                else
                    strValue = $"{Mdiv}M";
            }
            else if (Kdiv >= 1)
            {
                if (Hdiv > 0)
                    strValue = $"{Kdiv}.{Hdiv}K";
                else
                    strValue = $"{Kdiv}K";
            }
            else
            {
                strValue = $"{value}";
            }

            return d + strValue;
        }

        public static string ToStringValue(this TimeSpan t)
        {
            //string n = "";
            //if ((int)(t.TotalHours / 24) > 0)
            //{
            //    n = $"{((int)(t.TotalHours / 24)).ToString("00")}d {((int)(t.TotalHours % 24)).ToString("00")}h";
            //}
            //else if ((int)(t.TotalMinutes / 60) > 0)
            //{
            //    n = $"{((int)(t.TotalMinutes / 60)).ToString("00")}h {((int)(t.TotalMinutes % 60)).ToString("00")}m";
            //}
            //else if ((int)(t.TotalSeconds / 60) > 0)
            //{
            //    n = $"{((int)(t.TotalSeconds / 60)).ToString("00")}m {((int)(t.TotalSeconds % 60)).ToString("00")}s";
            //}
            //else
            //{
            //    n = $"00m {((int)(t.TotalSeconds % 60)).ToString("00")}s";
            //}
            //if (t.TotalDays > 0)
            //{
            //    n = $"{t.TotalDays.ToString("00")}d : {(t - new TimeSpan((int)(t.TotalDays * 24), 0, 0)).TotalHours.ToString("00")}h";
            //}
            //else if (t.TotalHours > 0)
            //{
            //    n = $"{t.TotalHours.ToString("00")}h : {(t - new TimeSpan((int)t.TotalHours, 0, 0)).TotalMinutes.ToString("00")}m";
            //}
            //else if (t.TotalMinutes > 0)
            //{
            //    n = $"{t.TotalMinutes.ToString("00")}m : {(t - new TimeSpan(0, (int)t.Minutes, 0)).TotalSeconds.ToString("00")}s";
            //}

            return SetTimeRequired((int)t.TotalSeconds);
        }

        public static IEnumerator Delay(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public static Tween IncreaseValue(float start, float end, float duration, Action<float> update = null,
            Action<float> complete = null)
        {
            return DOTween.To(() => start, x => start = x, end, duration).SetEase(Ease.Linear)
                .OnUpdate(delegate { update?.Invoke(start); }).OnComplete(delegate { complete?.Invoke(end); });
        }

        public static void LoadResourceAsyn<T>(Action<ResourceRequest> callback)
        {
            Mono.Instance.StartCoroutine(LoadTask(typeof(T).ToString(), callback));

            IEnumerator LoadTask(string assetname, Action<ResourceRequest> action)
            {
                ResourceRequest req = Resources.LoadAsync(assetname, typeof(T));
                while (!req.isDone)
                {
                    //Debug.LogError($"Load {typeof(T)} - {req.progress *100}%");
                    yield return null;
                }

                action?.Invoke(req);
            }
        }

        public static string SetTimeRequired(float _time)
        {
            int time = (int)_time;
            string requiedTime = "";
            int days = time / (86400);
            int remainHours = time % 86400;
            int hours = remainHours / 3600;
            if (days > 0)
            {
                requiedTime = string.Format("{0}d {1}h", days.ToString("00"), hours.ToString("00"));
            }
            else if (hours > 0)
            {
                int remainMinute = remainHours % 3600;
                int min = remainMinute / 60;
                requiedTime = string.Format("{0}h {1}m", hours.ToString("00"), min.ToString("00"));
            }
            else
            {
                int remainMinute = remainHours % 3600;
                int min = remainMinute / 60;
                int sec = remainMinute % 60;
                if (min > 0)
                    requiedTime = string.Format("{0}m {1}s", min.ToString("00"), sec.ToString("00"));
                else
                    requiedTime = string.Format("00m {0}s", sec.ToString("00"));
            }

            return requiedTime;
        }

        public static bool hasTextRun = false;
        public static bool forceDone = false;

        public static IEnumerator TextRunEffect(string mess, float duration, Action<string> updateAction = null,
            Action finishAction = null)
        {
            forceDone = false;
            hasTextRun = true;
            char[] chars = mess.ToArray();
            float delay = duration / (chars.Length);
            string temp = "";
            int count = 0;
            if (duration > 0)
            {
                while (count < chars.Length && !forceDone)
                {
                    temp += chars[count];
                    updateAction?.Invoke(temp);
                    yield return new WaitForSeconds(delay);
                    count++;
                }
            }
            else
            {
                temp = mess;
                updateAction?.Invoke(temp);
            }

            hasTextRun = false;
            updateAction?.Invoke(mess);
            finishAction?.Invoke();
        }

        public static void Punch(this RectTransform trans, Vector3 scale, float duration, Action complete = null)
        {
            trans.DOScale(scale, duration / 4).SetEase(Ease.Linear).OnComplete(delegate
            {
                trans.DOScale(scale, duration / 2).SetEase(Ease.Linear).OnComplete(delegate
                {
                    trans.DOScale(new Vector3(1, 1, 1), duration / 4).SetEase(Ease.Linear).OnComplete(delegate
                    {
                        complete?.Invoke();
                    });
                });
            });
        }

        public static DateTime EndOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }

        public static IEnumerator LoadImage(string url, Action<Texture2D> callback)
        {
            //Debug.Log($"LoadImage: {url}");
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            //Debug.Log($"[LoadImage]Result: {request.responseCode}");

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"[LoadImage] Error: {request.error}");
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
            }
        }

        public static string SubLastString(this string str, int count)
        {
            string res = "";
            int tol = str.Count();
            for (int i = tol - count; i < tol; i++)
            {
                if (i >= 0)
                    res += str[i];
            }
            return res;
        }
    }

    public class Mono : MonoBehaviour
    {
        private static Mono instance;

        public static Mono Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<Mono>();
                    instance.name = "Mono";
                }

                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}