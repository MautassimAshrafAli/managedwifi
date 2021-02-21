using Microsoft.VisualBasic;
using NativeWifi;
using System;
using System.Text;

namespace WifiExample
{
    class Program
    {

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString( ssid.SSID, 0, (int) ssid.SSIDLength );
        }
  


        static void Main( string[] args )
        {
            WlanClient client = new WlanClient();
            foreach ( WlanClient.WlanInterface wlanIface in client.Interfaces )
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList( 0 );
                foreach ( Wlan.WlanAvailableNetwork network in networks )
                {
                    if ( network.dot11DefaultCipherAlgorithm == Wlan.Dot11CipherAlgorithm.WEP )
                    {
                        Console.WriteLine( "Found WEP network with SSID {0}.", GetStringForSSID(network.dot11Ssid));
                    }
                }

                // Retrieves XML configurations of existing profiles.
                // This can assist you in constructing your own XML configuration
                // (that is, it will give you an example to follow).
                foreach ( Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles() )
                {
                    string name = profileInfo.profileName; // this is typically the network's SSID
                    string xml = wlanIface.GetProfileXml( profileInfo.profileName );
                }

                string net_name;
                string net_pass;
                int sec_net;

                // Connects to a known network
                Console.WriteLine("----->>Manage known network<<-----");
                Console.WriteLine();
                Console.WriteLine("Insert name of network");
                Console.WriteLine(">>");
                net_name = Console.ReadLine();// this is also the SSID
               //convert ssid to hex
                byte[] ba = Encoding.Default.GetBytes(net_name);
                var hexString = BitConverter.ToString(ba);
                hexString = hexString.Replace("-", "");
                string mac = hexString;

                Console.WriteLine("what is your neatwork security");
                Console.WriteLine("1) Open");
                Console.WriteLine("2) WEP");
                Console.WriteLine("3) WPA2-Personal AES");
                Console.WriteLine(">>");
                sec_net = Convert.ToInt32(Console.ReadLine());
                switch (sec_net)
                {
                    case 1:
                       
                        try
                        {
                            string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>manual</connectionMode><MSM><security><authEncryption><authentication>open</authentication><encryption>none</encryption><useOneX>false</useOneX></authEncryption></security></MSM><MacRandomization xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v3\"><enableRandomization>false</enableRandomization></MacRandomization></WLANProfile>", net_name, mac);
                            wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                            wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, net_name);

                            Console.WriteLine("NETWORK IS CONNECT");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("NETWORK NAME IS NOE VALID");
                        }

                        break;

                            case 2:

                        Console.WriteLine("Insert password of network");
                        Console.WriteLine(">>");
                        net_pass = Console.ReadLine();
                        try
                        {
                            string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID><nonBroadcast>true</nonBroadcast></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><MSM><security><authEncryption><authentication>WEP</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey></security></MSM></WLANProfile>", net_name, mac, net_pass);
                            wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                            wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, net_name);

                            Console.WriteLine("NETWORK IS CONNECT");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("NETWORK PASSWORD OR NAME IS NOE VALID");
                        }

                        break;

                    case 3:

                        Console.WriteLine("Insert password of network");
                        Console.WriteLine(">>");
                        net_pass = Console.ReadLine();
                        try
                        {
                            string profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID><nonBroadcast>true</nonBroadcast></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><MSM><security><authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>{2}</keyMaterial></sharedKey></security></MSM></WLANProfile>", net_name, mac, net_pass);
                            wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                            wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, net_name);

                            Console.WriteLine("NETWORK IS CONNECT");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("NETWORK PASSWORD OR NAME IS NOE VALID");
                        }

                        break;
                }


                
               

                

                
            }
        }
    }
}
