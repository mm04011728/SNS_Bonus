using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public class Member
    {

        #region 参数状态

        private List<MemberLevel> _levels;

        private BonusState _bonusState;

        private WalletState _walletState;

        private IWalletPolicy _walletPolicy;

        private IBonusPolicy _bonusPolicy;

        private IMemberPolicy _memberPolicy;

        #endregion

        public Member(List<MemberLevel> levels, BonusState bonusState, WalletState walletState,IWalletPolicy walletPolicy,IBonusPolicy bonusPolicy,IMemberPolicy memberPolicy)
        {
            this._bonusState = bonusState;
            this._levels = levels;
            this._walletState = walletState;
            this._walletPolicy = walletPolicy;
            this._bonusPolicy = bonusPolicy;
            this._memberPolicy = memberPolicy;
        }

        #region 基本信息

        public Guid ID { get; set; }

        public string Name { get; set; }

        public string Tel { get; set; }

        #endregion


        #region 计算相关方法

        //静态出局
        public bool StaticOut()
        {
            return this.TotalBonus <= this.StaticGotBonus + 1.0;
        }

        //一般出局
        public bool GeneralOut()
        {
            return this.TotalBonus <= this.TotalGotBonus;
        }

        //充值
        public void Recharge(double money)
        {
            this.RealMoney += money;
            double bonus = this._bonusPolicy.CalcBonus(this._bonusState.Index, money);
            this.increaseBonus(bonus);
            this.RecommendReward(money);
        }

        //红利增加
        private void increaseBonus(double bonus)
        {
            this.RestBonus += bonus;
            this.TotalBonus += bonus;
        }

        //领导奖
        public void LeaderReward()
        {
            double totalReward = this._bonusPolicy.LeaderReward(
                this,
                this._bonusState.StartOfLeaderReward,
                this.Level().LeaderRewardEachRatio
            );
            this.DynamicGotBonus += totalReward;
            this.increaseWallet(totalReward, this._walletState.DynamicBonusCashRatio, this._walletState.DynamicBonusSNSRatio);
        }

        //对碰奖
        public void BinaryReward()
        {
            double totalReward = this.BinaryBonus();
            this.DynamicGotBonus += totalReward;
            this.increaseWallet(totalReward, this._walletState.DynamicBonusCashRatio, this._walletState.DynamicBonusSNSRatio);
        }

        //对碰奖金额
        public double BinaryBonus()
            => this._bonusPolicy.BinaryReward(this,this.Level().BinaryRewardRatio,this.Level().TopOfBinaryReward);

        //见点奖
        public void WatchPointsReward()
        {
            double totalReward = this._bonusPolicy.WatchPointsReward(
                this,
                this._bonusState.StartTierOfWatchPointsReward,
                this._bonusState.TiersOfOfWatchPointsReward,
                this._bonusState.WatchPointsRewardRatio
            );
            this.DynamicGotBonus += totalReward;
            this.increaseWallet(totalReward, this._walletState.DynamicBonusCashRatio, this._walletState.DynamicBonusSNSRatio);
        }

        //推荐奖-奖给推荐人
        public void RecommendReward(double money)
        {
            if (this.Parent != null && this._bonusState.IsRecommendingOn)
            {
                double bonus = this._bonusPolicy.RecommendReward(Parent.Level().RecommendRewardRatio, money);
                this.Parent.DynamicGotBonus += bonus;
                this.Parent.increaseWallet(bonus, this.Parent._walletState.DynamicBonusCashRatio, this.Parent._walletState.DynamicBonusSNSRatio);
            }
        }

        //静态释放红利
        public void StaticReleaseBonus()
        {
            double staticReleaseBonus = this.StaticBonus();
            this.RestBonus -= staticReleaseBonus;
            this.StaticGotBonus += staticReleaseBonus;
            this.increaseWallet(staticReleaseBonus, this._walletState.StaticBonusCashRatio, this._walletState.StaticBonusSNSRatio);
        }

        //静态金额
        public double StaticBonus() => this._bonusPolicy.StaticReleaseBonus(this._bonusState.StaticReleaseRatio, this.RestBonus);


        //钱包增加
        private void increaseWallet(double total, double cashRatio, double SNSRatio)
        {
            this.CashWallet += this._walletPolicy.CalcStaticBonusCashWallet(
                total,
                cashRatio,
                this._walletState.ManagementCostRatio,
                this._walletState.MallRatio
            );
            this.MallWallet += this._walletPolicy.CalcMallWallet(total, this._walletState.MallRatio);
            this.SNSWallet += this._walletPolicy.CalcStaticBonusSNSWallet(
                total,
                SNSRatio,
                this._walletState.ManagementCostRatio,
                this._walletState.MallRatio
            );
        }

        #endregion


        #region 钱包相关属性

        //真实现金额
        public double RealMoney { get; set; }

        //总红利
        public double TotalBonus { get; set; }

        //剩余红利
        public double RestBonus { get; set; }

        //现金钱包
        public double CashWallet { get; set; }

        //商城钱包/二次消费钱包
        public double MallWallet { get; set; }

        //社交钱包/抢币钱包
        public double SNSWallet { get; set; }

        //静态获得红利
        public double StaticGotBonus { get; set; }

        //动态获得红利
        public double DynamicGotBonus { get; set; }

        //总获得红利
        public double TotalGotBonus
        {
            get
            {
                return this.StaticGotBonus + this.DynamicGotBonus;
            }
        }

        #endregion


        #region 会员关系属性

        //会员等级
        public MemberLevel Level() => this._memberPolicy.CalcLevel(this.RealMoney, this._levels);

        //上层领导
        public Member TopMember { get; set; }

        //左膀
        public Member LeftMember { get; set; }

        //右臂
        public Member RightMember { get; set; }

        //上代单亲
        public Member Parent { get; set; }

        //下代子孙
        public List<Member> Children { get; set; }

        #endregion 
    }

}