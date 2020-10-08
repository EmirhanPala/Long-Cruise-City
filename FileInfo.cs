using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LFS_External_Client
{
    static class FileInfo
    {
        const string Database = @"users";

        #region ' New Cruiser, Save User and Save Offline '

        static public void NewCruiser(string Username, string PlayerName)
        {
            if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == false)
            {
                System.IO.StreamWriter Sr = new System.IO.StreamWriter(Database + "\\" + Username + ".txt");
                
                Sr.WriteLine("Cash = 3500");
                Sr.WriteLine("BBal = 0");
                Sr.WriteLine("Gold = 0");
                Sr.WriteLine("Cars = UF1 XFG XRG");
                Sr.WriteLine("Distance = 300000");
                Sr.WriteLine("Health = 100");
                Sr.WriteLine("JobsDone = 0");

                Sr.WriteLine("Goods1 = 0");
                Sr.WriteLine("Goods2 = 0");

                Sr.WriteLine("Member = 0");
                Sr.WriteLine("Officer = 0");
                Sr.WriteLine("Cadet = 0");
                Sr.WriteLine("TowTruck = 0");

                Sr.WriteLine("Raffle = 0");
                Sr.WriteLine("Lotto = 0");

                Sr.WriteLine("Intrfc1 = 0");
                Sr.WriteLine("Intrfc2 = 0");
                Sr.WriteLine("Speedo = 0");
                Sr.WriteLine("Odometer = 0");
                Sr.WriteLine("Counter = 0");
                Sr.WriteLine("Panel = 1");
                
                Sr.WriteLine("Renting = 0");
                Sr.WriteLine("Rented = 0");
                Sr.WriteLine("Renter = 0");
                Sr.WriteLine("Renterr = 0");
                Sr.WriteLine("Rentee = 0");

                Sr.WriteLine("RegInfo = " + PlayerName);

                Sr.WriteLine("//// " + System.DateTime.Now + " New Cruiser");
                Sr.Flush();
                Sr.Close();

                
            }
        }

        static public void SaveUser(clsConnection C)
        {
            StreamWriter Sw = new StreamWriter(Database + "\\" + C.Username + ".txt");
            Sw.WriteLine("Cash = " + C.Cash);
            Sw.WriteLine("BBal = " + C.BankBalance);
            Sw.WriteLine("Gold = " + C.Gold);
            Sw.WriteLine("Cars = " + C.Cars);
            Sw.WriteLine("Distance = " + C.TotalDistance);
            Sw.WriteLine("Health = " + C.TotalHealth);
            Sw.WriteLine("JobsDone = " + C.TotalJobsDone);

            Sw.WriteLine("Goods1 = " + C.Electronics);
            Sw.WriteLine("Goods2 = " + C.Furniture);

            Sw.WriteLine("Member = " + C.IsModerator);
            Sw.WriteLine("Officer = " + C.CanBeOfficer);
            Sw.WriteLine("Cadet = " + C.CanBeCadet);
            Sw.WriteLine("TowTruck = " + C.CanBeTowTruck);

            Sw.WriteLine("Raffle = " + C.LastRaffle);
            Sw.WriteLine("Lotto = " + C.LastLotto);

            Sw.WriteLine("Intrfc1 = " + C.Interface);
            Sw.WriteLine("Intrfc2 = " + C.InGameIntrfc);
            Sw.WriteLine("Speedo = " + C.KMHorMPH);
            Sw.WriteLine("Odometer = " + C.Odometer);
            Sw.WriteLine("Counter = " + C.Counter);
            Sw.WriteLine("Panel = " + C.CopPanel);

            Sw.WriteLine("Renting = " + C.Renting);
            Sw.WriteLine("Rented = " + C.Rented);
            Sw.WriteLine("Renter = " + C.Renter);
            Sw.WriteLine("Renterr = " + C.Renterr);
            Sw.WriteLine("Rentee = " + C.Rentee);

            Sw.WriteLine("RegInfo = " + C.PlayerName);

            Sw.WriteLine("//// " + System.DateTime.Now);
            Sw.Flush();
            Sw.Close();
        }

        static public void SaveOfflineUser(string Username, string PlayerName, long Cash, long BankBalance, string Cars, byte TotalHealth, long TotalDistance, long Gold, int TotalJobsDone, byte Electronics, byte Furniture, byte IsModerator, byte CanBeOfficer, byte CanBeCadet, byte CanBeTowTruck, int LastRaffle, int LastLotto, byte Interface1, byte Interface2, byte Speedo, byte Odometer, byte Counter, byte CopPanel, byte Renting, byte Rented, string Renter, string Rentee, string Renterr)
        {
            StreamWriter Sw = new StreamWriter(Database + "\\" + Username + ".txt");
            Sw.WriteLine("Cash = " + Cash);
            Sw.WriteLine("BBal = " + BankBalance);
            Sw.WriteLine("Gold = " + Gold);
            Sw.WriteLine("Cars = " + Cars);
            Sw.WriteLine("Distance = " + TotalDistance);
            Sw.WriteLine("Health = " + TotalHealth);
            Sw.WriteLine("JobsDone = " + TotalJobsDone);

            Sw.WriteLine("Goods1 = " + Electronics);
            Sw.WriteLine("Goods2 = " + Furniture);

            Sw.WriteLine("Member = " + IsModerator);
            Sw.WriteLine("Officer = " + CanBeOfficer);
            Sw.WriteLine("Cadet = " + CanBeCadet);
            Sw.WriteLine("TowTruck = " + CanBeTowTruck);

            Sw.WriteLine("Raffle = " + LastRaffle);
            Sw.WriteLine("Lotto = " + LastLotto);

            Sw.WriteLine("Intrfc1 = " + Interface1);
            Sw.WriteLine("Intrfc2 = " + Interface2);
            Sw.WriteLine("Speedo = " + Speedo);
            Sw.WriteLine("Odometer = " + Odometer);
            Sw.WriteLine("Counter = " + Counter);
            Sw.WriteLine("Panel = " + CopPanel);

            Sw.WriteLine("Renting = " + Renting);
            Sw.WriteLine("Rented = " + Rented);
            Sw.WriteLine("Renter = " + Renter);
            Sw.WriteLine("Renterr = " + Renterr);
            Sw.WriteLine("Rentee = " + Rentee);

            Sw.WriteLine("RegInfo = " + PlayerName);

            Sw.WriteLine("Last Login : " + System.DateTime.Now);
            Sw.Flush();
            Sw.Close();
        }

        static public string GetUserPlayerName(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "RegInfo")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return Msg[1].Trim();
                }
            }
            Sr.Close();
            return "";
        }

        #endregion

        #region ' Player Stats '

        static public long GetUserCash(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 4) == "Cash")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return long.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public long GetUserBank(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 4) == "BBal")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return long.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public string GetUserCars(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 4) == "Cars")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return Msg[1].Trim();
                }
            }
            Sr.Close();
            return "";
        }

        static public byte GetUserHealth(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Health")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public long GetUserDistance(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 8) == "Distance")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return long.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public long GetUserGold(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 4) == "Gold")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return long.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public int GetUserJobsDone(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 8) == "JobsDone")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return int.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public byte GetUserElectronics(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Goods1")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public byte GetUserFurniture(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Goods2")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public int GetUserLastRaffle(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Raffle")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return int.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        static public int GetUserLastLotto(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 5) == "Lotto")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return int.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        #endregion

        #region ' Player Status '

        public static byte CanBeOfficer(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Officer")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte CanBeCadet(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 5) == "Cadet")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte CanBeTowTruck(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 8) == "TowTruck")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte IsMember(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Member")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        #endregion

        #region ' User Settings '

        public static byte GetInterface(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Intrfc1")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte GetInterface2(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Intrfc2")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte GetSpeedo(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Speedo")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte GetOdometer(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 8) == "Odometer")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte GetCounter(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Counter")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        public static byte GetCopPanel(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 5) == "Panel")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }

        #endregion

        #region ' Player Renting '

        static public byte GetUserRenting(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Renting")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }
        
        static public byte GetUserRented(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Rented")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return byte.Parse(Msg[1].Trim());
                }
            }
            Sr.Close();
            return 0;
        }
        
        static public string GetUserRentee(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Rentee")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return Msg[1].Trim();
                }
            }
            Sr.Close();
            return "";
        }
        
        static public string GetUserRenter(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 6) == "Renter")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return Msg[1].Trim();
                }
            }
            Sr.Close();
            return "";
        }
        
        static public string GetUserRenterr(string Username)
        {
            StreamReader Sr = new StreamReader(Database + "\\" + Username + ".txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 7) == "Renterr")
                {
                    string[] Msg = line.Split('=');
                    Sr.Close();
                    return Msg[1].Trim();
                }
            }
            Sr.Close();
            return "";
        }
        
        #endregion

        static public byte GetUserAdmin(string Username)
        {
            StreamReader Sr = new StreamReader("SuperAdmin.txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 5) == "Admin")
                {
                    string[] Msg = line.Split('=');
                    if (Msg[1].Trim() == Username)
                    {
                        Sr.Close();
                        return 1;
                    }
                }
            }
            Sr.Close();
            return 0;
        }

        static public byte GetUserPermBan(string Username)
        {
            StreamReader Sr = new StreamReader("BanList.txt");

            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 3) == "Ban")
                {
                    string[] Msg = line.Split('=');
                    if (Msg[1].Trim().ToLower() == Username)
                    {
                        Sr.Close();
                        return 1;
                    }
                }
            }
            Sr.Close();
            return 0;
        }

        static public byte SwearFilter(string SwearFilter)
        {
            StreamReader Sr = new StreamReader("Swears.txt");
            string line = null;
            while ((line = Sr.ReadLine()) != null)
            {
                if (line.Substring(0, 5) == "Swear")
                {
                    string[] Msg = line.Split('=');
                    if (Msg[1].Trim() == SwearFilter)
                    {
                        Sr.Close();
                        return 1;
                    }
                }
            }
            Sr.Close();
            return 0;
        }

        static public string AddBanList(string Username)
        {
            StreamReader ApR = new StreamReader("BanList.txt");
            string TempAPR = ApR.ReadToEnd();
            ApR.Close();
            StreamWriter Ap = new StreamWriter("BanList.txt");
            Ap.WriteLine(TempAPR + "Ban = " + Username);
            Ap.Flush();
            Ap.Close();
            return "";
        }
    }
}
