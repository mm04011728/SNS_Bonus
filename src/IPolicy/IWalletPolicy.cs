using System;
using System.Collections.Generic;

namespace SNS_Bonus{
    public interface IWalletPolicy
    {
        WalletState DefaultState();

        ///计算管理费用
        double CalcManagementCost(double totalCosts, double ratio);

        ///计算商城钱包
        double CalcMallWallet(double totalCosts, double ratio);

        ///计算现金钱包增加
        double CalcBonusCashWallet(double releasedStaticBonus, double bonusCashRatio, double managementCostRatio, double mallRatio);

        ///计算社交/抢币钱包增加
        double CalcBonusSNSWallet(double releasedStaticBonus, double bonusSNSRatio);
    }
}