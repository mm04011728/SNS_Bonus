using System;
using System.Collections.Generic;

namespace SNS_Bonus{
    public interface IBasePolicy
    {
        double TimesFunc(double a, double b);

        double MinFunc(double a, double b);

        double TreeTierFunc(Member member, Func<int, Member, double> releaseBonus, Action<Member, Queue<Member>> enqueue, Func<int, bool> isEnd);

        void TreeTierAction(Member member, Func<int, Member, double, double> eachMemberAction, Action<Member, Queue<Member>> enqueue);
    }
}