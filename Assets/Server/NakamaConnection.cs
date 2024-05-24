using System;
using System.Collections.Generic;
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

        public static ISocket CurrentSocket { get; set; }

        private static IApiAccount AccountInfo { get; set; }

        private static string NoticeCacheableCursor { get; set; }

        private static string leaderboardsCursor { get; set; }

        static NakamaConnection()
        {
            const string schema = "http";
            const string host = "localhost";
            // const string host = "44.204.24.106";
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
            Debug.Log("logged in with google: " + AccountInfo);
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
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            return await Client.GetAccountAsync(CurrentSession);
        }

        public static async Task UpdateAccount()
        {
            if (!IsAlive())
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
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            await Client.DeleteAccountAsync(CurrentSession);
        }

        public static async Task<Storage> GetSystemItems()
        {
            if (!IsAlive())
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
            if (!IsAlive())
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
            if (!IsAlive())
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
            if (!IsAlive())
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

        // first time load mailbox from server (when user login)
        public static async Task<List<IApiNotification>> LoadMailbox()
        {
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            var result = await Client.ListNotificationsAsync(CurrentSession, 10, null);
            NoticeCacheableCursor = result.CacheableCursor;
            return result.Notifications.ToList();
        }

        // load more mailbox from server each time user scroll to the end of the list
        public static async Task<List<IApiNotification>> LoadMoreMailboxs()
        {
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            if (!string.IsNullOrEmpty(NoticeCacheableCursor))
            {
                var result = await Client.ListNotificationsAsync(CurrentSession, 10, NoticeCacheableCursor);
                NoticeCacheableCursor = result.CacheableCursor;
                return result.Notifications.ToList();
            }

            return null;
        }

        // when user is online, use this callback to receive notification when server push
        public static void ReceiveMailbox()
        {
            CurrentSocket.ReceivedNotification += notification =>
            {
                Debug.Log("Received: " + notification);
                Debug.Log("Notification content: " + notification.Content);
            };
        }

        // pass a list of notification ids to delete
        public static async Task DeleteMailboxs(string[] notificationIds)
        {
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            await Client.DeleteNotificationsAsync(CurrentSession, notificationIds);
        }

        public static async Task<List<Notice>> GetNotices()
        {
            if (!IsAlive())
            {
                throw new Exception("Socket is not connected");
            }

            var readObjectId = new StorageObjectId
            {
                Collection = "Global",
                Key = "Notice"
            };

            var result =
                await Client.ReadStorageObjectsAsync(CurrentSession, new IApiReadStorageObjectId[] { readObjectId });


            if (!result.Objects.Any()) return new List<Notice>();

            var storageObject = result.Objects.First();
            Debug.Log("value: " + storageObject.Value.ToJson());

            var storage = storageObject.Value.FromJson<NoticeStorage>();
            return storage.notices;
        }

        public static async Task<IApiLeaderboardRecord> AddRankingScore(int score)
        {
            return await Client.WriteLeaderboardRecordAsync(CurrentSession, "global_ranking", score);
        }

        public static async Task<IApiLeaderboardRecordList> ShowGlobalRanking()
        {
            // Fetch all records from the leaderboard "global"
            var result = await Client.ListLeaderboardRecordsAsync(CurrentSession, "global_ranking", ownerIds: null,
                expiry: null, 20, leaderboardsCursor);
            leaderboardsCursor = result.NextCursor;
            return result;
        }
    }
}