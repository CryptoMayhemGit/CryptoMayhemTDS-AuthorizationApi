﻿namespace Mayhem.Dal.Tables
{
    public class GameUser
    {
        public int Id { get; set; }
        public string Wallet { get; set; }
        public int VoteCategoryId { get; set; }
        public VoteCategory VoteCategory { get; set; }
    }
}
