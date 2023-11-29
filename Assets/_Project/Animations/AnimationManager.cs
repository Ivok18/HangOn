using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HangOn.Animations
{
    public class AnimationManager : MonoBehaviour
    {
        public static AnimationManager Instance;
        [SerializeField] private List<Anim> animations;


        private void Awake()
        {
            Instance = this;
        }


        private void OnEnable()
        {
            Game_Anim1_FindLetter.OnRequestPlayFindLetterAnim += Play;
            Game_Anim1_FindWord.OnRequestPlayFindWordAnim += Play;
            Game_EndOfRun_Anim1_GoldMedal.OnRequestPlayGoldMedalAnim += Play;
            Game_EndOfRun_Anim1_SilverMedal.OnRequestPlaySilverMedalAnim += Play;
            Game_EndOfRun_Anim1_BronzeMedal.OnRequestPlayBronzeMedalAnim += Play;
            //Game_Anim1_WrongLetter.OnRequestPlayWrongLetterAnim += Play;
            /*Loading_Anim_Haystack.OnRequestPlayHaystackAnim += Play;
            Loading_Anim2_Haystack.OnRequestPlayHaystackAnim += Play;
            Loading_Anim1_WaterSteam.OnRequestPlayWaterSteamAnim += Play;
            Loading_Anim2_WaterSteam.OnRequestPlayWaterSteamAnim += Play;

            MainMenu_Anim_Haystack.OnRequestPlayHaystackAnim += Play;
            MainMenu_Anim_RedArrow.OnRequestPlayRedArrowAnim += Play;
            MainMenu_Anim1_GoatTail.OnRequestPlayGoatTailAnim += Play;
            MainMenu_Anim2_GoatTail.OnRequestPlayGoatTailAnim += Play;
            MainMenu_Anim3_GoatTail.OnRequestPlayGoatTailAnim += Play;
            MainMenu_Anim1_Star.OnRequestPlayStarAnim += Play;
            GameRules_Anim_ShopButton.OnRequestPlayShopButtonAnim += Play;
            MainMenu_Gamerules_Anim1_Star.OnRequestPlayStarAnim += Play;
            MainMenu_Parameters_Anim1_GoatTail.OnRequestPlayGoatTailAnim += Play;
            MainMenu_Parameters_Anim2_GoatTail.OnRequestPlayGoatTailAnim += Play;
            MainMenu_Parameters_Anim1_Haystack.OnRequestPlayHaystackAnim += Play;
            MainMenu_Medaillons_Anim1_RedArrow.OnRequestPlayRedArrowAnim += Play;
            MainMenu_Rater_Anim1_RedArrow.OnRequestPlayRedArrowAnim += Play;
            MainMenu_Shop_Anim1_Haystack.OnRequestPlayHaystackAnim += Play;
            MainMenu_Shop_Anim1_Lock.OnRequestPlayShopLockIconAnim += Play;
            MainMenu_Oops_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_Rater_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_Rapidity_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_TimeMaster_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_Invincibility_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_Flawless_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            MainMenu_Perseverant_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;

            Rounds_Oops_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            Rounds_GameDifficulties_Anim1_WhiteIndex.OnRequestPlayIndexAnim += Play;
            //Rounds_Anim1_Haystack.OnRequestPlayHaystackAnim += Play;
            Rounds_Fail_Anim1_Star.OnRequestPlayStarAnim += Play;
            Rounds_Victory_Anim1_Star.OnRequestPlayStarAnim += Play;
            Rounds_Languages_Anim1_Star.OnRequestPlayStarAnim += Play;
            Rounds_ActivateBonus_Anim1_Star.OnRequestPlayStarAnim += Play;
            Rounds_RoundReward_Anim1_TimeReward.OnRequestPlayTimeRewardAnim += Play;
            Rounds_RoundReward_Anim1_StageReward.OnRequestPlayStageRewardAnim += Play;
            Rounds_Credits_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            Rounds_TimeReward_Anim1_RedCross.OnRequestPlayRedCrossAnim += Play;
            Rounds_Stages_Anim1_ShopButton.OnRequestPlayShopButtonAnim += Play;
            Rounds_EndOfStage_Anim1_ShopButton.OnRequestPlayShopButtonAnim += Play;
            Rounds_GameAchievements_JumpAnim.OnRequestPlayAchievementJumpAnim += Play;
            Rounds_Parameters_Anim1_Haystack.OnRequestPlayHaystackAnim += Play;
            Rounds_Anim_GoatTail.OnRequestPlayGoatTailAnim += Play;
            Rounds_Anim1_WhiteIndex.OnRequestPlayWhiteIndexAnim += Play;
            Rounds_Anim1_Star.OnRequestPlayStarAnim += Play;
            Rounds_TipsFirstRound_Anim1_ShopButton.OnRequestPlayShopButtonAnim += Play;*/
        }

        private void OnDisable()
        {
            Game_Anim1_FindLetter.OnRequestPlayFindLetterAnim -= Play;
            Game_Anim1_FindWord.OnRequestPlayFindWordAnim -= Play;
            Game_EndOfRun_Anim1_GoldMedal.OnRequestPlayGoldMedalAnim -= Play;
            Game_EndOfRun_Anim1_SilverMedal.OnRequestPlaySilverMedalAnim -= Play;
            Game_EndOfRun_Anim1_BronzeMedal.OnRequestPlayBronzeMedalAnim -= Play;
            //Game_Anim1_WrongLetter.OnRequestPlayWrongLetterAnim -= Play;

            //Game_EndOfRun_Anim1_LeaderboardMention.OnRequestPlayLeaderboardMentionAnim -= Play;

            /*Loading_Anim_Haystack.OnRequestPlayHaystackAnim -= Play;
            Loading_Anim2_Haystack.OnRequestPlayHaystackAnim -= Play;
            Loading_Anim1_WaterSteam.OnRequestPlayWaterSteamAnim -= Play;
            Loading_Anim2_WaterSteam.OnRequestPlayWaterSteamAnim -= Play;

            MainMenu_Anim_Haystack.OnRequestPlayHaystackAnim -= Play;
            MainMenu_Anim_RedArrow.OnRequestPlayRedArrowAnim -= Play;
            MainMenu_Anim1_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            MainMenu_Anim2_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            MainMenu_Anim3_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            MainMenu_Anim1_Star.OnRequestPlayStarAnim -= Play;
            GameRules_Anim_ShopButton.OnRequestPlayShopButtonAnim -= Play;
            MainMenu_Gamerules_Anim1_Star.OnRequestPlayStarAnim -= Play;
            MainMenu_Parameters_Anim1_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            MainMenu_Parameters_Anim2_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            MainMenu_Parameters_Anim1_Haystack.OnRequestPlayHaystackAnim -= Play;
            MainMenu_Medaillons_Anim1_RedArrow.OnRequestPlayRedArrowAnim -= Play;
            MainMenu_Rater_Anim1_RedArrow.OnRequestPlayRedArrowAnim -= Play;
            MainMenu_Shop_Anim1_Haystack.OnRequestPlayHaystackAnim -= Play;
            MainMenu_Shop_Anim1_Lock.OnRequestPlayShopLockIconAnim -= Play;
            MainMenu_Oops_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_Rater_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_Rapidity_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_TimeMaster_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_Invincibility_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_Flawless_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            MainMenu_Perseverant_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;

            Rounds_Oops_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            Rounds_GameDifficulties_Anim1_WhiteIndex.OnRequestPlayIndexAnim -= Play;
            //Rounds_Anim1_Haystack.OnRequestPlayHaystackAnim -= Play;
            Rounds_Fail_Anim1_Star.OnRequestPlayStarAnim -= Play;
            Rounds_Victory_Anim1_Star.OnRequestPlayStarAnim -= Play;
            Rounds_Languages_Anim1_Star.OnRequestPlayStarAnim -= Play;
            Rounds_ActivateBonus_Anim1_Star.OnRequestPlayStarAnim -= Play;
            Rounds_RoundReward_Anim1_TimeReward.OnRequestPlayTimeRewardAnim -= Play;
            Rounds_RoundReward_Anim1_StageReward.OnRequestPlayStageRewardAnim -= Play;
            Rounds_Credits_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            Rounds_TimeReward_Anim1_RedCross.OnRequestPlayRedCrossAnim -= Play;
            Rounds_Stages_Anim1_ShopButton.OnRequestPlayShopButtonAnim -= Play;
            Rounds_EndOfStage_Anim1_ShopButton.OnRequestPlayShopButtonAnim -= Play;
            Rounds_GameAchievements_JumpAnim.OnRequestPlayAchievementJumpAnim -= Play;
            Rounds_Parameters_Anim1_Haystack.OnRequestPlayHaystackAnim -= Play;
            Rounds_Anim_GoatTail.OnRequestPlayGoatTailAnim -= Play;
            Rounds_Anim1_WhiteIndex.OnRequestPlayWhiteIndexAnim -= Play;
            Rounds_Anim1_Star.OnRequestPlayStarAnim -= Play;
            Rounds_TipsFirstRound_Anim1_ShopButton.OnRequestPlayShopButtonAnim -= Play;*/
        }

        public void Play(int id)
        {
            var anim = animations.Where(x => x.Id == id).FirstOrDefault();
            if (anim == null)
            {
                Debug.LogWarning($"Sorry, the animation with id : {id} does not exist");
            }
            anim.Play(id);
        }
    }
}
