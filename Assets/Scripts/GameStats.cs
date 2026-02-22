using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int PlayerHPMax;

    public bool LevelBrainHealthPickedUp;
    public bool LevelBrainDialogPlayed;
    public bool LevelBrainBossDialogPlayed;
    
    public bool LevelLungHealthPickedUp;
    public bool LevelLungDialogPlayed;
    public bool LevelLungBossDialogPlayed;
    
    public bool LevelEyeHealthPickedUp;
    public bool LevelEyeDialogPlayed;
    public bool LevelEyeBossDialogPlayed;
    
    public bool LevelHeartHealthPickedUp;
    public bool LevelHeartDialogPlayed;
    public bool LevelHeartBossDialogPlayed;
    
    
    private void Awake()
    {
        Instance = this;
    }

    public bool GetHealthItemPickedUp(int sceneId)
    {
        // 2 = Brain
        // 4 = Lung
        // 6 = Eye
        // 8 = Heart

        switch (sceneId)
        {
            case 1:
                return LevelBrainHealthPickedUp;
            case 3:
                return LevelLungHealthPickedUp;
            case 5:
                return LevelEyeHealthPickedUp;
            case 7:
                return LevelHeartHealthPickedUp;
            default:
                return false;
        }
    }

    public void SetHealthItemPickedUp(int sceneId)
    {
        switch (sceneId)
        {
            case 1:
                LevelBrainHealthPickedUp = true;
                break;
            case 3:
                LevelLungHealthPickedUp = true;
                break;
            case 5:
                LevelEyeHealthPickedUp = true;
                break;
            case 7:
                LevelHeartHealthPickedUp = true;
                break;
        }
        
        
    }
}
