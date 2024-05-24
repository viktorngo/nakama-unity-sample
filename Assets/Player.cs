using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using Server;
using UnityEngine;
using UnityEngine.UI;

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
        
        
        // test add ranking score
        // var rank = await NakamaConnection.AddRankingScore(100);
        // Debug.Log("rank: " + rank.ToJson());

        var leaderboardRecords = await NakamaConnection.ShowGlobalRanking();
        Debug.Log("Ranking Records: " + leaderboardRecords.Records.Count());
        foreach (var record in leaderboardRecords.Records)
        {
            Debug.Log("record: " + record.ToJson());
        }
        
    }

}