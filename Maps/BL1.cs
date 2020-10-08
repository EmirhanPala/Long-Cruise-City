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
        // MCI Update
        private void MCI_BlackwoodGP(int PLID)
        {
            try
            {
                /*
                 if only the Player completes to connect via Try respond..........tooking 500 millisecond of internet speed
                 * 
                 * 
                */


                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[IDX].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[IDX].Towee)];
                #endregion

                #region ' CompCar Detailzzz '

                var kmh = Conn.CompCar.Speed / 91;
                var mph = Conn.CompCar.Speed / 146;
                var direction = Conn.CompCar.Direction / 180;
                var node = Conn.CompCar.Node;
                var pathx = Conn.CompCar.X / 196608;
                var pathy = Conn.CompCar.Y / 196608;
                var pathz = Conn.CompCar.Z / 98304;
                var angle = Conn.CompCar.AngVel / 30;
                string Car = Conn.CurrentCar;
                string anglenew = "";

                anglenew = angle.ToString().Replace("-", "");

                #endregion

                #region ' Wrong Exit fines '

                if (Conn.IsBeingTowed == false && Conn.InTowProgress == false && Conn.IsSuspect == false && Conn.InChaseProgress == false)
                {
                    // Pit EXIT
                    if (pathx >= -27 && pathx <= -24 && pathy >= 65 && pathy <= 67)
                    {
                        if (Conn.ExitZone == 0)
                        {
                            if (direction > 330 || direction < 20)
                            {
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " was fined ^1$500 ^7for wrong exit");
                                Conn.Cash -= 500;
                                Conn.ExitZone = 1;
                            }
                        }
                    }
                    else
                    {
                        if (Conn.ExitZone == 1)
                        {
                            Conn.ExitZone = 0;
                        }
                    }

                    // Pit ENTRY
                    if (pathx >= -23 && pathx <= -21 && pathy >= -39 && pathy <= -35)
                    {
                        if (Conn.ExitZone == 0)
                        {
                            if (direction < 100)
                            {
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " was fined ^1$500 ^7for wrong entry");
                                Conn.Cash -= 500;
                                Conn.ExitZone = 2;
                            }
                        }
                    }
                    else
                    {
                        if (Conn.ExitZone == 2)
                        {
                            Conn.ExitZone = 0;
                        }
                    }
                }
                #endregion

                #region ' Leaves Pit Interface '

                if (pathx >= -35 && pathx <= -33 && pathy >= -14 && pathy <= 38 && Conn.LeavesPitLane == 1)
                {
                    if (Conn.OnScreenExit == 0)
                    {
                        Conn.OnScreenExit = 8;
                    }
                }
                else
                {
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                }

                #endregion

                #region ' Complete Tow Truck '

                if (pathx > -33 && pathx < -30 && pathy > -8 && pathy < 39 && kmh < 3 && Conn.IsBeingTowed == true)
                {
                    foreach (clsConnection Con in Connections)
                    {
                        if (Con.Towee == Conn.UniqueID)
                        {
                            MsgAll("^4|^7 " + Con.NoColPlyName + " completes the Tow Request!");
                            MsgAll("^4|^7 " + Con.NoColPlyName + " was rewarded ^2$300");
                            MsgAll("^4|^7 " + Conn.NoColPlyName + " is now in pitlane safely!");
                            Con.Cash += 300;
                            Con.InTowProgress = false;
                            Con.Towee = -1;
                        }
                    }
                    CautionSirenShutOff();
                    Conn.IsBeingTowed = false;
                }

                #endregion

                #region ' Location and Speed Limit '

                #region ' Service Station & Autobahn & Kings Hillway & City Bank '

                if (node > 260)
                {
                    #region ' PitZone '
                    if (pathx >= -50 && pathx <= -27 && pathy >= -18 && pathy <= 53)
                    {
                        if (pathx >= -35 && pathx <= -32 && pathy >= -14 && pathy <= -12)
                        {
                            #region ' Speedlimit 50kmh/31mph '
                            if (kmh > 50)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                                }
                                Conn.IsSpeeder = 1;
                            }
                            else
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                                }

                                Conn.IsSpeeder = 0;
                            }
                            #endregion

                            Conn.Location = "City Bank";
                            Conn.LocationBox = "^3City Bank";
                            Conn.LastSeen = "City Bank, Blackwood";
                        }
                        else
                        {
                            #region ' Speedlimit 80kmh/50mph '

                            if (kmh > 80)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^1" + mph + " mph / 50 mph";
                                }
                                Conn.IsSpeeder = 1;
                            }
                            else
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^2" + mph + " mph / 50 mph";
                                }

                                Conn.IsSpeeder = 0;
                            }
                            #endregion

                            Conn.LocationBox = "^2Service Station";
                            Conn.Location = "Service Station";
                            Conn.LastSeen = "Service Station, Blackwood";
                        }
                    }
                    #endregion

                    #region ' Autobahn & Kings Hillway '
                    else
                    {
                        #region ' Kings Hillway '
                        if (node < 287)
                        {
                            #region ' Speedlimit 80kmh/50mph '

                            if (kmh > 80)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^1" + mph + " mph / 50 mph";
                                }
                                Conn.IsSpeeder = 1;
                            }
                            else
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^2" + mph + " mph / 50 mph";
                                }

                                Conn.IsSpeeder = 0;
                            }
                            #endregion

                            #region ' Ahead Speedlimit Reversed '
                            if (node > 265 && node < 269 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4) // Reversed
                            {
                                if (direction > 100 && direction < 200)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^1Junction Rd. Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                    Conn.StreetSign = 4;
                                }
                            }
                            #endregion

                            #region ' Ahead Speedlimit Forward '
                            if (node > 276 && node < 280 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                            {
                                if (direction < 120)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^7140^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^787^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^1Autobahn Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                    Conn.StreetSign = 4;
                                }
                            }
                            #endregion

                            Conn.LocationBox = "^7Kings Hillway";
                            Conn.Location = "Kings Hillway";
                            Conn.LastSeen = "Kings Hillway, Blackwood";
                        }
                        #endregion

                        #region ' Autobahn '
                        else
                        {
                            #region ' Speedlimit 140kmh/87mph '

                            if (kmh > 140)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 140 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^1" + mph + " mph / 87 mph";
                                }
                                Conn.IsSpeeder = 1;
                            }
                            else
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    Conn.SpeedBox = "^2" + kmh + " kmh / 140 kmh";
                                }
                                else
                                {
                                    Conn.SpeedBox = "^2" + mph + " mph / 87 mph";
                                }

                                Conn.IsSpeeder = 0;
                            }
                            #endregion

                            #region ' Ahead Speedlimit Reversed '
                            if (node > 291 && node < 295 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4) // Reversed
                            {
                                if (direction > 160 && direction < 220)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^1Kings' Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                    Conn.StreetSign = 4;
                                }
                            }
                            #endregion

                            #region ' Ahead Speedlimit Forward '
                            if (pathx >= -2 && pathx < 7 && pathy >= 113 && pathy <= 118 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                            {
                                if (direction > 320 || direction < 20)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^1Top End Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                    Conn.StreetSign = 4;
                                }
                            }
                            #endregion

                            Conn.LocationBox = "^7Autobahn";
                            Conn.Location = "Autobahn";
                            Conn.LastSeen = "Autobahn, Blackwood";
                        }
                        #endregion
                    }
                    #endregion
                }

                #endregion

                #region ' Top End Corner & Kou's House '
                if (node < 38)
                {
                    if (pathx >= 8 && pathx <= 14 && pathy >= 132 && pathy <= 136)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3Kou's House";
                        Conn.Location = "Kou's House";
                        Conn.LastSeen = "Kou's House, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 97kmh/60mph '
                        if (kmh > 97)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + kmh + " mph / 60 mph";
                            }

                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + kmh + " mph / 60 mph";
                            }
                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (pathx >= 4 && pathx < 12 && pathy >= 158 && pathy <= 169 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 160 && direction < 230)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^7140^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^787^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Autobahn Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 30 && node < 34 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 180 && direction < 240)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Drifters Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7Top End Corner";
                        Conn.Location = "Top End Corner";
                        Conn.LastSeen = "Top End Corner, Blackwood";
                    }
                }
                #endregion

                #region ' Drifters Corner '
                if (Conn.CompCar.Node > 37 && Conn.CompCar.Node < 61)
                {
                    #region ' Speedlimit 80kmh/50mph '

                    if (kmh > 80)
                    {
                        if (Conn.KMHorMPH == 0)
                        {
                            Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                        }
                        else
                        {
                            Conn.SpeedBox = "^1" + mph + " mph / 50 mph";
                        }
                        Conn.IsSpeeder = 1;
                    }
                    else
                    {
                        if (Conn.KMHorMPH == 0)
                        {
                            Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                        }
                        else
                        {
                            Conn.SpeedBox = "^2" + mph + " mph / 50 mph";
                        }

                        Conn.IsSpeeder = 0;
                    }
                    #endregion

                    #region ' Ahead Speedlimit Reversed '
                    if (node > 40 && node < 44 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                    {
                        if (direction > 300 || direction < 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            InSim.Send_BTN_CreateButton("^1Top End Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                            Conn.StreetSign = 4;
                        }
                    }
                    #endregion

                    #region ' Ahead Speedlimit Forward '
                    if (node > 50 && node < 54 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                    {
                        if (direction > 190 && direction < 260)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                InSim.Send_BTN_CreateButton("^1(^7140^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^1(^787^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            InSim.Send_BTN_CreateButton("^1Hell's H-way Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                            Conn.StreetSign = 4;
                        }
                    }
                    #endregion

                    Conn.LocationBox = "^7Drifter's Corner";
                    Conn.Location = "Drifter's Corner";
                    Conn.LastSeen = "Drifter's Corner, Blackwood";
                }
                #endregion

                #region ' Hell's Highway & Johnson's House '
                if (node > 60 && node < 134)
                {
                    if (pathx >= 60 && pathx <= 64 && pathy >= 74 && pathy <= 79)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3Johnson's House";
                        Conn.Location = "Johnson's House";
                        Conn.LastSeen = "Johnson's House, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 140kmh/87mph '

                        if (kmh > 140)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 140 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 87 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 140 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 87 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (node > 65 && node < 71 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 350 || direction < 70)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Drifters Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 126 && node < 130 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 160 && direction < 220)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Hook's Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7Hell's Highway";
                        Conn.Location = "Hell's Highway";
                        Conn.LastSeen = "Hell's Highway, Blackwood";
                    }
                }
                #endregion

                #region ' The Hook's Corner & KinderGarten '
                if (node > 133 && node < 167)
                {
                    if (pathx >= 134 && pathx <= 138 && pathy >= -231 && pathy <= -228)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3KinderGarten";
                        Conn.Location = "KinderGarten";
                        Conn.LastSeen = "KinderGarten, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 97kmh/60mph '
                        if (kmh > 97)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + kmh + " mph / 60 mph";
                            }

                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + kmh + " mph / 60 mph";
                            }
                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (node > 133 && node < 137 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 350 || direction < 70)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^7140^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^787^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Hell's Highway Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 156 && node < 160 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 70 && direction < 170)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Hammerhead Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7The Hook's Corner";
                        Conn.Location = "The Hook's Corner";
                        Conn.LastSeen = "The Hook's Corner, Blackwood";
                    }
                }
                #endregion

                #region ' Hammerhead Way & Food Shop '
                if (node > 167 && node < 179)
                {
                    if (pathx >= 105 && pathx <= 110 && pathy >= -255 && pathy <= -253)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3Food Shop";
                        Conn.Location = "Food Shop";
                        Conn.LastSeen = "Food Shop, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 97kmh/60mph '
                        if (kmh > 97)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + kmh + " mph / 60 mph";
                            }

                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 97 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + kmh + " mph / 60 mph";
                            }
                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (node > 170 && node < 176 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 230 && direction < 290)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Hook's Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 176 && node < 180 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 20 && direction < 120)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Chain Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7Hammerhead Way";
                        Conn.Location = "Hammerhead Way";
                        Conn.LastSeen = "Hammerhead Way, Blackwood";
                    }
                }
                #endregion

                #region ' Chain Corner & Mick's Store '
                if (node > 178 && node < 205)
                {
                    if (pathx >= 43 && pathx <= 48 && pathy >= -248 && pathy <= -243)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3Mick's Store";
                        Conn.Location = "Mick's Store";
                        Conn.LastSeen = "Mick's Store, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 80kmh/50mph '

                        if (kmh > 80)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 50 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 50 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (node > 186 && node < 190 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 180 && direction < 280)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Hammerhead Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 194 && node < 198 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 330 || direction < 30)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1The Suburb Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7Chain Corner";
                        Conn.Location = "Chain Corner";
                        Conn.LastSeen = "Chain Corner, Blackwood";
                    }
                }
                #endregion

                #region ' Blackwood Suburb '
                if (Conn.CompCar.Node > 204 && Conn.CompCar.Node < 232)
                {
                    #region ' Speedlimit 97kmh/60mph '
                    if (kmh > 97)
                    {
                        if (Conn.KMHorMPH == 0)
                        {
                            Conn.SpeedBox = "^1" + kmh + " kmh / 97 kmh";
                        }
                        else
                        {
                            Conn.SpeedBox = "^1" + kmh + " mph / 60 mph";
                        }

                        Conn.IsSpeeder = 1;
                    }
                    else
                    {
                        if (Conn.KMHorMPH == 0)
                        {
                            Conn.SpeedBox = "^2" + kmh + " kmh / 97 kmh";
                        }
                        else
                        {
                            Conn.SpeedBox = "^2" + kmh + " mph / 60 mph";
                        }
                        Conn.IsSpeeder = 0;
                    }
                    #endregion

                    #region ' Ahead Speedlimit Reversed '
                    if (node > 205 && node < 209 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4) // Reversed
                    {
                        if (direction > 200 && direction < 270)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            InSim.Send_BTN_CreateButton("^1Chain Corner Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                            Conn.StreetSign = 4;
                        }
                    }
                    #endregion

                    #region ' Ahead Speedlimit Forward '
                    if (node > 225 && node < 229 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                    {
                        if (direction > 20 && direction < 120)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                            }
                            InSim.Send_BTN_CreateButton("^1Junction Rd. Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                            Conn.StreetSign = 4;
                        }
                    }
                    #endregion

                    Conn.LocationBox = "^7Blackwood Suburb";
                    Conn.Location = "Blackwood Suburb";
                    Conn.LastSeen = "Blackwood Suburb, Blackwood";
                }
                #endregion

                #region ' Junction Road & Shanen's House '
                if (node > 231 && node < 260)
                {
                    if (pathx >= -49 && pathx <= -47 && pathy >= -134 && pathy <= -129)
                    {
                        #region ' Speedlimit 50kmh/31mph '
                        if (kmh > 50)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 31 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 31 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        Conn.LocationBox = "^3Shanen's House";
                        Conn.Location = "Shanen's House";
                        Conn.LastSeen = "Shanen's House, Blackwood";
                    }
                    else
                    {
                        #region ' Speedlimit 80kmh/50mph '

                        if (kmh > 80)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / 50 mph";
                            }
                            Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / 50 mph";
                            }

                            Conn.IsSpeeder = 0;
                        }
                        #endregion

                        #region ' Ahead Speedlimit Reversed '
                        if (node > 250 && node < 254 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 100 && direction < 170)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^797^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^760^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1The Suburb Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        #region ' Ahead Speedlimit Forward '
                        if (node > 256 && node < 260 && Conn.StreetSign == 0 && Conn.CompCar.Speed / 91 > 4)
                        {
                            if (direction > 290 && direction < 330)
                            {
                                if (Conn.KMHorMPH == 0)
                                {
                                    InSim.Send_BTN_CreateButton("^1(^780^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7KMH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^1(^750^1)", Flags.ButtonStyles.ISB_LIGHT, 14, 15, 4, 143, 11, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7MPH", Flags.ButtonStyles.ISB_C1, 4, 11, 14, 145, 12, Conn.UniqueID, 2, false);
                                }
                                InSim.Send_BTN_CreateButton("^1Kings' Speed Limit", Flags.ButtonStyles.ISB_LIGHT, 5, 30, 18, 136, 13, Conn.UniqueID, 2, false);
                                Conn.StreetSign = 4;
                            }
                        }
                        #endregion

                        Conn.LocationBox = "^7Junction Road";
                        Conn.Location = "Junction Road";
                        Conn.LastSeen = "Junction Road, Blackwood";
                    }
                }
                #endregion

                #endregion

                #region ' Houses and Establishments '

                #region ' Establishment '

                #region ' City Bank '
                Conn.InBankDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-34 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-12 * 196608), 2)) / 65536);
                if (Conn.InBankDist <= 4 && (mph <= 4))
                {
                    #region ' Open Commands and Displays '
                    if (Conn.InBank == false)
                    {
                        string EstablishmentName = "Blackwood ^6City ^5Bank";
                        MsgPly("^4|^7 Welcome to the " + EstablishmentName, Conn.UniqueID);

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

                        if (Conn.DisplaysOpen == false && Conn.InGameIntrfc == 0)
                        {
                            #region ' Display Window '
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName, Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^7Your bank balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), Flags.ButtonStyles.ISB_LEFT, 4, 40, 65, 54, 114, Conn.UniqueID, 2, false);
                            if (Conn.BankBalance > 0)
                            {
                                InSim.Send_BTN_CreateButton("^7Time until Bonus ^1" + Minutes + ":" + Seconds + " ^7left", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^7You don't have any Bank Balance on Account!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);
                            }
                            InSim.Send_BTN_CreateButton("^1Insert", "Enter amount to Insert", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 54, 54, 116, Conn.UniqueID, 40, false);
                            InSim.Send_BTN_CreateButton("^1Withdraw", "Enter amount to Withdraw", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 64, 64, 117, Conn.UniqueID, 2, false);

                            Conn.DisplaysOpen = true;
                            #endregion
                        }
                        else
                        {
                            #region ' Command '
                            MsgPly("^2!bank ^7- To see your bank balance.", Conn.UniqueID);
                            MsgPly("^2!check ^7- To see your bank bonus time left.", Conn.UniqueID);
                            MsgPly("^2!insert [amount] ^7- To deposit a cash to your account.", Conn.UniqueID);
                            MsgPly("^2!withdraw [amount] ^7- To withdraw a cash from your account.", Conn.UniqueID);
                            //MsgPly("^2!rob - Rob the City Bank!", Conn.UniqueID);
                            #endregion
                        }

                        Conn.InBank = true;
                    }
                    #endregion
                }
                else
                {
                    #region ' Close Displays '
                    if (Conn.InBank == true)
                    {
                        #region ' Close Display '
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                        {
                            DeleteBTN(110, Conn.UniqueID);
                            DeleteBTN(111, Conn.UniqueID);
                            DeleteBTN(112, Conn.UniqueID);
                            DeleteBTN(113, Conn.UniqueID);
                            DeleteBTN(114, Conn.UniqueID);
                            DeleteBTN(115, Conn.UniqueID);
                            DeleteBTN(116, Conn.UniqueID);
                            DeleteBTN(117, Conn.UniqueID);
                            Conn.DisplaysOpen = false;
                        }
                        #endregion

                        Conn.InBank = false;
                    }
                    #endregion
                }
                #endregion

                #region ' Mick's Store '

                Conn.InStoreDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (44 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-247 * 196608), 2)) / 65536);
                if (Conn.InStoreDist <= 4 && (mph <= 4))
                {
                    #region ' Open Display or Commands '

                    if (Conn.InStore == false)
                    {
                        string EstablishmentName = "^2M^1i^2c^1k^2'^1s ^2S^1t^2o^1r^2e";
                        MsgPly("^4|^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                        {
                            #region ' Display '

                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "^7!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^2Buy a Electronics: ^1$190 ^7per each", Flags.ButtonStyles.ISB_LEFT, 4, 40, 65, 54, 114, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy a Furniture: ^1$150 ^7per each", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 54, 115, Conn.UniqueID, 2, false);

                            #region ' RaffleZ '
                            if (Conn.LastRaffle == 0)
                            {
                                if (Conn.TotalSale > 0)
                                {
                                    InSim.Send_BTN_CreateButton("^7Total Item bought: ^2(" + Conn.TotalSale + ")^7 Raffle for ^1$300", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                }
                                else
                                {
                                    InSim.Send_BTN_CreateButton("^7You must ^7buy a Item before you can Join the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                            }
                            else
                            {
                                if (Conn.LastRaffle > 120)
                                {
                                    InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Three (3)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else if (Conn.LastRaffle > 60)
                                {
                                    InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1Two (2)hours ^7to Rejoin the Raffle!", Flags.ButtonStyles.ISB_LEFT, 4, 130, 73, 54, 116, Conn.UniqueID, 2, false);
                                }
                                else
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
                            }

                            #endregion

                            #region ' Click teh Job '

                            if (Conn.InGame == 0)
                            {
                                InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                            {
                                InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                            {
                                InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                            {
                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                            {
                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            #endregion

                            InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Electronics, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 2, 118, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Furniture, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 2, 119, Conn.UniqueID, 2, false);


                            Conn.DisplaysOpen = true;

                            #endregion
                        }
                        else
                        {
                            #region ' Command '

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

                            #endregion
                        }

                        Conn.InStore = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '

                    if (Conn.InStore == true)
                    {
                        #region ' Close Display '
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
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
                        #endregion

                        Conn.InStore = false;
                    }
                    #endregion
                }

                #endregion

                #region ' Food Shop '

                Conn.InShopDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (108 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-254 * 196608), 2)) / 65536);
                if (Conn.InShopDist <= 4 && (mph <= 4))
                {
                    #region ' Command and Display '

                    if (Conn.InShop == false)
                    {

                        string EstablishmentName = "^4F^6o^4o^6d ^3S^2h^3o^2p";
                        MsgPly("^4|^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                        {
                            #region ' Display '
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "^7!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^2Buy a ^7Fried Chicken Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy a ^7Beer Costs: ^1$10 ^7Health: ^27%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy some ^7Donuts Costs: ^1$5 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 118, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);


                            #region ' Click teh Job '

                            if (Conn.InGame == 0)
                            {
                                InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                            {
                                InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                            {
                                InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                            {
                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                            {
                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100-200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                            }
                            #endregion

                            Conn.DisplaysOpen = true;

                            #endregion
                        }
                        else
                        {
                            #region ' Command '

                            MsgPly("^4|^7 !buy chicken - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                            MsgPly("^4|^7 !buy beer - Costs: ^1$10 ^7Health: ^27%", Conn.UniqueID);
                            MsgPly("^4|^7 !buy donuts - Costs: ^1$5 ^7Health: ^25%", Conn.UniqueID);

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
                                MsgPly("^4|^7 !job - Wages: ^2$100 - 200", Conn.UniqueID);
                            }


                            #endregion
                        }

                        Conn.InShop = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '

                    if (Conn.InShop == true)
                    {
                        #region ' Close Display '
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
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
                        #endregion

                        Conn.InShop = false;
                    }

                    #endregion
                }

                #endregion

                #region ' KinderGarten '

                Conn.InSchoolDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (135 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-230 * 196608), 2)) / 65536);
                if (Conn.InSchoolDist <= 4 && (mph <= 4))
                {
                    #region ' Command and Display '

                    if (Conn.InSchool == false)
                    {
                        #region ' Job Complete '

                        if (Conn.JobToSchool == true)
                        {
                            int prize = 0;
                            if (Conn.JobFromHouse1 == true)
                            {
                                prize = new Random().Next(100, 340);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completes a Job!");
                                Conn.JobFromHouse1 = false;
                            }
                            if (Conn.JobFromHouse2 == true)
                            {
                                prize = new Random().Next(100, 300);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completes a Job!");
                                Conn.JobFromHouse2 = false;
                            }
                            if (Conn.JobFromHouse3 == true)
                            {
                                prize = new Random().Next(100, 320);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completes a Job!");
                                Conn.JobFromHouse3 = false;
                            }
                            MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                            Conn.Cash += prize;
                            Conn.TotalJobsDone += 1;
                            Conn.JobToSchool = false;
                        }

                        #endregion

                        string EstablishmentName = "^5Kinder^3Garten";
                        MsgPly("^4|^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                        {
                            #region ' Display '

                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^2Buy a ^7Cake Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Drink some ^7Lemonade Costs: ^1$10 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);

                            InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 118, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);

                            #region ' Lotto Structure '
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
                            #endregion

                            Conn.DisplaysOpen = true;

                            #endregion
                        }
                        else
                        {
                            #region ' Command '
                            MsgPly("^4|^7 !buy cake - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                            MsgPly("^4|^7 !buy lemonade - Costs: ^1$10 ^7Health: ^25%", Conn.UniqueID);
                            MsgPly("^4|^7 !buy ticket pick - Costs: ^1$100 ^7Pick a number 1 - 10", Conn.UniqueID);
                            #endregion
                        }

                        Conn.InSchool = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '
                    if (Conn.InSchool == true)
                    {
                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
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
                        Conn.InSchool = false;
                    }
                    #endregion
                }

                #endregion

                #endregion

                #region ' Houses '

                #region ' Kou's House '

                Conn.InHouse1Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (11 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (134 * 196608), 2)) / 65536);
                if (Conn.InHouse1Dist <= 4 && (mph <= 4))
                {
                    #region ' Display and Command '

                    if (Conn.InHouse1 == false)
                    {
                        #region ' Job From Shop '
                        if (Conn.JobFromShop == true)
                        {
                            if (Conn.JobToHouse1 == true)
                            {
                                int prize = new Random().Next(100, 500);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromShop = false;
                                Conn.JobToHouse1 = false;
                            }
                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Johnson's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Shanen's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Job From Store '
                        if (Conn.JobFromStore == true)
                        {
                            if (Conn.JobToHouse1 == true)
                            {
                                int prize = new Random().Next(200, 600);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromStore = false;
                                Conn.JobToHouse1 = false;
                            }
                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Johnson's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Shanen's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Command and Display '
                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                        {
                            MsgPly("^4|^7 You have to finish your jobs first.", Conn.UniqueID);
                        }
                        else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                        {
                            if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                            {
                                string HouseName = "Kou's House";
                                MsgPly("^4|^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);

                                #region ' Random Sell Price '
                                if (Conn.Electronics > 0)
                                {
                                    Conn.SellElectronics = new Random().Next(50, 290);
                                }
                                if (Conn.Furniture > 0)
                                {
                                    Conn.SellFurniture = new Random().Next(60, 210);
                                }
                                #endregion

                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the ^3" + HouseName + "!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                                    if (Conn.Electronics > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Electronic Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 1, 118, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to Trade any Electronic!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.Furniture > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Furniture Items for ^1$" + Conn.SellFurniture + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 1, 119, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to trade any Furniture!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for KinderGarten Escort!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    #endregion


                                    Conn.DisplaysOpen = true;
                                }
                                else
                                {
                                    #region ' Command '

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

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        MsgPly("^2!job ^7- Escort a Children to ^3KinderGarten.", Conn.UniqueID);
                                    }
                                    else
                                    {
                                        MsgPly("^7 Jobs can be only done in Road Cars.", Conn.UniqueID);
                                    }

                                    #endregion
                                }
                            }
                            else
                            {
                                MsgPly("^4|^7 Come back later if you are off in duty.", Conn.UniqueID);
                            }
                        }
                        else
                        {
                            MsgPly("^4|^7 Come back later if your not a Suspect!", Conn.UniqueID);
                        }
                        #endregion

                        Conn.InHouse1 = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '

                    if (Conn.InHouse1 == true)
                    {
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
                        Conn.InHouse1 = false;
                    }

                    #endregion
                }

                #endregion

                #region ' Johnson's House '

                Conn.InHouse2Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (62 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (76 * 196608), 2)) / 65536);
                if (Conn.InHouse2Dist <= 4 && (mph <= 4))
                {
                    #region ' Display and Command '

                    if (Conn.InHouse2 == false)
                    {
                        #region ' Job From Shop '
                        if (Conn.JobFromShop == true)
                        {
                            if (Conn.JobToHouse2 == true)
                            {
                                int prize = new Random().Next(100, 500);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromShop = false;
                                Conn.JobToHouse2 = false;
                            }
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Kou's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Shanen's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Job From Store '
                        if (Conn.JobFromStore == true)
                        {
                            if (Conn.JobToHouse2 == true)
                            {
                                int prize = new Random().Next(200, 600);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromStore = false;
                                Conn.JobToHouse2 = false;
                            }
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Kou's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse3 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Shanen's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Command and Display '
                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                        {
                            MsgPly("^4|^7 You have to finish your jobs first.", Conn.UniqueID);
                        }
                        else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                        {
                            if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                            {
                                string HouseName = "Johnson's House";
                                MsgPly("^4|^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);

                                #region ' Random Sell Price '
                                if (Conn.Electronics > 0)
                                {
                                    Conn.SellElectronics = new Random().Next(50, 290);
                                }
                                if (Conn.Furniture > 0)
                                {
                                    Conn.SellFurniture = new Random().Next(60, 210);
                                }
                                #endregion

                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the ^3" + HouseName + "!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                                    if (Conn.Electronics > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Electronic Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 1, 118, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to Trade any Electronic!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.Furniture > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Furniture Items for ^1$" + Conn.SellFurniture + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 1, 119, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to trade any Furniture!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for KinderGarten Escort!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;
                                }
                                else
                                {
                                    #region ' Command '

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

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        MsgPly("^2!job ^7- Escort a Children to ^3KinderGarten.", Conn.UniqueID);
                                    }
                                    else
                                    {
                                        MsgPly("^7 Jobs can be only done in Road Cars.", Conn.UniqueID);
                                    }

                                    #endregion
                                }
                            }
                            else
                            {
                                MsgPly("^4|^7 Come back later if you are off in duty.", Conn.UniqueID);
                            }
                        }
                        else
                        {
                            MsgPly("^4|^7 Come back later if your not a Suspect!", Conn.UniqueID);
                        }
                        #endregion

                        Conn.InHouse2 = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '

                    if (Conn.InHouse2 == true)
                    {
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
                        Conn.InHouse2 = false;
                    }

                    #endregion
                }

                #endregion

                #region ' Shanen's House '

                Conn.InHouse3Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-48 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-132 * 196608), 2)) / 65536);
                if (Conn.InHouse3Dist <= 4 && (mph <= 4))
                {
                    #region ' Display and Command '

                    if (Conn.InHouse3 == false)
                    {
                        #region ' Job From Shop '
                        if (Conn.JobFromShop == true)
                        {
                            if (Conn.JobToHouse3 == true)
                            {
                                int prize = new Random().Next(100, 500);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromShop = false;
                                Conn.JobToHouse3 = false;
                            }
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Kou's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Johnson's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Job From Store '
                        if (Conn.JobFromStore == true)
                        {
                            if (Conn.JobToHouse3 == true)
                            {
                                int prize = new Random().Next(200, 600);
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Completed a Job!");
                                MsgAll("^4|^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                Conn.Cash += prize;
                                Conn.TotalJobsDone += 1;
                                Conn.JobFromStore = false;
                                Conn.JobToHouse3 = false;
                            }
                            if (Conn.JobToHouse1 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Kou's House!", Conn.UniqueID);
                            }
                            if (Conn.JobToHouse2 == true)
                            {
                                MsgPly("^4|^7 Complete Error. Not Johnson's House!", Conn.UniqueID);
                            }
                        }
                        #endregion

                        #region ' Command and Display '
                        if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                        {
                            MsgPly("^4|^7 You have to finish your jobs first.", Conn.UniqueID);
                        }
                        else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                        {
                            if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                            {
                                string HouseName = "Shanen's House";
                                MsgPly("^4|^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);

                                #region ' Random Sell Price '
                                if (Conn.Electronics > 0)
                                {
                                    Conn.SellElectronics = new Random().Next(50, 290);
                                }
                                if (Conn.Furniture > 0)
                                {
                                    Conn.SellFurniture = new Random().Next(60, 210);
                                }
                                #endregion

                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 50, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the ^3" + HouseName + "!", Flags.ButtonStyles.ISB_C1 | Flags.ButtonStyles.ISB_LEFT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1^J‚w", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 6, 52, 143, 113, Conn.UniqueID, 2, false);

                                    if (Conn.Electronics > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Electronic Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 1, 118, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to Trade any Electronic!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.Furniture > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Trade ^7your Furniture Items for ^1$" + Conn.SellFurniture + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 1, 119, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to trade any Furniture!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    }

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for KinderGarten Escort!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 73, 54, 116, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;
                                }
                                else
                                {
                                    #region ' Command '

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

                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        MsgPly("^2!job ^7- Escort a Children to ^3KinderGarten.", Conn.UniqueID);
                                    }
                                    else
                                    {
                                        MsgPly("^7 Jobs can be only done in Road Cars.", Conn.UniqueID);
                                    }

                                    #endregion
                                }
                            }
                            else
                            {
                                MsgPly("^4|^7 Come back later if you are off in duty.", Conn.UniqueID);
                            }
                        }
                        else
                        {
                            MsgPly("^4|^7 Come back later if your not a Suspect!", Conn.UniqueID);
                        }
                        #endregion

                        Conn.InHouse3 = true;
                    }

                    #endregion
                }
                else
                {
                    #region ' Close Display '

                    if (Conn.InHouse3 == true)
                    {
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
                        Conn.InHouse3 = false;
                    }

                    #endregion
                }

                #endregion

                #endregion

                #endregion


            }
            catch
            {
                MsgAll("^4|^7 Map Error. Please reload the Application!");
                MsgBox("> InSim is failure to load the Blackwood GP Map please restart the InSim.");

                Console.Beep(); // 1
                Console.Beep(); // 2
                Console.Beep(); // 3

                /*
                 Terms if only the InSim failed to connect or Load the Packet Player per Cars
                 */

                InSim.Close();
                Application.Exit();
            }
        }

    }
}