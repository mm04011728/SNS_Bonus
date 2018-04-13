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

        ///计算静态释放现金钱包增加
        double CalcStaticBonusCashWallet(double releasedStaticBonus, double staticBonusCashRatio, double managementCostRatio, double mallRatio);

        ///计算静态释放社交/抢币钱包增加
        double CalcStaticBonusSNSWallet(double releasedStaticBonus, double staticBonusSNSRatio, double managementCostRatio, double mallRatio);
    }
}