using System;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public interface IBonusPolicy
    {
        BonusState DefaultState();

        ///总红利
        double CalcBonus(double bonusIndex, double realMoney);

        ///静态释放红利
        double StaticReleaseBonus(double releaseBonusRatio, double restBonus);

        ///推荐奖
        double RecommendReward(double recommendRewardRatio, double money);

        ///见点奖
        double WatchPointsReward(Member member, int startTier, int tiers, double watchPointsRewardRatio);

        ///领导奖
        double LeaderReward(Member member, int startOfLeaderReward, List<double> leaderRewardEachRatio);

        ///对碰奖
        double BinaryReward(Member member, double binaryRewardRatio, double topOfBinaryReward);
    }


}