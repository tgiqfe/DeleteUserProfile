using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Management;

namespace DeleteUserProfile
{
    class Program
    {
        //  静的パラメータ
        const string KEY_PROFILE_LIST = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                //  削除対象のユーザー名
                string targetUser = args[0];

                //  現在のプロファイルフォルダーリストを確認
                List<UserInfo> userInfoList = new List<UserInfo>();
                using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(KEY_PROFILE_LIST))
                {
                    foreach (string sid in regKey.GetSubKeyNames())
                    {
                        UserInfo ui = new UserInfo(sid);
                        if (ui.IsEnable())
                        {
                            userInfoList.Add(ui);
                        }
                    }
                }

                //  UserInfoリストから削除対象ユーザーを探す
                if (userInfoList.Count > 0)
                {
                    UserInfo matchUI =
                        userInfoList.FirstOrDefault(x =>
                            x.Name.Equals(targetUser, StringComparison.OrdinalIgnoreCase) ||
                            x.UserName.Equals(targetUser, StringComparison.OrdinalIgnoreCase));
                    if (matchUI != null)
                    {
                        Console.WriteLine("削除対象：" + matchUI.Name);
                        Console.Write("削除します。よろしいですか? (y/N)");
                        string result = Console.ReadLine();
                        if (result.ToLower() == "y")
                        {
                            //  削除実行
                            matchUI.MO_Profile.Delete();
                        }
                        else
                        {
                            Console.WriteLine("削除中止");
                        }
                    }
                    else
                    {
                        Console.WriteLine("削除対象のプロファイルデータ無し");
                    }
                }
                else
                {
                    Console.WriteLine("削除対象のプロファイルデータ無し");
                }
            }
        }
    }
}
