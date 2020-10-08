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
        private int Messages;
        #region ' Event Loaders '

        // Form1 Loader
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                #region ' Chat Command Loader '
                foreach (MethodInfo mi in typeof(Form1).GetMethods())
                {
                    foreach (object o in mi.GetCustomAttributes(false))
                    {
                        if (o.GetType() == typeof(CommandAttribute))
                        {
                            CommandAttribute ca = (CommandAttribute)o;
                            CommandList com = new CommandList();
                            com.CommandArg = ca;
                            com.MethodInf = mi;
                            Commands.Add(com);
                        }
                    }
                }
                #endregion

                if (System.IO.Directory.Exists(Database) == false)
                {
                    System.IO.Directory.CreateDirectory(Database);
                }

                #region ' Check User '

                System.Timers.Timer CheckUser = new System.Timers.Timer(1000);
                CheckUser.Elapsed += new System.Timers.ElapsedEventHandler(CheckUser_Elapsed);
                CheckUser.Enabled = true;

                #endregion

                #region ' Second timers '

                System.Timers.Timer SecondTimers = new System.Timers.Timer(1000);
                SecondTimers.Elapsed += new System.Timers.ElapsedEventHandler(SecondTimers_Elapsed);
                SecondTimers.Enabled = true;

                #endregion

                #region ' OnScreen Effects '

                System.Timers.Timer OnScreen = new System.Timers.Timer(500);
                OnScreen.Elapsed += new System.Timers.ElapsedEventHandler(OnScreen_Elapsed);
                OnScreen.Enabled = true;
                #endregion

                #region ' Save Timer '

                // 1 minute
                System.Timers.Timer BackUp_Users = new System.Timers.Timer(60000);
                BackUp_Users.Elapsed += new System.Timers.ElapsedEventHandler(BackUp_Users_Elapsed);
                BackUp_Users.Enabled = true;

                #endregion

                #region ' 1 Minute Interval '

                System.Timers.Timer MinuteTimer = new System.Timers.Timer(60000);
                MinuteTimer.Elapsed += new System.Timers.ElapsedEventHandler(MinuteTimer_Elapsed);
                MinuteTimer.Enabled = true;

                #endregion

                #region ' Siren '

                System.Timers.Timer SystemSiren = new System.Timers.Timer(200);
                SystemSiren.Elapsed += new System.Timers.ElapsedEventHandler(SystemSiren_Elapsed);
                SystemSiren.Enabled = true;

                #endregion
            }
            catch { }
        }

        // System Siren
        private void SystemSiren_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' Show Siren '
                        if (Conn.InGame == 1)
                        {
                            #region ' Siren Check '
                            if (Conn.InChaseProgress != false)
                            {
                                byte SirenIndex = 0;

                                #region ' Remote Siren PLID '
                                for (byte o = 0; o < Connections.Count; o++)
                                {
                                    if (Connections[o].PlayerID != 0)
                                    {
                                        SirenIndex = o;
                                    }

                                    #region ' Get Siren '
                                    var ChaseCon = Connections[SirenIndex];
                                    ChaseCon.SirenDistance = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - (Conn.CompCar.X), 2) + Math.Pow(ChaseCon.CompCar.Y - (Conn.CompCar.Y), 2)) / 65536);
                                    if (ChaseCon.InChaseProgress == false)
                                    {
                                        if (ChaseCon.SirenDistance < 250)
                                        {
                                            ChaseCon.SirenShowned = true;
                                            #region ' Siren Index '
                                            if (ChaseCon.CopSiren == 0)
                                            {
                                                ChaseCon.CopSiren = 1;
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            if (ChaseCon.SirenShowned == true)
                                            {
                                                if (ChaseCon.CopSiren != 0)
                                                {
                                                    ChaseCon.CopSiren = 3;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ChaseCon.SirenShowned == true)
                                        {
                                            if (ChaseCon.CopSiren != 0)
                                            {
                                                ChaseCon.CopSiren = 3;
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region ' Show Siren '

                            if (Conn.CopSiren != 0 && Conn.SirenShowned != false)
                            {
                                if (Conn.CopSiren == 1)
                                {
                                    Conn.CopSiren = 10;
                                }
                                else if (Conn.CopSiren > 9)
                                {
                                    if (Conn.CopSiren > 14)
                                    {
                                        Conn.CopSiren = 10;
                                    }

                                    if (OnScreen == 0)
                                    {
                                        //InSim.Send_BTN_CreateButton("^4" + "((((".Insert((Conn.CopSiren % 10), + "^4^J£ ^7SIREN ^1^J£" + "^4" + "))))".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4" + "^1(".Insert((Conn.CopSiren % 10), "") + "^7!^1)(^7!" + " ^1WARNING POLICE " + "" + "^7!^1)(^7!".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        //InSim.Send_BTN_CreateButton("^4^J£ ^7SIREN ^1^J£", Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^4" + "^1(".Insert((Conn.CopSiren % 10), "") + "^7!^1)(^7!" + " ^1WARNING POLICE " + "" + "^7!^1)(^7!".Insert(4 - (Conn.CopSiren % 10), "^1)^4"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }

                                    InSim.Send_BTN_CreateButton("^1" + Conn.SirenDistance + " meters", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 74, 73, 93, 24, Conn.UniqueID, 2, false);
                                    Conn.CopSiren++;
                                }
                                else if (Conn.CopSiren == 3)
                                {
                                    if (Conn.SirenShowned == true)
                                    {
                                        DeleteBTN(23, Conn.UniqueID);
                                        DeleteBTN(24, Conn.UniqueID);
                                        Conn.CopSiren = 0;
                                        Conn.SirenShowned = false;
                                    }
                                }
                            }
                            #endregion

                            #region ' Caution Siren Check '
                            if (Conn.InTowProgress != false)
                            {
                                byte SirenIndex = 0;

                                #region ' Remote Siren PLID '
                                for (byte o = 0; o < Connections.Count; o++)
                                {
                                    if (Connections[o].PlayerID != 0)
                                    {
                                        SirenIndex = o;
                                    }

                                    #region ' Get Siren '
                                    var ChaseCon = Connections[SirenIndex];
                                    ChaseCon.SirenDistance = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - (Conn.CompCar.X), 2) + Math.Pow(ChaseCon.CompCar.Y - (Conn.CompCar.Y), 2)) / 65536);
                                    if (ChaseCon.InTowProgress == false)
                                    {
                                        if (ChaseCon.SirenDistance < 250)
                                        {
                                            ChaseCon.SirenShowned = true;
                                            #region ' Siren Index '
                                            if (ChaseCon.TowCautionSiren == 0)
                                            {
                                                ChaseCon.TowCautionSiren = 1;
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            if (ChaseCon.SirenShowned == true)
                                            {
                                                if (ChaseCon.TowCautionSiren != 0)
                                                {
                                                    ChaseCon.TowCautionSiren = 3;
                                                }
                                                ChaseCon.SirenShowned = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ChaseCon.SirenShowned == true)
                                        {
                                            if (ChaseCon.TowCautionSiren != 0)
                                            {
                                                ChaseCon.TowCautionSiren = 3;
                                            }
                                            ChaseCon.SirenShowned = false;
                                        }
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                            #endregion

                            #region ' Show Siren '

                            if (Conn.TowCautionSiren != 0 && Conn.SirenShowned != false)
                            {
                                if (Conn.TowCautionSiren == 1)
                                {
                                    Conn.TowCautionSiren = 10;
                                }
                                else if (Conn.TowCautionSiren > 9)
                                {
                                    if (Conn.TowCautionSiren > 14)
                                    {
                                        Conn.TowCautionSiren = 10;
                                    }

                                    if (OnScreen == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^3" + "((((".Insert((Conn.TowCautionSiren % 10), "^1(^3") + "^1(^3!^1) ^7" + "WARNING ^3(^1!^3)" + "^3" + "))))".Insert(4 - (Conn.TowCautionSiren % 10), "^1)^3"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^3" + "((((".Insert((Conn.TowCautionSiren % 10), "^1(^3") + "^3(^1!^3) ^7" + "WARNING ^1(^3!^1)" + "^3" + "))))".Insert(4 - (Conn.TowCautionSiren % 10), "^1)^3"), Flags.ButtonStyles.ISB_C1, 15, 74, 60, 63, 23, Conn.UniqueID, 2, false);
                                    }

                                    InSim.Send_BTN_CreateButton("^1" + Conn.SirenDistance + " meters", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 5, 74, 73, 93, 24, Conn.UniqueID, 2, false);
                                    Conn.TowCautionSiren++;
                                }
                                else if (Conn.TowCautionSiren == 3)
                                {
                                    if (Conn.SirenShowned == true)
                                    {
                                        DeleteBTN(23, Conn.UniqueID);
                                        DeleteBTN(24, Conn.UniqueID);
                                        Conn.TowCautionSiren = 0;
                                        Conn.SirenShowned = false;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region ' Spectators '
                        else if (Conn.InGame == 0)
                        {
                            if (Conn.CopSiren != 0)
                            {
                                Conn.CopSiren = 3;
                            }
                            else if (Conn.CopSiren == 3)
                            {
                                DeleteBTN(23, Conn.UniqueID);
                                DeleteBTN(24, Conn.UniqueID);
                                Conn.CopSiren = 0;
                            }
                            DeleteBTN(23, Conn.UniqueID);
                            DeleteBTN(24, Conn.UniqueID);
                        }
                        #endregion

                        #endregion

                        #region ' 1.0.7 COMING SOON '
                        /*
                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 85, 100, 50, 50, 170, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 83, 98, 51, 51, 171, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("HELP MENU WINDOW", Flags.ButtonStyles.ISB_C1, 6, 35, 53, 48, 172, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 142, 173, Conn.UniqueID, 2, false);

                        #region ' BTN Click '
                        InSim.Send_BTN_CreateButton("SERVER HELP", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 60, 54, 174, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("SHOWOFF", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 66, 54, 175, (Conn.UniqueID), 2, false);
                        if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                        {
                            InSim.Send_BTN_CreateButton("PITLANE FOR ^1$300", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 72, 54, 176, (Conn.UniqueID), 2, false);
                        }
                        else
                        {
                            InSim.Send_BTN_CreateButton("PITLANE FOR ^1$750", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 72, 54, 176, (Conn.UniqueID), 2, false);
                        }
                        InSim.Send_BTN_CreateButton("SEND", "Send amount of Cash", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 78, 54, 4, 177, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("SET UNIT (KMH / MPH)", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 84, 54, 178, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("SET UNIT (KMS / MI)", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 90, 54, 179, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("CALL TOW REQUEST", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 96, 54, 180, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("LOCATION", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 102, 54, 181, (Conn.UniqueID), 2, false);

                        InSim.Send_BTN_CreateButton("COPS ONLINE", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 108, 54, 182, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("TOW TRUCKS ONLINE", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 114, 54, 183, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("THE TEAM", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 120, 54, 184, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("ABOUT", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 126, 54, 185, (Conn.UniqueID), 2, false);
                        InSim.Send_BTN_CreateButton("CLOSE", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 26, 134, 54, 185, (Conn.UniqueID), 2, false);

                        #endregion



                        #region ' Help Menu '

                        if (Conn.CompCar.Speed / 91 < 2 && Conn.InAdminMenu == false && Conn.DisplaysOpen == false && Conn.InModerationMenu == 0 && Conn.InFineMenu == false)
                        {
                            InSim.Send_BTN_CreateButton("Help Menu", Flags.ButtonStyles.ISB_C4 | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 5, 15, 20, 60, 25, Conn.UniqueID, 2, false);
                        }
                        else
                        {
                            DeleteBTN(25, Conn.UniqueID);
                        }

                        #endregion

                        */
                        #endregion
                    }
                }
            }
            catch { }
        }

        // Minute Timers
        private void MinuteTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                #region ' Auto Message '
                if (Connections.Count > 1)
                {
                    switch (Messages)
                    {
                        case 0:
                            MsgAll("^6>>^7 ^CDrive on the ^1RIGHT side ^7of the road!");
                            break;

                        case 1:
                            MsgAll("^6>>^7 Respect all players and admins!");
                            break;


                        case 2:
                            MsgAll("^6>>^7 Comming soon ours forum!");
                            /*MsgAll("^6>>^7 " + Website);*/
                            break;

                        case 3:
                            MsgAll("^6>>^7 If you have problems with your car type !calltow");
                            break;

                        case 4:

                            MsgAll("^6>>^7 Do not spam or use hacks!");

                            break;

                        case 5:

                            MsgAll("^6>>^7 Be careful! You are not alone on the road..");

                            break;

                        case 6:

                            MsgAll("^6>>^0 "+CruiseName+" System ^7is working on version ^6" + InSimVer);
                            Messages = 0;
                            break;
                    }
                    Messages += 1;
                }
                #endregion

                #region ' Connection '

                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' Last Raffle Timer '

                        if (Conn.LastRaffle == 0 == false)
                        {
                            if (Conn.LastRaffle > 0)
                            {
                                Conn.LastRaffle -= 1;
                            }
                            #region ' Display Countdown '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InStore == true)
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
                                else
                                {
                                    if (Conn.TotalSale > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Total Item bought: " + Conn.TotalSale, Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7You must buy a Item before you can Join the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Last Lotto Timer '

                        if (Conn.LastLotto == 0 == false)
                        {
                            if (Conn.LastLotto > 0)
                            {
                                Conn.LastLotto -= 1;
                            }
                            #region ' Display Countdown '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InSchool == true)
                            {
                                if (Conn.LastLotto > 120)
                                {
                                    InSim.Send_BTN_CreateButton("^7You have to wait ^1Three (3) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastLotto > 60)
                                {
                                    InSim.Send_BTN_CreateButton("^7You have to wait ^1Two (2) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastLotto > 0)
                                {
                                    if (Conn.LastLotto > 1)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^2Try ^7Lotto Costs: ^1$100", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Lotto", "Pick your number 1 - 10", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 2, 120, Conn.UniqueID, 2, false);
                                }
                            }
                            #endregion
                        }

                        #endregion
                    }
                }

                #endregion
            }
            catch { }
        }

        // Save Users 2.5 Minutes
        private void BackUp_Users_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                #region ' Save Users '
                if (Connections.Count > 1)
                {
                    foreach (clsConnection C in Connections)
                    {
                        if (C.FailCon == 0)
                        {
                            FileInfo.SaveUser(C);
                        }
                    }
                }
                #endregion
            }
            catch
            {
                #region ' Save Failed '
                if (Connections.Count > 1)
                {
                    MsgAll("^1Auto-Save failed.");
                }
                #endregion
            }
        }

        // OnScreen Effects 500ms (.50 milliseconds)
        private void OnScreen_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' OnScreen Effects '
                        if (Conn.OnScreenExit > 3)
                        {
                            if (OnScreen == 0)
                            {
                                InSim.Send_BTN_CreateButton("^3!!^1›› ^7Turn on your lights ^1<<^3!!", Flags.ButtonStyles.ISB_LEFT, 30, 130, 50, 40, 10, Conn.UniqueID, 2, false);
                            }
                        }
                        #endregion
                        
                        #region ' Clear OnScreen '

                        if (Conn.OnScreenExit == 0 == false)
                        {
                            if (Conn.OnScreenExit > 0)
                            {
                                Conn.OnScreenExit -= 1;
                            }
                            if (Conn.OnScreenExit == 0)
                            {
                                DeleteBTN(10, Conn.UniqueID);
                                Conn.LeavesPitLane = 0;
                            }
                        }

                        #endregion
                    }
                }

                #region ' Effects '
                if (OnScreen == 0)
                {
                    OnScreen = 1;
                }
                else
                {
                    OnScreen = 0;
                }
                #endregion
            }
            catch { }
        }

        // Second Timers & Tick Event 1000ms (1 Second)
        private void SecondTimers_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {
                    if ((Conn.PlayerName == HostName && Conn.UniqueID == 0) == false)
                    {
                        #region ' CompCar Variables '
                        var kmh = Conn.CompCar.Speed / 91;
                        #endregion

                        #region ' Job GPS '

                        if (Conn.Interface == 0 && Conn.WaitIntrfc < 8)
                        {
                            #region ' Job To House 1 '
                            if (Conn.JobToHouse1 == true)
                            {
                                if (Conn.InHouse1Dist < 150)
                                {
                                    if (Conn.InHouse1Dist < 20)
                                    {
                                        if (Stage == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                    }
                                }
                                else if (Conn.InHouse1Dist < 300)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0•^3•• ••^0• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else if (Conn.InHouse1Dist < 500)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••^1• •^0•• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse1Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                            }
                            #endregion

                            #region ' Job To House 2 '
                            if (Conn.JobToHouse2 == true)
                            {
                                if (Conn.InHouse2Dist < 150)
                                {
                                    if (Conn.InHouse2Dist < 20)
                                    {
                                        if (Stage == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                    }
                                }
                                else if (Conn.InHouse2Dist < 300)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0•^3•• ••^0• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else if (Conn.InHouse2Dist < 500)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••^1• •^0•• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse2Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                            }
                            #endregion

                            #region ' Job To House 3 '
                            if (Conn.JobToHouse3 == true)
                            {
                                if (Conn.InHouse3Dist < 150)
                                {
                                    if (Conn.InHouse3Dist < 20)
                                    {
                                        if (Stage == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                    }
                                }
                                else if (Conn.InHouse3Dist < 300)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0•^3•• ••^0• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else if (Conn.InHouse3Dist < 500)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••^1• •^0•• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InHouse3Dist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                            }
                            #endregion

                            #region ' Job To School '
                            if (Conn.JobToSchool == true)
                            {
                                if (Conn.InSchoolDist < 150)
                                {
                                    if (Conn.InSchoolDist < 20)
                                    {
                                        if (Stage == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7GPS: ^2••• ••• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                    }
                                }
                                else if (Conn.InSchoolDist < 300)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0•^3•• ••^0• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else if (Conn.InSchoolDist < 500)
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••^1• •^0•• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7GPS: ^0••• ••• ^7" + Conn.InSchoolDist + " m", Flags.ButtonStyles.ISB_DARK, 5, 40, 10, 80, 27, Conn.UniqueID, 2, true);
                                }
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Cop Panel '

                        if (Conn.IsOfficer == true || Conn.IsCadet == true)
                        {
                            if (Conn.InGame == 1)
                            {
                                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 30, 27, 121, 170, 15, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7 Police panel", Flags.ButtonStyles.ISB_LIGHT, 4, 25, 123, 171, 16, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7 Cop Version : ^21.0", Flags.ButtonStyles.ISB_LIGHT, 4, 25, 145, 171, 22, Conn.UniqueID, 2, false);

                                clsConnection ChaseCon = Connections[GetConnIdx(Conn.Chasee)];

                                if (Conn.InChaseProgress == false)
                                {
                                    bool DetectSpeeders = false;
                                    byte Speeders = 0;

                                    #region ' Remote Speeder PLID '
                                    for (byte o = 0; o < Connections.Count; o++)
                                    {
                                        if (Connections[o].PlayerID != 0)
                                        {
                                            Speeders = o;
                                        }

                                        #region ' Detected '
                                        if (Connections[Speeders].IsOfficer == false && Connections[Speeders].IsCadet == false)
                                        {
                                            int SpeederDist = ((int)Math.Sqrt(Math.Pow(Connections[Speeders].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[Speeders].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                                            if (SpeederDist < 50)
                                            {
                                                if (DetectSpeeders == false)
                                                {
                                                    #region ' Panel '
                                                    InSim.Send_BTN_CreateButton("^7Name: " + Connections[Speeders].PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                                    if (Connections[Speeders].IsSpeeder == 0)
                                                    {
                                                        if (Conn.KMHorMPH == 0)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^2" + Connections[Speeders].CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                        else if (Conn.KMHorMPH == 1)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^2" + Connections[Speeders].CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                    }
                                                    else if (Connections[Speeders].IsSpeeder == 1)
                                                    {
                                                        if (Conn.KMHorMPH == 0)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                        else if (Conn.KMHorMPH == 1)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                                        }
                                                    }
                                                    InSim.Send_BTN_CreateButton("^7Distance: ^3" + SpeederDist + " ^7meters", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);
                                                    #endregion

                                                    #region ' Speeder Clock '

                                                    if (Connections[Speeders].IsSpeeder == 1)
                                                    {
                                                        if (kmh > 3)
                                                        {
                                                            if (Conn.SpeederClocked == false)
                                                            {
                                                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " clocked " + Connections[Speeders].NoColPlyName);
                                                                if (Conn.KMHorMPH == 0)
                                                                {
                                                                    MsgAll("^6>>^7 Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 91 + " kmh");
                                                                }
                                                                else if (Conn.KMHorMPH == 1)
                                                                {
                                                                    MsgAll("^6>>^7 Car: (^3" + Connections[Speeders].CurrentCar + "^7) Speed: ^1" + Connections[Speeders].CompCar.Speed / 146 + " mph");
                                                                }
                                                                Conn.SpeederClocked = true;
                                                            }
                                                        }
                                                    }
                                                    else if (Connections[Speeders].IsSpeeder == 0)
                                                    {
                                                        if (Conn.SpeederClocked == true)
                                                        {
                                                            Conn.SpeederClocked = false;
                                                        }
                                                    }

                                                    #endregion

                                                    DetectSpeeders = true;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region ' Undetected '
                                    if (DetectSpeeders == false)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Name: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7Car: None Speed: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7Distance: None", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);

                                        if (Conn.SpeederClocked == true)
                                        {
                                            Conn.SpeederClocked = false;
                                        }
                                    }
                                    #endregion

                                    #region ' Enable Click '
                                    if (Conn.CopPanel == 1)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            InSim.Send_BTN_CreateButton("^7ENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            #region ' Officer/Cadet Speed Trap Option '
                                            if (Conn.IsOfficer == true)
                                            {
                                                if (Conn.TrapSetted == false)
                                                {
                                                    if (kmh < 2)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^7SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^7REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^7", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            #region ' Officer/Cadet Speed Trap Option '
                                            if (Conn.IsOfficer == true)
                                            {
                                                if (Conn.TrapSetted == false)
                                                {

                                                    if (kmh < 2)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                    else
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                            #endregion
                                        }
                                    }
                                    else if (Conn.CopPanel == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);

                                        #region ' Officer/Cadet Speed Trap Option '
                                        if (Conn.IsOfficer == true)
                                        {
                                            if (Conn.TrapSetted == false)
                                            {
                                                if (kmh < 2)
                                                {
                                                    InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8SPEED TRAP", "Set Speed Trap.", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 3, 21, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8REMOVE TRAP", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    int ChaseDist = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - Conn.CompCar.X, 2) + Math.Pow(ChaseCon.CompCar.Y - Conn.CompCar.Y, 2)) / 65536);

                                    #region ' Chase Condition '
                                    if (Conn.ChaseCondition == 1)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4Level 1", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 2)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4Level 2", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 3)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4Level 3", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 4)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4Level 4", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.ChaseCondition == 5)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Condition: ^4Level 5", Flags.ButtonStyles.ISB_DARK, 4, 25, 128, 171, 17, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    #region ' Speed Check '
                                    if (ChaseCon.IsSpeeder == 0)
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^2" + ChaseCon.CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                        else if (Conn.KMHorMPH == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^2" + ChaseCon.CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    else if (ChaseCon.IsSpeeder == 1)
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^1" + ChaseCon.CompCar.Speed / 91 + " kmh", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                        else if (Conn.KMHorMPH == 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^7Car: (^3" + ChaseCon.CurrentCar + "^7) Speed: ^1" + ChaseCon.CompCar.Speed / 146 + " mph", Flags.ButtonStyles.ISB_DARK, 4, 25, 132, 171, 18, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    #endregion

                                    InSim.Send_BTN_CreateButton("^7 Distantion: ^3" + ChaseDist + " ^7 metres", Flags.ButtonStyles.ISB_DARK, 4, 25, 136, 171, 19, Conn.UniqueID, 2, false);

                                    #region ' Timer Bump '
                                    if (Conn.CopPanel == 1)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            if (Conn.JoinedChase == false)
                                            {
                                                if (Conn.ChaseCondition == 5 == false)
                                                {
                                                    if (Conn.AutoBumpTimer == 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^7ENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
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
                                                        InSim.Send_BTN_CreateButton("^8" + Minutes + ":" + Seconds, Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            /*if (Conn.AutoBumpTimer == 0)
                                            {*/
                                            InSim.Send_BTN_CreateButton("^7BUSTED", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            /*}
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8BUSTED", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }*/
                                        }

                                        InSim.Send_BTN_CreateButton("^7DISENGAGE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);

                                    }
                                    else if (Conn.CopPanel == 0)
                                    {
                                        if (Conn.Busted == false)
                                        {
                                            if (Conn.JoinedChase == false)
                                            {
                                                if (Conn.ChaseCondition == 5 == false)
                                                {
                                                    if (Conn.AutoBumpTimer == 0)
                                                    {
                                                        InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
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
                                                        InSim.Send_BTN_CreateButton("^8" + Minutes + ":" + Seconds, Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                    }
                                                }
                                                else
                                                {
                                                    InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^8ENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^8BUSTED", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 171, 20, Conn.UniqueID, 2, false);
                                        }
                                        InSim.Send_BTN_CreateButton("^8DISENGAGE", Flags.ButtonStyles.ISB_LIGHT, 4, 12, 140, 184, 21, Conn.UniqueID, 2, false);
                                    }

                                    #endregion
                                }
                            }
                        }

                        #endregion

                        #region ' Timers '

                        #region ' Pull Over Message '
                        if (Conn.IsSuspect == true)
                        {
                            if (Conn.PullOvrMsg == 0 == false)
                            {
                                if (Conn.PullOvrMsg > 0)
                                {
                                    Conn.PullOvrMsg -= 1;
                                }
                                if (Conn.PullOvrMsg == 0)
                                {
                                    MsgAll(Conn.PlayerName + " ^4‹-- ^1Stop!");
                                    Conn.PullOvrMsg = 30;
                                }
                            }
                        }
                        #endregion

                        #region ' Clear Warning '

                        if (Conn.ModerationWarn == 0 == false)
                        {
                            if (Conn.ModerationWarn > 0)
                            {
                                Conn.ModerationWarn -= 1;
                            }

                            if (Conn.ModerationWarn == 0)
                            {
                                DeleteBTN(10, Conn.UniqueID);
                            }
                        }

                        #endregion

                        #region ' Busted Timer '

                        if (Conn.BustedTimer == 5 == false)
                        {
                            if (Conn.BustedTimer > 0)
                            {
                                Conn.BustedTimer += 1;
                            }
                        }

                        #endregion

                        #region ' Bump Timer '

                        if (Conn.AutoBumpTimer == 0 == false)
                        {
                            if (Conn.InChaseProgress == true && Conn.Busted == false)
                            {
                                Conn.AutoBumpTimer -= 1;
                            }
                        }

                        #endregion

                        #region ' Bank Bonus Timer '

                        if (Conn.BankBonusTimer == 0 == false)
                        {
                            if (Conn.BankBonusTimer > 0)
                            {
                                if (Conn.IsAFK == false)
                                {
                                    Conn.BankBonusTimer -= 1;
                                }
                            }
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true && Conn.InBank == true)
                            {
                                #region ' String Timer '
                                string Minutes = "0";
                                string Seconds = "0";

                                Minutes = "" + (Conn.BankBonusTimer / 60);
                                if ((Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60)) < 10)
                                {
                                    Seconds = "0" + (Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60));
                                }
                                else
                                {
                                    Seconds = "" + (Conn.BankBonusTimer - ((Conn.BankBonusTimer / 60) * 60));
                                }
                                #endregion
                                if (Conn.IsAFK == false)
                                {
                                    if (Conn.BankBalance > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^7 Time to bonus ^1" + Minutes + ":" + Seconds, Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 65, 115, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7 You don't have money on your acccount!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 65, 115, Conn.UniqueID, 2, false);
                                    }
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7 You're AFK and your bank bonus is stoped!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 65, 115, Conn.UniqueID, 2, false);
                                }
                            }
                            if (Conn.BankBonusTimer == 0)
                            {
                                if (Conn.BankBalance > 0)
                                {
                                    Conn.BankBonus += Conn.BankBalance * 1 / 100;
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " get ^2$" + Conn.BankBonus + " ^7from bank bonus!");
                                    Conn.BankBalance += Conn.BankBonus;
                                    Conn.BankBonusTimer = 3600;
                                }
                            }
                        }

                        #endregion

                        #region ' Check AFK Tick '

                        if (Conn.BankBalance > 0)
                        {
                            #region ' Tick '
                            if (kmh < 2)
                            {
                                if (Conn.AFKTick == 1800 == false)
                                {
                                    Conn.AFKTick += 1;
                                }
                            }
                            else
                            {
                                if (Conn.AFKTick > 0)
                                {
                                    Conn.AFKTick = 0;
                                }
                                if (Conn.IsAFK == true)
                                {
                                    Conn.IsAFK = false;
                                    DeleteBTN(118, Conn.UniqueID);
                                }
                            }
                            #endregion

                            #region ' Warn '
                            if (Conn.AFKTick == 1800)
                            {
                                if (Conn.IsAFK == false)
                                {
                                    InSim.Send_BTN_CreateButton("^1WaRnInG! You are AFK!", Flags.ButtonStyles.ISB_DARK, 10, 80, 20, 60, 118, Conn.UniqueID, 2, false);
                                    MsgPly("^6>>^7 WARNING! You are AFK", Conn.UniqueID);
                                    MsgPly("^6>>^7 and your bank bonus is restarted!", Conn.UniqueID);
                                    Conn.IsAFK = true;
                                }
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Clear Filters '

                        if (Conn.SwearTime == 0 == false)
                        {
                            if (Conn.SwearTime > 0)
                            {
                                Conn.SwearTime -= 1;
                            }
                            if (Conn.SwearTime == 0)
                            {
                                Conn.Swear = 0;
                            }
                        }

                        if (Conn.SpamTime == 0 == false)
                        {
                            if (Conn.SpamTime > 0)
                            {
                                Conn.SpamTime -= 1;
                            }
                            if (Conn.SpamTime == 0)
                            {
                                Conn.Spam = 0;
                            }
                        }

                        #endregion

                        #region ' Clear Penalty '
                        if (Conn.Penalty == 0 == false)
                        {
                            if (Conn.Penalty > 0)
                            {
                                Conn.Penalty -= 1;
                            }
                            if (Conn.Penalty == 0)
                            {
                                ClearPen(Conn.Username);
                            }
                        }
                        #endregion

                        #region ' Clear Command Buffer '

                        if (Conn.WaitCMD == 0 == false)
                        {
                            if (Conn.WaitCMD > 0)
                            {
                                Conn.WaitCMD -= 1;
                            }
                        }

                        #endregion

                        #region ' Load Button Buffer '

                        if (Conn.WaitIntrfc == 0 == false)
                        {
                            if (Conn.WaitIntrfc > 0)
                            {
                                Conn.WaitIntrfc -= 1;
                            }
                        }

                        #endregion

                        #endregion
                    }
                }

                #region ' Staging '
                if (Stage == 0)
                {
                    Stage = 1;
                }
                else
                {
                    Stage = 0;
                }
                #endregion

            }
            catch { }
        }

        // Second Timers & Tick Event 1000ms (1 Second)
        private void SnowTimers_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection Conn in Connections)
                {

                }

            }
            catch { }
        }

        // Check User 1000ms (1 Second)
        private void CheckUser_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if ((C.PlayerName == HostName && C.UniqueID == 0) == false)
                    {
                        #region ' CompCar Detailzzz '

                        var kmh = C.CompCar.Speed / 91;
                        var mph = C.CompCar.Speed / 146;
                        var direction = C.CompCar.Direction / 180;
                        var node = C.CompCar.Node;
                        var pathx = C.CompCar.X / 196608;
                        var pathy = C.CompCar.Y / 196608;
                        var pathz = C.CompCar.Z / 98304;
                        var angle = C.CompCar.AngVel / 30;

                        #endregion

                        #region ' Cash Payout '

                        if (kmh > 230)
                        {
                            C.Cash += 3;
                        }
                        else if (kmh > 170)
                        {
                            C.Cash += 2;
                        }
                        else if (kmh > 90)
                        {
                            C.Cash += 1;
                        }

                        #endregion

                        #region ' Health Distance '

                        if (C.HealthDist > 1500)
                        {
                            C.TotalHealth -= 1;
                            C.HealthDist = 0;
                        }

                        if (C.TotalHealth < 15)
                        {
                            if (C.HealthWarn == 0)
                            {
                                MsgPly("^6>>^7 Warning: Your health is reached ^6" + C.TotalHealth + "%", C.UniqueID);
                                MsgPly("^6>>^7 May cause specting because of doctors checkup.", C.UniqueID);
                                C.HealthWarn = 1;
                            }
                        }

                        if (C.TotalHealth == 0)
                        {
                            int RandomFines = new Random().Next(500, 800);
                            MsgAll("^6>>^7 " + C.PlayerName + " ^2was charged ^1$" + RandomFines + " ^2from doctors bill.");
                            MsgPly("^6>>^7 You are spected because of your Health.", C.UniqueID);

                            C.Cash -= RandomFines;
                            C.TotalHealth = 100;
                            SpecID(C.PlayerName);
                            SpecID(C.Username);
                            C.HealthWarn = 0;
                        }

                        #endregion

                        #region ' PlayerHost Check '

                        if (C.UniqueID == 0 && C.PlayerName == HostName)
                        {
                            C.Cash = 3500;
                            C.BankBalance = 0;
                            C.Cars = "UF1 XFG XRG";
                            C.TotalHealth = 100;
                            C.TotalDistance = 0;
                            C.FailCon = 1;
                        }

                        #endregion

                        #region ' InSim Interface '
                        if (C.Interface == 0)
                        {
                            if (C.WaitIntrfc < 8)
                            {
                                #region ' Cash Inteface '
                                if (C.Cash > -1)
                                {
                                    if (kmh > 160)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^2$" + string.Format("{0:n0}", C.Cash) + " ^7+3", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else if (kmh > 90)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^2$" + string.Format("{0:n0}", C.Cash) + " ^7+2", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else if (kmh > 30)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^2$" + string.Format("{0:n0}", C.Cash) + " ^7+1", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^2$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                }
                                else
                                {
                                    if (kmh > 160)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^1$" + string.Format("{0:n0}", C.Cash) + " ^7+3", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else if (kmh > 90)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^1$" + string.Format("{0:n0}", C.Cash) + " ^7+2", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else if (kmh > 30)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^1$" + string.Format("{0:n0}", C.Cash) + " ^7+1", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7Cash: ^1$" + string.Format("{0:n0}", C.Cash), Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 62, 2, C.UniqueID, 2, true);
                                    }
                                }
                                #endregion

                                InSim.Send_BTN_CreateButton("^7Energy: ^6" + C.TotalHealth + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 0, 92, 4, C.UniqueID, 2, true);

                                #region ' Website/ChaseCon/Towed '
                                if (C.InTowProgress == false && C.InChaseProgress == false)
                                {
                                    if (C.IsSuspect == false && C.IsBeingTowed == false && C.IsBeingBusted == false)
                                    {
                                        if (C.JobToHouse1 == false && C.JobToHouse2 == false && C.JobToHouse3 == false && C.JobToSchool == false)
                                        {
                                            InSim.Send_BTN_CreateButton("^7" + Website, Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, true);
                                        }
                                    }
                                    else
                                    {
                                        if (C.IsSuspect == true)
                                        {
                                            if (C.ChaseCondition == 1)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 2)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 3)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 4)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^7^J™", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                            }
                                            else if (C.ChaseCondition == 5)
                                            {
                                                InSim.Send_BTN_CreateButton("^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï" + "^4^S¡ï", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                            }
                                        }
                                        else if (C.IsBeingBusted == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7You are Busted!", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                        }
                                        if (C.IsBeingTowed == true)
                                        {
                                            InSim.Send_BTN_CreateButton("^7You are being Towed!", Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, false);
                                        }
                                    }
                                }
                                else
                                {
                                    if (C.InTowProgress == true)
                                    {
                                        clsConnection TowCon = Connections[GetConnIdx(C.Towee)];
                                        InSim.Send_BTN_CreateButton("^7Tow: " + TowCon.PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, true);
                                    }
                                    if (C.InChaseProgress == true)
                                    {
                                        clsConnection ChaseCon = Connections[GetConnIdx(C.Chasee)];
                                        InSim.Send_BTN_CreateButton("^7Chase: " + ChaseCon.PlayerName, Flags.ButtonStyles.ISB_DARK, 4, 30, 0, 107, 6, C.UniqueID, 2, true);
                                    }
                                }
                                #endregion
                            }
                            if (C.WaitIntrfc < 4)
                            {
                                #region ' Distance Meter '

                                
                                        InSim.Send_BTN_CreateButton("^7Total: " + C.TotalDistance / 1000 + " ^J‡q", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 62, 3, C.UniqueID, 2, true);
                                   
                              

                                #endregion

                                #region ' Bonus Meter '

                                if (C.TotalBonusDone == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 400 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 1)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 600 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 2)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 800 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 3)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 1200 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 4)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 1600 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 5)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 2000 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 6)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 2600 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 7)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 3200 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 8)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 4000 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 9)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 5400 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }
                                else if (C.TotalBonusDone == 10)
                                {
                                    InSim.Send_BTN_CreateButton("^7Bonus: ^3" + C.BonusDistance / 6600 + "%", Flags.ButtonStyles.ISB_DARK, 4, 15, 4, 92, 5, C.UniqueID, 2, true);
                                }

                                #endregion

                                #region ' Job Identifier '

                                if (C.IsOfficer == false && C.IsCadet == false && C.IsTowTruck == false)
                                {
                                    if (C.IsSuspect == false)
                                    {
                                        if (C.JobToHouse1 == false && C.JobToHouse2 == false && C.JobToHouse3 == false && C.JobToSchool == false)
                                        {
                                            InSim.Send_BTN_CreateButton(CruiseName, Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                        }
                                        else
                                        {
                                            switch (TrackName)
                                            {
                                                #region ' Blackwood '
                                                case "WE1X":
                                                case "BL1X":
                                                case "RO1X":
                                                case "KY2X":

                                                    #region ' Job From House 1 '
                                                    if (C.JobFromHouse1 == true)
                                                    {
                                                        if (C.JobToSchool == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Escort to Lottery!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                    }
                                                    #endregion

                                                    #region ' Job From House 2 '
                                                    if (C.JobFromHouse2 == true)
                                                    {
                                                        if (C.JobToSchool == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Escort to Lottery!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }

                                                    }
                                                    #endregion

                                                    #region ' Job From House 3 '
                                                    if (C.JobFromHouse3 == true)
                                                    {
                                                        if (C.JobToSchool == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Escort to Lottery!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }

                                                    }
                                                    #endregion

                                                    #region ' Job From Shop '
                                                    if (C.JobFromShop == true)
                                                    {
                                                        if (C.JobToHouse1 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Kou-chan", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                        if (C.JobToHouse2 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Johnson's", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                        if (C.JobToHouse3 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Shanen's", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                    }
                                                    #endregion

                                                    #region ' Job From Store '
                                                    if (C.JobFromStore == true)
                                                    {
                                                        if (C.JobToHouse1 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Kou-chan", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                        if (C.JobToHouse2 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Johnson", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                        if (C.JobToHouse3 == true)
                                                        {
                                                            InSim.Send_BTN_CreateButton("^7Delivery to Shanen!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                                        }
                                                    }
                                                    #endregion

                                                    break;
                                                #endregion
                                            }
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7Escape the Police!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                    }
                                }
                                else
                                {
                                    if (C.IsOfficer == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Hello Officer!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                    }
                                    if (C.IsCadet == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Hello Cadet!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                    }
                                    if (C.IsTowTruck == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Hello Tow Truck!", Flags.ButtonStyles.ISB_DARK, 4, 30, 4, 107, 7, C.UniqueID, 2, true);
                                    }
                                }

                                #endregion
                            }
                            if (C.WaitIntrfc < 2)
                            {
                                #region ' Car List '

                                InSim.Send_BTN_CreateButton("^7Car:^2 " + C.CurrentCar + "  ^7| " + C.LocationBox+" ^2("+C.SpeedBox+"^2)  ^7|  Driver Level: ^2"+C.Furniture, Flags.ButtonStyles.ISB_DARK, 4, 75, 8, 62, 14, C.UniqueID, 2, true);
                                #endregion
                            }
                        }
                        /*else
                        {
                            InSim.Send_BTN_CreateButton(CruiseName, Flags.ButtonStyles.ISB_C4, 10, 50, 190, 0, 1, C.UniqueID, 2, false);
                        }*/
                        #endregion

                        #region ' Distance Bonus '

                        if (C.BonusDistance > 50000 && C.TotalBonusDone == 0)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$2500 ^7bonus distance!");
                            C.Cash += 2500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 50000 && C.TotalBonusDone == 1)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$4500 ^7bonus distance!");
                            C.Cash += 4500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 100000 && C.TotalBonusDone == 2)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$8000 ^7bonus distance!");
                            C.Cash += 8000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 150000 && C.TotalBonusDone == 3)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$12500 ^7bonus distance!");
                            C.Cash += 12500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 300000 && C.TotalBonusDone == 4)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$18500 ^7bonus distance!");
                            C.Cash += 18500;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 350000 && C.TotalBonusDone == 5)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$25000 ^7bonus distance!");
                            C.Cash += 25000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 400000 && C.TotalBonusDone == 6)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$34000 ^7bonus distance!");
                            C.Cash += 34000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 450000 && C.TotalBonusDone == 7)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$38000 ^7bonus distance!");
                            C.Cash += 38000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 500000 && C.TotalBonusDone == 8)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$44000 ^7bonus distance!");
                            C.Cash += 44000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 550000 && C.TotalBonusDone == 9)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$58000 ^7bonus distance!");
                            C.Cash += 58000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 600000 && C.TotalBonusDone == 10)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$82000 ^7bonus distance!");
                            C.Cash += 82000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 650000 && C.TotalBonusDone == 11)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$100000 ^7bonus distance!");
                            C.Cash += 100000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 700000 && C.TotalBonusDone == 12)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$120000 ^7bonus distance!");
                            C.Cash += 120000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 750000 && C.TotalBonusDone == 13)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$150000 ^7bonus distance!");
                            C.Cash += 150000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 800000 && C.TotalBonusDone == 14)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$170000 ^7bonus distance!");
                            C.Cash += 170000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone += 1;
                        }
                        else if (C.BonusDistance > 850000 && C.TotalBonusDone == 15)
                        {
                            MsgAll("^6>>^7 " + C.NoColPlyName + " earned ^2$187000 ^7bonus distance!");
                            MsgPly("^6>>^7 Your distance bonus is now reseted to Lvl. 1", C.UniqueID);
                            C.Cash += 187000;
                            C.BonusDistance = 0;
                            C.TotalBonusDone -= 14;
                        }
                        #endregion

                        #region ' help menu '
                     /*}
                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 100, 120, 60, 40, 101, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Long ^2Cruise ^1City ^7commands:", Flags.ButtonStyles.ISB_LIGHT, 10, 118, 61, 41, 102, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 60, 70, 80, 65, 103, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7placehelp", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 75, 42, 104, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7playerhelp", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 85, 42, 105, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Show", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 95, 42, 106, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Location", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 105, 42, 107, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Pitlane", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 115, 42, 108, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Online Cops", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 125, 42, 109, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Online Towtrucks", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 135, 42, 110, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Online VIPS", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 145, 42, 111, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Online admins", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 75, 137, 112, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Call tow", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 85, 137, 113, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7Km for cars", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 95, 137, 114, C.UniqueID, 2, false);
                    InSim.Send_BTN_CreateButton("^7About", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 10, 20, 105, 137, 115, C.UniqueID, 2, false);
                    {*/
                         #endregion

                        
                        
                    }
                }
            }
            catch { }
        }

        #endregion
    }
}