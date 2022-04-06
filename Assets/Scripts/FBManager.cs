using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Facebook.Unity;
using Facebook.MiniJSON;

// [SLua.CustomLuaClass]
public class FBManager
{
    public delegate void OnFBLoginSucced(Facebook.Unity.AccessToken token);
    public delegate void OnFBLoginFaild(bool isCancel, string errorInfo);
    public delegate void OnFBShareLinkSucced(string postId);
    public delegate void OnFBShareLinkFaild(bool isCancel, string errorInfo);
    public delegate void OnGotFBFriendInGame(string resultJsonStr);
    public delegate void OnGotFBMyInfo(string resultJsonStr);
    public delegate void OnFBInvitedSucceed(string resultJsonStr);
    private static string appLinkUrl;

    public static void Init()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
                {
                    Debug.Log("FB OnInitComplete!");
                    Debug.Log("FB.AppId: " + FB.AppId);
                    Debug.Log("FB.GraphApiVersion: " + FB.GraphApiVersion);
                    //获取应用链接
                    FBGetAPPLinkUrl();
                    FB.ActivateApp();
                },

                (bool isUnityShutDown) =>
                {
                    Debug.Log("FB OnHideUnity： " + isUnityShutDown);
                    if (isUnityShutDown)
                    {
                        //unity 恢复游戏状态

                    }
                    else
                    {
                        //unity 暂停游戏状态
                    }
                }
            );
        }
    }

    //事件
    public static void FBReportEvent()
    {
        FB.LogAppEvent(
                    AppEventName.UnlockedAchievement,
                    null,
                    new Dictionary<string, object>()
                    {
                        { AppEventParameterName.Description, "Clicked 'Log AppEvent' button" }
                    });
    }

    public static void FBLogin(OnFBLoginSucced onFBLoginSucced = null, OnFBLoginFaild onFBLoginFaild = null)
    {
        var perms = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(perms, (result) =>
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FBLoginSucceed");
                if (onFBLoginSucced != null)
                {
                    onFBLoginSucced(Facebook.Unity.AccessToken.CurrentAccessToken);
                }
            }
            else
            {
                Debug.Log("FBLoginFaild");
                Debug.Log(result.RawResult);
                if (onFBLoginFaild != null)
                {
                    onFBLoginFaild(result.Cancelled, result.Error);
                }
            }
        });
    }

    //分享, 例：
    //uri = "https://developers.facebook.com/";
    //contentTitle = "ShareLink";
    //contentDesc = "Look I'm sharing a link";
    //picUri = "https://ss1.bdstatic.com/5eN1bjq8AAUYm2zgoY3K/r/www/cache/holiday/habo/res/doodle/3.png";

    public static void FBShareLink(string uri, string contentTitle, string contentDesc, string picUri, OnFBShareLinkSucced onFBShareLinkSucced = null, OnFBShareLinkFaild onFBShareLinkFaild = null)
    {
        FBShareLink(new Uri(uri), contentTitle, contentDesc, new Uri(picUri), onFBShareLinkSucced, onFBShareLinkFaild);
    }

    private static void FBShareLink(Uri uri, string contentTitle, string contentDesc, Uri picUri, OnFBShareLinkSucced onFBShareLinkSucced = null, OnFBShareLinkFaild onFBShareLinkFaild = null)
    {
        FB.ShareLink(uri, contentTitle, contentDesc, picUri, (result) =>
        {
            if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
            {
                Debug.Log("ShareLink Faild");
                if (onFBShareLinkFaild != null)
                {
                    onFBShareLinkFaild(result.Cancelled, result.Error);
                }
            }
            else
            {
                Debug.Log("ShareLink success!");
                if (onFBShareLinkSucced != null)
                {
                    onFBShareLinkSucced(String.IsNullOrEmpty(result.PostId) ? "" : result.PostId);
                }
            }
        });
    }

    //获取自己的信息
    public static void GetMyInfo(OnGotFBMyInfo onGotFBMyInfo)
    {
        if (FB.IsLoggedIn == false)
        {
            // UIFloatTip.Create("Not Login in");
            return;
        }
        FB.API("me?fields=id,name,picture", HttpMethod.GET, (result) =>
        {
            Debug.Log(result.RawResult);
            if (onGotFBMyInfo != null)
            {
                onGotFBMyInfo(result.RawResult);
            }
        });
    }

    //获取游戏好友
    public static void GetFBFriendInGame(OnGotFBFriendInGame onGotFBFriendInGame = null)
    {
        Debug.Log("GetFBFriendInGame");
        if (FB.IsLoggedIn == false)
        {
            // UIFloatTip.Create("Not Login in");
            return;
        }

        FB.API("me/friends?fields=id,name,picture", HttpMethod.GET, (result) =>
        {
            Debug.Log(result.RawResult);
            if (onGotFBFriendInGame != null)
            {
                onGotFBFriendInGame(result.RawResult);
            }
        });
    }

    //获取可邀请好友, 获取失败 TODO
    public static void GetFBFriendInvitable()
    {
        if (FB.IsLoggedIn == false)
        {
            // UIFloatTip.Create("Not Login in");
            return;
        }
        FB.API("/me/invitable_friends?fields=id,name,picture", HttpMethod.GET, (result) =>
        {
            Debug.Log("result: ");
            Debug.Log(result.RawResult);
        });
    }

    //邀请, 
    // public static void FBInvite(string assignedLink, string previewImageUrl, OnFBInvitedSucceed onFBInvitedSucceed = null)
    // {
    //     if (String.IsNullOrEmpty(assignedLink))
    //     {
    //         assignedLink = appLinkUrl;
    //     }
    //     Debug.Log("appLinkUrl: " + appLinkUrl);
    //     Debug.Log("assignedLink: " + assignedLink);
    //     FBInvite(new Uri(assignedLink), new Uri(previewImageUrl), onFBInvitedSucceed);
    // }

    // private static void FBInvite(Uri appLinkUrl, Uri previewImageUrl = null, OnFBInvitedSucceed onFBInvitedSucceed = null)
    // {
    //     FB.Mobile.AppInvite(appLinkUrl, previewImageUrl, (result) =>
    //     {
    //         Debug.Log("rawResult: " + result.RawResult);
    //     });
    // }

    //获取APPLink, 获取失败，TODO
    public static void FBGetAPPLinkUrl()
    {
        FB.GetAppLink((result) =>
        {
            Debug.Log(result.RawResult);
            Debug.Log("Ref: " + result.Ref);
            Debug.Log("TargetUrl: " + result.TargetUrl);
            Debug.Log("Url: " + result.Url);
            appLinkUrl = result.Url;
        });
    }
}
