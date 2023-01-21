using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Dal.Dto.Dtos
{
    public class GameUserVoteDto
    {
        public int Id { get; set; }
        public string Wallet { get; set; }
        public int VotePower { get; set; }
    }
}
