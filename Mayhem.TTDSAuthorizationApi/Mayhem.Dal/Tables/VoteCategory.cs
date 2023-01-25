using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Dal.Tables
{
    public class VoteCategory
    {
        public int Id { get; set; }
        public int VotePower { get; set; } = 0;
        public InvestorCategory InvestorCategory { get; set; } = InvestorCategory.None;
    }

    public enum InvestorCategory
    {
        None,
        FirstSale, 
        SecondSale, 
        ThirdSale,
        TGLPNFT,
        Others
    }
}
