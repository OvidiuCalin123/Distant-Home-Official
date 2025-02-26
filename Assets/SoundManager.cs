using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource Music;
    public AudioSource PlayerSteps;
    public AudioSource ItemPick;
    public AudioSource ItemSelect;
    public AudioSource GroundClick;
    public AudioSource InteractEffect;
    public AudioSource NPC_sound;
    public AudioSource BackgroundNoise;
    public AudioSource BackgroundNoise2;

    [Header("Audio Clips")]
    public AudioClip StreetsMusic_clip;
    public AudioClip RichCompartmentMusic_clip;
    public AudioClip SewageMusic_clip;
    public AudioClip TrainGeneric_clip;
    public AudioClip FallMusic_clip;
    public AudioClip PlayerSteps_clip;
    public AudioClip Coin_clip;
    public AudioClip ItemSelect_clip;
    public AudioClip GroundClick_clip;
    public AudioClip GenericItemPick_clip;
    public AudioClip BrickRub_clip;
    public AudioClip MetalBucket_clip;
    public AudioClip Slip_clip;
    public AudioClip RemoveItem_clip;
    public AudioClip BackgroundNoise_clip;
    public AudioClip TrainDoor_clip;
    public AudioClip UnlockedDoor_clip;
    public AudioClip LockedDoor_clip;

    public AudioClip ErrorPickLockArrows_clip;

    public AudioClip ArrowsPickLockHit_clip;
    public AudioClip CogsHanoi_clip;

    [Header("Player Reference")]
    public PlayerMovement player;

    public enum Locations
    {
        Streets,
        Sewage,
        TrainGeneric,
        TrainRich,
    }

    private AudioClip currentMusicClip;

    void Start()
    {
        // Validate required references
        if (Music == null || player == null)
        {
            Debug.LogError("SoundManager: Missing required references!");
            return;
        }

        // Play the initial music based on the player's location
        UpdateMusic();
    }

    void Update()
    {
        // Only update if the location changes
        if (ShouldUpdateMusic())
        {
            UpdateMusic();
        }
    }

    /// <summary>
    /// Updates the background music based on the player's current location.
    /// </summary>
    private void UpdateMusic()
    {
        AudioClip newClip = GetMusicClipForLocation(player.currentLocation);

        // Change the music only if it’s different
        if (newClip != currentMusicClip)
        {
            currentMusicClip = newClip;
            if (currentMusicClip != null)
            {
                Music.clip = currentMusicClip;
                Music.Play();
            }
            else
            {
                Music.Stop(); // Stop music if no valid clip is found
            }
        }
    }

    /// <summary>
    /// Determines if the music should be updated based on the location change.
    /// </summary>
    /// <returns>True if music should be updated, otherwise false.</returns>
    private bool ShouldUpdateMusic()
    {
        return GetMusicClipForLocation(player.currentLocation) != currentMusicClip;
    }

    /// <summary>
    /// Returns the appropriate music clip for a given location.
    /// </summary>
    /// <param name="location">The player's current location.</param>
    /// <returns>The corresponding music clip.</returns>
    private AudioClip GetMusicClipForLocation(int location)
    {
        if (player.act1InteractionHandler != null)
        {
            if (player.act1InteractionHandler.didPlayerFallOnCatLadder)
            {
                return FallMusic_clip;
            }
            else if (location == (int)Locations.Streets)
            {
                return StreetsMusic_clip;
            }
            else if (location == (int)Locations.Sewage)
            {
                return SewageMusic_clip;
            }
            
        }else if(player.act2InteractionHandler != null)
        {
            if (location == (int)Locations.TrainGeneric)
            {
                if (!BackgroundNoise.isPlaying)
                {
                    BackgroundNoise.volume = 0.25f;
                    playBackgroundNoise();
                }
                return TrainGeneric_clip;
            } 
            else if (location == (int)Locations.TrainRich)
            {
                return RichCompartmentMusic_clip;
            }
        }
        
        return null; // Default to no music if the location is unrecognized
    }

    public void playArrowsPickLockHit()
    {
        ItemPick.clip = ArrowsPickLockHit_clip;
        ItemPick.Play();
    }

    public void playCogsHanoi()
    {
        BackgroundNoise2.clip = CogsHanoi_clip;
        BackgroundNoise2.Play();
    }

    public void playErrorPickLockArrows()
    {
        InteractEffect.clip = ErrorPickLockArrows_clip;
        InteractEffect.Play();
    }

    public void playCoinItemPicked()
    {
        ItemPick.clip = Coin_clip;
        ItemPick.Play();
    }

    public void playUnlockedDoor()
    {
        ItemPick.clip = UnlockedDoor_clip;
        ItemPick.Play();
    }

    public void playLockedDoor()
    {
        ItemPick.clip = LockedDoor_clip;
        ItemPick.Play();
    }

    public void playTrainDoor()
    {
        ItemPick.clip = TrainDoor_clip;
        ItemPick.Play();
    }

    public void playBackgroundNoise()
    {
        BackgroundNoise.clip = BackgroundNoise_clip;
        BackgroundNoise.Play();
    }

    public void playPlayerSteps()
    {
        PlayerSteps.clip = PlayerSteps_clip;
        PlayerSteps.Play();
    }

    public void playItemSelect()
    {
        ItemSelect.clip = ItemSelect_clip;
        ItemSelect.Play();
    }

    public void playGroundClick()
    {
        GroundClick.clip = GroundClick_clip;
        GroundClick.Play();
    }

    public void playMetalBucketClick()
    {
        InteractEffect.clip = MetalBucket_clip;
        InteractEffect.Play();
    }

    public void playSlipNPCClick()
    {
        NPC_sound.clip = Slip_clip;
        NPC_sound.Play();
    }

    public void playBrickRub()
    {
        ItemPick.clip = BrickRub_clip;
        ItemPick.Play();
    }

    public void playRemoveItem()
    {
        InteractEffect.clip = RemoveItem_clip;
        InteractEffect.Play();
    }

    public void playGenericItemRub()
    {
        ItemPick.clip = GenericItemPick_clip;
        ItemPick.Play();
    }
}
