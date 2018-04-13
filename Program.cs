using System;

namespace SNS_Bonus
{
    class Program
    {
        static void Main(string[] args)
        {
            IMemberPolicy memberPolicy  = new MemberPolicy();
            MemberLevel m = memberPolicy.CalcLevel(6000, memberPolicy.DefaultLevels());
            if (m != null)
            {
                Console.WriteLine("Level is {0},money is {1}", m.Level, m.Money);
            }

        }
    }
}
