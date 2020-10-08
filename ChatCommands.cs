using System;
using System.Windows.Forms;
using System.Collections.Generic;
using LFS_External;
using LFS_External.InSim;
using System.Threading;
using System.IO;
using System.Reflection;

namespace LFS_External_Client
{
    public partial class Form1 //Chat commands
    {
        public struct CommandList
        {
            public MethodInfo MethodInf;
            public CommandAttribute CommandArg;
        }
        public List<CommandList> Commands = new List<CommandList>();
        public class CommandAttribute : Attribute
        {
            public string Command;
            public string Syntax;
            public string Description;
            public CommandAttribute(string command, string syntax, string desc)
            {
                Command = command;
                Syntax = syntax;
                Description = desc;
            }
            public CommandAttribute(string command, string syntax)
            {
                Command = command;
                Syntax = syntax;
                Description = "";
            }
        }

        // Your Code Here

        #region ' Trade Car System '

        [Command("trade", "trade <car> <cash> <username>")]
        public void trade(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length >= 4)
            {

                int TradeCash = 0;
                try
                {
                    TradeCash = Convert.ToInt32(StrMsg[2]);

                    if (TradeCash >= 0)
                    {
                        if (Connections[GetConnIdx(MSO.UCID)].Cars.Contains(StrMsg[1].ToUpper()))
                        {
                            foreach (clsConnection R in Connections)
                            {
                                //bool Found = false;
                                string TradeCar = (StrMsg[1].ToUpper());
                                if (R.Username == Msg.Remove(0, (9 + StrMsg[1].Length + StrMsg[2].Length)))
                                {
                                    //Found = true;
                                    if (R.Cars.Contains(TradeCar))
                                    {
                                        if (TradeCar == "UF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "XFG")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "XRG")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "LX4")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "LX6")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "RB4")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FXO")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> FX GT Turbo (FXO) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        /*else if (TradeCar == "VWS")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }*/
                                        else if (TradeCar == "XRT")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "RAC")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FZ5")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "UFR")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "XFR")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FXR")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "XRR")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FZR")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "MRT")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FOX")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FBM")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Formula BMW FB02 (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "FO8")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                        else if (TradeCar == "BF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                        }
                                    }
                                    else if (R.Cash >= TradeCash)
                                    {
                                        string UserCars = Connections[GetConnIdx(SenderUCID)].Cars;
                                        int IdxCar = UserCars.IndexOf(StrMsg[1].ToUpper());
                                        Connections[GetConnIdx(MSO.UCID)].IdxCar = IdxCar;
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 67, 100, 52, 50, 133, R.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^7" + Connections[GetConnIdx(MSO.UCID)].PlayerName + " ^7wants to trade with you.", Flags.ButtonStyles.ISB_LIGHT, 8, 90, 55, 54, 132, R.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^1Car: " + TradeCar + ", ^7Price: €^3" + TradeCash, Flags.ButtonStyles.ISB_C1, 10, 100, 65, 50, 211, R.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^7Do you want to buy this car?", Flags.ButtonStyles.ISB_C1, 10, 100, 77, 50, 212, R.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^2Yes", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 10, 90, 90, 55, 130, R.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^1No", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 10, 90, 101, 55, 131, R.UniqueID, 40, false);
                                        InSim.Send_MTC_MessageToConnection("^3>^7 You have send your offer to: " + R.PlayerName, MSO.UCID, 0);
                                        InSim.Send_MTC_MessageToConnection("^3>^7Car: " + TradeCar + " for: €" + TradeCash, MSO.UCID, 0);
                                        InSim.Send_MTC_MessageToConnection("^3>^7Please wait till he/she decides...", MSO.UCID, 0);
                                        SenderUCID = MSO.UCID;
                                        SentCar = TradeCar;
                                        SentMoney = TradeCash;
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^3>^7 The recipient doesn't have enough cash!", MSO.UCID, 0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            InSim.Send_MTC_MessageToConnection("^3>^7 You don't have that car for this transaction", MSO.UCID, 0);
                        }
                    }
                    else
                    {
                        InSim.Send_MTC_MessageToConnection("^3>^1Error", MSO.UCID, 0);
                    }
                }
                catch
                {
                    InSim.Send_MTC_MessageToConnection("^3>^7Invalid car", MSO.UCID, 0);
                }
            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^3>^1Invalid command. Please see ^2!help^7 for a command list", MSO.UCID, 0);
            }
        }

        #endregion

        #region ' InSim Settings '

        
        [Command("track", "track")]
        public void track(string Msg, string[] StrMsg, Packets.IS_MSO mso)
        {
            var Conn = Connections[GetConnIdx(mso.UCID)];
            if (StrMsg.Length == 2)
            {
                if (Conn.Username == "hristian232")
                {
                    TrackName = Msg.Remove(0, 7);
                    MsgPly("^6>>^7 Track Setted: " + Msg.Remove(0, 7), mso.UCID);
                    AdmBox("> Track Setted in " + Msg.Remove(0, 7));
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", mso.UCID);
            }
            Connections[GetConnIdx(mso.UCID)].WaitCMD = 4;
        }

        #endregion

        #region ' help menu '

        [Command("help", "help")]
        public void help(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 110, 59, 17, 28, 118, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton(CruiseName+" ^7help panel:", Flags.ButtonStyles.ISB_DARK, 6, 50, 19, 30, 101, Conn.UniqueID, 2, false);
                
                InSim.Send_BTN_CreateButton("^2!placehelp ^7- See all commands in places!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 25, 30, 102, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!playerhelp ^7- See all Commands in Player Settings!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 30, 30, 103, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!show ^7- Show your current status to Everyone.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 35, 30, 104, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!location ^7- Show your current location to Everyone.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 40, 30, 105, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!pitlane ^7- Pitlane to pit!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 45, 30, 106, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!cops ^7- See all Online Cops", Flags.ButtonStyles.ISB_LEFT, 5, 55, 50, 30, 107, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!tows ^7- See all Online TowTruck", Flags.ButtonStyles.ISB_LEFT, 5, 55, 55, 30, 108, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!vips ^7- See all Online Members", Flags.ButtonStyles.ISB_LEFT, 5, 55, 60, 30, 109, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!admins ^7- See all Online Admins", Flags.ButtonStyles.ISB_LEFT, 5, 55, 65, 30, 110, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!calltow ^7- Call a Tow Request!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 70, 30, 111, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!buy X ^7- Buy a Car see (!prices)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 75, 30, 112, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!sell X ^7- Sell a Car see (!prices)", Flags.ButtonStyles.ISB_LEFT, 5, 55, 80, 30, 113, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!prices ^7- See all car prices", Flags.ButtonStyles.ISB_LEFT, 5, 55, 85, 30, 114, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!send X Y ^7- Send cash to the username", Flags.ButtonStyles.ISB_LEFT, 5, 55, 90, 30, 115, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!pm X Y ^7- Send Private Message to the username", Flags.ButtonStyles.ISB_LEFT, 5, 55, 95, 30, 116, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!about ^7- See the Information of " + CruiseName, Flags.ButtonStyles.ISB_LEFT, 5, 55, 115, 30, 119, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!trade <car> <money> <license> ^7- Trade cars with other players!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 120, 30, 123, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!kmforcars ^7- See the kilometres for all cars! ", Flags.ButtonStyles.ISB_LEFT, 5, 55, 105, 30, 117, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!fines ^7- See all fines!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 110, 30, 120, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2!report ^7- Report player!", Flags.ButtonStyles.ISB_LEFT, 5, 55, 100, 30, 122, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^1CLOSE [X]", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 13, 127, 50, 121, Conn.UniqueID, 2, false);
                
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 1;
        }

        #endregion

        #region ' Playerhelp '

        [Command("cops", "cops")]
        public void cops(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsOfficer == true && i.CanBeOfficer == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Officer: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsOfficer == false && i.CanBeOfficer == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Officer: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }

                if (i.IsCadet == true && i.CanBeCadet == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Cadet: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsCadet == false && i.CanBeCadet == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Cadet: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^6>>^7 There are no currently Cops online", MSO.UCID);
            }

            Conn.WaitCMD = 4;
        }

        [Command("canceljob", "canceljob")]
        public void canceljob(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];

            if (StrMsg.Length == 1)
            {
                if (Conn.InGame == 0)
                {
                    MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                }
                else if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                {
                    MsgPly("^6>>^7 You don't have any jobs to be canceled!", MSO.UCID);
                }
                else
                {
                    DeleteBTN(27, Conn.UniqueID);
                    DeleteBTN(7, Conn.UniqueID);
                    MsgAll("^6>>^7 " + Conn.PlayerName + " canceled their current job");
                    if (TrackName == "BL1" || TrackName == "KY2X" || TrackName == "BL1X" || TrackName == "WE1X" || TrackName == "AU1X" || TrackName == "AU1")
                    {
                        #region ' JobFromHouses '
                        if (Conn.JobToSchool == true)
                        {
                            if (Conn.JobFromHouse1 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Escort to ^3Lottery^7!", MSO.UCID);
                                Conn.JobFromHouse1 = false;
                            }
                            if (Conn.JobFromHouse2 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Escort to ^3Lottery^7!", MSO.UCID);
                                Conn.JobFromHouse2 = false;
                            }
                            if (Conn.JobFromHouse3 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Escort to ^3Lottery^7!", MSO.UCID);
                                Conn.JobFromHouse3 = false;
                            }
                            Conn.JobToSchool = false;
                        }
                        #endregion

                        #region ' JobFromShop '

                        if (Conn.JobFromShop == true)
                        {
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Hriso's House^7!", MSO.UCID);
                                Conn.JobToHouse1 = false;
                            }

                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Dany's House^7!", MSO.UCID);
                                Conn.JobToHouse2 = false;
                            }

                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Elly's House^7!", MSO.UCID);
                                Conn.JobToHouse3 = false;
                            }

                            Conn.JobFromShop = false;
                        }

                        #endregion

                        #region ' JobFromStore '

                        if (Conn.JobFromStore == true)
                        {
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Hriso's House^7!", MSO.UCID);
                                Conn.JobToHouse1 = false;
                            }

                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Dany's House^7!", MSO.UCID);
                                Conn.JobToHouse2 = false;
                            }

                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^6>>^7 Job Canceled: Delivery to ^3Elly's House^7!", MSO.UCID);
                                Conn.JobToHouse3 = false;
                            }

                            Conn.JobFromStore = false;
                        }

                        #endregion
                    }
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("tows", "tows")]
        public void towtruck(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsTowTruck == true && i.CanBeTowTruck == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Tow Truck: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^2ON-DUTY", MSO.UCID);
                }
                if (i.IsTowTruck == false && i.CanBeTowTruck == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Tow Truck: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^1OFF-DUTY", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^6>>^7 There are no currently TowTrucks online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("vips", "vips")]
        public void members(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                if (i.IsModerator == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 VIP: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^2ONLINE", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^6>>^7 There are no currently Members online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("admins", "admins")]
        public void admins(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            bool Found = false;

            foreach (clsConnection i in Connections)
            {
                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7Information for UF1!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2Required Driver Level ^10", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65, 114, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2Engine: 1.0 litre,Power: 41kW(55 HP)", Flags.ButtonStyles.ISB_LEFT, 4, 40, 66, 65, 115, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^2Top Speed: ^1165 km/h", Flags.ButtonStyles.ISB_LEFT, 4, 40, 71, 65, 116, Conn.UniqueID, 2, false);
                if (i.IsAdmin == 1 && i.IsSuperAdmin == 1)
                {
                    Found = true;
                    MsgPly(" ^6>>^7^3 Admin: ^7" + i.NoColPlyName + " (" + i.Username + ") ^7- ^2ONLINE", MSO.UCID);
                }
            }

            if (Found == false)
            {
                MsgPly("^6>>^7 There are no currently Admins online", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("playerhelp", "playerhelp")]
        public void playerhelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                MsgPly("^6>>^7 Player Help Command: ", MSO.UCID);

                MsgPly("^6>>^7 You can be tow, cadet or officer if you're active player!", MSO.UCID);

                MsgPly("^2!interface ^7- close or open your Interface Display!", MSO.UCID);
                if (Connections[GetConnIdx(MSO.UCID)].CanBeOfficer == 1 || Connections[GetConnIdx(MSO.UCID)].CanBeCadet == 1)
                {
                    MsgPly("^2!cophelp ^7- See all Cop Commands", MSO.UCID);
                }
                if (Connections[GetConnIdx(MSO.UCID)].CanBeTowTruck == 1)
                {
                    MsgPly("^2!towhelp ^7- See all Tow Truck Commands", MSO.UCID);
                }
                if (Connections[GetConnIdx(MSO.UCID)].IsAdmin == 1)
                    MsgPly("^2!adminhelp ^7- See all admin commands!", MSO.UCID);
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("cophelp", "cophelp")]
        public void cophelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                if (Connections[GetConnIdx(MSO.UCID)].CanBeOfficer == 1 || Connections[GetConnIdx(MSO.UCID)].CanBeCadet == 1)
                {
                    MsgPly("^6>>^7 Cop Help Command: ", MSO.UCID);
                    MsgPly("^2!chase  ^7- start a chase or backup", MSO.UCID);
                    MsgPly("^2!disengage ^7- stops the chase on suspect", MSO.UCID);
                    MsgPly("^2!busted ^7- busted suspect when 5 seconds", MSO.UCID);
                    MsgPly("^2!backup ^7- Call a backup (only works cond.2 upwards)", MSO.UCID);
                    MsgPly("^2!cc <message> ^7- Cop Chat", MSO.UCID);
                }
                else
                {
                    MsgPly("^6>>^7 You need to be a Cop in this system!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("towhelp", "towhelp")]
        public void towhelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                if (Connections[GetConnIdx(MSO.UCID)].CanBeTowTruck == 1)
                {
                    MsgPly("^6>>^7 Tow Help Command: ", MSO.UCID);
                    MsgPly("^2!accepttow <username>  ^7- when only call is requested by user!", MSO.UCID);
                    MsgPly("^2!starttow ^7- engage the tow siren!", MSO.UCID);
                    MsgPly("^2!stoptow ^7- stop tow in progress!", MSO.UCID);
                    MsgPly("^2!tc - Tow chat.", MSO.UCID);
                }
                else
                {
                    MsgPly("^6>>^7 You need to be a Tow Truck in this system!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("placehelp", "placehelp")]
        public void placehelp(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            string Car = Connections[GetConnIdx(MSO.UCID)].CurrentCar;
            bool Found = false;
            if (StrMsg.Length == 1)
            {
                MsgPly("^6>>^7 Place Help Command: ", MSO.UCID);
                #region ' Command In Bank '
                if (Conn.InBank == true)
                {
                    Found = true;
                    MsgPly("^2!bank ^7- To see your bank balance.", Conn.UniqueID);
                    MsgPly("^2!check ^7- To see your bank bonus time left.", Conn.UniqueID);
                    MsgPly("^2!insert [amount] ^7- To deposit a cash to your account.", Conn.UniqueID);
                    MsgPly("^2!withdraw [amount] ^7- To withdraw a cash from your account.", Conn.UniqueID);
                }
                #endregion

                #region ' Command In Store '

                if (Conn.InStore == true)
                {
                    Found = true;
                    MsgPly("^2!buy electronic [amount] ^7- Costs: ^1$190 ^7each item", Conn.UniqueID);
                    MsgPly("^2!buy furniture [amount] ^7- Costs: ^1$150 ^7each item", Conn.UniqueID);
                    MsgPly("^2!buy raffle ^7- Costs: ^1$300 ^7Buy Items and win big prizes!", Conn.UniqueID);

                    if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                    {
                        MsgPly("^7Can't take a job whilst being duty!", Conn.UniqueID);
                    }
                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                    {
                        MsgPly("^7Can't do more than 1 job", Conn.UniqueID);
                    }
                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                    {
                        MsgPly("^7Jobs can be only done in Road Cars!", Conn.UniqueID);
                    }
                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                    {
                        MsgPly("^2!job ^7- Wages: ^2$200 - 300", Conn.UniqueID);
                    }
                }

                #endregion

                #region ' Command In Shop '

                if (Conn.InShop == true)
                {
                    Found = true;
                    MsgPly("^6>>^7 !buy chicken - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                    MsgPly("^6>>^7 !buy beer - Costs: ^1$10 ^7Health: ^27%", Conn.UniqueID);
                    MsgPly("^6>>^7 !buy donuts - Costs: ^1$5 ^7Health: ^25%", Conn.UniqueID);

                    if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                    {
                        MsgPly("^7Can't take a job whilst being duty!", Conn.UniqueID);
                    }
                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                    {
                        MsgPly("^7Can't do more than 1 job", Conn.UniqueID);
                    }
                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                    {
                        MsgPly("^7Jobs can be only done in Road Cars!", Conn.UniqueID);
                    }
                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                    {
                        MsgPly("^6>>^7 !job - Wages: ^2$100 - 200", Conn.UniqueID);
                    }
                }

                #endregion

                #region ' Command In School '
                if (Conn.InSchool == true)
                {
                    Found = true;
                    MsgPly("^6>>^7 !buy cake - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                    MsgPly("^6>>^7 !buy lemonade - Costs: ^1$10 ^7Health: ^25%", Conn.UniqueID);
                    MsgPly("^6>>^7 !buy ticket pick - Costs: ^1$100 ^7Pick a number 1 - 10", Conn.UniqueID);
                }
                #endregion

                #region ' Command In Houses '

                if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                {
                    Found = true;
                    if (Conn.Electronics > 0)
                    {
                        MsgPly("^2!sell electronic [amount] ^7- for ^2$" + Conn.SellElectronics + " ^7each.", Conn.UniqueID);
                    }
                    else
                    {
                        MsgPly("^7 You don't have enough items to trade any Electronic!", Conn.UniqueID);
                    }

                    if (Conn.Furniture > 0)
                    {
                        MsgPly("^2!sell furniture [amount] ^7- for ^2$" + Conn.SellFurniture + " ^7each.", Conn.UniqueID);
                    }
                    else
                    {
                        MsgPly("^7 You don't have enough items to trade any Furniture!", Conn.UniqueID);
                    }


                    if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                    {
                        MsgPly("^7Can't take a job whilst being duty!", Conn.UniqueID);
                    }
                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                    {
                        MsgPly("^7Can't do more than 1 job", Conn.UniqueID);
                    }
                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                    {
                        MsgPly("^7Jobs can be only done in Road Cars!", Conn.UniqueID);
                    }
                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                    {
                        MsgPly("^2!job ^7- Escort a Children to ^3Lottery.", Conn.UniqueID);
                    }
                }

                #endregion

                if (Found == false)
                {
                    MsgPly("^6>>^7 You need to be in House or Establishments!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("showoff", "showoff")]
        public void showoff(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            
            #region ' Normal Showoff '
            if (StrMsg.Length == 1)
            {
                MsgAll(" ^2///////////////////////////////////////////////////");
                MsgAll(" ^6>>^1 Showoff: ^7" + Conn.NoColPlyName + " (" + Conn.Username + ")");
                MsgAll(" ^6>>^1 Cash: ^7$" + string.Format("{0:n0}", Conn.Cash));
                MsgAll(" ^6>>^1 Bank Cash: ^7$" + string.Format("{0:n0}", Conn.BankBalance));
                MsgAll(" ^6>>^1 Distance: ^7" + Conn.TotalDistance / 1000 + " kms");


                #region ' Bonus Lines '
                if (Conn.TotalBonusDone == 0)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 400 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 1)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 600 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 2)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 800 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 3)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 1200 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 4)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 1600 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 5)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 2000 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 6)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 2600 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 7)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 3200 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 8)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 4000 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 9)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 5400 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                else if (Conn.TotalBonusDone == 10)
                {
                    MsgAll(" ^6>>^7^1 Bonus: ^7" + Conn.BonusDistance / 6600 + "% ^7/^1 Health: ^7" + Conn.TotalHealth + "%");
                }
                #endregion

                #region ' Status '
                if (Conn.CanBeOfficer == 1 && Conn.CanBeTowTruck == 1 && Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Officer, Tow Truck.");
                }
                else if (Conn.CanBeCadet == 1 && Conn.CanBeTowTruck == 1 && Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Cadet, Tow Truck.");
                }
                else if (Conn.CanBeOfficer == 1 && Conn.CanBeTowTruck == 1 && Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator, Officer, Tow Truck.");
                }
                else if (Conn.CanBeCadet == 1 && Conn.CanBeTowTruck == 1 && Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator, Cadet, Tow Truck.");
                }


                else if (Conn.CanBeCadet == 1 && Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Cadet.");
                }
                else if (Conn.CanBeOfficer == 1 && Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Officer.");
                }
                else if (Conn.CanBeTowTruck == 1 && Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Tow Truck.");
                }

                else if (Conn.CanBeCadet == 1 && Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator, Cadet.");
                }
                else if (Conn.CanBeOfficer == 1 && Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator, Officer.");
                }
                else if (Conn.CanBeTowTruck == 1 && Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator, Tow Truck.");
                }


                else if (Conn.CanBeCadet == 1 && Conn.CanBeTowTruck == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Cadet, Tow Truck.");
                }
                else if (Conn.CanBeOfficer == 1 && Conn.CanBeTowTruck == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Officer, Tow Truck.");
                }

                else if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Admin.");
                }
                else if (Conn.CanBeCadet == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Cadet.");
                }
                else if (Conn.CanBeOfficer == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Officer.");
                }
                else if (Conn.CanBeTowTruck == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Tow Truck.");
                }
                else if (Conn.IsModerator == 1)
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Moderator.");
                }
                else
                {
                    MsgAll(" ^6>>^7^1 Status: ^7Civilian.");
                }
                #endregion

                #region ' Car Lines '
                if (Conn.Cars.Length > 52 && Conn.Cars.Length < 84)
                {
                    MsgAll(" ^6>>^7^1 Cars: ^7" + Conn.Cars.Remove(39, Conn.Cars.Length - 39));
                    MsgAll(" ^6>>^7^1 ^7" + Conn.Cars.Remove(0, 40));
                }
                else
                {
                    MsgAll(" ^6>>^7^1 Cars: ^7" + Conn.Cars);
                }
                #endregion

                MsgAll(" ^2///////////////////////////////////////////////////");
            }
            #endregion

            #region ' Force Showoff '
            else if (StrMsg.Length > 1)
            {
                if (Conn.IsModerator == 1 || Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                {
                    if (Conn.Username == Msg.Remove(0, 9))
                    {
                        MsgPly("^6>>^7 You can't force show-off yourself!", MSO.UCID);
                    }
                    else
                    {
                        bool Found = false;
                        foreach (clsConnection C in Connections)
                        {
                            if (C.Username == Msg.Remove(0, 9))
                            {
                                Found = true;
                                MsgAll(" ^2///////////////////////////////////////////////////");
                                MsgAll(" ^6>>^7^1 Force Showoff: ^7" + C.NoColPlyName + " (" + C.Username + ")");
                                MsgAll(" ^6>>^1 Cash: ^7$" + string.Format("{0:n0}", Conn.Cash));
                                MsgAll(" ^6>>^1 Bank Cash: ^7$" + string.Format("{0:n0}", Conn.BankBalance));
                                MsgAll(" ^6>>^7^1 Distance: ^7" + C.TotalDistance / 1000 + " kms");

                                #region ' Bonus Lines '
                                if (C.TotalBonusDone == 0)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 400 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 600 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 2)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 800 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 3)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 1200 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 4)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 1600 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 5)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 2000 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 6)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 2600 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 7)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 3200 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 8)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 4000 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 9)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 5400 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                else if (C.TotalBonusDone == 10)
                                {
                                    MsgAll(" ^6>>^7^1 Bonus: ^7" + C.BonusDistance / 6600 + "% ^7/^1 Health: ^7" + C.TotalHealth + "%");
                                }
                                #endregion

                                #region ' Status '
                                if (C.CanBeOfficer == 1 && C.CanBeTowTruck == 1 && C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Officer, Tow Truck.");
                                }
                                else if (C.CanBeCadet == 1 && C.CanBeTowTruck == 1 && C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Cadet, Tow Truck.");
                                }
                                else if (C.CanBeOfficer == 1 && C.CanBeTowTruck == 1 && C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP, Officer, Tow Truck.");
                                }
                                else if (C.CanBeCadet == 1 && C.CanBeTowTruck == 1 && C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP, Cadet, Tow Truck.");
                                }


                                else if (C.CanBeCadet == 1 && C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Cadet.");
                                }
                                else if (C.CanBeOfficer == 1 && C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Officer.");
                                }
                                else if (C.CanBeTowTruck == 1 && C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin, Tow Truck.");
                                }

                                else if (C.CanBeCadet == 1 && C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP, Cadet.");
                                }
                                else if (C.CanBeOfficer == 1 && C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP, Officer.");
                                }
                                else if (C.CanBeTowTruck == 1 && C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP, Tow Truck.");
                                }


                                else if (C.CanBeCadet == 1 && C.CanBeTowTruck == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Cadet, Tow Truck.");
                                }
                                else if (C.CanBeOfficer == 1 && C.CanBeTowTruck == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Officer, Tow Truck.");
                                }

                                else if (C.IsAdmin == 1 && C.IsSuperAdmin == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Admin.");
                                }
                                else if (C.CanBeCadet == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Cadet.");
                                }
                                else if (C.CanBeOfficer == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Officer.");
                                }
                                else if (C.CanBeTowTruck == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Tow Truck.");
                                }
                                else if (C.IsModerator == 1)
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7VIP.");
                                }
                                else
                                {
                                    MsgAll(" ^6>>^7^1 Status: ^7Civilian.");
                                }
                                #endregion

                                #region ' Car Lines '
                                if (C.Cars.Length > 52 && C.Cars.Length < 84)
                                {
                                    MsgAll(" ^6>>^7^1 Cars: ^7" + C.Cars.Remove(39, C.Cars.Length - 39));
                                    MsgAll(" ^6>>^7^1 ^7" + C.Cars.Remove(0, 40));
                                }
                                else
                                {
                                    MsgAll(" ^6>>^7^1 Cars: ^7" + C.Cars);
                                }
                                #endregion

                                MsgAll(" ^2///////////////////////////////////////////////////");
                            }
                        }
                        if (Found == false)
                        {
                            MsgPly("^6>>^7 Username not found.", MSO.UCID);
                        }
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized.", MSO.UCID);
                }
            }
            #endregion

            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("about", "about")]
        public void about(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                MsgPly("^7About: " + CruiseName + " ^7v" + InSimVer, MSO.UCID);
                MsgPly("^7All credits goes to:", MSO.UCID);
                MsgPly("^7Crazyboy and all cruisers who keep", MSO.UCID);
                MsgPly("^7this server alive!", MSO.UCID);
                MsgPly("^7Comming soon our forum!!!", MSO.UCID);
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("location", "location")]
        public void locate(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsSuperAdmin == 1)
            if (StrMsg.Length == 1)
            {
                MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^1is located at ^7" + Conn.Location);
                if (Conn.InGame == 1)
                {
                    MsgAll("^1  Positioned at ^7X: " + Conn.CompCar.X / 196608 + " Y: " + Conn.CompCar.Y / 196608);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("pitlane", "pitlane")]
        public void pitlane(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Towee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.Location.Contains("Spectators") || Conn.Location.Contains("Fix 'N' Repair Station"))
                {
                    MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                }
                else
                {
                    if (Conn.IsSuspect == false && RobberUCID != MSO.UCID && Conn.IsBeingBusted == false)
                    {
                        if (Conn.IsOfficer == false && Conn.InTowProgress == false && Conn.IsBeingTowed == false)
                        {
                            #region ' Not Officer '
                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                            {
                                if (Conn.Cash > 500)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitlaned for ^1$500^7.");
                                    Conn.Cash -= 500;
                                    PitlaneID(Conn.PlayerName);
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Not Enough cash to get pitlaned!", MSO.UCID);
                                }
                            }
                            else
                            {
                                if (Conn.Cash > 500)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitlaned for ^1$500^7.");
                                    Conn.Cash -= 500;
                                    PitlaneID(Conn.PlayerName);
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Not Enough cash to get pitlaned!", MSO.UCID);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (Conn.IsOfficer == true)
                            {
                                if (Conn.InChaseProgress == true)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7lost ^3" + ChaseCon.NoColPlyName + "!");

                                    #region ' Disengage Joined in chase '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            Conn.JoinedChase = false;
                                        }
                                        Conn.ChaseCondition = 0;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        Conn.BustedTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Disengage '
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        AddChaseLimit -= 1;
                                        Conn.AutoBumpTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.BustedTimer = 0;
                                        Conn.Chasee = -1;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitlaned.");
                                    PitlaneID(Conn.Username);
                                }
                                else
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitlaned.");
                                    PitlaneID(Conn.Username);
                                }
                            }
                            if (Conn.InTowProgress == true)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + TowCon.NoColPlyName + "!");
                                TowCon.IsBeingTowed = false;
                                Conn.Towee = -1;
                                Conn.InTowProgress = false;
                                CautionSirenShutOff();

                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitlaned.");
                                PitlaneID(Conn.Username);
                            }
                            if (Conn.IsBeingTowed == true)
                            {
                                MsgPly("^6>>^7 Can't pitlane whilst being towed.", MSO.UCID);
                            }
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 Can't pitlane whilst being chased/busted!", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 3;
        }

        [Command("send", "send <amount>")]
        public void send(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 2)
            {
                try
                {
                    int Send = int.Parse(StrMsg[1]);
                    bool SendFound = false;
                    var Conn = Connections[GetConnIdx(MSO.UCID)];
                    #region ' Send Access '
                    if (Connections[GetConnIdx(MSO.UCID)].Username == Msg.Remove(0, 7 + StrMsg[1].Length))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 You can't send money to yourself!", MSO.UCID, 0);
                    }
                    else
                    {
                        foreach (clsConnection C in Connections)
                        {
                            if (C.Username == Msg.Remove(0, 7 + StrMsg[1].Length))
                            {
                                SendFound = true;
                                if (StrMsg[1].Contains("-"))
                                {
                                    InSim.Send_MTC_MessageToConnection("^6>>^7 Send Error. Please don't use minus values!", MSO.UCID, 0);
                                }
                                else if (Send > 9001)
                                {
                                    InSim.Send_MTC_MessageToConnection("^6>>^7 Can't Send more than 9000!", MSO.UCID, 0);
                                }
                                else if (Connections[GetConnIdx(MSO.UCID)].Cash < Send)
                                {
                                    InSim.Send_MTC_MessageToConnection("^6>>^7 Not Enough Money to Send the Transfer", MSO.UCID, 0);
                                }
                                else
                                {
                                    C.Cash += Send;
                                    Conn.Cash -= Send;
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7sent ^2$" + Send);
                                    MsgAll("^6>>^7 To: " + C.NoColPlyName + " (" + C.Username + ")");
                                }
                            }
                        }
                        if (SendFound == false)
                        {
                            InSim.Send_MTC_MessageToConnection("^6>>^7 Username not Exists or Not Found", MSO.UCID, 0);
                        }
                    }
                    #endregion
                }
                catch
                {
                    InSim.Send_MTC_MessageToConnection("^1»^7 Values to high or Incomplete Command", MSO.UCID, 0);
                }
            }
            else
            {
                if (StrMsg.Length == 1)
                {
                    MsgPly("^6>>^7 Sending parameter ^2!send X Y", MSO.UCID);
                    MsgPly("^6>>^7 X: Username Y: Send Cash", MSO.UCID);
                }
                else
                {
                    MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }

        [Command("pm", "pm <username> <message>")]
        public void pm(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 2)
            {
                if (Connections[GetConnIdx(MSO.UCID)].Username == StrMsg[1] && StrMsg[1].Length > 1)
                {
                    InSim.Send_MTC_MessageToConnection("^6>>^7 You can't send PM to yourself!", MSO.UCID, 0);
                }
                else
                {
                    clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];
                    bool PMUserFound = false;
                    foreach (clsConnection C in Connections)
                    {
                        string Message = Msg.Remove(0, C.Username.Length + 5);

                        if (C.Username == StrMsg[1] && StrMsg[1].Length > 1)
                        {
                            PMUserFound = true;

                            InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 Message Sent To: ^7" + C.NoColPlyName + " (" + C.Username + ")", MSO.UCID, 0);
                            InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 Msg: ^7" + Message, MSO.UCID, 0);

                            PMBox("> PM Msg From: " + Conn.NoColPlyName + " (" + Conn.Username + ") to " + C.NoColPlyName + " (" + C.Username + ")");
                            PMBox("> Msg: " + Message);
                            
                            InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 PM From: ^7" + Conn.NoColPlyName + " (" + Conn.Username + ")", C.UniqueID, 0);
                            InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 Msg: ^7" + Message, C.UniqueID, 0);
                            InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 To reply use ^2!pm " + Conn.Username + " <message>", C.UniqueID, 0);

                            foreach (clsConnection F in Connections)
                            {
                                if ((F.IsAdmin == 1 && F.IsSuperAdmin == 1) && F.UniqueID != MSO.UCID)
                                {
                                    InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 PM From: ^7" + Conn.NoColPlyName + " to " + C.NoColPlyName, F.UniqueID, 0);
                                    InSim.Send_MTC_MessageToConnection(" ^6>>^7^3 Msg: ^7" + Message, F.UniqueID, 0);
                                }
                            }
                        }
                    }
                    if (PMUserFound == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Username not found.", MSO.UCID, 0);
                    }
                }
            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^6>>^7 Invalid Command.", MSO.UCID, 0);
            }
        }

        [Command("calltow", "calltow")]
        public void towreq(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];

            if (Conn.InGame == 0)
            {
                MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
            }
            else
            {
                if (Conn.Location.Contains("Service Station"))
                {
                    MsgPly("^6>>^7 You can't call a tow request in Service Station!", MSO.UCID);
                }
                else
                {
                    if (Conn.IsTowTruck == false)
                    {

                        if (Conn.IsBeingTowed == false)
                        {
                            if (Conn.CompCar.Speed / 91 < 5)
                            {
                                if (Conn.CalledRequest == false)
                                {
                                    #region ' get request '
                                    bool Found = false;
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.IsTowTruck == true && i.CanBeTowTruck == 1)
                                        {
                                            Found = true;
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " ^7called a Request!", i.UniqueID);
                                            MsgPly(" ^6>>^7^3 Located at ^3" + Conn.Location, i.UniqueID);
                                            MsgPly(" ^6>>^7^3 To Accept Request ^2!accepttow " + Conn.Username, i.UniqueID);
                                        }
                                    }
                                    if (Found == true)
                                    {
                                        if (Conn.CalledRequest == false)
                                        {
                                            MsgPly("^6>>^7 Please wait till your request reached!", MSO.UCID);
                                            Conn.CalledRequest = true;
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 There are no Tow Trucks online :(", MSO.UCID);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 You have called a Tow Request Please wait.", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 You can't call a request whilst your vehicle is running!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Your already being towed!", MSO.UCID);

                        }
                    }
                    else
                    {
                       MsgPly("^6>>^7 You can't call a request whilst being duty!", MSO.UCID);
                    }
                }
            }

            Conn.WaitCMD = 4;
        }

        [Command("fines", "fines")]
        public void fines(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length == 1)
            {
                MsgPly(CruiseName + " ^7fine list:", MSO.UCID);
                MsgPly("^7swearing - $750", MSO.UCID);
                MsgPly("^7speeding - $250", MSO.UCID);
                MsgPly("^7wrong way - $500", MSO.UCID);
                MsgPly("^7ramming - $500", MSO.UCID);
                MsgPly("^7lights - $250", MSO.UCID);
                MsgPly("^7dangerous driving - $1000", MSO.UCID);
                MsgPly("^7illegal parking - $500", MSO.UCID);
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Connections[GetConnIdx(MSO.UCID)].WaitCMD = 4;
        }


        /*[Command("speedo", "speedo <MPH/KMH>")]
        public void speedo(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 2)
            {
                bool FoundCMD = false;

                if (StrMsg[1].ToLower() == "kmh")
                {
                    if (Conn.KMHorMPH == 1)
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^3 Your Settings is now in Kilometers per Hour mode.", MSO.UCID);
                        Conn.KMHorMPH = 0;
                    }
                    else
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^3 The Settings is now in Kilometers per Hour mode.", MSO.UCID);
                    }
                }
                if (StrMsg[1].ToLower() == "mph")
                {
                    if (Conn.KMHorMPH == 0)
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 Your Settings is now in Miles per Hour mode.", MSO.UCID);
                        Conn.KMHorMPH = 1;
                    }
                    else
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 The Settings is now in Miles per Hour mode.", MSO.UCID);
                    }
                }

                if (FoundCMD == false)
                {
                    MsgPly("^6>>^7 Correct Cmnd format ^2!speedo mph or kmh", MSO.UCID);
                }
            }
            else
            {
                if (StrMsg.Length == 1)
                {
                    MsgPly("^6>>^7 Correct Cmnd format ^2!speedo mph or kmh", MSO.UCID);
                }
                else
                {
                    MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            Conn.WaitCMD = 4;
        }*/

        /*[Command("odometer", "odometer <KMS/MILES")]
        public void distance(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 2)
            {
                bool FoundCMD = false;

                if (StrMsg[1].ToLower() == "kms")
                {
                    if (Conn.Counter == 1)
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 Your Settings is now in Kilometers Distance mode.", MSO.UCID);
                        Conn.Counter = 0;
                    }
                    else
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 The Settings is now in Kilometers Distance mode.", MSO.UCID);
                    }
                }
                if (StrMsg[1].ToLower() == "miles")
                {
                    if (Conn.Counter == 0)
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 Your Settings is now in Miles Distance mode.", MSO.UCID);
                        Conn.Counter = 1;
                    }
                    else
                    {
                        FoundCMD = true;
                        MsgPly("^6>>^7 The Settings is now in Miles Distance mode.", MSO.UCID);
                    }
                }

                if (FoundCMD == false)
                {
                    MsgPly("^6>>^7 Correct Cmnd format ^2!distance kms or miles", MSO.UCID);
                }
            }
            else
            {
                if (StrMsg.Length == 1)
                {
                    MsgPly("^6>>^7 Correct Cmnd format ^2!distance kms or miles", MSO.UCID);
                }
                else
                {
                    MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            Conn.WaitCMD = 4;
        }
        */

       
        #endregion

        #region ' Player Settings Command '

        [Command("coppanel", "coppanel")]
        public void coppanel(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.CopPanel == 0)
                    {
                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            MsgPly("^6>>^7 Panel Clicks are now Enabled.", MSO.UCID);
                        }
                        else
                        {
                            MsgPly("^6>>^7 Panel Clicks are now Enabled.", MSO.UCID);
                            MsgPly("  ^7You need to be duty to look at the Panel!", MSO.UCID);
                        }
                        Conn.CopPanel = 1;
                    }
                    else if (Conn.CopPanel == 1)
                    {
                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            MsgPly("^6>>^7 Panel Clicks are now Disabled.", MSO.UCID);
                        }
                        else
                        {
                            MsgPly("^6>>^7 Panel Clicks are now Disabled.", MSO.UCID);
                            MsgPly("  ^7You need to be duty to look at the Panel!", MSO.UCID);
                        }
                        Conn.CopPanel = 0;
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^6>>^7 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion

        #region ' Prices, Tags, Report, Buy and Sell Commands '

        [Command("tags", "tags")]
        public void tagi(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                MsgPly(CruiseName + " ^7Tag List:", MSO.UCID);

                MsgPly("^3>^3 Officer tag ^7- " + OfficerTag, MSO.UCID);
                MsgPly("^3>^3 Cadet tag ^7- " + CadetTag, MSO.UCID);
                MsgPly("^3>^3 TowTruck tag ^7- " + TowTruckTag, MSO.UCID);
            }
        }

        [Command("report", "report <msg>")]
        public void repor1t(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            if (StrMsg.Length > 1)
            {
                {
                    MsgPly("^3>^7 Your message will be forwarded to the administrator!", MSO.UCID);
                }
                clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];

                foreach (clsConnection C in Connections)
                {

                    string Message2 = Msg.Remove(0, 7);

                    if (C.IsAdmin == 1 && C.IsSuperAdmin == 1 && C.UniqueID != MSO.UCID)
                    {
                        {
                            InSim.Send_MTC_MessageToConnection("^3>^3 Report by: ^7" + Conn.PlayerName, C.UniqueID, 0);
                            InSim.Send_MTC_MessageToConnection("^3>^3" + Message2, C.UniqueID, 0);
                            AdmBox(">>Report by:" + Conn.NoColPlyName);
                            AdmBox(">> " + Message2);
                        }
                    }


                }

            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^3>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID, 0);
            }
        }
        
        [Command("prices", "prices")]
        public void ludvig(String Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 90, 59, 17, 28, 118, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton(CruiseName+" ^7Price list:", Flags.ButtonStyles.ISB_DARK, 6, 50, 19, 30, 101, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7LX4 - ^2$" + Dealer.GetCarPrice("LX4") + " ^7Required Driver Level ^11", Flags.ButtonStyles.ISB_LEFT, 5, 55, 25, 30, 102, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7LX6 - ^2$" + Dealer.GetCarPrice("LX6") + " ^7Required Driver Level ^11", Flags.ButtonStyles.ISB_LEFT, 5, 55, 30, 30, 103, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7RB4 - ^2$" + Dealer.GetCarPrice("RB4") + " ^7Required Driver Level ^11", Flags.ButtonStyles.ISB_LEFT, 5, 55, 35, 30, 104, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7FXO - ^2$" + Dealer.GetCarPrice("FXO") + " ^7Required Driver Level ^12", Flags.ButtonStyles.ISB_LEFT, 5, 55, 40, 30, 105, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7XRT - ^2$" + Dealer.GetCarPrice("XRT") + " ^7Required Driver Level ^12", Flags.ButtonStyles.ISB_LEFT, 5, 55, 45, 30, 106, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7RAC - ^2$" + Dealer.GetCarPrice("RAC") + " ^7Required Driver Level ^13", Flags.ButtonStyles.ISB_LEFT, 5, 55, 50, 30, 107, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7FZ5 - ^2$" + Dealer.GetCarPrice("FZ5") + " ^7Required Driver Level ^13", Flags.ButtonStyles.ISB_LEFT, 5, 55, 55, 30, 108, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7UFR - ^2$" + Dealer.GetCarPrice("UFR") + " ^7Required Driver Level ^14", Flags.ButtonStyles.ISB_LEFT, 5, 55, 60, 30, 109, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7XFR - ^2$" + Dealer.GetCarPrice("XFR") + " ^7Required Driver Level ^14", Flags.ButtonStyles.ISB_LEFT, 5, 55, 65, 30, 110, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7FXR - ^2$" + Dealer.GetCarPrice("FXR") + " ^7Required Driver Level ^15", Flags.ButtonStyles.ISB_LEFT, 5, 55, 70, 30, 111, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7XRR - ^2$" + Dealer.GetCarPrice("XRR") + " ^7Required Driver Level ^15", Flags.ButtonStyles.ISB_LEFT, 5, 55, 75, 30, 112, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7FZR - ^2$" + Dealer.GetCarPrice("FZR") + " ^7Required Driver Level ^16", Flags.ButtonStyles.ISB_LEFT, 5, 55, 80, 30, 113, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7MRT - ^2$" + Dealer.GetCarPrice("MRT") + " ^7Required Driver Level ^16", Flags.ButtonStyles.ISB_LEFT, 5, 55, 85, 30, 114, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^7FBM - ^2$" + Dealer.GetCarPrice("FBM") + " ^7Required Driver Level ^16", Flags.ButtonStyles.ISB_LEFT, 5, 55, 90, 30, 115, Conn.UniqueID, 2, false);
                InSim.Send_BTN_CreateButton("^1CLOSE [X]", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 13, 97, 50, 121, Conn.UniqueID, 2, false);

            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
        }

       

        #endregion

        

        #region ' Cop and TowTruck Commands '

        [Command("duty", "duty")]
        public void duty(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.InGame == 0)
                {
                    MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                }
                else if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                {
                    #region ' Officer Duty '
                    if (Conn.PlayerName.Contains(OfficerTag))
                    {
                        if (Conn.CanBeOfficer == 1)
                        {
                            if (Conn.Plate == "Police")
                            {
                                if (Conn.IsOfficer == false)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2DUTY ^7as a Cop!");
                                    Conn.LastName = Conn.NoColPlyName;

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is disabled", MSO.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", MSO.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is enabled", MSO.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", MSO.UCID);
                                    }

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    TotalOfficers += 1;
                                    Conn.IsOfficer = true;
                                    Conn.LastName = Conn.NoColPlyName;
                                }
                                else
                                {
                                    MsgPly("^6>>^7 You are now duty as a Cop.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Your Platenumber must be Police!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Not Authorized Cop!", MSO.UCID);
                        }
                    }
                    #endregion

                    #region ' Cadet Duty '
                    if (Conn.PlayerName.Contains(CadetTag))
                    {
                        if (Conn.CanBeCadet == 1)
                        {
                            if (Conn.Plate == "Police")
                            {
                                if (Conn.IsCadet == false)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2DUTY ^7as a Cop!");
                                    Conn.LastName = Conn.NoColPlyName;

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is disabled", MSO.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", MSO.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is enabled", MSO.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", MSO.UCID);
                                    }

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    Conn.IsCadet = true;
                                    Conn.LastName = Conn.NoColPlyName;
                                }
                                else
                                {
                                    MsgPly("^6>>^7 You are now duty as a Cop.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Your Platenumber must be Police!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Not Authorized Cop!", MSO.UCID);
                        }
                    }
                    #endregion

                    #region ' Tow Truck Duty '
                    if (Conn.PlayerName.Contains(TowTruckTag))
                    {
                        if (Conn.CanBeTowTruck == 1)
                        {
                            if (Conn.Plate == "Tow")
                            {
                                if (Conn.CurrentCar == "FBM")
                                {
                                    MsgPly("^6>>^7 You cannot get duty whilst using FBM!", MSO.UCID);
                                }
                                
                                else if (Conn.IsTowTruck == false)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2ON-DUTY ^7as Tow Truck!");

                                    #region ' Close BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                                        {
                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(117, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            DeleteBTN(121, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;
                                        }
                                        if (Conn.InShop == true || Conn.InStore == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                            DeleteBTN(121, Conn.UniqueID);
                                        }
                                    }
                                    #endregion

                                    if (Conn.CalledRequest == true)
                                    {
                                        Conn.CalledRequest = false;
                                    }

                                    Conn.LastName = Conn.NoColPlyName;
                                    Conn.IsTowTruck = true;
                                }
                                else
                                {
                                    MsgPly("^6>>^7 You are now duty as a TowTruck.", MSO.UCID);
                                    MsgPly("  ^7If you want to get ^1OFF-DUTY ^7please remove the Tag!", MSO.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Not Authorized Tow Truck!", MSO.UCID);
                        }
                    }
                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Finish or Cancel your current Job!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #region ' Cops Command '

        [Command("settrap", "settrap <kmh>")]
        public void settrap(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsOfficer == true && Conn.CanBeOfficer == 1)
            {
                if (StrMsg.Length == 2)
                {
                    if (Conn.InGame == 0)
                    {
                        MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                    else if (Conn.InChaseProgress == false)
                    {
                        try
                        {
                            int TrapSpeed = Convert.ToInt32(StrMsg[1]);

                            if (TrapSpeed.ToString().Contains("-"))
                            {
                                MsgPly("^6>>^7 Invalid Input. Don't put minus values!", MSO.UCID);
                            }
                            else
                            {
                                if (Conn.TrapSetted == false)
                                {
                                    if (Conn.CompCar.Speed / 91 < 3)
                                    {
                                        var BTT = MSO;
                                        if (TrapSpeed > 50 && TrapSpeed < 230)
                                        {
                                            Conn.TrapX = Conn.CompCar.X / 196608;
                                            Conn.TrapY = Conn.CompCar.Y / 196608;
                                            Conn.TrapSpeed = TrapSpeed;
                                            foreach (clsConnection i in Connections)
                                            {
                                                if (i.IsOfficer == true && i.CanBeOfficer == 1 || i.IsCadet == true && i.CanBeCadet == 1)
                                                {
                                                    MsgPly("^6>>^7 " + Conn.NoColPlyName + " set a Trap", i.UniqueID);
                                                    MsgPly("^6>>^7 Located: " + Conn.Location, i.UniqueID);
                                                    MsgPly("^6>>^7 Trap Setted at X: ^3" + Conn.TrapX + " ^7Y: ^3" + Conn.TrapY, i.UniqueID);
                                                    MsgPly("^6>>^7 Speed: ^3" + Conn.TrapSpeed + " kmh", i.UniqueID);
                                                }
                                            }
                                            Conn.TrapSetted = true;
                                        }
                                        else
                                        {
                                            if (TrapSpeed < 50)
                                            {
                                                MsgPly("^6>>^7 Speed Traps can't be setted lower 50kmh!", BTT.UCID);
                                            }
                                            else if (TrapSpeed > 230)
                                            {
                                                MsgPly("^6>>^7 Speed Traps can't be setted more than 230kmh!", BTT.UCID);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Can't Set a Trap whilst being driving!", MSO.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 The Trap has been setted in this Area!", MSO.UCID);
                                }
                            }
                        }
                        catch
                        {
                            MsgPly("^6>>^7 Trap Error. Please check your values!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 Can't set a Trap whilst in chase progress!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Not Authorized Officer!", MSO.UCID);
            }

        }

        [Command("remtrap", "remtrap")]
        public void remtrap(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsOfficer == true && Conn.CanBeOfficer == 1)
            {
                if (StrMsg.Length == 1)
                {
                    if (Conn.TrapSetted == true)
                    {
                        MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                        Conn.TrapY = 0;
                        Conn.TrapX = 0;
                        Conn.TrapSpeed = 0;
                        Conn.TrapSetted = false;
                    }
                    else
                    {
                        MsgPly("^6>>^7 No Trap has been Setted!", Conn.UniqueID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Not Authorized Officer!", MSO.UCID);
            }
        }

        [Command("chase", "chase")]
        public void chase(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                #region ' Engage! '
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            #region ' Engage '
                            if (Conn.InChaseProgress == false)
                            {
                                try
                                {
                                    if (Conn.UniqueID == MSO.UCID)
                                    {
                                        #region ' Object Variables '
                                        int LowestDistance = 250;
                                        byte ChaseeIndex = 0;
                                        int Distance = 0;
                                        int ChaseeUCID = -1;
                                        #endregion

                                        #region ' Chase Setup '
                                        for (int i = 0; i < Connections.Count; i++)
                                        {
                                            if (Connections[i].PlayerID != 0)
                                            {
                                                Distance = ((int)Math.Sqrt(Math.Pow(Connections[i].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[i].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                                Connections[i].DistanceFromCop = Distance;
                                            }
                                        }
                                        for (int i = 0; i < Connections.Count; i++)
                                        {
                                            if (Connections[i].PlayerID != 0)
                                            {
                                                if ((Connections[i].DistanceFromCop < LowestDistance) && (Connections[i].DistanceFromCop > 0) && (Connections[i].PlayerName != Conn.PlayerName) && (Connections[i].IsOfficer == false) && (Connections[i].IsCadet == false))
                                                {
                                                    LowestDistance = Connections[i].DistanceFromCop;

                                                    ChaseeUCID = Connections[i].UniqueID;
                                                    ChaseeIndex = (byte)i;
                                                }
                                            }
                                        }
                                        #endregion

                                        #region ' Detect '

                                        if (Conn.PlayerName == HostName == false)
                                        {
                                            if ((LowestDistance < 150) && (Connections[GetConnIdx(ChaseeUCID)].DistanceFromCop > 0))
                                            {
                                                #region ' New Engage '
                                                if (Connections[ChaseeIndex].IsSuspect == false)
                                                {
                                                    if (ChaseLimit == AddChaseLimit && Conn.InChaseProgress == false)
                                                    {
                                                        MsgPly(" ^6>>^7^3 Maximum Pursuit Limit: ^7" + AddChaseLimit, MSO.UCID);
                                                    }
                                                    else
                                                    {
                                                        #region ' Start Chase '
                                                        if (Connections[ChaseeIndex].IsBeingBusted == false)
                                                        {
                                                            Conn.Chasee = ChaseeUCID;
                                                            Conn.InChaseProgress = true;
                                                            Conn.ChaseCondition = 1;
                                                            Conn.AutoBumpTimer = 50;
                                                            Connections[ChaseeIndex].CopInChase = 1;
                                                            Connections[ChaseeIndex].ChaseCondition = 1;
                                                            Connections[ChaseeIndex].PullOvrMsg = 30;
                                                            AddChaseLimit += 1;

                                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^1started a chase!");
                                                            MsgAll(" ^6>>^2 Suspect Name : ^7" + Connections[ChaseeIndex].NoColPlyName + " (" + Connections[ChaseeIndex].Username + ")");
                                                            MsgAll(" ^6>>^2 Chase Condition : ^7" + Connections[ChaseeIndex].ChaseCondition);
                                                            
                                                            Connections[ChaseeIndex].IsSuspect = true;
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 " + Connections[ChaseeIndex].NoColPlyName + " is being busted a cop.", MSO.UCID);
                                                        }
                                                        #endregion
                                                    }
                                                }
                                                #endregion

                                                #region ' Join Chase '
                                                else if (Connections[ChaseeIndex].IsSuspect == true && Connections[ChaseeIndex].ChaseCondition >= 2)
                                                {
                                                    if (Conn.InChaseProgress == false)
                                                    {
                                                        Connections[ChaseeIndex].CopInChase += 1;
                                                        Conn.ChaseCondition = Connections[ChaseeIndex].ChaseCondition;
                                                        Conn.Chasee = ChaseeUCID;
                                                        Conn.JoinedChase = true;
                                                        Conn.InChaseProgress = true;

                                                        #region ' Connection List '
                                                        foreach (clsConnection Con in Connections)
                                                        {
                                                            if (Con.Chasee == Connections[ChaseeIndex].UniqueID)
                                                            {
                                                                Conn.BumpButton = Con.BumpButton;
                                                            }
                                                        }
                                                        #endregion

                                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^1joins in backup chase!");
                                                        MsgAll(" ^6>>^2 Suspect Name: " + Connections[ChaseeIndex].NoColPlyName + " (" + Connections[ChaseeIndex].Username + ")");
                                                        MsgAll(" ^6>>^2 Cops In Chase: " + Connections[ChaseeIndex].CopInChase);
                                                        MsgAll(" ^6>>^2 Chase Condition: ^7" + Connections[ChaseeIndex].ChaseCondition);
                                                        Connections[ChaseeIndex].IsSuspect = true;
                                                    }
                                                }
                                                else
                                                {
                                                    MsgPly("^6>>^7 Cannot join in the Police Pursuit.", MSO.UCID);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                MsgPly("^6>>^7 No Civilian found in 150 meters", MSO.UCID);
                                            }
                                        }

                                        #endregion
                                    }
                                }
                                catch
                                {
                                    MsgPly("^6>>^7 Engage Error.", MSO.UCID);
                                }
                            }
                            #endregion

                            #region ' Bump '
                            else if (Conn.InChaseProgress == true)
                            {
                                if (Conn.Busted == false)
                                {
                                    if (Conn.AutoBumpTimer == 0)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            MsgPly("^6>>^7 You can't extend the Condition without the Leader increase!", Conn.UniqueID);
                                        }
                                        else if (Conn.ChaseCondition > 0 && Conn.ChaseCondition < 5 && Conn.JoinedChase == false)
                                        {

                                            #region ' Cops In chase! '
                                            if (ChaseCon.CopInChase > 1)
                                            {
                                                foreach (clsConnection Con in Connections)
                                                {
                                                    if (Con.Chasee == ChaseCon.UniqueID)
                                                    {
                                                        MsgAll("^6>>^7 " + Con.NoColPlyName + " ^3still chasing ^7" + ChaseCon.NoColPlyName + "!");
                                                        Con.BumpButton += 1;
                                                    }
                                                }
                                                MsgAll(" ^6>>^7^3 Cops In Chase: ^7" + ChaseCon.CopInChase);
                                            }
                                            else if (ChaseCon.CopInChase == 1)
                                            {
                                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^3still chasing ^7" + ChaseCon.NoColPlyName + "!");
                                                Conn.BumpButton += 1;
                                            }
                                            #endregion

                                            #region ' Chase Condition '
                                            switch (Conn.BumpButton)
                                            {
                                                case 1:

                                                    MsgAll(" ^6>>^2 Suspect Name : ^7" + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                                    MsgAll(" ^6>>2 Chase Condition : ^72");
                                                    InSim.Send_MTC_MessageToConnection("^6>>^7 YOU HAVE REACHED LEVEL 2 OF CHASING!", ChaseCon.UniqueID, 0);


                                                    Conn.ChaseCondition = 2;
                                                    ChaseCon.ChaseCondition = 2;
                                                    Conn.AutoBumpTimer = 70;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 2;
                                                            }
                                                        }
                                                    }
                                                    #endregion


                                                    break;

                                                case 2:

                                                    MsgAll(" ^6>>^2 Suspect Name : ^7" + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                                    MsgAll(" ^6>>^2 Chase Condition : ^73");
                                                    InSim.Send_MTC_MessageToConnection("^6>>^7 YOU HAVE REACHED LEVEL 3 OF CHASING!", ChaseCon.UniqueID, 0);

                                                    Conn.ChaseCondition = 3;
                                                    ChaseCon.ChaseCondition = 3;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 3;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    Conn.AutoBumpTimer = 80;
                                                    break;

                                                case 3:

                                                    MsgAll(" ^6>>^2 Suspect Name : ^7" + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                                    MsgAll(" ^6>>^2 Chase Condition : ^74");

                                                    InSim.Send_MTC_MessageToConnection("^6>>^7 YOU HAVE REACHED LEVEL 4 OF CHASING!", ChaseCon.UniqueID, 0);

                                                    Conn.ChaseCondition = 4;
                                                    ChaseCon.ChaseCondition = 4;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 4;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    Conn.AutoBumpTimer = 90;
                                                    break;


                                                case 4:

                                                    MsgAll(" ^6>>^2 Suspect Name : ^7" + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                                    MsgAll(" ^6>>^2 Chase Condition : ^75");


                                                    InSim.Send_MTC_MessageToConnection("^6>>^7 YOU HAVE REACHED THE FINAL LEVEL OF CHASING!", ChaseCon.UniqueID, 0);
                                                    Conn.ChaseCondition = 5;
                                                    ChaseCon.ChaseCondition = 5;

                                                    #region ' Connection List '
                                                    foreach (clsConnection C in Connections)
                                                    {
                                                        if (C.Chasee == ChaseCon.UniqueID)
                                                        {
                                                            if (C.JoinedChase == true)
                                                            {
                                                                C.ChaseCondition = 5;
                                                            }
                                                        }
                                                    }
                                                    #endregion

                                                    break;
                                            }
                                            #endregion
                                        }
                                        else if (Conn.ChaseCondition == 5)
                                        {
                                            MsgPly("^6>>^7 Chase Condition is already reached the Final", Conn.UniqueID);
                                        }
                                    }
                                    else
                                    {
                                        #region ' String Timer '
                                        string Minutes = "0";
                                        string Seconds = "0";
                                        Minutes = "" + (Conn.AutoBumpTimer / 60);
                                        if ((Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60)) < 10)
                                        {
                                            Seconds = "0" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                        }
                                        else
                                        {
                                            Seconds = "" + (Conn.AutoBumpTimer - ((Conn.AutoBumpTimer / 60) * 60));
                                        }
                                        #endregion

                                        MsgPly("^6>>^7 Wait for the Bump timer for ^2" + Minutes + ":" + Seconds, Conn.UniqueID);
                                        MsgPly("^7  To Increase the Condition!", Conn.UniqueID);
                                    }
                                }
                                else if (Conn.Busted == true)
                                {
                                    MsgPly("^6>>^7 type ^2!busted ^7to busted the suspect", MSO.UCID);
                                    MsgPly("^7  or ^2!disengage ^7to stop the chase!", MSO.UCID);
                                }
                            }
                            #endregion
                        }
                        else if (Conn.InGame == 0)
                        {
                            MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be duty before you can start a Chase!", MSO.UCID);
                    }
                }
                #endregion

                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^6>>^7 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("disengage", "disengage")]
        public void disengage(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InFineMenu == false)
                        {
                            if (Conn.InGame == 1)
                            {
                                if (Conn.ChaseCondition != 0)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^1ends chase on ^7" + ChaseCon.NoColPlyName + "!");

                                    #region ' Disengage Joined in chase '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (Conn.JoinedChase == true)
                                        {
                                            Conn.JoinedChase = false;
                                        }
                                        Conn.ChaseCondition = 0;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        Conn.BustedTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Disengage '
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        AddChaseLimit -= 1;
                                        Conn.AutoBumpTimer = 0;
                                        Conn.BumpButton = 0;
                                        Conn.BustedTimer = 0;
                                        Conn.Chasee = -1;
                                        Conn.Busted = false;
                                        Conn.InChaseProgress = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 You aren't in chase!", MSO.UCID);
                                }
                            }
                            else if (Conn.InGame == 0)
                            {
                                MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                            }
                        }
                        else if (Conn.InFineMenu == true)
                        {
                            MsgPly("^6>>^7 Set a Tickets to " + ChaseCon.NoColPlyName + "!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be duty before you can access this command!", MSO.UCID);
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^6>>^7 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("busted", "busted")]
        public void busted(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            if (Conn.InChaseProgress == true)
                            {
                                #region ' Enabled Busted '
                                if (Conn.Busted == true)
                                {
                                    if (Conn.BustedTimer == 5)
                                    {
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7busts the suspect!");
                                        MsgAll("^6>>^2 Suspect Name: " + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                        if (ChaseCon.CopInChase > 1)
                                        {
                                            MsgAll("^6>>^7 Cops In Chase: " + ChaseCon.CopInChase);
                                        }
                                        MsgAll("^6>>^7 Suspect Chase Condition: " + Conn.ChaseCondition);
                                        MsgPly("^6>>^7 Don't move away whilst being busted!", ChaseCon.UniqueID);
                                        MsgPly("^6>>^7 Please wait to receive your Ticket!", ChaseCon.UniqueID);

                                        #region ' List of Connection Joined Chase '
                                        foreach (clsConnection Con in Connections)
                                        {
                                            if (Con.Chasee == ChaseCon.UniqueID)
                                            {
                                                if (ChaseCon.CopInChase > 1)
                                                {
                                                    if (Con.JoinedChase == true && Con.Busted == false)
                                                    {
                                                        MsgPly("^6>>^7 " + Conn.NoColPlyName + " busts the suspect.", Con.UniqueID);
                                                        MsgPly("^6>>^7 Please wait to get your rewards!", Con.UniqueID);

                                                    }
                                                    else if (Con.JoinedChase == false && Con.Busted == false)
                                                    {
                                                        MsgPly("^6>>^7 " + Conn.NoColPlyName + " busts the suspect.", Con.UniqueID);
                                                        MsgPly("^6>>^7 Please wait to get your rewards!", Con.UniqueID);

                                                    }
                                                    Con.Busted = true;
                                                    Con.BustedTimer = 0;
                                                    Con.AutoBumpTimer = 0;
                                                    Con.BumpButton = 0;
                                                    Con.InChaseProgress = false;
                                                }
                                                else if (ChaseCon.CopInChase == 1)
                                                {
                                                    Con.Busted = true;
                                                    Con.InFineMenu = true;
                                                    Con.AutoBumpTimer = 0;
                                                    Con.BumpButton = 0;
                                                    Con.BustedTimer = 0;
                                                    Con.InChaseProgress = false;
                                                }
                                            }
                                        }
                                        #endregion

                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Suspect Name: " + ChaseCon.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                        #region ' Condition '
                                        if (Conn.ChaseCondition == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 54, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 2)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$1,300", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 3)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$2,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 4)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$3,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        if (Conn.ChaseCondition == 5)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$5,000", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (Conn.UniqueID), 2, false);
                                        }
                                        #endregion

                                        // Click Buttons
                                        InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Enter a Fine Amount And Reason For The Chase", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Conn.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Warn", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Conn.UniqueID), 40, false);
                                        InSim.Send_BTN_CreateButton("^7Issue", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Conn.UniqueID), 40, false);

                                        Conn.InFineMenu = true;
                                        AddChaseLimit -= 1;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.IsBeingBusted = true;
                                        ChaseCon.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                }
                                #endregion

                                else if (Conn.Busted == false)
                                {
                                    if (ChaseCon.IsBeingBusted == true)
                                    {
                                        MsgPly("^6>>^7 " + ChaseCon.NoColPlyName + " being fined at the momment!", MSO.UCID);
                                        MsgPly("  ^7Please wait for awhile when the fine is accepted or refused", MSO.UCID);
                                    }
                                    else if (ChaseCon.IsBeingBusted == false)
                                    {
                                        MsgPly("^6>>^7 You must pull over " + ChaseCon.NoColPlyName + " to busted!", MSO.UCID);
                                    }
                                }
                            }
                            else if (Conn.InChaseProgress == false)
                            {
                                MsgPly("^6>>^7 You aren't in chase!", MSO.UCID);
                            }
                        }

                        else if (Conn.InGame == 0)
                        {
                            MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be duty before you can access this command!", MSO.UCID);
                    }
                }
                #region ' Not Authorized or Failed '
                else
                {
                    MsgPly("^6>>^7 Not Authorized.", MSO.UCID);
                }
                #endregion
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("backup", "backup")]
        public void backup(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Chasee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.CanBeOfficer == 1 || Conn.CanBeCadet == 1)
                {
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        if (Conn.InGame == 1)
                        {
                            if (Conn.ChaseCondition == 1)
                            {
                                MsgPly("^6>>^7 You cannot call a backup whilst in Conditon 1!", MSO.UCID);
                            }
                            else if (Conn.ChaseCondition > 1)
                            {
                                bool Found = false;

                                foreach (clsConnection i in Connections)
                                {
                                    if (i.IsCadet == true && i.InChaseProgress == false || i.IsOfficer == true && i.InChaseProgress == false)
                                    {
                                        Found = true;
                                    }
                                }

                                if (Found == false)
                                {
                                    MsgPly("^6>>^7 There are no Officers/Cadet can be found!", MSO.UCID);
                                }
                                if (Found == true)
                                {
                                    MsgAll(" ^6>>^7^2 Backup Request: ^7" + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll(" ^6>>^7^2 Suspect Name: ^7" + ChaseCon.NoColPlyName + " (" + ChaseCon.Username + ")");
                                    MsgAll(" ^6>>^7^2 Suspect Condition: ^7" + Conn.ChaseCondition);
                                    MsgAll(" ^6>>^7^2 Suspect Location: ^7" + ChaseCon.Location);
                                }
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be duty before you can access the cmd!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("cc", "cc")]
        public void copchat(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.CanBeOfficer == 1 && Conn.IsOfficer == true || Conn.CanBeCadet == 1 && Conn.IsCadet == true)
            {
                if (StrMsg.Length > 1)
                {
                    string MsgCC = Msg.Remove(0, 4);

                    foreach (clsConnection u in Connections)
                    {
                        if (u.IsOfficer == true && u.CanBeOfficer == 1 || u.IsCadet == true && u.CanBeCadet == 1)
                        {
                            MsgPly("^6>>^7 Cop Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")", u.UniqueID);
                            MsgPly("^6>>^7 Msg: " + MsgCC, u.UniqueID);
                        }
                    }

                    AdmBox("> Cop Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                    AdmBox("> Msg: " + MsgCC);
                }
                else
                {
                    if (StrMsg.Length == 1)
                    {
                        MsgPly("^6>>^7 Using cop chat ^2!cc <message>", MSO.UCID);
                    }
                    else
                    {
                        MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }
        
        #endregion

        #region ' Tow Truck Command '

        [Command("accepttow", "acceptow <usrname>")]
        public void acceptrequest(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    bool Found = false;
                    byte Accepted = 0;
                    string Username = Msg.Remove(0, 11);
                    
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Username == Username)
                        {
                            if (i.IsBeingTowed == false)
                            {
                                if (i.CalledRequest == true)
                                {
                                    Found = true;
                                    Accepted = 1;
                                    MsgPly("^6>>^7 " + Conn.NoColPlyName + " accepted your request!", i.UniqueID);
                                    i.CallAccepted = true;
                                    i.CalledRequest = false;
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 The User is being Towed.", MSO.UCID);
                            }
                        }
                    }

                    if (Accepted == 0)
                    {
                        MsgPly("^6>>^7 No Call Request from this user or Being Accepted", MSO.UCID);
                    }
                    else if (Accepted == 1)
                    {
                        MsgPly("^6>>^7 Tow Request is now Accepted.", MSO.UCID);
                    }
                    
                    if (Found == false)
                    {
                        MsgPly("^6>>^7 " + Username + " wasn't found or offline", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("starttow", "starttow")]
        public void starttow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length == 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    if (Conn.InGame == 1)
                    {
                        if (Conn.InTowProgress == false)
                        {
                            try
                            {
                                int LowestDistance = 150;
                                int Distance = 0;
                                byte TowUCID = 0;
                                byte TowIndex = 0;
                                #region ' Start Tow '
                                if (Conn.UniqueID == MSO.UCID)
                                {
                                    #region ' Instance Tow '
                                    for (int i = 0; i < Connections.Count; i++)
                                    {
                                        if (Connections[i].PlayerID != 0)
                                        {
                                            Distance = ((int)Math.Sqrt(Math.Pow(Connections[i].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[i].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                            Connections[i].DistanceFromTow = Distance;
                                        }
                                    }
                                    for (int i = 0; i < Connections.Count; i++)
                                    {
                                        if (Connections[i].PlayerID != 0)
                                        {
                                            if ((Connections[i].DistanceFromTow < LowestDistance) && (Connections[i].DistanceFromTow > 0) && (Connections[i].PlayerName != Conn.PlayerName) && (Connections[i].IsTowTruck == false))
                                            {
                                                LowestDistance = Connections[i].DistanceFromTow;

                                                TowUCID = Connections[i].UniqueID;
                                                TowIndex = (byte)i;
                                            }
                                        }
                                    }
                                    #endregion

                                    if ((LowestDistance < 150) && (Connections[GetConnIdx(TowUCID)].DistanceFromTow > 0))
                                    {
                                        if (Connections[TowIndex].CallAccepted == true || Connections[TowIndex].CalledRequest == true)
                                        {
                                            Conn.Towee = TowUCID;
                                            Connections[TowIndex].IsBeingTowed = true;
                                            Conn.InTowProgress = true;

                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " started a In Tow Progress!");
                                            MsgAll("^6>>^7 Tow Request: " + Connections[TowIndex].NoColPlyName);
                                            Connections[TowIndex].CallAccepted = false;
                                            Connections[TowIndex].CalledRequest = false;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 No Call Requested/Accepted on this user!", MSO.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 No Tow Request found in 50 meters!", MSO.UCID);
                                    }
                                }
                                #endregion
                            }
                            catch
                            {
                                MsgPly("^6>>^7 Engage Error.", MSO.UCID);
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 You are already in Tow in Progress!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("stoptow", "stoptow")]
        public void stoptow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(MSO.UCID)].Towee)];
            if (StrMsg.Length == 1)
            {
                if (Conn.IsTowTruck == true && Conn.CanBeTowTruck == 1)
                {
                    if (Conn.InGame == 1)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + TowCon.NoColPlyName + "!");
                            TowCon.IsBeingTowed = false;
                            Conn.Towee = -1;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                        }
                        else
                        {
                            MsgPly("^6>>^7 Your not in Tow in Progress!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 You must be in vehicle before you access this command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("tc", "tc")]
        public void towchat(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.CanBeTowTruck == 1 && Conn.IsTowTruck == true)
            {
                if (StrMsg.Length > 1)
                {
                    string MsgTC = Msg.Remove(0, 4);

                    foreach (clsConnection u in Connections)
                    {
                        if (u.IsTowTruck == true && u.CanBeTowTruck == 1)
                        {
                            MsgPly("^6>>^7 Tow Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")", u.UniqueID);
                            MsgPly("^6>>^7 Msg: " + MsgTC, u.UniqueID);
                        }
                    }

                    AdmBox("> Tow Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                    AdmBox("> Msg: " + MsgTC);
                }
                else
                {
                    if (StrMsg.Length == 1)
                    {
                        MsgPly("^6>>^7 Using tow chat ^2!tc <message>", MSO.UCID);
                    }
                    else
                    {
                        MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion

        #endregion

        #region ' Admin/Moderator Command '

        [Command("park", "park1 <username> <message>")]
        public void park11(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            clsConnection Conn = Connections[GetConnIdx(MSO.UCID)];

            if (StrMsg.Length > 2)
            {
                clsConnection PlayerConn = null;
                foreach (clsConnection C in Connections) // Update Player Info[]
                {

                    if (C.UniqueID == MSO.UCID)
                    {
                        C.PlayerID = MSO.PLID;
                        PlayerConn = C;
                    }
                }

                if (Conn.IsAdmin == 1 || Conn.IsSuperAdmin == 1 || Conn.IsModerator == 1)
                {

                    bool PMUserFound2 = false;
                    foreach (clsConnection C in Connections)
                    {

                        string Message20 = Msg.Remove(0, C.Username.Length + 6);


                        if (C.Username == StrMsg[1] && StrMsg[1].Length > 1)
                        {
                            PMUserFound2 = true;
                            MsgAll("^2/////////////////////////////////////////////");
                            MsgAll("^1" + C.NoColPlyName + " was force pitlaned by " + Conn.NoColPlyName);
                            MsgAll(C.NoColPlyName + " ^1paid 500 for " + Message20);
                            MsgAll("^2/////////////////////////////////////////////");
                            C.Cash -= 500;

                            PitlaneID(C.Username);

                        }
                    }
                    if (PMUserFound2 == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^3 Player is not online.", MSO.UCID, 0);
                    }
                }



            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^6>>^7 Invalid Command. ^2!help ^7for help", MSO.UCID, 0);
            }

        }

        [Command("addofficer", "addofficer <username>")]
        public void addofficer(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 12);
                    #region ' Online '
                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (l.CanBeOfficer == 0)
                            {
                                MsgAll("^6>>^7 " + l.NoColPlyName + " can be now an Officer!");
                                MsgPly("^6>>^7 To get in duty use the " + OfficerTag + " ^7to get duty!", l.UniqueID);
                                AdmBox("> " + Conn.NoColPlyName + " added " + l.NoColPlyName + " in Police Officer Force!");
                                l.CanBeOfficer = 1;
                            }
                            else if (l.CanBeOfficer == 1)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is already an Officer!", MSO.UCID);
                            }

                            // Remove Cadetory
                            if (l.CanBeCadet == 0 || l.CanBeCadet == 1)
                            {
                                l.CanBeCadet = 2;
                            }
                        }
                    }
                    #endregion

                    #region ' Offline '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool SaveUser = false;
                            if (CanBeOfficer == 0)
                            {
                                SaveUser = true;
                                CanBeOfficer = 1;
                                MsgAll("^6>>^7 " + NoColPlyName + " can be now an Officer!");
                                AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " in Police Officer Force!");
                            }
                            else if (CanBeOfficer == 1)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already an Officer!", MSO.UCID);
                            }

                            // Remove Cadetory
                            if (CanBeCadet == 1 || CanBeCadet == 0)
                            {
                                SaveUser = true;
                                CanBeCadet = 2;
                            }

                            #region ' Save User '
                            if (SaveUser == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.PlayerName + " (" + Conn.Username + ") tried to access the Add Officer Command!");
                    MsgAll("^6>>^7 " + Conn.PlayerName + " tried to use Add Officer Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("removecop", "removecop <username>")]
        public void removecop(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);
                    
                    #region ' Online '
                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Chasee)];
                            if (l.IsOfficer == true)
                            {
                                #region ' InChase Progress '
                                if (l.InChaseProgress == true)
                                {
                                    #region ' In Chase Progress '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (l.JoinedChase == true)
                                        {
                                            l.JoinedChase = false;
                                        }
                                        l.ChaseCondition = 0;
                                        l.Busted = false;
                                        l.BustedTimer = 0;
                                        l.BumpButton = 0;
                                        l.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgAll("^6>>^7 " + l.NoColPlyName + " sighting lost " + ChaseCon.NoColPlyName + "!");
                                        MsgAll("^7 Total Cops In Chase: " + ChaseCon.CopInChase);

                                    }
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        MsgAll("^6>>^7 " + l.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                                        MsgAll("^7Suspect Runs away from being chased!");
                                        AddChaseLimit -= 1;
                                        l.AutoBumpTimer = 0;
                                        l.BumpButton = 0;
                                        l.BustedTimer = 0;
                                        l.Chasee = -1;
                                        l.Busted = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.ChaseCondition = 0;
                                        l.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    l.InChaseProgress = false;

                                    MsgAll("^6>>^7 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Officer!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                else
                                {
                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    MsgAll("^6>>^7 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Officer!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                #endregion

                                #region ' Busted Remove '
                                if (l.InFineMenu == true)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " released " + ChaseCon.NoColPlyName + "!");

                                    #region ' Chasee Connection '
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.UniqueID == ChaseCon.UniqueID)
                                        {
                                            if (i.IsBeingBusted == true)
                                            {
                                                if (i.AcceptTicket == 1)
                                                {
                                                    #region ' Close Region LOL '
                                                    DeleteBTN(30, i.UniqueID);
                                                    DeleteBTN(31, i.UniqueID);
                                                    DeleteBTN(32, i.UniqueID);
                                                    DeleteBTN(33, i.UniqueID);
                                                    DeleteBTN(34, i.UniqueID);
                                                    DeleteBTN(35, i.UniqueID);
                                                    DeleteBTN(36, i.UniqueID);
                                                    DeleteBTN(37, i.UniqueID);
                                                    DeleteBTN(38, i.UniqueID);
                                                    DeleteBTN(39, i.UniqueID);
                                                    DeleteBTN(40, i.UniqueID);
                                                    #endregion
                                                    i.AcceptTicket = 0;
                                                }
                                                i.ChaseCondition = 0;
                                                i.AcceptTicket = 0;
                                                i.TicketRefuse = 0;
                                                i.CopInChase = 0;
                                                i.IsBeingBusted = false;
                                            }
                                        }

                                        if (i.Chasee == ChaseCon.UniqueID)
                                        {
                                            if (i.JoinedChase == true)
                                            {
                                                i.JoinedChase = false;
                                            }
                                            i.TicketAmount = 0;
                                            i.TicketAmountSet = false;
                                            i.TicketReason = "";
                                            i.TicketReasonSet = false;
                                            i.Busted = false;
                                            i.Chasee = -1;
                                            i.ChaseCondition = 0;
                                        }
                                    }
                                    #endregion

                                    #region ' Close Region LOL '
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    #endregion

                                    if (l.InFineMenu == true)
                                    {
                                        l.InFineMenu = false;
                                    }

                                    l.Busted = false;
                                }
                                #endregion

                                l.IsOfficer = false;

                                if (l.CanBeOfficer == 1)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Officer!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " in Police Officer Force!");
                                    l.CanBeOfficer = 0;
                                }
                            }
                            else if (l.IsOfficer == false)
                            {
                                if (l.CanBeOfficer == 1)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Officer!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " in Police Officer Force!");
                                    l.CanBeOfficer = 0;
                                }
                            }

                            if (l.CanBeOfficer == 0)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is not an Officer!", MSO.UCID);
                            }
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion
                            bool Complete = true;
                            // Your Command here!
                            if (CanBeOfficer == 1)
                            {
                                Complete = true;
                                CanBeOfficer = 0;
                                MsgAll("^6>>^7 " + NoColPlyName + " is now removed as Officer!");
                                AdmBox("> " + Conn.NoColPlyName + " removed " + NoColPlyName + " in Police Officer Force!");
                            }
                            else if (CanBeOfficer == 0)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is not an Officer!", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }
                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Remove Cop Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Remove Cop Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addcadet", "addcadet <username>")]
        public void addcadet(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 10);
                    
                    #region ' Online Adding '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;

                            if (l.CanBeOfficer == 1)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName+ " is already a Officer", MSO.UCID);
                            }
                            else if (l.CanBeOfficer == 0)
                            {
                                if (l.CanBeCadet == 0 || l.CanBeCadet == 2 || l.CanBeCadet == 3)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now added as Cadet!");
                                    AdmBox("> " + Conn.NoColPlyName + " added " + l.NoColPlyName + " in Police Cadet Force!");
                                    l.CanBeCadet = 1;
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Adding '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            
                            if (CanBeOfficer == 0)
                            {
                                if (CanBeCadet == 0 || CanBeCadet == 2 || CanBeCadet == 3)
                                {
                                    Complete = true;
                                    MsgAll("^6>>^7 " + NoColPlyName + " can be now a Cadet!");
                                    AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " in Police Cadet Force!");
                                    CanBeCadet = 1;
                                }
                            }
                            else if (CanBeOfficer == 1)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already a Officer!", MSO.UCID);
                            }

                            if (CanBeCadet == 1)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already a Cadet!", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Add Cadet Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Add Cadet Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remcadet", "remcadet <username>")]
        public void removecadet(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 10);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Chasee)];
                            if (l.IsCadet == true)
                            {
                                #region ' InChase Progress '
                                if (l.InChaseProgress == true)
                                {
                                    #region ' In Chase Progress '
                                    if (ChaseCon.CopInChase > 1)
                                    {
                                        if (l.JoinedChase == true)
                                        {
                                            l.JoinedChase = false;
                                        }
                                        l.ChaseCondition = 0;
                                        l.Busted = false;
                                        l.BustedTimer = 0;
                                        l.BumpButton = 0;
                                        l.Chasee = -1;
                                        ChaseCon.CopInChase -= 1;

                                        #region ' Connection List '
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            foreach (clsConnection Con in Connections)
                                            {
                                                if (Con.Chasee == ChaseCon.UniqueID)
                                                {
                                                    if (Con.JoinedChase == true)
                                                    {
                                                        Con.JoinedChase = false;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgAll("^6>>^7 " + l.NoColPlyName + " sighting lost " + ChaseCon.NoColPlyName + "!");
                                        MsgAll("^7 Total Cops In Chase: " + ChaseCon.CopInChase);

                                    }
                                    else if (ChaseCon.CopInChase == 1)
                                    {
                                        MsgAll("^6>>^7 " + l.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                                        MsgAll("^7Suspect Runs away from being chased!");
                                        AddChaseLimit -= 1;
                                        l.AutoBumpTimer = 0;
                                        l.BumpButton = 0;
                                        l.BustedTimer = 0;
                                        l.Chasee = -1;
                                        l.Busted = false;
                                        ChaseCon.PullOvrMsg = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        ChaseCon.ChaseCondition = 0;
                                        l.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    l.InChaseProgress = false;

                                    MsgAll("^6>>^7 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Cadet!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                else
                                {
                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, l.UniqueID);
                                    DeleteBTN(16, l.UniqueID);
                                    DeleteBTN(17, l.UniqueID);
                                    DeleteBTN(18, l.UniqueID);
                                    DeleteBTN(19, l.UniqueID);
                                    DeleteBTN(20, l.UniqueID);
                                    DeleteBTN(21, l.UniqueID);
                                    DeleteBTN(22, l.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (l.InGameIntrfc == 0 && l.DisplaysOpen == true)
                                    {
                                        if (l.InShop == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                        if (l.InStore == true)
                                        {
                                            if (l.CurrentCar == "UF1" || l.CurrentCar == "XFG" || l.CurrentCar == "XRG" || l.CurrentCar == "LX4" || l.CurrentCar == "LX6" || l.CurrentCar == "RB4" || l.CurrentCar == "FXO" || l.CurrentCar == "XRT" || l.CurrentCar == "VWS" || l.CurrentCar == "RAC" || l.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, l.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, l.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    MsgAll("^6>>^7 " + l.LastName + " was forced to ^1OFF-DUTY ^7as a Cadet!");
                                    TotalOfficers -= 1;
                                    l.LastName = "";
                                }
                                #endregion

                                #region ' Busted Remove '
                                if (l.InFineMenu == true)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " released " + ChaseCon.NoColPlyName + "!");

                                    #region ' Chasee Connection '
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.UniqueID == ChaseCon.UniqueID)
                                        {
                                            if (i.IsBeingBusted == true)
                                            {
                                                if (i.AcceptTicket == 1)
                                                {
                                                    #region ' Close Region LOL '
                                                    DeleteBTN(30, i.UniqueID);
                                                    DeleteBTN(31, i.UniqueID);
                                                    DeleteBTN(32, i.UniqueID);
                                                    DeleteBTN(33, i.UniqueID);
                                                    DeleteBTN(34, i.UniqueID);
                                                    DeleteBTN(35, i.UniqueID);
                                                    DeleteBTN(36, i.UniqueID);
                                                    DeleteBTN(37, i.UniqueID);
                                                    DeleteBTN(38, i.UniqueID);
                                                    DeleteBTN(39, i.UniqueID);
                                                    DeleteBTN(40, i.UniqueID);
                                                    #endregion
                                                    i.AcceptTicket = 0;
                                                }
                                                i.ChaseCondition = 0;
                                                i.AcceptTicket = 0;
                                                i.TicketRefuse = 0;
                                                i.CopInChase = 0;
                                                i.IsBeingBusted = false;
                                            }
                                        }

                                        if (i.Chasee == ChaseCon.UniqueID)
                                        {
                                            if (i.JoinedChase == true)
                                            {
                                                i.JoinedChase = false;
                                            }
                                            i.TicketAmount = 0;
                                            i.TicketAmountSet = false;
                                            i.TicketReason = "";
                                            i.TicketReasonSet = false;
                                            i.Busted = false;
                                            i.Chasee = -1;
                                            i.ChaseCondition = 0;
                                        }
                                    }
                                    #endregion

                                    #region ' Close Region LOL '
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    #endregion

                                    if (l.InFineMenu == true)
                                    {
                                        l.InFineMenu = false;
                                    }

                                    l.Busted = false;
                                }
                                #endregion

                                l.IsCadet = false;

                                if (l.CanBeCadet == 1)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Cadet!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " in Police Cadet Force!");
                                    l.CanBeCadet = 3;
                                }
                            }
                            else if (l.IsOfficer == false)
                            {
                                if (l.CanBeCadet == 1)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Officer!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " in Police Officer Force!");
                                    l.CanBeCadet = 3;
                                }
                            }

                            if (l.CanBeCadet == 0 || l.CanBeCadet == 2 || l.CanBeCadet == 3)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is not an Cadet!", MSO.UCID);
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeCadet == 1)
                            {
                                Complete = true;
                                MsgAll("^6>>^7 " + NoColPlyName + " is now removed as Cadet!");
                                AdmBox("> " + Conn.NoColPlyName + " removed " + NoColPlyName + " in Police Cadet Force!");
                                CanBeCadet = 3;
                            }
                            else if (CanBeCadet == 0 || CanBeCadet == 2 || CanBeCadet == 3)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is not an Cadet!", MSO.UCID);
                            }



                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Remove Cadet Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Remove Cadet Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addtow", "addtow <username>")]
        public void addtow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 8);

                    #region ' Online Adding '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (l.CanBeTowTruck == 0)
                            {
                                l.CanBeTowTruck = 1;
                                MsgAll("^6>>^7 " + l.NoColPlyName + " can be now a Tow Truck!");
                                AdmBox("> " + Conn.NoColPlyName + " added " + l.NoColPlyName + " as Tow Truck!");
                            }
                            else
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is already a Tow Truck", MSO.UCID);
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Adding '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeTowTruck == 0)
                            {
                                Complete = true;
                                CanBeTowTruck = 1;
                                MsgAll("^6>>^7 " + NoColPlyName + " can be now a TowTruck!");
                                AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " as Tow Truck!");
                            }
                            else
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already a Tow Truck", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Add TowTruck Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Add TowTruck Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remtow", "remtow <username>")]
        public void remtow(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 8);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            var TowCon = Connections[GetConnIdx(Connections[GetConnIdx(l.UniqueID)].Towee)];
                            if (l.IsTowTruck == true)
                            {
                                if (l.InTowProgress == true)
                                {
                                    if (TowCon.IsBeingTowed == true)
                                    {
                                        TowCon.IsBeingTowed = false;
                                    }
                                    l.Towee = -1;
                                    l.InTowProgress = false;
                                    CautionSirenShutOff();
                                }

                                l.IsTowTruck = false;
                                MsgAll("^6>>^7 " + l.NoColPlyName + " was forced ^1OFF-DUTY ^7as TowTruck!");
                                if (l.CanBeTowTruck == 1)
                                {
                                    l.CanBeTowTruck = 0;
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Tow Truck!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " as Tow Truck!");
                                }
                            }
                            else
                            {
                                if (l.CanBeTowTruck == 1)
                                {
                                    l.CanBeTowTruck = 0;
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as Tow Truck!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + l.NoColPlyName + " as Tow Truck!");
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + l.NoColPlyName + " is not a Tow Truck", MSO.UCID);
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (CanBeTowTruck == 1)
                            {
                                Complete = true;
                                CanBeTowTruck = 0;
                                MsgAll("^6>>^7 " + NoColPlyName + " is now removed as Tow Truck!");
                                AdmBox("> " + Conn.NoColPlyName + " removed " + NoColPlyName + " as Tow Truck!");
                            }
                            else if (CanBeTowTruck == 0)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is not a Tow Truck", MSO.UCID);
                            }

                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Remove TowTruck Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Remove TowTruck Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("addmember", "addmember <username>")]
        public void addmember(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (l.IsModerator == 0)
                                {
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " is now VIP!");
                                    AdmBox("> " + Conn.NoColPlyName + " added " + l.NoColPlyName + " as Moderator!");
                                    l.IsModerator = 1;
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + l.NoColPlyName + " is already a VIP!", MSO.UCID);
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (IsModerator == 0)
                                {
                                    Complete = true;
                                    IsModerator = 1;
                                    MsgAll("^6>>^7 " + NoColPlyName + " is now added as VIP!");
                                    AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " as VIP!");

                                    AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " as VIP!");
                                }
                                else if (IsModerator == 1)
                                {
                                    MsgPly("^6>>^7 " + NoColPlyName + " is already a VIP!", MSO.UCID);
                                }
                            }
                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the add VIP Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Add VIP Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remmember", "removemember <username>")]
        public void remmember(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 11);

                    #region ' Online Removing '

                    foreach (clsConnection l in Connections)
                    {
                        if (l.Username == Username)
                        {
                            Found = true;
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^6>>^7 " + l.NoColPlyName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (l.InModerationMenu > 0)
                                {
                                    #region ' Close Region LOL '
                                    DeleteBTN(30, l.UniqueID);
                                    DeleteBTN(31, l.UniqueID);
                                    DeleteBTN(32, l.UniqueID);
                                    DeleteBTN(33, l.UniqueID);
                                    DeleteBTN(34, l.UniqueID);
                                    DeleteBTN(35, l.UniqueID);
                                    DeleteBTN(36, l.UniqueID);
                                    DeleteBTN(37, l.UniqueID);
                                    DeleteBTN(38, l.UniqueID);
                                    DeleteBTN(39, l.UniqueID);
                                    DeleteBTN(40, l.UniqueID);
                                    DeleteBTN(41, l.UniqueID);
                                    DeleteBTN(42, l.UniqueID);
                                    DeleteBTN(43, l.UniqueID);
                                    #endregion

                                    l.ModerationWarn = 0;
                                    l.ModUsername = "";
                                    l.ModReason = "";
                                    l.ModReasonSet = false;
                                    l.InModerationMenu = 0;


                                    if (l.IsModerator == 1)
                                    {
                                        MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as VIP!");
                                        l.IsModerator = 0;
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 " + l.NoColPlyName + " is not a VIP!", MSO.UCID);
                                    }
                                }
                                else
                                {
                                    if (l.IsModerator == 1)
                                    {
                                        MsgAll("^6>>^7 " + l.NoColPlyName + " is now removed as VIP!");
                                        l.IsModerator = 0;
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 " + l.NoColPlyName + " is not a VIP!", MSO.UCID);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region ' Offline Removing '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            bool Complete = false;
                            // Your Command here!
                            if (FileInfo.GetUserAdmin(Username) == 1)
                            {
                                MsgPly("^6>>^7 " + NoColPlyName + " is already a Admin!", MSO.UCID);
                            }
                            else
                            {
                                if (IsModerator == 1)
                                {
                                    Complete = true;
                                    IsModerator = 0;
                                    MsgAll("^6>>^7 " + NoColPlyName + " is now removed as VIP!");
                                    AdmBox("> " + Conn.NoColPlyName + " removed " + NoColPlyName + " as VIP!");
                                }
                                else if (IsModerator == 0)
                                {
                                    MsgPly("^6>>^7 " + NoColPlyName + " is not a VIP!", MSO.UCID);
                                }
                            }
                            #region ' Save User '
                            if (Complete == true)
                            {
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);
                            }
                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Remove VIP Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Remove VIP Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("refund", "refund <amount> <username>")]
        public void refund(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    try
                    {
                        bool Found = false;
                        string Username = Msg.Remove(0, 9 + StrMsg[1].Length);
                        int Refund = int.Parse(StrMsg[1]);

                        if (Refund.ToString().Contains("-"))
                        {
                            MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online Refunding '

                            foreach (clsConnection l in Connections)
                            {
                                if (l.Username == Username)
                                {
                                    Found = true;
                                    // All Players
                                    MsgAll("^6>>^7 "+ l.NoColPlyName + " (" + l.Username + ") was refunded");
                                    MsgAll("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Amount: ^2$" + Refund);
                                    
                                    // To Admin Box
                                    AdmBox("> " + l.NoColPlyName + " (" + l.Username + ") was refunded");
                                    AdmBox("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("^7Refund Amount: ^2$" + Refund);
                                    l.Cash += Refund;
                                }
                            }

                            #endregion

                            #region ' Offline Refund '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (NoColPlyName.Contains("^0"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^0", "");
                                    }
                                    if (NoColPlyName.Contains("^1"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^1", "");
                                    }
                                    if (NoColPlyName.Contains("^2"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^2", "");
                                    }
                                    if (NoColPlyName.Contains("^3"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^3", "");
                                    }
                                    if (NoColPlyName.Contains("^4"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^4", "");
                                    }
                                    if (NoColPlyName.Contains("^5"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^5", "");
                                    }
                                    if (NoColPlyName.Contains("^6"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^6", "");
                                    }
                                    if (NoColPlyName.Contains("^7"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^7", "");
                                    }
                                    if (NoColPlyName.Contains("^8"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^6>>^7 " + NoColPlyName + " (" + Username + ") was refunded");
                                    MsgAll("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Amount: ^2$" + Refund);

                                    // To Admin Box
                                    AdmBox("> " + NoColPlyName + " (" + Username + ") was refunded");
                                    AdmBox("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("^7Refund Amount: ^2$" + Refund);
                                    Cash += Refund;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Refund Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Refund Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("fine", "fine <amount> <username>")]
        public void fine(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "crazyboy232" | Conn.IsModerator == 1)
                {
                    try
                    {
                        bool Found = false;
                        string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                        int Fine = int.Parse(StrMsg[1]);

                        if (Fine.ToString().Contains("-"))
                        {
                            MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online Force Fine '

                            foreach (clsConnection l in Connections)
                            {
                                if (l.Username == Username)
                                {
                                    Found = true;
                                    // All Players
                                    MsgAll("^6>>^7 " + l.NoColPlyName + " (" + l.Username + ") was Forced Fine");
                                    MsgAll("^7Fined by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fine Amount: ^1$" + Fine);

                                    // To Admin Box
                                    AdmBox("> " + l.NoColPlyName + " (" + l.Username + ") was Forced Fined");
                                    AdmBox("^7Forced Fine by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("^7Fine Amount: ^1$" + Fine);
                                    l.Cash -= Fine;
                                }
                            }

                            #endregion

                            #region ' Offline Force Fine '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (NoColPlyName.Contains("^0"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^0", "");
                                    }
                                    if (NoColPlyName.Contains("^1"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^1", "");
                                    }
                                    if (NoColPlyName.Contains("^2"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^2", "");
                                    }
                                    if (NoColPlyName.Contains("^3"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^3", "");
                                    }
                                    if (NoColPlyName.Contains("^4"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^4", "");
                                    }
                                    if (NoColPlyName.Contains("^5"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^5", "");
                                    }
                                    if (NoColPlyName.Contains("^6"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^6", "");
                                    }
                                    if (NoColPlyName.Contains("^7"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^7", "");
                                    }
                                    if (NoColPlyName.Contains("^8"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^6>>^7 " + NoColPlyName + " (" + Username + ") was Forced Fine");
                                    MsgAll("^7Forced Fine by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Force Fine Amount: ^1$" + Fine);

                                    // To Admin Box
                                    AdmBox("> " + NoColPlyName + " (" + Username + ") was Forced Fine");
                                    AdmBox("^7Forced Fine by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("^7Force Fine Amount: ^1$" + Fine);
                                    Cash -= Fine;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Fine Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Fine Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("mod", "mod <username>")]
        public void mod(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.IsModerator == 1 || Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    string Username = Msg.Remove(0, 5);
                    bool Found = false;

                    if (Conn.InFineMenu == false && Conn.AcceptTicket == 0)
                    {
                        if (Conn.InModerationMenu == 0)
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 30, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 31, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^7Moderation Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Moderation Name: " + i.PlayerName + " ^7(" + i.Username + ")", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Cash: ^2$" + i.Cash + " ^7Bank Balance: ^2$" + i.BankBalance + " ^7Distance: ^2" + i.TotalDistance / 1000 + " km ^7/ ^2" + i.TotalDistance / 1609 + " mi", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                    if (i.Cars.Length > 52 && i.Cars.Length < 84)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + i.Cars.Remove(39, i.Cars.Length - 39), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4> ^7" + i.Cars.Remove(0, 40), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 75, 54, 36, Conn.UniqueID, 2, false);
                                        
                                    }
                                    else if (i.Cars.Length < 52)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + i.Cars, Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, MSO.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, MSO.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 118, 43, MSO.UCID, 40, false);
                                        
                                    Conn.ModUsername = Username;
                                    Conn.InModerationMenu = 1;
                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion


                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 30, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 77, 50, 50, 31, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^7Moderation Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Moderation Name: " + PlayerName + " ^7(" + Username + ")", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 60, 54, 33, (Conn.UniqueID), 2, false);
                                    InSim.Send_BTN_CreateButton("^4> ^7Cash: ^2$" + Cash + " ^7Bank Balance: ^2$" + BBal + " ^7Distance: ^2" + TotalDistance / 1000 + " km ^7/ ^2" + TotalDistance / 1609 + " mi", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 5, 70, 65, 54, 34, (Conn.UniqueID), 2, false);

                                    if (Cars.Length > 52 && Cars.Length < 84)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + Cars.Remove(39, Cars.Length - 39), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4> ^7" + Cars.Remove(0, 40), Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 75, 54, 36, Conn.UniqueID, 2, false);

                                    }
                                    else if (Cars.Length < 52)
                                    {
                                        InSim.Send_BTN_CreateButton("^4> ^7Cars: " + Cars, Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 70, 70, 54, 35, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 118, 43, MSO.UCID, 40, false);
                                        

                                    Conn.ModUsername = Username;
                                    Conn.InModerationMenu = 2;
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            MsgPly("^6>>^7 Close your current Moderation first!", MSO.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 Complete the Ticket Menu Panel First!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Moderation Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Moderation Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("banlist", "banlist <username>")]
        public void banlist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 1)
            {
                if (Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Remove(0, 9);
                    #region ' Online '

                    foreach (clsConnection i in Connections)
                    {
                        if (i.Username == Username)
                        {
                            Found = true;
                            MsgAll("^6>>^7 " + i.NoColPlyName + " was banned by " + Conn.NoColPlyName);
                            MsgPly("^6>>^7 You are banlisted by: " + Conn.NoColPlyName, i.UniqueID);
                            AdmBox("> " + Conn.NoColPlyName + " added " + i.NoColPlyName + " (" + i.Username + ") to the user banlist!");
                            FileInfo.AddBanList(Username);
                            BanID(i.Username, 0);
                        }
                    }

                    #endregion

                    #region ' Offline '

                    if (Found == false)
                    {
                        if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                        {
                            #region ' Objects '
                            long Cash = FileInfo.GetUserCash(Username);
                            long BBal = FileInfo.GetUserBank(Username);
                            string Cars = FileInfo.GetUserCars(Username);
                            long Gold = FileInfo.GetUserGold(Username);

                            long TotalDistance = FileInfo.GetUserDistance(Username);
                            byte TotalHealth = FileInfo.GetUserHealth(Username);
                            int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                            byte Electronics = FileInfo.GetUserElectronics(Username);
                            byte Furniture = FileInfo.GetUserFurniture(Username);

                            int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                            int LastLotto = FileInfo.GetUserLastLotto(Username);

                            byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                            byte CanBeCadet = FileInfo.CanBeCadet(Username);
                            byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                            byte IsModerator = FileInfo.IsMember(Username);

                            byte Interface1 = FileInfo.GetInterface(Username);
                            byte Interface2 = FileInfo.GetInterface2(Username);
                            byte Speedo = FileInfo.GetSpeedo(Username);
                            byte Odometer = FileInfo.GetOdometer(Username);
                            byte Counter = FileInfo.GetCounter(Username);
                            byte Panel = FileInfo.GetCopPanel(Username);

                            byte Renting = FileInfo.GetUserRenting(Username);
                            byte Rented = FileInfo.GetUserRented(Username);
                            string Renter = FileInfo.GetUserRenter(Username);
                            string Renterr = FileInfo.GetUserRenterr(Username);
                            string Rentee = FileInfo.GetUserRentee(Username);

                            string PlayerName = FileInfo.GetUserPlayerName(Username);
                            #endregion

                            #region ' Special PlayerName Colors Remove '

                            string NoColPlyName = PlayerName;
                            if (NoColPlyName.Contains("^0"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^0", "");
                            }
                            if (NoColPlyName.Contains("^1"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^1", "");
                            }
                            if (NoColPlyName.Contains("^2"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^2", "");
                            }
                            if (NoColPlyName.Contains("^3"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^3", "");
                            }
                            if (NoColPlyName.Contains("^4"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^4", "");
                            }
                            if (NoColPlyName.Contains("^5"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^5", "");
                            }
                            if (NoColPlyName.Contains("^6"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^6", "");
                            }
                            if (NoColPlyName.Contains("^7"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^7", "");
                            }
                            if (NoColPlyName.Contains("^8"))
                            {
                                NoColPlyName = NoColPlyName.Replace("^8", "");
                            }
                            #endregion

                            MsgAll("^6>>^7 " + NoColPlyName + " was banned by " + Conn.NoColPlyName);
                            AdmBox("> " + Conn.NoColPlyName + " added " + NoColPlyName + " (" + Username + ") to the user banlist!");
                            FileInfo.AddBanList(Username);
                            BanID(Username, 0);
                        }
                        else
                        {
                            MsgPly("^6>>^7 " + Username + " wasn't found in database!", MSO.UCID);
                        }
                    }

                    #endregion
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Banlist Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Banlist Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("refdist", "refdist <distance> <username>")]
        public void refdist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                    bool Found = false;
                    int Amount = int.Parse(StrMsg[1]);
                    
                    try
                    {
                        if (Amount.ToString().Contains("-"))
                        {
                            MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    MsgAll("^6>>^7 " + i.NoColPlyName + " (" + i.Username + ") was refunded in Distance");
                                    MsgAll("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Distance: ^2" + Amount + " km");
                                    i.TotalDistance += Amount * 1000;

                                    AdmBox("> " + i.NoColPlyName + " (" + Username + ") was refunded");
                                    AdmBox("> Refund by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("> Refund Distance: ^1" + Amount + " km");
                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (NoColPlyName.Contains("^0"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^0", "");
                                    }
                                    if (NoColPlyName.Contains("^1"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^1", "");
                                    }
                                    if (NoColPlyName.Contains("^2"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^2", "");
                                    }
                                    if (NoColPlyName.Contains("^3"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^3", "");
                                    }
                                    if (NoColPlyName.Contains("^4"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^4", "");
                                    }
                                    if (NoColPlyName.Contains("^5"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^5", "");
                                    }
                                    if (NoColPlyName.Contains("^6"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^6", "");
                                    }
                                    if (NoColPlyName.Contains("^7"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^7", "");
                                    }
                                    if (NoColPlyName.Contains("^8"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^6>>^7 " + NoColPlyName + " (" + Username + ") was refunded in Distance");
                                    MsgAll("^7Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Refund Distance: ^2" + Amount + " km");

                                    // To Admin Box
                                    AdmBox("> " + NoColPlyName + " (" + Username + ") was refunded");
                                    AdmBox("> Refunded by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("> Refund Distance: ^2" + Amount + " km");
                                    TotalDistance += Amount * 1000;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the Refund distance Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use Refund distance Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("objectenergy", "objectenergy <distance> <username>")]
        public void objectfinishenergy(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "")
                {
                    string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                    bool Found = false;
                    int Amount = int.Parse(StrMsg[1]);

                    try
                    {
                        if (Amount.ToString().Contains("-"))
                        {
                            MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online '
                            
                            foreach (clsConnection i in Connections)
                            {
                                if (i.PlayerName == i.PlayerName)
                                {
                                    Found = true;
                                    Conn.TotalHealth -= 1;
                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (NoColPlyName.Contains("^0"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^0", "");
                                    }
                                    if (NoColPlyName.Contains("^1"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^1", "");
                                    }
                                    if (NoColPlyName.Contains("^2"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^2", "");
                                    }
                                    if (NoColPlyName.Contains("^3"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^3", "");
                                    }
                                    if (NoColPlyName.Contains("^4"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^4", "");
                                    }
                                    if (NoColPlyName.Contains("^5"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^5", "");
                                    }
                                    if (NoColPlyName.Contains("^6"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^6", "");
                                    }
                                    if (NoColPlyName.Contains("^7"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^7", "");
                                    }
                                    if (NoColPlyName.Contains("^8"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    Conn.TotalHealth -= 1;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the fine distance Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use fine distance Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 0;
        }

        [Command("finedist", "finedist <distance> <username>")]
        public void finedist(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    string Username = Msg.Remove(0, StrMsg[0].Length + StrMsg[1].Length + 2);
                    bool Found = false;
                    int Amount = int.Parse(StrMsg[1]);

                    try
                    {
                        if (Amount.ToString().Contains("-"))
                        {
                            MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", MSO.UCID);
                        }
                        else
                        {
                            #region ' Online '

                            foreach (clsConnection i in Connections)
                            {
                                if (i.Username == Username)
                                {
                                    Found = true;
                                    MsgAll("^6>>^7 " + i.NoColPlyName + " (" + i.Username + ") was fined in Distance");
                                    MsgAll("^7Fined by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fined Distance: ^1" + Amount + " km");
                                    i.TotalDistance -= Amount * 1000;

                                    AdmBox("> " + i.NoColPlyName + " (" + i.Username + ") was fined");
                                    AdmBox("> Fine by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("> Fine Distance: ^1" + Amount + " km");
                                }
                            }

                            #endregion

                            #region ' Offline '

                            if (Found == false)
                            {
                                if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Username);
                                    long BBal = FileInfo.GetUserBank(Username);
                                    string Cars = FileInfo.GetUserCars(Username);
                                    long Gold = FileInfo.GetUserGold(Username);

                                    long TotalDistance = FileInfo.GetUserDistance(Username);
                                    byte TotalHealth = FileInfo.GetUserHealth(Username);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                    byte Electronics = FileInfo.GetUserElectronics(Username);
                                    byte Furniture = FileInfo.GetUserFurniture(Username);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                    int LastLotto = FileInfo.GetUserLastLotto(Username);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                    byte IsModerator = FileInfo.IsMember(Username);

                                    byte Interface1 = FileInfo.GetInterface(Username);
                                    byte Interface2 = FileInfo.GetInterface2(Username);
                                    byte Speedo = FileInfo.GetSpeedo(Username);
                                    byte Odometer = FileInfo.GetOdometer(Username);
                                    byte Counter = FileInfo.GetCounter(Username);
                                    byte Panel = FileInfo.GetCopPanel(Username);

                                    byte Renting = FileInfo.GetUserRenting(Username);
                                    byte Rented = FileInfo.GetUserRented(Username);
                                    string Renter = FileInfo.GetUserRenter(Username);
                                    string Renterr = FileInfo.GetUserRenterr(Username);
                                    string Rentee = FileInfo.GetUserRentee(Username);

                                    string PlayerName = FileInfo.GetUserPlayerName(Username);
                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerName;
                                    if (NoColPlyName.Contains("^0"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^0", "");
                                    }
                                    if (NoColPlyName.Contains("^1"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^1", "");
                                    }
                                    if (NoColPlyName.Contains("^2"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^2", "");
                                    }
                                    if (NoColPlyName.Contains("^3"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^3", "");
                                    }
                                    if (NoColPlyName.Contains("^4"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^4", "");
                                    }
                                    if (NoColPlyName.Contains("^5"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^5", "");
                                    }
                                    if (NoColPlyName.Contains("^6"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^6", "");
                                    }
                                    if (NoColPlyName.Contains("^7"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^7", "");
                                    }
                                    if (NoColPlyName.Contains("^8"))
                                    {
                                        NoColPlyName = NoColPlyName.Replace("^8", "");
                                    }
                                    #endregion

                                    //bool Complete = false;
                                    // Your Command here!

                                    // All Players
                                    MsgAll("^6>>^7 " + NoColPlyName + " (" + Username + ") was fined in Distance");
                                    MsgAll("^7Fined by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    MsgAll("^7Fined Distance: ^1" + Amount + " km");

                                    // To Admin Box
                                    AdmBox("> " + NoColPlyName + " (" + Username + ") was fined");
                                    AdmBox("> Fine by: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                    AdmBox("> Fine Distance: ^1" + Amount + " km");
                                    TotalDistance -= Amount * 1000;

                                    #region ' Save User '
                                    FileInfo.SaveOfflineUser(Username,
                                        PlayerName,
                                        Cash,
                                        BBal,
                                        Cars,
                                        TotalHealth,
                                        TotalDistance,
                                        Gold,
                                        TotalJobsDone,
                                        Electronics,
                                        Furniture,
                                        IsModerator,
                                        CanBeOfficer,
                                        CanBeCadet,
                                        CanBeTowTruck,
                                        LastRaffle,
                                        LastLotto,
                                        Interface1,
                                        Interface2,
                                        Speedo,
                                        Odometer,
                                        Counter,
                                        Panel,
                                        Renting,
                                        Rented,
                                        Renter,
                                        Rentee,
                                        Renterr);

                                    #endregion
                                }
                                else
                                {
                                    MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                                }
                            }

                            #endregion
                        }
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has Occured. Re-consider the Amount!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the fine distance Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use fine distance Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }
        
        [Command("refcar", "refcar <car> <username>")]
        public void refcar(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1 || Conn.Username == "crazyboy232")
                {
                    bool Found = false;
                    string Username = Msg.Substring(StrMsg[0].Length + StrMsg[1].Length + 2);
                    string RefundCar = StrMsg[1].ToUpper();
                    try
                    {
                        #region ' Online '
                        foreach (clsConnection i in Connections)
                        {
                            if (i.Username == Username)
                            {
                                Found = true;
                                #region ' Exist '
                                if (i.Cars.Contains(RefundCar))
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula BMW FB02 (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                }
                                #endregion

                                #region ' Coudn't be added '

                                else if (Dealer.GetCarPrice(RefundCar) == 0)
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF1000 (UF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTi (XFG) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula V8 (FO8) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula XR (FOX) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 BMW Sauber F1.06 (BF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 " + RefundCar + " ^7does not exist on Dealership", MSO.UCID, 0);
                                    }
                                }

                                #endregion

                                #region ' Add '
                                else
                                {
                                    switch (RefundCar)
                                    {
                                         case "UF1":
                                            i.Cars += " " + "UF1";
                                            break;
                                        case "XFG":
                                            i.Cars += " " + "XFG";
                                            break;
                                        case "XRG":
                                            i.Cars += " " + "XRG";
                                            break;
                                        case "LX4":
                                            i.Cars += " " + "LX4";
                                            break;
                                        case "LX6":
                                            i.Cars += " " + "LX6";
                                            break;
                                        case "RB4":
                                            i.Cars += " " + "RB4";
                                            break;
                                        case "FXO":
                                            i.Cars += " " + "FXO";
                                            break;
                                        case "XRT":
                                            i.Cars += " " + "XRT";
                                            break;
                                        case "RAC":
                                            i.Cars += " " + "RAC";
                                            break;
                                        case "FZ5":
                                            i.Cars += " " + "FZ5";
                                            break;
                                        case "UFR":
                                            i.Cars += " " + "UFR";
                                            break;
                                        case "XFR":
                                            i.Cars += " " + "XFR";
                                            break;
                                        case "FXR":
                                            i.Cars += " " + "FXR";
                                            break;
                                        case "XRR":
                                            i.Cars += " " + "XRR";
                                            break;
                                        case "FZR":
                                            i.Cars += " " + "FZR";
                                            break;

                                        case "MRT":
                                            i.Cars += " " + "MRT";
                                            break;

                                        case "FBM":
                                            i.Cars += " " + "FBM";
                                            break;
                                    }

                                    MsgAll("^6>>^7 " + i.NoColPlyName + " received a " + RefundCar + " from " + Conn.NoColPlyName);

                                    AdmBox("> " + Conn.NoColPlyName + " refunded " + i.NoColPlyName + " a " + RefundCar);
                                    
                                }
                                #endregion
                            }
                        }
                        #endregion

                        #region ' Offline '

                        if (Found == false)
                        {
                            if (System.IO.File.Exists(Database + "\\" + Username + ".txt") == true)
                            {
                                #region ' Objects '
                                long Cash = FileInfo.GetUserCash(Username);
                                long BBal = FileInfo.GetUserBank(Username);
                                string Cars = FileInfo.GetUserCars(Username);
                                long Gold = FileInfo.GetUserGold(Username);

                                long TotalDistance = FileInfo.GetUserDistance(Username);
                                byte TotalHealth = FileInfo.GetUserHealth(Username);
                                int TotalJobsDone = FileInfo.GetUserJobsDone(Username);

                                byte Electronics = FileInfo.GetUserElectronics(Username);
                                byte Furniture = FileInfo.GetUserFurniture(Username);

                                int LastRaffle = FileInfo.GetUserLastRaffle(Username);
                                int LastLotto = FileInfo.GetUserLastLotto(Username);

                                byte CanBeOfficer = FileInfo.CanBeOfficer(Username);
                                byte CanBeCadet = FileInfo.CanBeCadet(Username);
                                byte CanBeTowTruck = FileInfo.CanBeTowTruck(Username);
                                byte IsModerator = FileInfo.IsMember(Username);

                                byte Interface1 = FileInfo.GetInterface(Username);
                                byte Interface2 = FileInfo.GetInterface2(Username);
                                byte Speedo = FileInfo.GetSpeedo(Username);
                                byte Odometer = FileInfo.GetOdometer(Username);
                                byte Counter = FileInfo.GetCounter(Username);
                                byte Panel = FileInfo.GetCopPanel(Username);

                                byte Renting = FileInfo.GetUserRenting(Username);
                                byte Rented = FileInfo.GetUserRented(Username);
                                string Renter = FileInfo.GetUserRenter(Username);
                                string Renterr = FileInfo.GetUserRenterr(Username);
                                string Rentee = FileInfo.GetUserRentee(Username);

                                string PlayerName = FileInfo.GetUserPlayerName(Username);
                                #endregion

                                #region ' Special PlayerName Colors Remove '

                                string NoColPlyName = PlayerName;
                                if (NoColPlyName.Contains("^0"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^0", "");
                                }
                                if (NoColPlyName.Contains("^1"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^1", "");
                                }
                                if (NoColPlyName.Contains("^2"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^2", "");
                                }
                                if (NoColPlyName.Contains("^3"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^3", "");
                                }
                                if (NoColPlyName.Contains("^4"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^4", "");
                                }
                                if (NoColPlyName.Contains("^5"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^5", "");
                                }
                                if (NoColPlyName.Contains("^6"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^6", "");
                                }
                                if (NoColPlyName.Contains("^7"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^7", "");
                                }
                                if (NoColPlyName.Contains("^8"))
                                {
                                    NoColPlyName = NoColPlyName.Replace("^8", "");
                                }
                                #endregion

                                #region ' Exist '
                                if (Cars.Contains(RefundCar))
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF1000 (UF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTi (XFG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GTi (XRG) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 LX4 (LX4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 LX6 (LX6) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 RB GT Turbo (RB4) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Volkswagen Scirroco (VWS) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GT Turbo (XRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Raceabout (RAC) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FZ50 (FZ5) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF GTR (UFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTR (XFR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FX GTR (FXR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XR GTR (XRR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 FZ GTR (FZR) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 McGill Racing Kart (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula XR (FOX) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula BMW FB02 (MRT) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula V8 (FO8) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 BMW Sauber 1.06 (BF1) ^7is already exist on the Garage", MSO.UCID, 0);
                                    }
                                }
                                #endregion

                                #region ' Coudn't be added '

                                else if (Dealer.GetCarPrice(RefundCar) == 0)
                                {
                                    if (RefundCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 UF1000 (UF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 XF GTi (XFG) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula V8 (FO8) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 Formula XR (FOX) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else if (RefundCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 BMW Sauber F1.06 (BF1) ^7is not available in Dealership", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection(" ^6>>^7 " + RefundCar + " ^7does not exist on Dealership", MSO.UCID, 0);
                                    }
                                }

                                #endregion

                                #region ' Add '
                                else
                                {
                                    switch (RefundCar)
                                    {
                                        case "UF1":
                                            Cars += " " + "UF1";
                                            break;
                                        case "XFG":
                                            Cars += " " + "XFG";
                                            break;
                                        case "XRG":
                                            Cars += " " + "XRG";
                                            break;
                                        case "LX4":
                                            Cars += " " + "LX4";
                                            break;
                                        case "LX6":
                                            Cars += " " + "LX6";
                                            break;
                                        case "RB4":
                                            Cars += " " + "RB4";
                                            break;
                                        case "FXO":
                                            Cars += " " + "FXO";
                                            break;
                                        case "XRT":
                                            Cars += " " + "XRT";
                                            break;
                                        case "RAC":
                                            Cars += " " + "RAC";
                                            break;
                                        case "FZ5":
                                            Cars += " " + "FZ5";
                                            break;
                                        case "UFR":
                                            Cars += " " + "UFR";
                                            break;
                                        case "XFR":
                                            Cars += " " + "XFR";
                                            break;
                                        case "FXR":
                                            Cars += " " + "FXR";
                                            break;
                                        case "XRR":
                                            Cars += " " + "XRR";
                                            break;
                                        case "FZR":
                                            Cars += " " + "FZR";
                                            break;

                                        case "MRT":
                                            Cars += " " + "MRT";
                                            break;

                                        case "FBM":
                                            Cars += " " + "FBM";
                                            break;
                                    }

                                    MsgAll("^6>>^7 " + NoColPlyName + " received a " + RefundCar + " from " + Conn.NoColPlyName);
                                    AdmBox("> " + Conn.NoColPlyName + " refunded " + NoColPlyName + " a " + RefundCar);

                                }
                                #endregion

                                #region ' Save User '
                                FileInfo.SaveOfflineUser(Username,
                                    PlayerName,
                                    Cash,
                                    BBal,
                                    Cars,
                                    TotalHealth,
                                    TotalDistance,
                                    Gold,
                                    TotalJobsDone,
                                    Electronics,
                                    Furniture,
                                    IsModerator,
                                    CanBeOfficer,
                                    CanBeCadet,
                                    CanBeTowTruck,
                                    LastRaffle,
                                    LastLotto,
                                    Interface1,
                                    Interface2,
                                    Speedo,
                                    Odometer,
                                    Counter,
                                    Panel,
                                    Renting,
                                    Rented,
                                    Renter,
                                    Rentee,
                                    Renterr);

                                #endregion
                            }
                            else
                            {
                                MsgPly("^6>>^7 " + Username + " wasn't found on database", MSO.UCID);
                            }
                        }

                        #endregion
                    }
                    catch
                    {
                        MsgPly("^6>>^7 An Error has occured. Please retype the Command!", MSO.UCID);
                    }
                }
                else
                {
                    MsgPly("^6>>^7 Not Authorized User!", MSO.UCID);
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") tried to access the refund vehicle Command!");
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " tried to use refund vehicle Command!");
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        [Command("remcar", "remcar <car> <username>")]
        public void remcar(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (StrMsg.Length > 2)
            {
                if (Conn.IsAdmin == 1)
                {
                    // bool Found = false;
                    string Username = Msg.Substring(StrMsg[0].Length + StrMsg[1].Length + 2);
                    string RemoveCar = StrMsg[1].ToUpper();
                    try
                    {
                        #region ' Online '

                        foreach (clsConnection i in Connections)
                        {
                            if (i.Username == Username)
                            {
                                //Found = true;
                                #region ' Check Owned '
                                if (i.Cars.Contains(RemoveCar))
                                {
                                    #region ' Check can be Removed '
                                    if (Dealer.GetCarPrice(RemoveCar) > 0)
                                    {
                                        string UserCars = i.Cars;
                                        int IdxCar = UserCars.IndexOf(RemoveCar);
                                        try
                                        {
                                            i.Cars = i.Cars.Remove(IdxCar, 4);
                                        }
                                        catch
                                        {
                                            i.Cars = i.Cars.Remove(IdxCar, 3);
                                        }
                                        if (Conn.PlayerName == "host")
                                        {
                                            MsgPly("^7Car Removed by Trading or New Player", MSO.UCID);
                                        }
                                        else
                                        {

                                            MsgAll("^7Force Car Remove:");
                                            MsgAll("^7" + i.PlayerName + "^1 was force removed ^3" + RemoveCar);
                                            MsgAll("^7Removed by: " + Conn.PlayerName);
                                        }
                                    }
                                    #endregion

                                    #region ' Check if couldn't removed '
                                    else
                                    {
                                        if (RemoveCar == "UF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> UF1000 (UF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "XFG")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> XF GTi (XFG) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FO8")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Formula V8 (FO8) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "FOX")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> Formula XR (FOX) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else if (RemoveCar == "BF1")
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> BMW Sauber F1.06 (BF1) ^7coudn't be removed", MSO.UCID, 0);
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^2> " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region ' Check Doesn't Own '
                                else
                                {
                                    if (RemoveCar == "UF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1UF1000 (UF1)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1XF GTi (XFG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRG")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1XR GTi (XRG)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1LX4 (LX4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "LX6")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1LX6 (LX6)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RB4")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1RB GT Turbo (RB4)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXO")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1FX GT Turbo (FXO)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "VWS")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1Volkswagen Scirroco (VWS)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1XR GT Turbo (XRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "RAC")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1Raceabout (RAC)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZ5")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1FZ50 GT (FZ5)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "UFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1UF GTR (UFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XFR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1XF GTR (XFR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FXR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1FX GTR (FXR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "XRR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1XR GTR (XRR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FZR")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1FZ GTR (FZR)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "MRT")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1McGill Racing Kart (MRT)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FBM")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1Formula BMW FB02 (FBM)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FO8")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1Formula V8 (FO8)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "FOX")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1Formula XR (FOX)", MSO.UCID, 0);
                                    }
                                    else if (RemoveCar == "BF1")
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^7 " + i.PlayerName + " doesn't own ^1BMW Sauber F1.06 (BF1)", MSO.UCID, 0);
                                    }
                                    else
                                    {
                                        InSim.Send_MTC_MessageToConnection("^2>^3 " + RemoveCar + " ^7Invalid Car Garage List", MSO.UCID, 0);
                                    }
                                }
                                #endregion
                            }
                        }

                        #endregion
                    }
                    catch
                    {
                        InSim.Send_MTC_MessageToConnection("^2>^7 An Error has occured. Please retype the Command!", MSO.UCID, 0);
                    }
                }
                else
                {
                    InSim.Send_MTC_MessageToConnection("^2>^7 Not Authorized User!", MSO.UCID, 0);
                    MsgAll("^2>^7 " + Conn.PlayerName + " tried to use remove vehicle Command!");
                }
            }
            else
            {
                InSim.Send_MTC_MessageToConnection("^2>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID, 0);
            }

        }

        [Command("ac", "ac")]
        public void adminchat(string Msg, string[] StrMsg, Packets.IS_MSO MSO)
        {
            var Conn = Connections[GetConnIdx(MSO.UCID)];
            if (Conn.IsAdmin == 1 || Conn.IsSuperAdmin == 1 || Conn.IsModerator == 1)
            {
                if (StrMsg.Length > 1)
                {
                    string MsgAC = Msg.Remove(0, 4);

                    foreach (clsConnection u in Connections)
                    {
                        if (Conn.IsAdmin == 1 || Conn.IsSuperAdmin == 1 || Conn.IsModerator == 1)
                        {
                            MsgPly("^6>>^7 Admin Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")", u.UniqueID);
                            MsgPly("^6>>^7 Msg: " + MsgAC, u.UniqueID);
                        }
                    }

                    AdmBox("> Admin Chat: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                    AdmBox("> Msg: " + MsgAC);
                }
                else
                {
                    if (StrMsg.Length == 1)
                    {
                        MsgPly("^6>>^7 Using admin chat ^2!ac <message>", MSO.UCID);
                    }
                    else
                    {
                        MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                    }
                }
            }
            else
            {
                MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
            }
            Conn.WaitCMD = 4;
        }

        #endregion
    }
}
