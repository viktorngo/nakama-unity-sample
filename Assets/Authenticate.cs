using Server;
using UnityEngine;

public class Authenticate : MonoBehaviour
{
    public string idToken;
    async void Start()
    {
        await NakamaConnection.AuthenticateWithGoogle(
            idToken);
    }
}