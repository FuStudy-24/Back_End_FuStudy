using FuStudy_Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuStudy_Model.DTO.Response
{
    public class MentorResponse
    {
        public long Id { get; set; }

        public string AccademyLevel { get; set; }

        public string WorkPlace { get; set; }

        public string OnlineStatus { get; set; }

        public string Skill { get; set; }

        public string Video { get; set; }

        public bool VerifyStatus { get; set; }

        public User User { get; set; }

        public Major Major { get; set; }
    }
}
