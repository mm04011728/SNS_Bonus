using System;
using System.Collections.Generic;


namespace SNS_Bonus
{
    class Program
    {
        static void Main(string[] args)
        {
            IBasePolicy basePolicy = new BasePolicy();
            IMemberPolicy memberPolicy = new MemberPolicy();
            IBonusPolicy bonusPolicy = new BonusPolicy(basePolicy);
            IWalletPolicy walletPolicy = new WalletPolicy(basePolicy);
            BonusState defaultBonus = bonusPolicy.DefaultState();
            WalletState defaultWallet = walletPolicy.DefaultState();
            List<MemberLevel> defaultMemberLevels = memberPolicy.DefaultLevels();

            printStates(defaultBonus, defaultWallet, defaultMemberLevels);

            List<string> memberTierNums = memberNumGenerator("00", 2, 2, 2, false, 1000);
            List<string> memberGenerationNums = memberNumGenerator("00", 2, 2, 5, true, 1000);
            List<Member> members = memberInitGenarator(
                memberTierNums,
                memberGenerationNums,
                2,
                defaultMemberLevels,
                defaultBonus,
                defaultWallet,
                walletPolicy,
                bonusPolicy,
                memberPolicy
            );

            //充值和推荐奖测试
            testOfRecharge(1000, 100000, members);

            //见点奖测试
            testOfWatchPointsReward(members);

            //对碰奖测试
            testOfBinaryReward(members);

            //领导奖测试
            testOfLeaderReward(members);

            //静态释放测试
            testOfStaticReleaseBonus(members);

            //输出结果
            printResult(members);
        }

        //充值和推荐奖测试
        private static void testOfRecharge(int min, int max, List<Member> members)
        {
            Random random = new Random();
            foreach (var item in members)
            {
                int money = random.Next(min, max);
                item.Recharge(money);

                if (item.Parent != null)
                {
                    // System.Console.WriteLine("【{0}】充值【{1}】过后的推荐人的状态为：", item.Name, money);
                    // System.Console.WriteLine(item.Parent.ToString());
                }
            }
        }


        //见点奖测试
        private static void testOfWatchPointsReward(List<Member> members)
        {
            foreach (var item in members)
            {
                item.WatchPointsReward();
                // System.Console.WriteLine("【{0}】见点奖过后的状态为：", item.Name);
                // System.Console.WriteLine(item.ToString());
            }
        }


        //对碰奖测试
        private static void testOfBinaryReward(List<Member> members)
        {
            foreach (var item in members)
            {
                item.BinaryReward();
                // System.Console.WriteLine("【{0}】对碰奖过后的状态为：", item.Name);
                // System.Console.WriteLine(item.ToString());
            }
        }

        //领导奖测试
        private static void testOfLeaderReward(List<Member> members)
        {
            foreach (var item in members)
            {
                item.LeaderReward();
                // System.Console.WriteLine("【{0}】领导奖过后的状态为：", item.Name);
                // System.Console.WriteLine(item.ToString());
            }
        }

        //静态释放测试
        private static void testOfStaticReleaseBonus(List<Member> members)
        {
            foreach (var item in members)
            {
                item.StaticReleaseBonus();
                // System.Console.WriteLine("【{0}】静态释放过后的状态为：", item.Name);
                // System.Console.WriteLine(item.ToString());
            }
        }

        //输出结果
        private static void printResult(List<Member> members)
        {
            foreach (var item in members)
            {
                System.Console.WriteLine(item.ToString());
            }
        }

        //会员初始化构建器
        private static List<Member> memberInitGenarator(
            List<string> memberTierNums,
            List<string> memberGenerationNums,
            int levelNumLen,
            List<MemberLevel> levels,
            BonusState bonusState,
            WalletState walletState,
            IWalletPolicy walletPolicy,
            IBonusPolicy bonusPolicy,
            IMemberPolicy memberPolicy
        )
        {
            if (memberTierNums.Count != memberGenerationNums.Count)
            {
                throw new Exception("两个编号列表必须数量一致");
            }
            Random random = new Random();
            List<Member> members = new List<Member>();
            Dictionary<string, Member> tierMembers = new Dictionary<string, Member>();
            Dictionary<string, Member> generationMembers = new Dictionary<string, Member>();
            for (int i = 0; i < memberTierNums.Count; i++)
            {
                string tierNum = memberTierNums[i];
                string generationNum = memberGenerationNums[i];
                Member member = new Member(levels, bonusState, walletState, walletPolicy, bonusPolicy, memberPolicy)
                {
                    Name = string.Format("{0}-{1}", tierNum, generationNum),
                    ID = Guid.NewGuid()
                };
                members.Add(member);
                tierMembers.Add(tierNum, member);
                generationMembers.Add(generationNum, member);
                //根据编号层级添加关系
                if (i != 0)
                {
                    Member topMember = tierMembers[tierNum.Substring(0, tierNum.Length - levelNumLen)];
                    Member parent = generationMembers[generationNum.Substring(0, generationNum.Length - levelNumLen)];
                    member.TopMember = topMember;
                    if (topMember.LeftMember == null)
                    {
                        topMember.LeftMember = member;
                    }
                    else
                    {
                        topMember.RightMember = member;
                    }
                    member.Parent = parent;
                    parent.Children.Add(member);
                }
            }
            return members;
        }


        //打印系统设置信息
        private static void printStates(BonusState defaultBonus, WalletState defaultWallet, List<MemberLevel> defaultMemberLevels)
        {
            printTitle();
            printBonusState(defaultBonus);
            printWalletState(defaultWallet);
            printMemberLevels(defaultMemberLevels);
        }
        private static void printTitle()
        {
            System.Console.WriteLine("以下为系统信息");
            System.Console.WriteLine("--------------------------------------------------------------");
        }
        private static void printBonusState(BonusState defaultBonus)
        {
            System.Console.WriteLine("当前系统的【红利指数】为：{0}", defaultBonus.Index);
            System.Console.WriteLine("当前系统的【静态释放比例】为：{0}", defaultBonus.StaticReleaseRatio);
            System.Console.WriteLine("当前系统的【见点奖起始层数】为：{0}", defaultBonus.StartTierOfWatchPointsReward);
            System.Console.WriteLine("当前系统的【见点奖持续层数】为：{0}", defaultBonus.TiersOfOfWatchPointsReward);
            System.Console.WriteLine("当前系统的【见点奖比例】为：{0}", defaultBonus.WatchPointsRewardRatio);
            System.Console.WriteLine("当前系统的【推荐奖是否开启】为：{0}", defaultBonus.IsRecommendingOn);
            System.Console.WriteLine("当前系统的【领导奖起始代数】为：{0}", defaultBonus.StartOfLeaderReward);
            System.Console.WriteLine("--------------------------------------------------------------");
        }

        private static void printWalletState(WalletState defaultWallet)
        {
            System.Console.WriteLine("当前系统的【静态释放现金钱包比例】为：{0}", defaultWallet.StaticBonusCashRatio);
            System.Console.WriteLine("当前系统的【动态释放现金钱包比例】为：{0}", defaultWallet.DynamicBonusCashRatio);
            System.Console.WriteLine("当前系统的【静态释放抢币钱包比例】为：{0}", defaultWallet.StaticBonusSNSRatio);
            System.Console.WriteLine("当前系统的【动态释放抢币钱包比例】为：{0}", defaultWallet.DynamicBonusSNSRatio);
            System.Console.WriteLine("当前系统的【二次消费比例】为：{0}", defaultWallet.MallRatio);
            System.Console.WriteLine("当前系统的【管理费用比例】为：{0}", defaultWallet.ManagementCostRatio);
            System.Console.WriteLine("--------------------------------------------------------------");
        }

        private static void printMemberLevels(List<MemberLevel> levels)
        {
            foreach (var item in levels)
            {
                System.Console.WriteLine("会员的【等级】为：{0}", item.Level);
                System.Console.WriteLine("会员【达到该等级需要充值的金额】为：{0}", item.Money);
                System.Console.WriteLine("该会员等级的【推荐奖比例】为：{0}", item.RecommendRewardRatio);
                System.Console.WriteLine("该会员等级的【对碰奖比例】为：{0}", item.BinaryRewardRatio);
                System.Console.WriteLine("该会员等级的【对碰奖封顶额度】为：{0}", item.TopOfBinaryReward);
                System.Console.WriteLine("该会员等级的【领导奖个代比例比例】为：{0}", string.Join(',', item.LeaderRewardEachRatio));
                System.Console.WriteLine("--------------------------------------------------------------");

            }
        }

        //编号生成器
        private static List<string> memberNumGenerator(string initNum, int numLen, int eachTierMin, int eachTierMax, bool eachTierRandom, int totalCount)
        {
            if ((10 ^ numLen) <= eachTierMax && eachTierRandom)
            {
                throw new Exception("在开启随机的情况下numLen位数不能小于eachTierMax的位数");
            }
            else if ((10 ^ numLen) <= eachTierMin)
            {
                throw new Exception("在不开启随机的情况下numLen位数不能小于eachTierMin的位数");
            }
            else if (eachTierMin > eachTierMax)
            {
                throw new Exception("最小值和最大值不能颠倒");
            }
            int count = 0;
            String current = null;
            Queue<String> queue = new Queue<String>();
            List<string> list = new List<string>();
            list.Add(initNum);
            queue.Enqueue(initNum);
            int cur, last;
            while (queue.Count != 0)
            {
                //记录本层已经遍历的节点个数
                cur = 0;
                //当遍历完当前层以后，队列里元素全是下一层的元素，队列的长度是这一层的节点的个数
                last = queue.Count;
                //当还没有遍历到本层最后一个节点时循环
                while (cur < last)
                {
                    current = queue.Dequeue();
                    cur++;
                    Random random = new Random();
                    int tierCount = eachTierRandom ? random.Next(eachTierMin, eachTierMax + 1) : eachTierMin;
                    for (int i = 0; i < tierCount; i++)
                    {
                        if (count < totalCount)
                        {
                            string toEnqueue = current + i.ToString(string.Format("D{0}", numLen));
                            list.Add(toEnqueue);
                            queue.Enqueue(toEnqueue);
                            count++;
                        }
                        else
                        {
                            return list;
                        }
                    }
                }
            }
            return list;
        }
    }
}
