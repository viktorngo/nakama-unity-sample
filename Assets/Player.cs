using System.Collections.Generic;
using System.Threading.Tasks;
using Nakama;
using Nakama.TinyJson;
using Server;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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
            "eyJhbGciOiJSUzI1NiIsImtpZCI6ImFjM2UzZTU1ODExMWM3YzdhNzVjNWI2NTEzNGQyMmY2M2VlMDA2ZDAiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIzMTc1Njc2ODQwOC1tOGU2dHQzNTB0aW50dmxpZjhhcW9mNnZkZGxwMDlibC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsImF1ZCI6IjMxNzU2NzY4NDA4LW04ZTZ0dDM1MHRpbnR2bGlmOGFxb2Y2dmRkbHAwOWJsLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA5MDkxOTgxMTY2ODUxNjA2MDIzIiwiZW1haWwiOiJ0aGFuZ3l0Ymc5OUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiYXRfaGFzaCI6IkZrMHlVd21JOUd1NDc4RmFiWG81TWciLCJpYXQiOjE3MTQ5NzQzMzYsImV4cCI6MTcxNDk3NzkzNn0.a_GgWmg-tuZtKRSoLtSRORQ8vyVBtnLat63FSC8PwjhS_j8cXDayUyxwPGL92dL6X_-SvTDQn2SakeCjQZUS6APHeAHy2ddrjBeUZ3MDEtFkERgiMZZS07lVYGEoXSMQmnndplEM5Mn_9lcLi2qhQH4mG-bgVjIAvgeHmyoHb9WaQqLIdmt9kvQox5naFRj54-kSDaJ094AnEtQ8pHFy-6c7hVmbEhYtY0uD17I47vJlko7AM-bhzbjcG6m_iAbiVAAkv0EbQNoSBa8-UCFxkFGmz679bz3GhuMfS2JNrdc1e1aLG9XlzjFcMmBJBqe3SUQNUzCwpyj5T9W-XqRi9g");

        // load storage of user
        Storage = await NakamaConnection.GetStorage();
        
        // update storage
        // add some clothes to the storage
        Storage.Clothes.Add(new Clothe("Red Hat", "Hat", "A red hat", false));
        Storage.Clothes.Add(new Clothe("Red Shirt", "Shirt", "A red shirt", false));

        await NakamaConnection.WriteStorage(Storage);
        
        // load play map
        Config config = await NakamaConnection.GetConfig();
        Debug.Log("playing map: "+ config.MapConfig);
        
        
        Debug.Log("updated clothes: " + Storage.Clothes.ToJson());
    }

}