using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using ArchimedesMiniGame;

[CreateAssetMenu(fileName = "IntegrationEventSO", menuName = "SO/IntegrationEvent")]
public class IntegrationEventScriptableObject : ScriptableObject
{
    public EIntegrationEvent integrationEvent;

    [Header("LIGHTS")]
    [Range(0,1)]
    public float LightningIncreaser;
    [Space(20)]
    [Header("CINEMATICS")]
    public GameObject CinematicToPlay;
    [Space(20)]
    [Header("SOUNDS")]
    public GameObject SoundToPlay;

    public void Invoke(GameManager gameManager, GameManagerES gameManagerES)
    {
        switch (integrationEvent)
        {
            case EIntegrationEvent.LightUp:

                Debug.Log("Luci modificate");
                break;
            case EIntegrationEvent.DeactiveZeroG:

                PubSub.PubSub.Publish(new ZeroGMessage(false));
                break;
            case EIntegrationEvent.StartCinematic:

                Debug.Log("Inizio di cinematica");
                //CinematicToPlay GO
                break;
            case EIntegrationEvent.PlaySound:


                Debug.Log("Suono riprodotto");
                break;
            case EIntegrationEvent.SetNextModule:

                gameManagerES.SetNextModuleToCommandPlat();
                break;
        }
    }
}
