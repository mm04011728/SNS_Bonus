using System;
using System.Collections.Generic;

namespace SNS_Bonus
{
    public interface IMemberPolicy
    {
        ///默认会员制度
        List<MemberLevel> DefaultLevels();

        //根据金额计算会员等级
        MemberLevel CalcLevel(double total_money, List<MemberLevel> levels);

        //为一个会员列表添加树状关系，根据会员的上级和上一代ID值建立关系
        void GenarateMembersRelationship(List<Member> members);
    }
}