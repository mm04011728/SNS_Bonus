using System;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public class MemberLevel
    {
        //用于排序的序数
        public int Num { get; set; }

        //等级
        public string Level { get; set; }

        //金额        
        public double Money { get; set; }

        public Guid ID { get; set; }

        //推荐奖比例
        public double RecommendRewardRatio { get; set; }

        //对碰奖比例
        public double BinaryRewardRatio { get; set; }

        //对碰奖封顶额度
        public double TopOfBinaryReward { get; set; }

        //领导奖个代比例比例，序号从0开始
        public List<double> LeaderRewardEachRatio { get; set; }
    }
}
