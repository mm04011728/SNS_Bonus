using System;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public class BonusPolicy : IBonusPolicy
    {

        public BonusPolicy(IBasePolicy basePolicy)
        {
            this._basePolicy = basePolicy;
        }

        private IBasePolicy _basePolicy;
        public BonusState DefaultState() => new BonusState()
        {
            ID = Guid.NewGuid(),
            Index = 6.0,
            StaticReleaseRatio = 0.003,
            WatchPointsRewardRatio = 0.01,
            TiersOfOfWatchPointsReward = 10,
            StartTierOfWatchPointsReward = 6,
            IsRecommendingOn = true,
            StartOfLeaderReward = 1
        };

        //总红利
        public double CalcBonus(double bonusIndex, double realMoney)
            => this._basePolicy.TimesFunc(bonusIndex, realMoney);

        //静态释放红利
        public double StaticReleaseBonus(double releaseBonusRatio, double restBonus)
            => this._basePolicy.TimesFunc(releaseBonusRatio, restBonus);

        //推荐奖
        public double RecommendReward(double recommendRewardRatio, double money)
            => this._basePolicy.TimesFunc(recommendRewardRatio, money);

        //见点奖
        public double WatchPointsReward(Member member, int startTier, int tiers, double watchPointsRewardRatio)
            => this._basePolicy.TreeTierFunc(
                member,
                //匿名函数releaseBonus(int tier, Member current)
                (tier, current) => eachBonus(
                    tier,
                    startTier,
                    watchPointsRewardRatio,
                    current.StaticBonus),
                //左右节点入队
                leftRightEnqueue,
                //匿名函数isEnd(int tier)
                currentTier => isEnd(currentTier, startTier, tiers)
            );

        //领导奖
        public double LeaderReward(Member member, int startOfLeaderReward, List<double> leaderRewardEachRatio)
            => this._basePolicy.TreeTierFunc(
                member,
                //匿名函数releaseBonus(int tier, Member current)
                (tier, current) => eachBonus(
                    tier,
                    startOfLeaderReward,
                    leaderRewardEachRatio[tier - 1],
                    current.BinaryBonus),
                //子孙后代入队
                childrenEnqueue,
                //匿名函数isEnd(int tier)
                currentTier => isEnd(currentTier, startOfLeaderReward, leaderRewardEachRatio.Count)
            );

        //计算每个符合资格的子成员对上级的贡献
        private double eachBonus(int tier, int start, double ratio, Func<double> memberFunc)
        {
            double bonus = 0;
            if (tier >= start)
            {
                bonus = this._basePolicy.TimesFunc(memberFunc(), ratio);
            }
            return bonus;
        }

        //判断在多少层停止搜索
        private bool isEnd(int currentTier, int startTier, int tiers)
        {
            if (currentTier == startTier + tiers)
            {
                return true;
            }
            return false;
        }

        //对碰奖
        public double BinaryReward(Member member, double binaryRewardRatio, double topOfBinaryReward)
        {
            double leftPartitionReward = this._basePolicy.TimesFunc(partitionReward(member.LeftMember), binaryRewardRatio);
            double rigthPartitionReward = this._basePolicy.TimesFunc(partitionReward(member.RightMember), binaryRewardRatio);
            double min = this._basePolicy.MinFunc(leftPartitionReward, rigthPartitionReward);
            return this._basePolicy.MinFunc(min, topOfBinaryReward);
        }

        //分区算法
        private double partitionReward(Member leftOrRight)
            => this._basePolicy.TreeTierFunc(
                leftOrRight,
                //匿名函数releaseBonus(int tier, Member current)
                (tier, current) => current.StaticBonus(),
                //左右节点入队
                leftRightEnqueue,
                //匿名函数isEnd(int tier)
                tier => false
            );

        //左右节点入队
        private void leftRightEnqueue(Member current, Queue<Member> queue)
        {
            if (current.LeftMember != null)
            {
                queue.Enqueue(current.LeftMember);
            }
            if (current.RightMember != null)
            {
                queue.Enqueue(current.RightMember);
            }
        }

        //子孙后代入队
        private void childrenEnqueue(Member current, Queue<Member> queue)
        {
            foreach (var item in current.Children)
            {
                queue.Enqueue(item);
            }
        }
    }
}