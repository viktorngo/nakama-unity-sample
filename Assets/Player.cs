using Nakama;
using Nakama.TinyJson;
using Server;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string idToken;
    private IApiAccount NakamaAccount { get; set; }
    private Storage Storage { get; set; }

    public void OnButtonClick()
    {
        // Code to execute when the button is clicked
        Debug.Log("server is alive: " + NakamaConnection.IsAlive());
    }

    async void Start()
    {
        await NakamaConnection.AuthenticateWithGoogle(
            idToken);

        // load storage of user
        // Storage = await NakamaConnection.GetStorage();

        // update storage
        // add some clothes to the storage
        // Storage.Clothes.Add(new Clothe("Red Hat", "Hat", "A red hat", false));
        // Storage.Clothes.Add(new Clothe("Red Shirt", "Shirt", "A red shirt", false));
        //
        // await NakamaConnection.WriteStorage(Storage);
        //
        // // load play map
        // Config config = await NakamaConnection.GetConfig();
        // Debug.Log("playing map: "+ config.MapConfig);
        //
        //
        // Debug.Log("updated clothes: " + Storage.Clothes.ToJson());

        // ----------------------- Ranking -----------------------
        // test add ranking score
        // var rank = await NakamaConnection.AddRankingScore(100);
        // Debug.Log("rank: " + rank.ToJson());

        // var leaderboardRecords = await NakamaConnection.GetGlobalRanks();
        // Debug.Log("Ranking Records: " + leaderboardRecords.Records.Count());
        // foreach (var record in leaderboardRecords.Records)
        // {
        //     Debug.Log("record: " + record.ToJson());
        // }

        // var myLeaderboardRecords = await NakamaConnection.GetMyRank();
        // Debug.Log("My Ranking Records: " + myLeaderboardRecords.Records.Count());
        // foreach (var record in myLeaderboardRecords.Records)
        // {
        //     Debug.Log("record: " + record.ToJson());


        // ----------------------- Notice -----------------------
        // var notices = await NakamaConnection.GetNotices();
        // Debug.Log("notices: " + notices.ToJson());


        // ----------------------- Mailbox -----------------------
        // load all mailboxs
        // var mailBoxes = await NakamaConnection.LoadMailbox();
        // if (mailBoxes != null)
        // {
        //     Debug.Log("received mailboxes: " + mailBoxes.Count);
        //     foreach (var notification in mailBoxes)
        //     {
        //         // Process notification data (subject, content, etc.)
        //         Debug.Log($"Mailbox: {notification.ToJson()}");
        //     }
        // }

        // receive notification immediately when server push
        NakamaConnection.ReceiveNotification();
        
        // Delete mailbox
        // await NakamaConnection.DeleteMailboxs(new[] { "f56e6722-7de0-4e7b-b1d5-ba9e447bddb5" });
        //
        // Debug.Log("after delete 1 mailbox");
        // mailBoxes = await NakamaConnection.LoadMailbox();
        // Debug.Log($"length: {mailBoxes.Count} mailboxes: {mailBoxes.ToJson()}");
    }
}