using System.Collections.Generic;

namespace khainguyen_FirstStep.Models
{
    public class FacebookFriendsModel
    {
        public List<FacebookFriend> friendsListing { get; set; }
        
    }
    public class FacebookFriend
    {
        public string name { get; set; }
        public string id { get; set; }
        public string emailfriend { get; set; }
    }

}