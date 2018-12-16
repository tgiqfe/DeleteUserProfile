using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;

namespace DeleteUserProfile
{
    class UserInfo
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string DomainName { get; set; }
        public string ProfileDir { get; set; }
        public string SID { get; set; }
        public ManagementObject MO_Profile { get; set; }
        public ManagementObject MO_Account { get; set; }

        public UserInfo() { }
        public UserInfo(string sid)
        {
            this.SID = sid;
            MO_Account = new ManagementClass("Win32_UserAccount").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => x["SID"].ToString() == sid);
            MO_Profile = new ManagementClass("Win32_UserProfile").
                GetInstances().
                OfType<ManagementObject>().
                FirstOrDefault(x => x["SID"].ToString() == sid);
            if (MO_Account != null)
            {
                this.Name = MO_Account["Domain"].ToString() + "\\" + MO_Account["Name"].ToString();
                this.DomainName = MO_Account["Domain"].ToString();
                this.UserName = MO_Account["Name"].ToString();
            }
            if (MO_Profile != null)
            {
                this.ProfileDir = MO_Profile["LocalPath"].ToString();
            }
        }

        public bool IsEnable()
        {
            return 
                Name != null &&
                DomainName != null &&
                UserName != null &&
                ProfileDir != null &&
                Directory.Exists(ProfileDir);
        }
    }
}
