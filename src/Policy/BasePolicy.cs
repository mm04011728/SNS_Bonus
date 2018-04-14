using System;
using System.Linq;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public class BasePolicy : IBasePolicy
    {
        public double TimesFunc(double a, double b)
        {
            return a * b;
        }

        public double MinFunc(double a, double b)
        {
            return a <= b ? a : b;
        }

        public double TreeTierFunc(Member member, Func<int, Member, double> releaseBonus, Action<Member, Queue<Member>> enqueue, Func<int, bool> isEnd)
        {
            if (member == null)
                return 0;
            Member current = null;
            Queue<Member> queue = new Queue<Member>();
            queue.Enqueue(member);
            int cur, last;
            int tier = 0;
            double totalReleaseBonus = 0;
            while (queue.Count != 0)
            {
                //记录本层已经遍历的节点个数
                cur = 0;
                //当遍历完当前层以后，队列里元素全是下一层的元素，队列的长度是这一层的节点的个数
                last = queue.Count;
                //当还没有遍历到本层最后一个节点时循环    
                while (cur < last)
                {
                    //出队一个元素    
                    current = queue.Dequeue();

                    //需要传入一个函数，函数里面需要处理怎么计算释放的红利
                    totalReleaseBonus += releaseBonus(tier, current);

                    cur++;

                    //需要传入一个函数，函数里面要处理怎么将子节点入队
                    enqueue(current, queue);
                }

                //每遍历完一层+1
                tier++;

                //需要传入一个函数，函数要返回一个bool值，指示是否结束树的搜索
                if (isEnd(tier))
                {
                    return totalReleaseBonus;
                }
            }
            return totalReleaseBonus;
        }

        public void TreeTierAction(Member member, Action<Member> eachMemberAction, Action<Member, Queue<Member>> enqueue)
        {
            if (member == null)
                return;
            Member current = null;
            Queue<Member> queue = new Queue<Member>();
            queue.Enqueue(member);
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
                    //出队一个元素    
                    current = queue.Dequeue();
                    //需要传入方法，怎么处理每个元素
                    eachMemberAction(current);
                    cur++;
                    //需要传入一个函数，函数里面要处理怎么将子节点入队
                    enqueue(current, queue);
                }
            }
        }
    }
}