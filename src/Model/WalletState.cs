using System;

namespace SNS_Bonus
{
    public class WalletState
    {
        public Guid ID { get; set; }

        //静态释放现金钱包比例
        public double StaticBonusCashRatio { get; set; }

        //动态释放现金钱包比例
        public double DynamicBonusCashRatio { get; set; }


        //静态释放抢币钱包比例
        public double StaticBonusSNSRatio
        {
            get
            {
                return 1 - this.StaticBonusCashRatio;
            }
        }

        //动态释放抢币钱包比例
        public double DynamicBonusSNSRatio
        {
            get
            {
                return 1 - this.DynamicBonusSNSRatio;
            }
        }


        //商城/二次消费比例
        public double MallRatio { get; set; }

        //管理费用比例
        public double ManagementCostRatio { get; set; }
    }
}