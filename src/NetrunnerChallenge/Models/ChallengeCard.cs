﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetrunnerChallenge.Models
{
    public class ChallengeCard
    {
        public string Code { get; set; }
        [Key]
        public int Id { get; set; } 
    }
}