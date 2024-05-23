using Nakama.TinyJson;
using Server;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private async void Start()
    {
        await NakamaConnection.AuthenticateWithGoogle(
            "eyJhbGciOiJSUzI1NiIsImtpZCI6IjMyM2IyMTRhZTY5NzVhMGYwMzRlYTc3MzU0ZGMwYzI1ZDAzNjQyZGMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIzMTc1Njc2ODQwOC1tOGU2dHQzNTB0aW50dmxpZjhhcW9mNnZkZGxwMDlibC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsImF1ZCI6IjMxNzU2NzY4NDA4LW04ZTZ0dDM1MHRpbnR2bGlmOGFxb2Y2dmRkbHAwOWJsLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA5MDkxOTgxMTY2ODUxNjA2MDIzIiwiZW1haWwiOiJ0aGFuZ3l0Ymc5OUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6InRuMDVaYmhLQ0M2OTB4UERtNFpnYVEiLCJpYXQiOjE3MTY0MzY5MjgsImV4cCI6MTcxNjQ0MDUyOH0.XT47VAsW2-jND-conm_66KtUL4G0j6w8p54JYDXYKSdblly7hyvgPsVgNloaozUiTxevFer33H3WJ4wjYPWirreocGb8ouQkrP1wkIZbUXA-lqd0OJ-8VygxCLoF_KLT1zgrvqewAf7iRwvSCsz_kgUsQOGZhE8ZBZmyFKWDId2pWry4sfhesJLdVbLXGoWGqy-5fbaRT6RGf4z_ACaRhLKO8b2jjZqO9XFQfSM3UHlN73ZkPqqUZPjYeVQeJJ0ppSesOK2FZB__aVWuZhn47m-VbAaRKuQuhhQMkoh_wriKZn9EVp1hy0ln_nqgJwOrGSkbcZUwF68QinqzDQ8DSA");

        var notices = await NakamaConnection.GetNotices();
        Debug.Log("notices: " + notices.ToJson());
        
        // load all notifications
        var notifications = await NakamaConnection.LoadMailbox();
        if (notifications != null)
        {
            Debug.Log("received notifications: " + notifications.Count);
            foreach (var notification in notifications)
            {
                // Process notification data (subject, content, etc.)
                Debug.Log($"Notification: {notification.Subject}");
            }
        }
        
        // receive notification immediately when server push
        NakamaConnection.ReceiveMailbox();
    }
}