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
            "eyJhbGciOiJSUzI1NiIsImtpZCI6IjMyM2IyMTRhZTY5NzVhMGYwMzRlYTc3MzU0ZGMwYzI1ZDAzNjQyZGMiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJhenAiOiIzMTc1Njc2ODQwOC1tOGU2dHQzNTB0aW50dmxpZjhhcW9mNnZkZGxwMDlibC5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsImF1ZCI6IjMxNzU2NzY4NDA4LW04ZTZ0dDM1MHRpbnR2bGlmOGFxb2Y2dmRkbHAwOWJsLmFwcHMuZ29vZ2xldXNlcmNvbnRlbnQuY29tIiwic3ViIjoiMTA5MjQyMjc5OTA3NDY5MzMzNTg5IiwiZW1haWwiOiJ2aWt0b3JuZ28ub25saW5lQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhdF9oYXNoIjoiMUc3WDFCWks3YVBNRG5Fa1NLVzlkZyIsImlhdCI6MTcxNjA4MDgwNiwiZXhwIjoxNzE2MDg0NDA2fQ.jqOA4nw5AXPtk1MsOVeeInRVZClm9BE6lpEtAla7scqPw_Elui-xdNfp6WO1qHyAeY_PV1dcwh4xz9xoKJyHSjMkXjYLCWbTZIAuBapToPnNZwdmdowMOTEj2cx_1dzi_hwZB5-JrcxUMYWW_bB_avnzustDUd_1pFcbYmqQ54E3xYf3paVc6C01nJwoh1eXi1s-FMaw19FLPMP-9Xu4QXfOccQmGCT7zCxCaJCxEP8VlrP5BYEJS_cFhLkYrylmGjgqRzPlIbmYKuo0WaDacVOQEH7p9DiEfKb2SyxjkItU_UnzWQNUK49V2NVs0hZ4thfSfAMz-W-NS9fW-ShbAw");
    
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