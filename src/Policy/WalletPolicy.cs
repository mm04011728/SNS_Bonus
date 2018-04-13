using System;

namespace SNS_Bonus
{
    public class WalletPolicy : IWalletPolicy
    {

        public WalletPolicy(IBasePolicy basePolicy)
        {
            this._basePolicy = basePolicy;
        }
        private IBasePolicy _basePolicy;

        public WalletState DefaultState() => new WalletState()
        {
            ID = Guid.NewGuid(),
            StaticBonusCashRatio = 0.6,
            DynamicBonusCashRatio = 0.6,
            MallRatio = 0.2,
            ManagementCostRatio = 0.1
        };

        //计算管理费用
        public double CalcManagementCost(double totalCosts, double ratio)
            => this._basePolicy.TimesFunc(totalCosts, ratio);

        //计算商城钱包
        public double CalcMallWallet(double totalCosts, double ratio)
            => this._basePolicy.TimesFunc(totalCosts, ratio);

        //计算静态释放现金钱包增加
        public double CalcStaticBonusCashWallet(double releasedStaticBonus, double staticBonusCashRatio, double managementCostRatio, double mallRatio)
        {
            return this._basePolicy.TimesFunc(this._basePolicy.TimesFunc(releasedStaticBonus, staticBonusCashRatio), 1 - managementCostRatio - mallRatio);
        }

        //计算静态释放社交/抢币钱包增加
        public double CalcStaticBonusSNSWallet(double releasedStaticBonus, double staticBonusSNSRatio, double managementCostRatio, double mallRatio)
            => CalcStaticBonusCashWallet(releasedStaticBonus, staticBonusSNSRatio, managementCostRatio, mallRatio);
    }
}