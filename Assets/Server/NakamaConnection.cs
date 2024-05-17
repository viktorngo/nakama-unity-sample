using System;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using UnityEngine;

namespace Server
{
    public static class NakamaConnection
    {
        private static IClient Client { get; set; }

        private static ISession CurrentSession { get; set; }

        private static ISocket CurrentSocket { get; set; }
        
        private static IApiAccount AccountInfo { get; set; }
        
        private static string NoticeCacheableCursor { get; set; }

        static NakamaConnection()
        {
            const string schema = "http";
            // const string host = "localhost";
            const string host = "44.204.24.106";
            const int port = 7350;
            const string serverKey = "JKoZIhvcNAQEBBQADSgAwRwJAZTVQ9W82HLIC";
            Client = new Client(schema, host, port, serverKey);
            CurrentSocket = Client.NewSocket();
        }

        public static async Task AuthenticateWithDevice()
        {
            // If the user's device ID is already stored, grab that - alternatively get the System's unique device identifier.
            var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);

            // If the device identifier is invalid then let's generate a unique one.
            if (deviceId == SystemInfo.unsupportedIdentifier)
            {
                deviceId = Guid.NewGuid().ToString();
            }

            // Save the user's device ID to PlayerPrefs so it can be retrieved during a later play session for re-authenticating.
            PlayerPrefs.SetString("deviceId", deviceId);

            // Authenticate with the Nakama server using Device Authentication.
            CurrentSession = await Client.AuthenticateDeviceAsync(deviceId);
            await CurrentSocket.ConnectAsync(CurrentSession, true);
            AccountInfo = await GetAccount();
        }

        public static async Task AuthenticateWithFacebook(string tokenString, string username = null)
        {
            CurrentSession = await Client.AuthenticateFacebookAsync(tokenString, username);
            await CurrentSocket.ConnectAsync(CurrentSession, true);
            AccountInfo = await GetAccount();
        }

        public static async Task AuthenticateWithGoogle(string tokenString, string username = null)
        {
            CurrentSession = await Client.AuthenticateGoogleAsync(tokenString, username);
            await CurrentSocket.ConnectAsync(CurrentSession, true);
            AccountInfo = await GetAccount();
        }

        public static async Task AuthenticateWithApple(string tokenString, string username = null)
        {
            CurrentSession = await Client.AuthenticateAppleAsync(tokenString, username);
            await CurrentSocket.ConnectAsync(CurrentSession, true);
            AccountInfo = await GetAccount();
        }

        public static async Task Logout()
        {
            await Client.SessionLogoutAsync(CurrentSession);
        }

        public static bool IsAlive()
        {
            return CurrentSocket.IsConnected;
        }

        public static async Task<IApiAccount> GetAccount()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            return await Client.GetAccountAsync(CurrentSession);
        }

        public static async Task UpdateAccount()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            // sample data
            var newUsername = "NotTheImp0ster";
            var newDisplayName = "Innocent Dave";
            var newAvatarUrl = "https://example.com/imposter.png";
            var newLangTag = "en";
            var newLocation = "Edinburgh";
            var newTimezone = "BST";
            
            await Client.UpdateAccountAsync(CurrentSession, newUsername, newDisplayName, newAvatarUrl, newLangTag,
                newLocation,
                newTimezone);
        }
        
        public static async Task DeleteAccount()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            await Client.DeleteAccountAsync(CurrentSession);
        }

        public static async Task<Storage> GetSystemItems()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            var readObjectId = new StorageObjectId
            {
                Collection = "SystemItems",
                Key = "Storage"
            };

            var result =
                await Client.ReadStorageObjectsAsync(CurrentSession, new IApiReadStorageObjectId[] { readObjectId });

            if (!result.Objects.Any()) return new Storage();

            var storageObject = result.Objects.First();
            var storage = storageObject.Value.FromJson<Storage>();
            return storage;
        }

        public static async Task<Storage> GetStorage()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            var readObjectId = new StorageObjectId
            {
                Collection = "Items",
                Key = "Storage",
                UserId = CurrentSession.UserId
            };

            var result =
                await Client.ReadStorageObjectsAsync(CurrentSession, new IApiReadStorageObjectId[] { readObjectId });

            if (!result.Objects.Any()) return new Storage();

            var storageObject = result.Objects.First();
            var storage = storageObject.Value.FromJson<Storage>();
            return storage;
        }

        public static async Task WriteStorage(Storage storage)
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            var writeObject = new WriteStorageObject
            {
                Collection = "Items",
                Key = "Storage",
                Value = storage.ToJson(),
                PermissionRead = 1, // Only the server and owner can read
                PermissionWrite = 1, // The server and owner can write
            };

            await Client.WriteStorageObjectsAsync(CurrentSession, new IApiWriteStorageObject[] { writeObject });
        }

        public static async Task<Config> GetConfig()
        {
            if (IsAlive())
            {
                throw new Exception("Socket is not connected");
            }
            
            var readObjectId = new StorageObjectId
            {
                Collection = "SystemItems",
                Key = "Config"
            };

            var result =
                await Client.ReadStorageObjectsAsync(CurrentSession, new IApiReadStorageObjectId[] { readObjectId });

            if (!result.Objects.Any()) return new Config();

            var storageObject = result.Objects.First();
            var config = storageObject.Value.FromJson<Config>();
            return config;
        }

        // first time load notifications from server (when user login)
        public static async Task<IApiNotificationList> LoadNotifications()
        {
            return await Client.ListNotificationsAsync(CurrentSession, 10);
        }
        
        // load more notifications from server each time user scroll to the end of the list
        public static async Task<IApiNotificationList> LoadMoreNotifications()
        {
            if (!string.IsNullOrEmpty(NoticeCacheableCursor))
            {
                return await Client.ListNotificationsAsync(CurrentSession, 10, NoticeCacheableCursor);
            }

            return null;
        }
        
        public static async Task ReceiveNotification()
        {
            CurrentSocket.ReceivedNotification += notification =>
            {
                Console.WriteLine("Received: {0}", notification);
                Console.WriteLine("Notification content: '{0}'", notification.Content);
            };
        }
        
        // pass a list of notification ids to delete
        public static async Task DeleteNotifications(string[] notificationIds)
        {
            await Client.DeleteNotificationsAsync(CurrentSession, notificationIds);
        }
        
    }
}