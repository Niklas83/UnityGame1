using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GameWCFservice
{
    [DataContract]
    public class UserInformation
    {
        private int _Id;            //Ska hämtas och sättas i DB (Spelar dock ingen roll i denna klass, då den settas från db)

        private string _Name;

        private int _achievementPoints;


        [DataMember(Name = "Id")]
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        [DataMember(Name = "Name")]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [DataMember(Name = "AchievementPoints")]
        public int AchievementPoints
        {
            get { return _achievementPoints; }
            set { _achievementPoints = value; }
        }

    }
}