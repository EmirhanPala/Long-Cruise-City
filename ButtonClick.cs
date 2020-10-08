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
    public partial class Form1
    {
        // A player clicked a custom button
        private void BTC_ClientButtonClicked(Packets.IS_BTC BTC)
        {
            try
            {
                var Conn = Connections[GetConnIdx(BTC.UCID)];
                var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(BTC.UCID)].Chasee)];
                string Car = Connections[GetConnIdx(BTC.UCID)].CurrentCar;

                #region ' Admin panel '
                if (Conn.InAdminMenu == true)
                {
                #region ' close '
                if (BTC.ClickID == 113)
                {
                    DeleteBTN(110, Conn.UniqueID);
                    DeleteBTN(111, Conn.UniqueID);
                    DeleteBTN(112, Conn.UniqueID);
                    DeleteBTN(113, Conn.UniqueID);
                    DeleteBTN(113, Conn.UniqueID);
                    DeleteBTN(190, Conn.UniqueID);
                    DeleteBTN(191, Conn.UniqueID);
                    DeleteBTN(192, Conn.UniqueID);
                    DeleteBTN(193, Conn.UniqueID);
                    DeleteBTN(194, Conn.UniqueID);
                    DeleteBTN(195, Conn.UniqueID);
                    DeleteBTN(196, Conn.UniqueID);
                    DeleteBTN(197, Conn.UniqueID);
                    DeleteBTN(198, Conn.UniqueID);
                }
                #endregion

                #region ' MSG STOP SPAM '
                if (BTC.ClickID == 190)
                {
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                    MsgAll("^1STOP SPAM");
                }
                #endregion

                #region ' reset on '
                if (BTC.ClickID == 191)
                {
                    MsgAll("^4>>>^7Car reset is turned on!");
                    Message("/canreset yes");
                }
                #endregion

                #region ' reset off '
                if (BTC.ClickID == 192)
                {
                    MsgAll("^1>>>^7Car reset is turned off!");
                    Message("/canreset no");
                }
                #endregion

                #region ' pit_all '
                if (BTC.ClickID == 193)
                {
                    MsgAll("^2>>>^7All cars were pitlaned!");
                    Message("/pit_all");
                }
                #endregion

                #region ' restart '
                /*if (BTC.ClickID == 195)
                {
                    if (Conn.Username == "crazyboy232")
                    {
                        {
                            #region ' Save Users '
                            if (Connections.Count > 1)
                            {

                                //FileInfo.SaveUser(Conn);
                                MsgAll("^2Stats Saved!");
                            }
                            #endregion

                            MsgAll("^6>>^7 Insim Restarting!");
                            Application.Restart();
                        }
                    }
                }*/
                #endregion

                #region ' refundall cash '
                /*if (BTC.ClickID == 196)
                {
                    if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                    {

                        foreach (clsConnection C in Connections)
                        {
                            C.Cash += 5000;

                        }

                        InSim.Send_MST_Message("/msg ^1>>>^7 All users received ^65000$");
                        InSim.Send_MST_Message("/msg ^1>>>^7 Sponsor : " + Conn.Username);

                    }
                }*/
                #endregion

                #region ' refundall bonus '
                if (BTC.ClickID == 197)
                {
                    if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                    {

                        foreach (clsConnection C in Connections)
                        {
                            C.TotalBonusDone += 1;

                        }

                        InSim.Send_MST_Message("/msg ^1>>>^7 All users received ^61 Level Bonus!");
                        InSim.Send_MST_Message("/msg ^1>>>^7 Sponsor : " + Conn.Username);

                    }
                }
                #endregion

                #region ' refundall health '
                if (BTC.ClickID == 198)
                {
                    if (Conn.IsAdmin == 1 && Conn.IsSuperAdmin == 1)
                    {

                        foreach (clsConnection C in Connections)
                        {
                            C.TotalHealth += 50;

                        }

                        InSim.Send_MST_Message("/msg ^1>>>^7 All users received ^150% ^7energy!");
                        InSim.Send_MST_Message("/msg ^1>>>^7 Sponsor : " + Conn.Username);

                    }
                }
                #endregion
                }
                #endregion

                #region ' Close BUTTON! '

                if (BTC.ClickID == 113)
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

                    if (Conn.InAdminMenu == true)
                    {
                        Conn.InAdminMenu = false;
                    }

                    if (Conn.DisplaysOpen == true)
                    {
                        Conn.DisplaysOpen = false;
                    }
                }

                #endregion

                #region ' Close Welcome Screen! '

                if (BTC.ClickID == 239)
                {
                    DeleteBTN(232, BTC.UCID);
                    DeleteBTN(233, BTC.UCID);
                    DeleteBTN(234, BTC.UCID);
                    DeleteBTN(235, BTC.UCID);
                    DeleteBTN(236, BTC.UCID);
                    DeleteBTN(237, BTC.UCID);
                    DeleteBTN(238, BTC.UCID);
                    DeleteBTN(239, BTC.UCID);
                }

                #endregion

                #region ' help menu '
                /*{
                    switch (BTC.ClickID)
                    {
                        #region ' km for cars '
                        case 119:
                    DeleteBTN(101, BTC.UCID);
                    DeleteBTN(102, BTC.UCID);
                    DeleteBTN(103, BTC.UCID);
                    DeleteBTN(104, BTC.UCID);
                    DeleteBTN(105, BTC.UCID);
                    DeleteBTN(106, BTC.UCID);
                    DeleteBTN(107, BTC.UCID);
                    DeleteBTN(108, BTC.UCID);
                    DeleteBTN(109, BTC.UCID);
                    DeleteBTN(110, BTC.UCID);
                    DeleteBTN(111, BTC.UCID);
                    DeleteBTN(112, BTC.UCID);
                    DeleteBTN(113, BTC.UCID);
                    DeleteBTN(114, BTC.UCID);
                    DeleteBTN(115, BTC.UCID);
                    DeleteBTN(116, BTC.UCID);
                    DeleteBTN(117, BTC.UCID);
                    DeleteBTN(118, BTC.UCID);
                    DeleteBTN(119, BTC.UCID);
                    DeleteBTN(121, BTC.UCID);
                    Thread.Sleep(1000);
                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 90, 59, 17, 28, 118, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^6[SC] ^7km for cars:", Flags.ButtonStyles.ISB_DARK, 6, 50, 19, 30, 101, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7LX4 - 500 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 25, 30, 102, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7LX6 - 800 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 30, 30, 103, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7RB4 - 900 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 35, 30, 104, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7FXO - 1200 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 40, 30, 105, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7XRT - 1600 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 45, 30, 106, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7RAC - 1800 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 50, 30, 107, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7FZ5 - 2000 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 55, 30, 108, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7UFR - 2200 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 60, 30, 109, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7XFR - 2400 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 65, 30, 110, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7FXR - 3000 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 70, 30, 111, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7XRR - 3500 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 75, 30, 112, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7FZR - 4000 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 80, 30, 113, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7MRT - 1000 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 85, 30, 114, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7FBM - 10000 km.", Flags.ButtonStyles.ISB_LEFT, 5, 55, 90, 30, 115, Conn.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^1CLOSE [X]", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 13, 97, 50, 121, Conn.UniqueID, 2, false);

                            break;
                        #endregion

                        #region ' about '
                        case 117:
                    DeleteBTN(101, BTC.UCID);
                    DeleteBTN(102, BTC.UCID);
                    DeleteBTN(103, BTC.UCID);
                    DeleteBTN(104, BTC.UCID);
                    DeleteBTN(105, BTC.UCID);
                    DeleteBTN(106, BTC.UCID);
                    DeleteBTN(107, BTC.UCID);
                    DeleteBTN(108, BTC.UCID);
                    DeleteBTN(109, BTC.UCID);
                    DeleteBTN(110, BTC.UCID);
                    DeleteBTN(111, BTC.UCID);
                    DeleteBTN(112, BTC.UCID);
                    DeleteBTN(113, BTC.UCID);
                    DeleteBTN(114, BTC.UCID);
                    DeleteBTN(115, BTC.UCID);
                    DeleteBTN(116, BTC.UCID);
                    DeleteBTN(117, BTC.UCID);
                    DeleteBTN(118, BTC.UCID);
                    DeleteBTN(119, BTC.UCID);
                    DeleteBTN(121, BTC.UCID);
                    Thread.Sleep(1000);
                    MsgPly("^7About: " + CruiseName + " ^7v" + InSimVer, BTC.UCID);
                    MsgPly("^7All credits goes to:", BTC.UCID);
                    MsgPly("^7Crazyboy and all cruisers who keep", BTC.UCID);
                    MsgPly("^7this server alive!", BTC.UCID);
                    MsgPly("^7Comming soon our forum!!!", BTC.UCID);
                            break;
                        #endregion
                    }
                }*/
                #endregion

                #region ' Close Help Screen! '

                if (BTC.ClickID == 121)
                {
                    DeleteBTN(101, BTC.UCID);
                    DeleteBTN(102, BTC.UCID);
                    DeleteBTN(103, BTC.UCID);
                    DeleteBTN(104, BTC.UCID);
                    DeleteBTN(105, BTC.UCID);
                    DeleteBTN(106, BTC.UCID);
                    DeleteBTN(107, BTC.UCID);
                    DeleteBTN(108, BTC.UCID);
                    DeleteBTN(109, BTC.UCID);
                    DeleteBTN(110, BTC.UCID);
                    DeleteBTN(111, BTC.UCID);
                    DeleteBTN(112, BTC.UCID);
                    DeleteBTN(113, BTC.UCID);
                    DeleteBTN(114, BTC.UCID);
                    DeleteBTN(115, BTC.UCID);
                    DeleteBTN(116, BTC.UCID);
                    DeleteBTN(117, BTC.UCID);
                    DeleteBTN(118, BTC.UCID);
                    DeleteBTN(119, BTC.UCID);
                    DeleteBTN(120, BTC.UCID);
                    DeleteBTN(121, BTC.UCID);
                    DeleteBTN(122, BTC.UCID);
                    DeleteBTN(123, BTC.UCID);
                }

                #endregion

                #region ' Close adminhelp Screen! '

                if (BTC.ClickID == 107)
                {
                    DeleteBTN(100, BTC.UCID);
                    DeleteBTN(101, BTC.UCID);
                    DeleteBTN(102, BTC.UCID);
                    DeleteBTN(103, BTC.UCID);
                    DeleteBTN(104, BTC.UCID);
                    DeleteBTN(105, BTC.UCID);
                    DeleteBTN(106, BTC.UCID);
                    DeleteBTN(107, BTC.UCID);
                }

                #endregion

                #region ' Establishments/houses '
                switch (TrackName)
                {
                    case "BL1":
                    case "BL1X":
                    case "WE1X":
                    case "KY2X":
                    case "AU1X":
                    case "AU1":

                        #region ' In Store '

                        if (Conn.InStore == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Raffle '
                                case 120:
                                    if (Conn.Cash >= 300)
                                    {
                                        if (Conn.TotalSale >= 0)
                                        {
                                            #region ' Raffle Accept '
                                            if (Conn.LastRaffle == 0)
                                            {
                                                #region ' Total Sale Line '
                                                if (Conn.TotalSale > 16)
                                                {
                                                    int prize = new Random().Next(3000, 5000);
                                                    Conn.Cash += prize;
                                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from the Raffle!");
                                                }
                                                else if (Conn.TotalSale > 11)
                                                {
                                                    int prize = new Random().Next(2500, 3000);
                                                    Conn.Cash += prize;
                                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from the Raffle!");
                                                }
                                                else if (Conn.TotalSale > 6)
                                                {
                                                    int prize = new Random().Next(1000, 2500);
                                                    Conn.Cash += prize;
                                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from the Raffle!");
                                                }
                                                else if (Conn.TotalSale > 3)
                                                {
                                                    int prize = new Random().Next(750, 1000);
                                                    Conn.Cash += prize;
                                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from the Raffle!");
                                                }
                                                else
                                                {
                                                    int prize = new Random().Next(300, 750);
                                                    Conn.Cash += prize;
                                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from the Raffle!");
                                                }
                                                #endregion

                                                Conn.LastRaffle = 180;

                                                #region ' Replace Display '
                                                if (Conn.DisplaysOpen == true && Conn.InGameIntrfc == 0)
                                                {
                                                    if (Conn.LastRaffle > 120)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Three (3)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                                    }
                                                    else if (Conn.LastRaffle > 60)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Two (2)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                                    }
                                                    else if (Conn.LastRaffle > 0)
                                                    {
                                                        if (Conn.LastRaffle > 1)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                                        }
                                                        else
                                                        {
                                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                                        }
                                                    }
                                                    DeleteBTN(120, Conn.UniqueID);
                                                }
                                                #endregion

                                                Conn.Cash -= 300;
                                                Conn.TotalSale = 0;
                                            }
                                            else
                                            {
                                                #region ' Time Warning '
                                                if (Conn.LastRaffle > 120)
                                                {
                                                    MsgPly("^6>>^7 You have to wait ^1Three (3) hours ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                else if (Conn.LastRaffle > 60)
                                                {
                                                    MsgPly("^6>>^7 You have to wait ^1Two (2) hours ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                else
                                                {
                                                    MsgPly("^6>>^7 You have to wait ^1" + Conn.LastRaffle + " Minutes ^7to rejoin the Raffle", BTC.UCID);
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 You need to buy items before you raffle!", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enough Cash to join this raffle", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Jobs '
                                case 121:
                                    if (Conn.InGame == 0)
                                    {
                                        MsgPly("^6>>^7 You must be in vehicle before you access this command!", BTC.UCID);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        MsgPly("^6>>^7 You can only do a Job whilst not in duty!", BTC.UCID);
                                    }
                                    else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                                    {
                                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                        {
                                            MsgPly("^6>>^7 You can only do 1 Job at a time!", BTC.UCID);
                                        }
                                        else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                        {
                                            MsgPly("^6>>^7 Jobs Can be only done in Road Cars!", BTC.UCID);
                                        }
                                        else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                        {
                                            if (Conn.JobFromStore == false)
                                            {
                                                int JobRandom = new Random().Next(1, 45);
                                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " started a job!");

                                                #region ' Job To Hriso's '
                                                if (JobRandom > 0 && JobRandom < 15)
                                                {
                                                    if (Conn.JobToHouse1 == false)
                                                    {
                                                        if (JobRandom > 0 && JobRandom < 7)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Nintendo Wii to ^3Hriso's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Logitech G27 to ^3Hriso's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse1 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Martin's Farm '
                                                if (JobRandom > 14 && JobRandom < 30)
                                                {
                                                    if (Conn.JobToHouse2 == false)
                                                    {
                                                        if (JobRandom > 14 && JobRandom < 22)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Wooden Chair to ^3Martin's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Electronic Starter to ^7Martin's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse2 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Elly '
                                                if (JobRandom > 29)
                                                {
                                                    if (Conn.JobToHouse3 == false)
                                                    {
                                                        if (JobRandom > 29 && JobRandom < 34)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Telescope to ^3Elly's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a PSP Games to ^3Elly's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse3 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Interface if Activate '
                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    DeleteBTN(121, Conn.UniqueID);
                                                    InSim.Send_BTN_CreateButton("^2You ^7can only do 1 Job at a time", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                }
                                                #endregion

                                                Conn.JobFromStore = true;
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Can't start a Job whilst being chased.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Only in Establishments and House can start a job!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' In Shop '

                        if (Conn.InShop == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Fried Chicken '
                                case 118:
                                    if (Conn.Cash > 15)
                                    {
                                        if (Conn.TotalHealth < 89)
                                        {
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " ate Fried Chicken!", BTC.UCID);
                                            Conn.Cash -= 15;
                                            Conn.TotalHealth += 10;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Beer '
                                case 119:
                                    if (Conn.Cash > 10)
                                    {
                                        if (Conn.TotalHealth < 92)
                                        {
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " drank some Beer!", BTC.UCID);
                                            Conn.Cash -= 10;
                                            Conn.TotalHealth += 7;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash.", BTC.UCID);
                                    }
                                    break;

                                #endregion

                                #region ' Donuts '

                                case 120:
                                    if (Conn.Cash > 5)
                                    {
                                        if (Conn.TotalHealth < 94)
                                        {
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " bite some Donuts!", BTC.UCID);
                                            Conn.Cash -= 5;
                                            Conn.TotalHealth += 5;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Job '
                                case 121:

                                    if (Conn.InGame == 0)
                                    {
                                        MsgPly("^6>>^7 You must be in vehicle before you access this command!", BTC.UCID);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        MsgPly("^6>>^7 You can only do a Job whilst not in duty!", BTC.UCID);
                                    }
                                    else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                                    {
                                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                        {
                                            MsgPly("^6>>^7 You can only do 1 Job at a time!", BTC.UCID);
                                        }
                                        else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                        {
                                            MsgPly("^6>>^7 Jobs Can be only done in Road Cars!", BTC.UCID);
                                        }
                                        else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                        {
                                            if (Conn.JobFromShop == false)
                                            {
                                                int JobRandom = new Random().Next(1, 45);
                                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " started a job!");
                                                #region ' Job To Hriso's House '
                                                if (JobRandom > 0 && JobRandom < 15)
                                                {
                                                    if (Conn.JobToHouse1 == false)
                                                    {
                                                        if (JobRandom > 0 && JobRandom < 7)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Hot Fries to ^3Hriso's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Pizza to ^3Hriso's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse1 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Martin's '
                                                if (JobRandom > 14 && JobRandom < 30)
                                                {
                                                    if (Conn.JobToHouse2 == false)
                                                    {
                                                        if (JobRandom > 14 && JobRandom < 22)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Burgers to ^3Martin's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Healthy Salad to ^3Martin's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse2 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Job To Elly's '
                                                if (JobRandom > 29)
                                                {
                                                    if (Conn.JobToHouse3 == false)
                                                    {
                                                        if (JobRandom > 29 && JobRandom < 34)
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Donuts to ^3Elly's House^7!", BTC.UCID);
                                                        }
                                                        else
                                                        {
                                                            MsgPly("^6>>^7 Deliver a Burgers to ^3Elly's House^7!", BTC.UCID);
                                                        }
                                                        Conn.JobToHouse3 = true;
                                                    }
                                                }
                                                #endregion

                                                #region ' Interface if Activate '
                                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                                {
                                                    DeleteBTN(121, Conn.UniqueID);
                                                    InSim.Send_BTN_CreateButton("^2You ^7can only do 1 Job at a time", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                }
                                                #endregion

                                                Conn.JobFromShop = true;
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Can't start a Job whilst being chased.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Only in Establishments and House can start a job!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' In School '

                        if (Conn.InSchool == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Cake! '
                                case 118:
                                    if (Conn.Cash > 15)
                                    {
                                        if (Conn.TotalHealth < 89)
                                        {
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " eat some Cake!", BTC.UCID);
                                            Conn.Cash -= 15;
                                            Conn.TotalHealth += 10;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion

                                #region ' Lemonade '
                                case 119:
                                    if (Conn.Cash > 10)
                                    {
                                        if (Conn.TotalHealth < 92)
                                        {
                                            MsgPly("^6>>^7 " + Conn.NoColPlyName + " drank a Lemonade!", BTC.UCID);
                                            Conn.Cash -= 10;
                                            Conn.TotalHealth += 7;
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Too much health. Can't buy anymore.", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash.", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' InHouses '
                        if ((Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true) && BTC.ClickID == 120)
                        {
                            if (Conn.InGame == 0)
                            {
                                MsgPly("^6>>^7 You must be in vehicle before you access this command!", BTC.UCID);
                            }
                            else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                            {
                                MsgPly("^6>>^7 You can only do a Job whilst not in duty!", BTC.UCID);
                            }
                            else if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InStore == true || Conn.InShop == true)
                            {
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You can only do 1 Job at a time!", BTC.UCID);
                                }
                                else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                {
                                    MsgPly("^6>>^7 Jobs Can be only done in Road Cars!", BTC.UCID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != BTC.UCID)
                                {
                                    if (Conn.JobToSchool == false)
                                    {
                                        #region ' Clear Display '

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {

                                            DeleteBTN(110, Conn.UniqueID);
                                            DeleteBTN(111, Conn.UniqueID);
                                            DeleteBTN(112, Conn.UniqueID);
                                            DeleteBTN(113, Conn.UniqueID);
                                            DeleteBTN(114, Conn.UniqueID);
                                            DeleteBTN(115, Conn.UniqueID);
                                            DeleteBTN(116, Conn.UniqueID);
                                            DeleteBTN(118, Conn.UniqueID);
                                            DeleteBTN(119, Conn.UniqueID);
                                            DeleteBTN(120, Conn.UniqueID);
                                            Conn.DisplaysOpen = false;

                                        }

                                        #endregion

                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " started a job!");

                                        #region ' Hriso's House '
                                        if (Conn.InHouse1 == true)
                                        {
                                            if (Conn.JobFromHouse1 == false)
                                            {
                                                MsgPly("^6>>^7 Escort ^3Hriso's ^7children in ^3Lottery ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse1 = true;
                                            }
                                        }
                                        #endregion

                                        #region ' Martin's Farm '
                                        if (Conn.InHouse2 == true)
                                        {
                                            if (Conn.JobFromHouse2 == false)
                                            {
                                                MsgPly("^6>>^7 Escort ^3Martin's ^7children in ^3Lottery ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse2 = true;
                                            }
                                        }
                                        #endregion

                                        #region ' Elly's House '
                                        if (Conn.InHouse3 == true)
                                        {
                                            if (Conn.JobFromHouse3 == false)
                                            {
                                                MsgPly("^6>>^7 Escort ^3Elly's ^7children in ^3Lottery ^7Safely!", Conn.UniqueID);
                                                Conn.JobFromHouse3 = true;
                                            }
                                        }
                                        #endregion

                                        Conn.JobToSchool = true;
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Can't start a Job whilst being chased.", BTC.UCID);
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Only in Establishments and House can start a job!", BTC.UCID);
                            }
                        }
                        #endregion

                        #region ' In Dealer '

                        if (Conn.InDealer == true)
                        {
                            switch (BTC.ClickID)
                            {
                                //buy buttons
                                #region 'LX4'
                                case 133:
                                    {
                                        if (Conn.Cars.Contains("LX4") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("LX4"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1LX4 (LX4)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("LX4") + " ^7You need: ^2$" + (Dealer.GetCarPrice("LX4") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "LX4";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("LX4");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1LX4");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1LX4 (LX4) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("LX4") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» LX4 (LX4) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'LX4'
                                case 134:
                                    {
                                        if (Conn.Cars.Contains("LX6") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("LX6"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1LX6 (LX6)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("LX6") + " ^7You need: ^2$" + (Dealer.GetCarPrice("LX6") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "LX6";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("LX6");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1LX6");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1LX6 (LX6) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("LX6") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» LX6(LX6) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'Rb4'
                                case 135:
                                    {
                                        if (Conn.Cars.Contains("RB4") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("RB4"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1RB GT Turbo (RB4)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("RB4") + " ^7You need: ^2$" + (Dealer.GetCarPrice("RB4") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "RB4";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("RB4");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1RB4");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1RB GT Turbo (RB4) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("RB4") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» RB GT Turbo (RB4) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'Rb4'
                                case 136:
                                    {
                                        if (Conn.Cars.Contains("FXO") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FXO"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FX GT Turbo (FXO)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FXO") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FXO") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FXO";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FXO");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FXO");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FX GT Turbo (FXO) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FXO") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FX GT Turbo (FXO) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'XRT'
                                case 137:
                                    {
                                        if (Conn.Cars.Contains("XRT") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("XRT"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1XR GT Turbo (XRT)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("XRT") + " ^7You need: ^2$" + (Dealer.GetCarPrice("XRT") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "XRT";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("XRT");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1XRT");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1XR GT Turbo (XRT) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("XRT") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» XR GT Turbo (XRT) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'RAC'
                                case 138:
                                    {
                                        if (Conn.Cars.Contains("RAC") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("RAC"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1Raceabout (RAC)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("RAC") + " ^7You need: ^2$" + (Dealer.GetCarPrice("RAC") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "RAC";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("RAC");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1RAC");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1Raceabout (RAC) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("RAC") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» Raceabout (RAC) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'FZ5'
                                case 139:
                                    {

                                        if (Conn.Cars.Contains("FZ5") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FZ5"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FZ50 GT (FZ5)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FZ5") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FZ5") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FZ5";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FZ5");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FZ5");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FZ50 GT (FZ5) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FZ5") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FZ50 GT (FZ5) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'UFR'
                                case 140:
                                    {
                                        if (Conn.Cars.Contains("UFR") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("UFR"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1UFR", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("UFR") + " ^7You need: ^2$" + (Dealer.GetCarPrice("UFR") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "UFR";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("UFR");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1UFR");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1UFR ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("UFR") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» UFR ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'XFR'
                                case 141:
                                    {
                                        if (Conn.Cars.Contains("XFR") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("XFR"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1XF GTR (XFR)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("XFR") + " ^7You need: ^2$" + (Dealer.GetCarPrice("XFR") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "XFR";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("XFR");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1XFR");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1XF GTR (XFR) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("XFR") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» XF GTR (XFR) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'FXR'
                                case 142:
                                    {
                                        if (Conn.Cars.Contains("FXR") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FXR"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FXR (FXR)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FXR") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FXR") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FXR";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FXR");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FXR");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FXR (FXR) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FXR") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FXR (FXR) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'XRR'
                                case 143:
                                    {
                                        if (Conn.Cars.Contains("XRR") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("XRR"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1XR GTR (XRR)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("XRR") + " ^7You need: ^2$" + (Dealer.GetCarPrice("XRR") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "XRR";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("XRR");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1XRR");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1XR GTR (XRR) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("XRR") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» XR GTR (XRR) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'FZR'
                                case 144:
                                    {
                                        if (Conn.Cars.Contains("FZR") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FZR"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FZR (FZR)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FZR") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FZR") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FZR";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FZR");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FZR");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FZR (FZR) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FZR") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FZR (FZR) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'MRT'
                                case 145:
                                    {
                                        if (Conn.Cars.Contains("MRT") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("MRT"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1MRT (MRT)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("MRT") + " ^7You need: ^2$" + (Dealer.GetCarPrice("MRT") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "MRT";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("MRT");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1MRT");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1MRT (MRT) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("MRT") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» MRT (MRT) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'FBM'
                                case 146:
                                    {
                                        if (Conn.Cars.Contains("FBM") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FBM"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FBM (FBM)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FBM") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FBM") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FBM";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FBM");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FBM");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FBM (FBM) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FBM") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FBM (FBM) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'FOX'
                                case 147:
                                    {
                                        if (Conn.Cars.Contains("FOX") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("FOX"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1FOX (FOX)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("FOX") + " ^7You need: ^2$" + (Dealer.GetCarPrice("FOX") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "FOX";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("FOX");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1FOX");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1FOX (FOX) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("FOX") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» FOX (FOX) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'BF1'
                                case 148:
                                    {
                                        if (Conn.Cars.Contains("BF1") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("BF1"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1BF1 (BF1)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("BF1") + " ^7You need: ^2$" + (Dealer.GetCarPrice("BF1") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "BF1";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("BF1");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1BF1");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1BF1 (BF1) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("BF1") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» BF1 (BF1) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion

                                #region 'VWS'
                                case 150:
                                    {
                                        if (Conn.Cars.Contains("VWS") == false)
                                        {
                                            if (Connections[GetConnIdx(BTC.UCID)].Cash <= Dealer.GetCarPrice("VWS"))
                                            {
                                                InSim.Send_MTC_MessageToConnection("^7Not Enough Cash for ^1VWS (VWS)", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price: ^1$" + Dealer.GetCarPrice("VWS") + " ^7You need: ^2$" + (Dealer.GetCarPrice("VWS") - Connections[GetConnIdx(BTC.UCID)].Cash), BTC.UCID, 0);
                                            }
                                            else
                                            {
                                                string Cars = Connections[GetConnIdx(BTC.UCID)].Cars;
                                                Cars = Cars + " " + "VWS";
                                                Connections[GetConnIdx(BTC.UCID)].Cars = Cars;
                                                Connections[GetConnIdx(BTC.UCID)].Cash -= Dealer.GetCarPrice("VWS");
                                                MsgAll("^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7bought a ^1VW Scirocco");
                                                InSim.Send_MTC_MessageToConnection("^7You have bought ^1VWS (VWS) ^7in Garage", BTC.UCID, 0);
                                                InSim.Send_MTC_MessageToConnection("^7Price Tag: ^1$" + Dealer.GetCarPrice("VWS") + " ^7You have: ^2$" + Connections[GetConnIdx(BTC.UCID)].Cash + " ^7left", BTC.UCID, 0);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_MTC_MessageToConnection("^1» VWS (VWS) ^7is already exist on the Garage", BTC.UCID, 0);
                                        }
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' Ticket Zone '

                        if (Conn.InPayZone == true | Conn.InPayZone1 == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' buy ticket! '
                                case 120:
                                    if (Conn.Cash > 50)
                                    {
                                        if (Conn.TickedPay == 0)
                                        {
                                            Conn.Cash -= 50;
                                            MsgAll(Conn.PlayerName + " ^7bought entry ticket!");
                                            Conn.TickedPay = 1;
                                        }
                                        else
                                        {
                                            MsgPly(">>^7You already have ticked!", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        #region ' Parking Zone '

                        if (Conn.InParking == true)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' buy ticket! '
                                case 120:
                                    if (Conn.Cash > 500)
                                    {
                                        if (Conn.ParkingPay == 0)
                                        {
                                            Conn.Cash -= 500;
                                            MsgAll(Conn.PlayerName + " ^7bought ticket for parking!");
                                            Conn.ParkingPay = 1;
                                        }
                                        else
                                        {
                                            MsgPly(">>^7You already have ticked!", BTC.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enought cash!", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }

                        #endregion

                        break;
                }
                #endregion

                #region ' Cop Panel '

                if (Conn.IsOfficer == true || Conn.IsCadet == true)
                {
                    if (Conn.CopPanel == 1)
                    {
                        #region ' Engage '
                        if (Conn.InChaseProgress == false)
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Engage! '
                                case 20:
                                    try
                                    {
                                        if (Conn.UniqueID == BTC.UCID)
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
                                                            MsgPly(" ^6>>^7^3 Maximum Pursuit Limit: ^7" + AddChaseLimit, BTC.UCID);
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
                                                                MsgPly("^6>>^7 " + Connections[ChaseeIndex].NoColPlyName + " is being busted a cop.", BTC.UCID);
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

                                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^3joins in backup chase!");
                                                            MsgAll(" ^6>>^2 Suspect Name: " + Connections[ChaseeIndex].NoColPlyName + " (" + Connections[ChaseeIndex].Username + ")");
                                                            MsgAll(" ^6>>^2 Cops In Chase: " + Connections[ChaseeIndex].CopInChase);
                                                            MsgAll(" ^6>>^2 Chase Condition: ^7" + Connections[ChaseeIndex].ChaseCondition);
                                                            Connections[ChaseeIndex].IsSuspect = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MsgPly("^6>>^7 Cannot join in the Police Pursuit.", BTC.UCID);
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    MsgPly("^6>>^7 No Civilian found in 150 meters", BTC.UCID);
                                                }
                                            }

                                            #endregion
                                        }
                                    }
                                    catch
                                    {
                                        MsgPly("^6>>^7 Engage Error.", BTC.UCID);
                                    }
                                    break;
                                #endregion
                            }
                        }
                        #endregion

                        #region ' In Chase Progress '
                        else
                        {
                            switch (BTC.ClickID)
                            {
                                #region ' Bump or Busted '
                                case 20:

                                    #region ' Busted '
                                    if (Conn.Busted == true)
                                    {
                                        if (Conn.BustedTimer == 5)
                                        {
                                            #region ' Close Moderation if Possible '
                                            if (Conn.InModerationMenu == 1 || Conn.InModerationMenu == 2)
                                            {
                                                DeleteBTN(30, BTC.UCID);
                                                DeleteBTN(31, BTC.UCID);
                                                DeleteBTN(32, BTC.UCID);
                                                DeleteBTN(33, BTC.UCID);
                                                DeleteBTN(34, BTC.UCID);
                                                DeleteBTN(35, BTC.UCID);
                                                DeleteBTN(36, BTC.UCID);
                                                DeleteBTN(37, BTC.UCID);
                                                DeleteBTN(38, BTC.UCID);
                                                DeleteBTN(39, BTC.UCID);
                                                DeleteBTN(40, BTC.UCID);
                                                DeleteBTN(41, BTC.UCID);
                                                DeleteBTN(42, BTC.UCID);
                                                DeleteBTN(43, BTC.UCID);
                                                Conn.ModReason = "";
                                                Conn.ModReasonSet = false;
                                                Conn.ModUsername = "";
                                                Conn.InModerationMenu = 0;
                                            }
                                            #endregion

                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^1busts the suspect!");
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

                                    #region ' Chase Bump '
                                    else if (Conn.Busted == false)
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
                                                        MsgAll(" ^6>>^2 Chase Condition : ^72");
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
                                        MsgPly("^6>>^7 type ^2!busted ^7to busted the suspect", BTC.UCID);
                                        MsgPly("^7  or ^2!disengage ^7to stop the chase!", BTC.UCID);
                                    }

                                    #endregion

                                    break;

                                #endregion

                                #region ' Disengage '
                                case 21:
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
                                                MsgPly("^6>>^7 You aren't in chase!", BTC.UCID);
                                            }
                                        }
                                        else if (Conn.InGame == 0)
                                        {
                                            MsgPly("^6>>^7 You must be in vehicle before you access this command!", BTC.UCID);
                                        }
                                    }
                                    else if (Conn.InFineMenu == true)
                                    {
                                        MsgPly("^6>>^7 Set a Tickets to " + ChaseCon.NoColPlyName + "!", BTC.UCID);
                                    }

                                    break;
                                #endregion
                            }
                        }
                        #endregion

                        #region ' Remove Trap '

                        if (Conn.TrapSetted == true && BTC.ClickID == 21)
                        {
                            MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                            Conn.TrapY = 0;
                            Conn.TrapX = 0;
                            Conn.TrapSpeed = 0;
                            Conn.TrapSetted = false;
                        }

                        #endregion
                    }
                }

                #endregion

                #region ' Busted Panel & Accept/Refuse Issue '

                if (Conn.InFineMenu == true)
                {
                    switch (BTC.ClickID)
                    {
                        #region ' Warn '
                        case 39:

                            if (Conn.TicketReasonSet == true)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " issued a warn to " + ChaseCon.NoColPlyName);

                                #region ' To Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.UniqueID == ChaseCon.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                        InSim.Send_BTN_CreateButton("^7Fine: None ", Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                        InSim.Send_BTN_CreateButton("^7You are only warned from this Ticket!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                        InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 39, (Con.UniqueID), 40, false);
                                        Con.AcceptTicket = 2;
                                        Con.IsBeingBusted = false;
                                        Con.TicketRefuse = 0;
                                    }
                                }
                                #endregion

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion

                                if (Conn.JoinedChase == true)
                                {
                                    Conn.JoinedChase = false;
                                }
                                if (Conn.InFineMenu == true)
                                {
                                    Conn.InFineMenu = false;
                                }
                                Conn.TicketAmount = 0;
                                Conn.TicketAmountSet = false;
                                Conn.TicketReason = "";
                                Conn.TicketReasonSet = false;
                                Conn.Busted = false;
                                Conn.Chasee = -1;
                                Conn.ChaseCondition = 0;
                            }
                            else if (Conn.TicketReasonSet == false)
                            {
                                MsgPly("^6>>^7 Warn Error. You must have Reason.", Conn.UniqueID);
                            }

                            break;
                        #endregion

                        #region ' Issue '
                        case 40:

                            if (Conn.TicketReasonSet == true && Conn.TicketAmountSet == true)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " issued a fines to " + ChaseCon.NoColPlyName);

                                #region ' To Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.UniqueID == ChaseCon.UniqueID)
                                    {
                                        if (Con.TicketRefuse == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7Fine ^1$" + Conn.TicketAmount, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7This Ticket is being issued for being Busted!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Refuse", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Con.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Pay", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Con.UniqueID), 40, false);
                                            Con.AcceptTicket = 1;
                                            Con.TicketRefuse = 1;
                                            Con.TicketReason = Conn.TicketReason;
                                            Con.TicketAmount = Conn.TicketAmount;
                                        }
                                        else if (Con.TicketRefuse == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Issued by: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + Conn.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7Fine ^1$" + Conn.TicketAmount, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 36, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Reason: " + Conn.TicketReason, Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 37, (Con.UniqueID), 2, false);

                                            InSim.Send_BTN_CreateButton("^7This is the last Ticket Issue. Remember you'll get fined in the next Refuse!", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (Con.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Refuse", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (Con.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Pay", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (Con.UniqueID), 40, false);
                                            Con.AcceptTicket = 1;
                                            Con.TicketRefuse = 2;
                                            Con.TicketReason = Conn.TicketReason;
                                            Con.TicketAmount = Conn.TicketAmount;
                                        }
                                    }
                                    if (Con.Chasee == ChaseCon.UniqueID)
                                    {
                                        if (ChaseCon.CopInChase > 1)
                                        {
                                            Con.TicketAmount = Conn.TicketAmount;
                                            Con.TicketReason = Conn.TicketReason;
                                        }
                                    }
                                }
                                #endregion

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }
                            else if (Conn.TicketAmountSet == false && Conn.TicketReasonSet == false)
                            {
                                MsgPly("^6>>^7 Issue Error. You must have Reason and Ticket Fines.", Conn.UniqueID);
                            }

                            break;
                        #endregion
                    }
                }

                #region ' Pay/Warn/Refuse '

                #region ' Pay/Refuse '
                if (Conn.AcceptTicket == 1)
                {
                    switch (BTC.ClickID)
                    {
                        #region ' Refuse '
                        case 39:

                            if (Conn.TicketRefuse == 1)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " refused to pay the first ticket!");
                                MsgPly(" ^6>>^7^1 WARNING: ^7Refusing the second ticket may cause max fine!", BTC.UCID);
                                #region ' To Connection List '

                                foreach (clsConnection o in Connections)
                                {
                                    if (o.Chasee == Conn.UniqueID)
                                    {
                                        if (o.InFineMenu == true)
                                        {
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 30, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 60, 100, 50, 50, 31, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Ticket Window Re-Issue", Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 32, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Suspect Name: " + Conn.PlayerName, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 60, 54, 33, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Chase Condition: " + o.ChaseCondition, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 65, 54, 34, (o.UniqueID), 2, false);

                                            #region ' Condition '
                                            if (Conn.ChaseCondition == 1)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 54, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 2)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$1,300", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 3)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$2,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 4)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$3,500", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            if (Conn.ChaseCondition == 5)
                                            {
                                                InSim.Send_BTN_CreateButton("^7Max Fine For Chase: ^1$5,000", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_C1, 4, 70, 70, 58, 35, (o.UniqueID), 2, false);
                                            }
                                            #endregion

                                            // Click Buttons
                                            InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Enter a Fine Amount And Reason For The Chase", Flags.ButtonStyles.ISB_C1, 4, 70, 95, 65, 38, (o.UniqueID), 2, false);
                                            InSim.Send_BTN_CreateButton("^7Warn", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 107, 39, (o.UniqueID), 40, false);
                                            InSim.Send_BTN_CreateButton("^7Issue", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 4, 10, 103, 82, 40, (o.UniqueID), 40, false);

                                        }
                                    }
                                }
                                #endregion

                                Conn.TicketReason = "";
                                Conn.TicketAmount = 0;
                                Conn.AcceptTicket = 0;

                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }
                            else if (Conn.TicketRefuse == 2)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " refused to pay the fines!");
                                MsgAll("  ^7was fined ^1$5000 ^7for refusing second ticket!");

                                #region ' Connection List '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Chasee == Conn.UniqueID)
                                    {
                                        if (i.InFineMenu == true)
                                        {
                                            i.InFineMenu = false;
                                        }

                                        if (i.IsOfficer == true)
                                        {
                                            MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.4)));
                                            i.Cash += (Convert.ToInt16(5000 * 0.4));
                                        }
                                        if (i.IsCadet == true)
                                        {
                                            MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.4)));
                                            i.Cash += (Convert.ToInt16(5000 * 0.2));
                                        }
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

                                Conn.Cash -= 5000;
                                Conn.AcceptTicket = 0;
                                Conn.IsBeingBusted = false;
                                Conn.CopInChase = 0;
                                Conn.TicketRefuse = 0;
                                #region ' Close Region LOL '
                                DeleteBTN(30, BTC.UCID);
                                DeleteBTN(31, BTC.UCID);
                                DeleteBTN(32, BTC.UCID);
                                DeleteBTN(33, BTC.UCID);
                                DeleteBTN(34, BTC.UCID);
                                DeleteBTN(35, BTC.UCID);
                                DeleteBTN(36, BTC.UCID);
                                DeleteBTN(37, BTC.UCID);
                                DeleteBTN(38, BTC.UCID);
                                DeleteBTN(39, BTC.UCID);
                                DeleteBTN(40, BTC.UCID);
                                #endregion
                            }

                            break;
                        #endregion

                        #region ' Pay '
                        case 40:

                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " paid the fine for ^1$" + Conn.TicketAmount + "^7!");
                            MsgAll("^6>>^7 Reason: " + Conn.TicketReason);


                            #region ' Connection List '
                            foreach (clsConnection i in Connections)
                            {
                                if (i.Chasee == Conn.UniqueID)
                                {
                                    if (i.InFineMenu == true)
                                    {
                                        i.InFineMenu = false;
                                    }
                                    if (i.IsOfficer == true)
                                    {
                                        MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(i.TicketAmount * 0.4)));
                                        i.Cash += (Convert.ToInt16(i.TicketAmount * 0.4));
                                    }
                                    if (i.IsCadet == true)
                                    {
                                        MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(i.TicketAmount * 0.2)));
                                        i.Cash += (Convert.ToInt16(i.TicketAmount * 0.2));
                                    }
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

                            Conn.Cash -= Conn.TicketAmount;

                            Conn.TicketReason = "";
                            Conn.TicketAmount = 0;
                            Conn.TicketRefuse = 0;
                            Conn.AcceptTicket = 0;
                            Conn.IsBeingBusted = false;
                            Conn.CopInChase = 0;

                            #region ' Close Region LOL '
                            DeleteBTN(30, BTC.UCID);
                            DeleteBTN(31, BTC.UCID);
                            DeleteBTN(32, BTC.UCID);
                            DeleteBTN(33, BTC.UCID);
                            DeleteBTN(34, BTC.UCID);
                            DeleteBTN(35, BTC.UCID);
                            DeleteBTN(36, BTC.UCID);
                            DeleteBTN(37, BTC.UCID);
                            DeleteBTN(38, BTC.UCID);
                            DeleteBTN(39, BTC.UCID);
                            DeleteBTN(40, BTC.UCID);
                            #endregion

                            break;
                        #endregion
                    }
                }
                #endregion

                #region ' Warn '
                else if (Conn.AcceptTicket == 2)
                {
                    #region ' Close Warn '
                    if (BTC.ClickID == 39)
                    {
                        #region ' Connection List '
                        foreach (clsConnection i in Connections)
                        {
                            if (i.Chasee == Conn.UniqueID)
                            {
                                if (i.InFineMenu == true)
                                {
                                    i.InFineMenu = false;
                                }
                                if (i.JoinedChase == true)
                                {
                                    i.JoinedChase = false;
                                }
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " accepted the Warning.");
                                MsgAll("^6>>^7 Reason: " + i.TicketReason);
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

                        Conn.AcceptTicket = 0;
                        Conn.IsBeingBusted = false;
                        Conn.CopInChase = 0;

                        #region ' Close Region LOL '
                        DeleteBTN(30, BTC.UCID);
                        DeleteBTN(31, BTC.UCID);
                        DeleteBTN(32, BTC.UCID);
                        DeleteBTN(33, BTC.UCID);
                        DeleteBTN(34, BTC.UCID);
                        DeleteBTN(35, BTC.UCID);
                        DeleteBTN(36, BTC.UCID);
                        DeleteBTN(37, BTC.UCID);
                        DeleteBTN(38, BTC.UCID);
                        DeleteBTN(39, BTC.UCID);
                        DeleteBTN(40, BTC.UCID);
                        #endregion
                    }
                    #endregion
                }
                #endregion

                #endregion

                #endregion

                #region ' Tradecar '
                switch (BTC.ClickID)
                {
                    case 130:
                        {
                            Connections[GetConnIdx(SenderUCID)].Cash += Convert.ToInt32(SentMoney - (SentMoney / 20));
                            Connections[GetConnIdx(BTC.UCID)].Cars = Conn.Cars + " " + SentCar;
                            Connections[GetConnIdx(BTC.UCID)].Cash -= SentMoney;
                            InSim.Send_BFN_DeleteButton(0, 132, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 211, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 212, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 130, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 131, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 133, BTC.UCID);
                            InSim.Send_MTC_MessageToConnection("^3>^7 Transaction complete. Thank you for you custom!", BTC.UCID, 0);
                            InSim.Send_MTC_MessageToConnection("^3>^7 Transaction complete.", SenderUCID, 0);
                            InSim.Send_MTC_MessageToConnection("^3>^7 You received $" + SentMoney + " - 5% tax ($" + Convert.ToInt32(SentMoney - (SentMoney / 20)) + ")", SenderUCID, 0);
                            InSim.Send_MST_Message("/msg ^7" + Connections[GetConnIdx(SenderUCID)].PlayerName + " ^7sent " + SentCar);
                            InSim.Send_MST_Message("/msg ^7to " + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7for $" + SentMoney);
                            InSim.Send_MST_Message("!remcar " + SentCar + " " + Connections[GetConnIdx(SenderUCID)].Username);
                        }
                        break;

                    case 131:
                        {
                            InSim.Send_BFN_DeleteButton(0, 132, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 211, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 212, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 130, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 131, BTC.UCID);
                            InSim.Send_BFN_DeleteButton(0, 133, BTC.UCID);
                            InSim.Send_MTC_MessageToConnection("^3>^7" + Connections[GetConnIdx(BTC.UCID)].PlayerName + " ^7rejected your offer", SenderUCID, 0);
                        }
                        break;
                }
                #endregion

                #region ' Moderation Panel '

                #region ' Close Moderation '
                if (BTC.ClickID == 43)
                {
                    if (Conn.InModerationMenu == 1 || Conn.InModerationMenu == 2)
                    {
                        DeleteBTN(30, BTC.UCID);
                        DeleteBTN(31, BTC.UCID);
                        DeleteBTN(32, BTC.UCID);
                        DeleteBTN(33, BTC.UCID);
                        DeleteBTN(34, BTC.UCID);
                        DeleteBTN(35, BTC.UCID);
                        DeleteBTN(36, BTC.UCID);
                        DeleteBTN(37, BTC.UCID);
                        DeleteBTN(38, BTC.UCID);
                        DeleteBTN(39, BTC.UCID);
                        DeleteBTN(40, BTC.UCID);
                        DeleteBTN(41, BTC.UCID);
                        DeleteBTN(42, BTC.UCID);
                        DeleteBTN(43, BTC.UCID);
                        Conn.ModReason = "";
                        Conn.ModReasonSet = false;
                        Conn.ModUsername = "";
                        Conn.InModerationMenu = 0;
                    }
                }

                #endregion

                if (Conn.InModerationMenu == 1)
                {

                    switch (BTC.ClickID)
                    {
                        #region ' Warn '
                        case 38:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                bool Complete = false;
                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;

                                        if (i.OnScreenExit > 0)
                                        {
                                            MsgPly("^6>>^7 Please wait until" + i.NoColPlyName + " completes the exit screen!", BTC.UCID);
                                        }
                                        else
                                        {
                                            Complete = true;
                                            InSim.Send_BTN_CreateButton(Conn.PlayerName + "^7#(W) : " + Conn.ModReason, Flags.ButtonStyles.ISB_C2, 14, 200, 50, 0, 10, i.UniqueID, 2, false);
                                            MsgPly("^6>>^7 You are warned by " + Conn.NoColPlyName, i.UniqueID);
                                            AdmBox("> " + Conn.NoColPlyName + " warned " + i.NoColPlyName);
                                            AdmBox("> Reason: " + Conn.ModReason);
                                            i.ModerationWarn = 5;
                                        }
                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true && Complete == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Offline '
                                if (Found == false)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                    long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                    string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                    long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                    long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                    byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                    byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                    byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                    int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                    byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                    byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                    byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                    byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                    byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                    byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                    byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                    byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                    byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                    string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                    string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                    string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                    string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                    MsgPly("^6>>^7 " + NoColPlyName + " couldn't be warned.", BTC.UCID);
                                    MsgPly("^6>>^7 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^6>>^7 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion

                        #region ' Spectate '
                        case 40:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                bool Complete = false;
                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;

                                        if (i.InGame == 0)
                                        {
                                            MsgPly("^6>>^7 " + i.NoColPlyName + " is not ingame!", BTC.UCID);
                                        }
                                        else
                                        {
                                            Complete = true;
                                            MsgAll("^6>>^7 " + i.NoColPlyName + " was spected by " + Conn.NoColPlyName + "!");
                                            MsgPly("^6>>^7 You are spected by " + Conn.NoColPlyName, i.UniqueID);
                                            MsgAll("^6>>^7 Reason: " + Conn.ModReason);
                                            AdmBox("> " + Conn.NoColPlyName + " spected " + i.NoColPlyName);
                                            AdmBox("> Reason: " + Conn.ModReason);
                                            SpecID(i.Username);
                                            SpecID(i.PlayerName);
                                        }

                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true && Complete == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Goes Offline '
                                if (Found == false)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                    long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                    string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                    long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                    long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                    byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                    byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                    byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                    int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                    byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                    byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                    byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                    byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                    byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                    byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                    byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                    byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                    byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                    string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                    string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                    string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                    string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                    MsgPly("^6>>^7 " + NoColPlyName + " couldn't be spectated.", BTC.UCID);
                                    MsgPly("^6>>^7 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^6>>^7 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion

                        #region ' Kick '
                        case 41:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;

                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;
                                        MsgAll("^6>>^7 " + i.NoColPlyName + " was kicked by  " + Conn.NoColPlyName + "!");
                                        MsgPly("^6>>^7 You are kicked by " + Conn.NoColPlyName, i.UniqueID);
                                        MsgAll("^6>>^7 Reason: " + Conn.ModReason);
                                        AdmBox("> " + Conn.NoColPlyName + " kicked " + i.NoColPlyName);
                                        AdmBox("> Reason: " + Conn.ModReason);
                                        KickID(i.Username);
                                    }
                                }
                                #endregion

                                #region ' Found '

                                if (Found == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, BTC.UCID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, BTC.UCID, 2, false);

                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }

                                #endregion

                                #region ' Offline '
                                if (Found == false)
                                {
                                    #region ' Objects '
                                    long Cash = FileInfo.GetUserCash(Conn.ModUsername);
                                    long BBal = FileInfo.GetUserBank(Conn.ModUsername);
                                    string Cars = FileInfo.GetUserCars(Conn.ModUsername);
                                    long Gold = FileInfo.GetUserGold(Conn.ModUsername);

                                    long TotalDistance = FileInfo.GetUserDistance(Conn.ModUsername);
                                    byte TotalHealth = FileInfo.GetUserHealth(Conn.ModUsername);
                                    int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.ModUsername);

                                    byte Electronics = FileInfo.GetUserElectronics(Conn.ModUsername);
                                    byte Furniture = FileInfo.GetUserFurniture(Conn.ModUsername);

                                    int LastRaffle = FileInfo.GetUserLastRaffle(Conn.ModUsername);
                                    int LastLotto = FileInfo.GetUserLastLotto(Conn.ModUsername);

                                    byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.ModUsername);
                                    byte CanBeCadet = FileInfo.CanBeCadet(Conn.ModUsername);
                                    byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.ModUsername);
                                    byte IsModerator = FileInfo.IsMember(Conn.ModUsername);

                                    byte Interface1 = FileInfo.GetInterface(Conn.ModUsername);
                                    byte Interface2 = FileInfo.GetInterface2(Conn.ModUsername);
                                    byte Speedo = FileInfo.GetSpeedo(Conn.ModUsername);
                                    byte Odometer = FileInfo.GetOdometer(Conn.ModUsername);
                                    byte Counter = FileInfo.GetCounter(Conn.ModUsername);
                                    byte Panel = FileInfo.GetCopPanel(Conn.ModUsername);

                                    byte Renting = FileInfo.GetUserRenting(Conn.ModUsername);
                                    byte Rented = FileInfo.GetUserRented(Conn.ModUsername);
                                    string Renter = FileInfo.GetUserRenter(Conn.ModUsername);
                                    string Renterr = FileInfo.GetUserRenterr(Conn.ModUsername);
                                    string Rentee = FileInfo.GetUserRentee(Conn.ModUsername);

                                    string PlayerName = FileInfo.GetUserPlayerName(Conn.ModUsername);
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

                                    MsgPly("^6>>^7 " + NoColPlyName + " couldn't be kicked.", BTC.UCID);
                                    MsgPly("^6>>^7 The user goes offline mode!", BTC.UCID);

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTC.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                    Conn.ModReasonSet = false;
                                    Conn.ModReason = "";
                                }
                                #endregion
                            }
                            else
                            {
                                MsgPly("^6>>^7 Reason not yet setted.", BTC.UCID);
                            }

                            break;
                        #endregion
                    }

                }

                #endregion

            }
            catch { }
        }
    }
}