using Server;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private async void Start()
    {
        await NakamaConnection.AuthenticateWithGoogle(
            "eyJhbGciOiJSUzI1NiIsImtpZCI6IjMyM2IyMTRhZTY5NzVhMGYwMzRlYTc3MzU0ZGMwYzI1ZDAzNjQyZGMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIzMTc1Njc2ODQwOC1tOGU2dHQzNTB0aW50dmxpZjhhcW9mNnZkZGxwMDlibC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsImF1ZCI6IjMxNzU2NzY4NDA4LW04ZTZ0dDM1MHRpbnR2bGlmOGFxb2Y2dmRkbHAwOWJsLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA5MDkxOTgxMTY2ODUxNjA2MDIzIiwiZW1haWwiOiJ0aGFuZ3l0Ymc5OUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6InJFci1nSy1GSWZYRzZYREh3RVhkREEiLCJpYXQiOjE3MTYwODQ0MjksImV4cCI6MTcxNjA4ODAyOX0.vSb8zwVTVcBWwDU2abkt5ZLax6-pcpzE_1K5cjOYbfYO7t7d2_0LBx6wOhIloxlNLpdyHH4axyjrbGnn6mXkS4w-KbL62IuZulcNBbr2WL7JcBqbZQ7kcxhbZrMFymh-rg4OEESfvb2IQsk2oYY4RJ1pFYjvGk1MNj6yJ61hhzh6CECpIX5BcXZSYmw-Hlz45dzkajgq4IvWw15rb-FADL4M_duAr5lgzFue6RJfPOZsXDa1WpOb1uNzZNMJE56107xBTt6dajrMdka0PvqV5dnZe0qn6JSEXeyI-ZW1aUX-8mZ_sO7h3_ADEN8lI-vTp9rRTRSdK7ic_11NbLozLA");


        // load all notifications
        var notifications = await NakamaConnection.LoadNotifications();
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
        NakamaConnection.ReceiveNotification();
    }
}