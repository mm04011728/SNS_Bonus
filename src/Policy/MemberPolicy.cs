using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SNS_Bonus
{
    public class MemberPolicy : IMemberPolicy
    {
        //默认会员制度
        public List<MemberLevel> DefaultLevels() => new List<MemberLevel>(){
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V1",
                    Num = 1,
                    Money = 1000,
                    RecommendRewardRatio = 0.05,
                    BinaryRewardRatio = 0.1,
                    TopOfBinaryReward = 100,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0,0,0,0,0,0,0,0,0
                    }
                },
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V2",
                    Num = 2,
                    Money = 5000,
                    RecommendRewardRatio = 0.06,
                    BinaryRewardRatio = 0.3,
                    TopOfBinaryReward = 500,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0.05,0,0,0,0,0,0,0,0
                    }
                },
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V3",
                    Num = 3,
                    Money = 10000,
                    RecommendRewardRatio = 0.07,
                    BinaryRewardRatio = 0.35,
                    TopOfBinaryReward = 1000,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0.05,0.03,0,0,0,0,0,0,0
                    }
                },
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V4",
                    Num = 4,
                    Money = 30000,
                    RecommendRewardRatio = 0.08,
                    BinaryRewardRatio = 0.4,
                    TopOfBinaryReward = 2000,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0.05,0.03,0.02,0,0,0,0,0,0
                    }
                },
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V5",
                    Num = 5,
                    Money = 50000,
                    RecommendRewardRatio = 0.09,
                    BinaryRewardRatio = 0.45,
                    TopOfBinaryReward = 3000,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0.05,0.03,0.02,0.01,0,0,0,0,0
                    }
                },
                new MemberLevel(){
                    ID = Guid.NewGuid(),
                    Level = "V6",
                    Num = 6,
                    Money = 100000,
                    RecommendRewardRatio = 0.1,
                    BinaryRewardRatio = 0.5,
                    TopOfBinaryReward = 4000,
                    LeaderRewardEachRatio = new List<double>(){
                        0.05,0.05,0.03,0.02,0.01,0.01,0,0,0,0
                    }
                }
            };

        //根据金额计算会员等级
        public MemberLevel CalcLevel(double total_money, List<MemberLevel> levels)
        {
            List<MemberLevel> sorted_levels = levels.OrderBy<MemberLevel, int>(k => k.Num).ToList();
            MemberLevel last = null;
            MemberLevel selected = null;
            for (int i = sorted_levels.Count - 1; i >= 0; i--)
            {
                MemberLevel current = sorted_levels[i];
                if (last != null && current.Money > last.Money)
                {
                    throw new Exception("会员等级列表不正确,无法计算会员等级");
                }
                else
                {
                    if (total_money >= current.Money)
                    {
                        if (selected == null)
                        {
                            selected = current;
                        }
                    }
                }
                last = current;
            }
            return selected;
        }
    }
}