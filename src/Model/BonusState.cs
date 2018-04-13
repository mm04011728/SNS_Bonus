using System;

namespace SNS_Bonus
{
    public class BonusState
    {
        public Guid ID { get; set; }

        //红利指数
        public double Index { get; set; }

        //静态释放比例
        public double StaticReleaseRatio { get; set; }

        //见点奖起始层数，层数从1开始
        public int StartTierOfWatchPointsReward { get; set; }

        //见点奖持续层数
        public int TiersOfOfWatchPointsReward { get; set; }

        //见点奖比例
        public double WatchPointsRewardRatio { get; set; }

        //推荐奖是否开启
        public bool IsRecommendingOn { get; set; }

        //领导奖起始代数，代数从1开始
        public int StartOfLeaderReward { get; set; }
    }
}