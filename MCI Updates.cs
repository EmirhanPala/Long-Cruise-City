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
        private void MCI_Update(int PLID)
        {
            try
            {
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

                #region ' Cruise '

                decimal SpeedMS = (decimal)(((Conn.CompCar.Speed / 32768f) * 100f) / 2);
                decimal Speed = (decimal)((Conn.CompCar.Speed * (100f / 32768f)) * 3.6f);

                Conn.TotalDistance += Convert.ToInt32(SpeedMS);
                Conn.TripMeter += Convert.ToInt32(SpeedMS);
                Conn.BonusDistance += Convert.ToInt32(SpeedMS);
                Conn.HealthDist += Convert.ToInt32(SpeedMS);

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

                #region ' XY NODES CONSTRUCTION '

                if (Conn.IsAdmin == 1)
                {
                    InSim.Send_BTN_CreateButton("^1X: " + pathx + " ^2Y: " + pathy + " ^3Z: " + pathz, Flags.ButtonStyles.ISB_DARK, 5, 25, 50, 157, 100, (Conn.UniqueID), 2, false);
                   
                }
                
                #endregion                

                #region ' Gotovi Karti '
                
                switch (TrackName)
                {

                    case "RO1X":

                        #region ' RO1X '
                        {
                            if (pathx >= -96 && pathx <= -56 && pathy >= -67 && pathy <= 33)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 60)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 60 kmh";
                                        // SpecID(Conn.Username);
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 60 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^2Service Station";
                                
                                Conn.Location = "Pit Station";
                                Conn.LastSeen = "Pit Station";
                            }
                            else if (pathx >= -57 && pathx <= -40 && pathy >= -62 && pathy <= -49)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 50)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^3Gas Station";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^050", Flags.ButtonStyles.ISB_C1, 18, 18, 100, 176, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Gas Station";
                                Conn.LastSeen = "Gas Station";
                            }
                            else if (pathx >= -10 && pathx <= -5 && pathy >= -136 && pathy <= -98 && pathz >= 0 && pathz <= 0)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 80)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion


                                Conn.LocationBox = "^3Payed Zone";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^080", Flags.ButtonStyles.ISB_C1, 18, 18, 100, 176, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Payed Zone";
                                Conn.LastSeen = "Payed Zone";
                            }
                            else if (pathx >= -11 && pathx <= -8 && pathy >= -98 && pathy <= -92)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 40)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 40 kmh";
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 40 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^1*^3 EntryZone ^1*";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^040", Flags.ButtonStyles.ISB_C1, 18, 18, 100, 176, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Paying Zone";
                                Conn.LastSeen = "Paying Zone";
                            }
                            else if (pathx >= -8 && pathx <= -5 && pathy >= -142 && pathy <= -136)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 40)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 40 kmh";
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 40 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^1*^3 EntryZone ^1*";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^040", Flags.ButtonStyles.ISB_C1, 18, 18, 100, 176, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Paying Zone";
                                Conn.LastSeen = "Paying Zone";
                            }
                            else if (pathx >= -11 && pathx <= 8 && pathy >= 7 && pathy <= 44)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 70)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 70 kmh";
                                    }
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 70 kmh";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^2Parking";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^070", Flags.ButtonStyles.ISB_C1, 18, 18, 100, 176, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "^2Parking";
                                Conn.LastSeen = "^2Parking";
                            }
                            else
                            {
                                #region ' Speedlimit 140kmh/99ph '
                                if (kmh > 160)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 160 kmh";
                                    }
                                    else
                                    {
                                        Conn.SpeedBox = "^1" + mph + " mph / 99 mph";
                                    }
                                    Conn.IsSpeeder = 1;
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 160 kmh";
                                    }
                                    else
                                    {
                                        Conn.SpeedBox = "^2" + mph + " mph / 99 mph";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion
                                Conn.LocationBox = "^7Rockingham City";
                                InSim.Send_BTN_CreateButton("^1•", Flags.ButtonStyles.ISB_C1, 70, 70, 72, 150, 10, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^7•", Flags.ButtonStyles.ISB_C1, 59, 58, 78, 156, 11, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^0160", Flags.ButtonStyles.ISB_C1, 18, 13, 100, 178, 12, Conn.UniqueID, 2, false);
                                Conn.Location = "Rockingham City";
                                Conn.LastSeen = "Rockingham City";
                                Conn.TickedPay = 0;
                            }
                        }
                        #endregion

                        #region ' Houses and Establishments '

                        #region ' Establishment '

                        #region ' FDriver Level '
                        //redi
                        Conn.InExpDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-21 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-164 * 196608), 2)) / 65536);
                        if (Conn.InExpDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InExp == false)
                            {

                                string EstablishmentName = "^7Driving School";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 135, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 85, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 136, 66, 113, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^11 - ^3$6500", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65, 114, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^12 - ^3$10000", Flags.ButtonStyles.ISB_LEFT, 4, 100, 72, 65, 115, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^13 - ^3$15000", Flags.ButtonStyles.ISB_LEFT, 4, 100, 84, 65, 116, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 65, 65, 118, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 77, 65, 119, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 89, 65, 120, Conn.UniqueID, 2, false);


                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^14 - ^3$17000", Flags.ButtonStyles.ISB_LEFT, 4, 40, 95, 65, 142, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 101, 65, 139, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^15 - ^3$18000", Flags.ButtonStyles.ISB_C1,  4, 40, 107, 60, 143, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 111, 65, 140, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Driver Level ^16 - ^3$18000", Flags.ButtonStyles.ISB_C1, 4, 40, 117, 60, 121, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 122, 65, 141, Conn.UniqueID, 2, false);

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    
                                }

                                Conn.InExp = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '

                            if (Conn.InExp == true)
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
                                    //
                                    DeleteBTN(142, Conn.UniqueID);
                                    DeleteBTN(139, Conn.UniqueID);
                                    DeleteBTN(143, Conn.UniqueID);
                                    DeleteBTN(140, Conn.UniqueID);
                                    //DeleteBTN(121, Conn.UniqueID);
                                    DeleteBTN(141, Conn.UniqueID);
                                    Conn.DisplaysOpen = false;
                                }
                                #endregion

                                Conn.InExp = false;
                            }

                            #endregion
                        }

                        #endregion


                        #region ' Dealer '

                        Conn.InDealerDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-86 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (47 * 196608), 2)) / 65536);
                        if (Conn.InDealerDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InDealer == false)
                            {

                                string EstablishmentName = "^1*^7* Car Dealer ^7*^1* ";
                                MsgPly("^1*^7 Welcome to the " + EstablishmentName, Conn.UniqueID);
                                if (Conn.GpsToDealer == 1 && Conn.GPS == 1)
                                {
                                    MsgPly("^1*^7 You have reached your destination !", Conn.UniqueID);
                                    Thread.Sleep(3000);
                                    Conn.GPS = 0;
                                    Conn.GpsToDealer = 0;
                                }
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 85, 60, 50, 70, 110, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 85, 60, 50, 70, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("" + EstablishmentName, Flags.ButtonStyles.ISB_LIGHT, 6, 58, 51, 71, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1LX4" +" ^7- ^2$" + Dealer.GetCarPrice("LX4"), Flags.ButtonStyles.ISB_LIGHT , 4, 40, 62, 71, 113, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 62, 111, 133, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1LX6" + " ^7- ^2$" + Dealer.GetCarPrice("LX6"), Flags.ButtonStyles.ISB_LIGHT , 4, 40, 66, 71, 114, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 66, 111, 134, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1RB4" + " ^7- ^2$" + Dealer.GetCarPrice("RB4"), Flags.ButtonStyles.ISB_LIGHT , 4, 40, 70, 71, 115, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 70, 111, 135, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FXO" + " ^7- ^2$" + Dealer.GetCarPrice("FXO"), Flags.ButtonStyles.ISB_LIGHT , 4, 40, 74, 71, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 74, 111, 136, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1XRT" + " ^7- ^2$" + Dealer.GetCarPrice("XRT"), Flags.ButtonStyles.ISB_LIGHT , 4, 40, 78, 71, 117, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 78, 111, 137, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1RAC" + " ^7- ^2$" + Dealer.GetCarPrice("RAC"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 82, 71, 118, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 82, 111, 138, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FZ5" + " ^7- ^2$" + Dealer.GetCarPrice("FZ5"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 86, 71, 119, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 86, 111, 139, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1UFR" + " ^7- ^2$" + Dealer.GetCarPrice("UFR"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 90, 71, 120, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 90, 111, 140, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1XFR" + " ^7- ^2$" + Dealer.GetCarPrice("XFR"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 94, 71, 121, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 94, 111, 141, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FXR" + " ^7- ^2$" + Dealer.GetCarPrice("FXR"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 98, 71, 122, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 98, 111, 142, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1XRR" + " ^7- ^2$" + Dealer.GetCarPrice("XRR"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 102, 71, 123, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 102, 111, 143, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FZR" + " ^7- ^2$" + Dealer.GetCarPrice("FZR"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 106, 71, 124, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 106, 111, 144, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1MRT" + " ^7- ^2$" + Dealer.GetCarPrice("MRT"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 110, 71, 125, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 110, 111, 145, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FBM" + " ^7- ^2$" + Dealer.GetCarPrice("FBM"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 114, 71, 126, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 114, 111, 146, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FOX" + " ^7- ^2$" + Dealer.GetCarPrice("FOX"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 118, 71, 127, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 118, 111, 147, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1FO8" + " ^7- ^2$" + Dealer.GetCarPrice("FO8"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 122, 71, 128, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 122, 111, 148, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1BF1" + " ^7- ^2$" + Dealer.GetCarPrice("BF1"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 126, 71, 129, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 126, 111, 149, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Volkswagen Scirocco" + " ^7- ^2$" + Dealer.GetCarPrice("VWS"), Flags.ButtonStyles.ISB_LIGHT, 4, 40, 130, 71, 130, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^3!! ^2Buy ^3!!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 18, 130, 111, 150, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Car and Buy Value", Flags.ButtonStyles.ISB_DARK, 5, 40, 57, 71, 131, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Action", Flags.ButtonStyles.ISB_DARK, 5, 18, 57, 111, 132, Conn.UniqueID, 2, false);




                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    
                                }

                                Conn.InDealer = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '

                            if (Conn.InDealer == true)
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
                                    DeleteBTN(122, Conn.UniqueID);
                                    DeleteBTN(123, Conn.UniqueID);
                                    DeleteBTN(124, Conn.UniqueID);
                                    DeleteBTN(125, Conn.UniqueID);
                                    DeleteBTN(126, Conn.UniqueID);
                                    DeleteBTN(127, Conn.UniqueID);
                                    DeleteBTN(128, Conn.UniqueID);
                                    DeleteBTN(129, Conn.UniqueID);
                                    DeleteBTN(130, Conn.UniqueID);
                                    DeleteBTN(131, Conn.UniqueID);
                                    DeleteBTN(132, Conn.UniqueID);
                                    DeleteBTN(133, Conn.UniqueID);
                                    DeleteBTN(134, Conn.UniqueID);
                                    DeleteBTN(135, Conn.UniqueID);
                                    DeleteBTN(136, Conn.UniqueID);
                                    DeleteBTN(137, Conn.UniqueID);
                                    DeleteBTN(138, Conn.UniqueID);
                                    DeleteBTN(139, Conn.UniqueID);
                                    DeleteBTN(140, Conn.UniqueID);
                                    DeleteBTN(141, Conn.UniqueID);
                                    DeleteBTN(142, Conn.UniqueID);
                                    DeleteBTN(143, Conn.UniqueID);
                                    DeleteBTN(144, Conn.UniqueID);
                                    DeleteBTN(145, Conn.UniqueID);
                                    DeleteBTN(146, Conn.UniqueID);
                                    DeleteBTN(147, Conn.UniqueID);
                                    DeleteBTN(148, Conn.UniqueID);
                                    DeleteBTN(149, Conn.UniqueID);
                                    DeleteBTN(150, Conn.UniqueID);
                                    
                                    Conn.DisplaysOpen = false;
                                }
                                #endregion

                                Conn.InDealer = false;
                            }
                            #endregion
                        }

                        #endregion

                        #region ' City Bank '
                        //redi
                        Conn.InBankDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-7 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (52 * 196608), 2)) / 65536);
                        if (Conn.InBankDist <= 4 && (mph <= 4))
                        {
                            #region ' Open Commands and Displays '
                            if (Conn.InBank == false)
                            {
                                string EstablishmentName = "^7Rockingam City Bank";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName, Conn.UniqueID);

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
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^7Your bank balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), Flags.ButtonStyles.ISB_LEFT, 4, 40, 65, 65, 114, Conn.UniqueID, 2, false);
                                    if (Conn.BankBalance > 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Time until Bonus ^1" + Minutes + ":" + Seconds + " ^7left", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 65, 115, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^7You don't have any Bank Balance on Account!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 69, 65, 115, Conn.UniqueID, 2, false);
                                    }
                                    InSim.Send_BTN_CreateButton("^1Insert", "Enter amount to Insert", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 6, 12, 73, 87, 75, 116, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^1Withdraw", "Enter amount to Withdraw", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 6, 12, 73, 99, 64, 117, Conn.UniqueID, 2, false);

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

                        #region ' Dany's Store '
                        //regi
                        Conn.InStoreDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-53 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (4 * 196608), 2)) / 65536);
                        if (Conn.InStoreDist <= 4 && (mph <= 4))
                        {
                            #region ' Open Display or Commands '

                            if (Conn.InStore == false)
                            {
                                string EstablishmentName = "^3Job Center";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Fried Chicken Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65, 114, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Beer Costs: ^1$10 ^7Health: ^27%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy some ^7Donuts Costs: ^1$5 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);

                                    //InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 65, 65, 118, Conn.UniqueID, 2, false);
                                    // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);
                                    // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);


                                    #region ' Click teh Job '

                                    if (Conn.InGame == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100-200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 75, 65, 121, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '

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
                        //redi
                        Conn.InShopDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-97 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (62 * 196608), 2)) / 65536);
                        if (Conn.InShopDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InShop == false)
                            {

                                string EstablishmentName = "^7Food Shop";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Fried Chicken Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65,114, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Beer Costs: ^1$10 ^7Health: ^27%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy some ^7Donuts Costs: ^1$5 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 65, 65, 118, Conn.UniqueID, 2, false);
                                   // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);
                                   // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);


                                    #region ' Click teh Job '

                                    if (Conn.InGame == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100-200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 75, 65, 121, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '

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

                        #region ' Lottery '
                        //redi
                        Conn.InSchoolDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-54 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (7 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse1 = false;
                                    }
                                    if (Conn.JobFromHouse2 == true)
                                    {
                                        prize = new Random().Next(100, 300);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse2 = false;
                                    }
                                    if (Conn.JobFromHouse3 == true)
                                    {
                                        prize = new Random().Next(100, 320);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse3 = false;
                                    }
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                    DeleteBTN(27, Conn.UniqueID);
                                    Conn.Cash += prize;
                                    Conn.TotalJobsDone += 1;
                                    Conn.JobToSchool = false;
                                }

                                #endregion

                                string EstablishmentName = "^2Lottery";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '

                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT , 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                   

                                    #region ' Lotto Structure '
                                    if (Conn.LastLotto > 120)
                                    {
                                        InSim.Send_BTN_CreateButton("^7You have to wait ^1Three (3) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.LastLotto > 60)
                                    {
                                        InSim.Send_BTN_CreateButton("^7You have to wait ^1Two (2) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.LastLotto > 0)
                                    {
                                        if (Conn.LastLotto > 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Buy Lottery Ticket: ^1$100", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Buy", "Pick your number 1 - 10", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 65, 120, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '
                                    MsgPly("^6>>^7 !buy cake - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                                    MsgPly("^6>>^7 !buy lemonade - Costs: ^1$10 ^7Health: ^25%", Conn.UniqueID);
                                    MsgPly("^6>>^7 !buy ticket pick - Costs: ^1$100 ^7Pick a number 1 - 10", Conn.UniqueID);
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

                        #region ' Ticket Zone 1 '
                        //redi
                        Conn.InPayZoneDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-10 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-96 * 196608), 2)) / 65536);
                        if (Conn.InPayZoneDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InPayZone == false)
                            {
                                

                                string EstablishmentName = "^5Ticket Zone";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);


                                    InSim.Send_BTN_CreateButton("^2Buy Entry ticket: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);

                                    #endregion
                                }

                                Conn.InPayZone = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InPayZone == true)
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
                                Conn.InPayZone = false;
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Ticket Zone 2 '
                        //redi
                        Conn.InPayZone1Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-6 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-139 * 196608), 2)) / 65536);
                        if (Conn.InPayZone1Dist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InPayZone1 == false)
                            {
                                

                                string EstablishmentName = "^5Ticket Zone";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);


                                    InSim.Send_BTN_CreateButton("^2Buy Entry ticket: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);

                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }

                                Conn.InPayZone1 = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InPayZone1 == true)
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
                                Conn.InPayZone1 = false;
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Parking Zone '

                        Conn.InParkingDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-193 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (268 * 196608), 2)) / 65536);
                        if (Conn.InParkingDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InParking == false)
                            {
                                string EstablishmentName = "^1Paid ^1Parking";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '

                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                   
                                        InSim.Send_BTN_CreateButton("^2Buy a ticket to entry in parking: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                   
                                    #endregion

                                   

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }

                                Conn.InParking = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InParking == true)
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
                                Conn.InParking = false;
                            }
                            #endregion
                        }

                        #endregion

                        #endregion

                        #region ' Houses '

                        #region ' Hriso's House '
                        //ready ro1x
                        Conn.InHouse1Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (36 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (27 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        DeleteBTN(27, Conn.UniqueID);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse1 = false;
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        DeleteBTN(27, Conn.UniqueID);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse1 = false;
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Hriso's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);
                                                                                
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                            }
                                            #endregion


                                            Conn.DisplaysOpen = true;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        #region ' Martin's House '

                        Conn.InHouse2Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (90 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (23 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse2 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse2 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Martin's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);
                                        
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                            }
                                            #endregion

                                            Conn.DisplaysOpen = true;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        #region ' Elly's House '

                        Conn.InHouse3Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (68 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-79 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse3 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse3 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Elly's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
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
                                                MsgPly("^2!job ^7- Escort a Children to ^3Lottery.", Conn.UniqueID);
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
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        break;




                    case "WE1X":

                        #region ' Complete Tow Truck '

                        if (pathx > -1137 && pathx < 1709 && pathy > -1882 && pathy < 1520 && kmh < 300 && Conn.IsBeingTowed == true)
                        {
                            foreach (clsConnection Con in Connections)
                            {
                                if (Con.Towee == Conn.UniqueID)
                                {
                                    MsgAll("^6>>^7 " + Con.NoColPlyName + " completes the Tow Request!");
                                    MsgAll("^6>>^7 " + Con.NoColPlyName + " was rewarded ^2$500");
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now in pitlane safely!");
                                    Con.Cash += 500;
                                    Con.InTowProgress = false;
                                    Con.Towee = -1;
                                }
                            }
                            CautionSirenShutOff();
                            Conn.IsBeingTowed = false;
                        }

                        #endregion

                        #region ' Location and Speed Limit '

                        #region ' Westhill way '
                  #region gotovi sfetofari
                        /*
                        //SFETOFARA NA PESHEHODNATA PATEKA OT KUM KRUGOVOTO
                        //Westhill X-68 Y88 X-66 Y78
                        if (Conn.CompCar.X / 196608 >= -68 && Conn.CompCar.X / 196608 <= -66 && Conn.CompCar.Y / 196608 >= 78 && Conn.CompCar.Y / 196608 <= 88)
                        {
                            //InSim.Send_BTN_CreateButton("^7^TTRAFFİC SİNGS TEST WAY", Flags.ButtonStyles.ISB_C1, 4, 25, 89, 167, 14, Conn.UniqueID, 2, false);
                            Conn.Location = "^7^TTRAFFİC SİNGS TEST WAY";
                            Conn.LastSeen = "^7^TTRAFFİC SİNGS TEST WAY";

                            //BackGround+Red Timer
                            {
                                System.Timers.Timer arkaplankirmiziisik = new System.Timers.Timer { Interval = 1, AutoReset = false };
                                arkaplankirmiziisik.Interval = 1;
                                arkaplankirmiziisik.Elapsed += delegate
                                {
                                    //insim background+timer
                                    string[] trafficsingsbackbuttonground =
                                        {
                                            string.Format(""),
                                            string.Format(""),
                                        };

                                    for (byte i = 0, id = 88, top = 40; i < trafficsingsbackbuttonground.Length; i++, id++, top += 0)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsbackbuttonground[i], Flags.ButtonStyles.ISB_DARK, 38, 10, top, 155, id, Conn.UniqueID, 2, false);
                                    }
                                    //traffic sings timer red+background
                                    string[] trafficsingsred =
                                        {
                                            string.Format("^1•"),
                                        };

                                    for (byte i = 0, id = 90, top = 30; i < trafficsingsred.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsred[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplankirmiziisik.Start();
                            }

                            //Yellow Timer
                            {
                                System.Timers.Timer arkaplansariisik = new System.Timers.Timer { Interval = 10000, AutoReset = false };
                                arkaplansariisik.Interval = 10000;
                                arkaplansariisik.Elapsed += delegate
                                {

                                    //traffic sings timer yellow
                                    string[] trafficsingsyellow =
                                        {
                                            string.Format("^3•"),
                                        };

                                    for (byte i = 0, id = 91, top = 43; i < trafficsingsyellow.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsyellow[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplansariisik.Start();
                            }
                            //Green Timer
                            {
                                System.Timers.Timer arkaplanyesilisik = new System.Timers.Timer { Interval = 20000, AutoReset = false };
                                arkaplanyesilisik.Interval = 20000;
                                arkaplanyesilisik.Elapsed += delegate
                                {
                                    //traffic sings timer green
                                    string[] trafficsingsgreen =
                                        {
                                            string.Format("^2•"),
                                        };

                                    for (byte i = 0, id = 92, top = 56; i < trafficsingsgreen.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsgreen[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplanyesilisik.Start();
                            }
                        }
                        else
                        {
                            DeleteBTN(88, Conn.UniqueID);
                            DeleteBTN(89, Conn.UniqueID);
                            DeleteBTN(90, Conn.UniqueID);
                            DeleteBTN(91, Conn.UniqueID);
                            DeleteBTN(92, Conn.UniqueID);

                        }
                        //TRAFFIC
                        //Westhill X-68 Y88 X-66 Y78
                        if (Conn.CompCar.X / 196608 >= -68 && Conn.CompCar.X / 196608 <= -66 && Conn.CompCar.Y / 196608 >= 78 && Conn.CompCar.Y / 196608 <= 88)
                        {
                            InSim.Send_BTN_CreateButton("^7^TTRAFFİC SİNGS TEST WAY", Flags.ButtonStyles.ISB_C1, 4, 25, 89, 167, 14, Conn.UniqueID, 2, false);
                            Conn.Location = "^7^TTRAFFİC SİNGS TEST WAY";
                            Conn.LastSeen = "^7^TTRAFFİC SİNGS TEST WAY";

                            //BackGround+Red Timer
                            {
                                System.Timers.Timer arkaplankirmiziisik = new System.Timers.Timer { Interval = 1, AutoReset = false };
                                arkaplankirmiziisik.Interval = 1;
                                arkaplankirmiziisik.Elapsed += delegate
                                {
                                    //insim background+timer
                                    string[] trafficsingsbackbuttonground =
                                        {
                                            string.Format(""),
                                            string.Format(""),
                                        };

                                    for (byte i = 0, id = 88, top = 40; i < trafficsingsbackbuttonground.Length; i++, id++, top += 0)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsbackbuttonground[i], Flags.ButtonStyles.ISB_DARK, 38, 10, top, 155, id, Conn.UniqueID, 2, false);
                                    }
                                    //traffic sings timer red+background
                                    string[] trafficsingsred =
                                        {
                                            string.Format("^1•"),
                                        };

                                    for (byte i = 0, id = 90, top = 30; i < trafficsingsred.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsred[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplankirmiziisik.Start();
                            }

                            //Yellow Timer
                            {
                                System.Timers.Timer arkaplansariisik = new System.Timers.Timer { Interval = 10000, AutoReset = false };
                                arkaplansariisik.Interval = 10000;
                                arkaplansariisik.Elapsed += delegate
                                {

                                    //traffic sings timer yellow
                                    string[] trafficsingsyellow =
                                        {
                                            string.Format("^3•"),
                                        };

                                    for (byte i = 0, id = 91, top = 43; i < trafficsingsyellow.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsyellow[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplansariisik.Start();
                            }
                            //Green Timer
                            {
                                System.Timers.Timer arkaplanyesilisik = new System.Timers.Timer { Interval = 20000, AutoReset = false };
                                arkaplanyesilisik.Interval = 20000;
                                arkaplanyesilisik.Elapsed += delegate
                                {
                                    //traffic sings timer green
                                    string[] trafficsingsgreen =
                                        {
                                            string.Format("^2•"),
                                        };

                                    for (byte i = 0, id = 92, top = 56; i < trafficsingsgreen.Length; i++, id++, top += 13)
                                    {
                                        InSim.Send_BTN_CreateButton(trafficsingsgreen[i], Flags.ButtonStyles.ISB_C2, 30, 90, top, 115, id, Conn.UniqueID, 2, false);
                                    }
                                };
                                arkaplanyesilisik.Start();
                            }
                        }
                        else
                        {
                            DeleteBTN(88, Conn.UniqueID);
                            DeleteBTN(89, Conn.UniqueID);
                            DeleteBTN(90, Conn.UniqueID);
                            DeleteBTN(91, Conn.UniqueID);
                            DeleteBTN(92, Conn.UniqueID);

                        }

                        */
#endregion
                        {
                            if (pathx >= -62 && pathx <= 41 && pathy >= 194 && pathy <= 212)
                            {
                                #region ' Speedlimit 40kmh/99ph '
                                if (kmh > 40)
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^1" + kmh + " kmh / 40 kmh";
                                    }
                                    else
                                    {
                                        Conn.SpeedBox = "^1" + mph + " mph / 99 mph";
                                    }
                                    Conn.IsSpeeder = 1;
                                }
                                else
                                {
                                    if (Conn.KMHorMPH == 0)
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 40 kmh";
                                    }
                                    else
                                    {
                                        Conn.SpeedBox = "^2" + mph + " mph / 99 mph";
                                    }

                                    Conn.IsSpeeder = 0;
                                }
                                #endregion

                                Conn.LocationBox = "^3Gas Station";
                                Conn.Location = "Gas Station";
                                Conn.LastSeen = "Gas Station";
                            }
                            else if (pathx >= -68 && pathx <= -64 && pathy >= 78 && pathy <= 186)
                            {
                                #region ' Speedlimit 140kmh/99ph '
                                if (kmh > 80)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Pit Zone";
                                Conn.Location = "Pit Zone";
                                Conn.LastSeen = "Pit Zone";

                            }
                            else if (pathx >= -78 && pathx <= -74 && pathy >= -395 && pathy <= -390)
                            {
                                #region ' Speedlimit 140kmh/99ph '
                                if (kmh > 50)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Drag Zone Entry";
                                Conn.Location = "Drag Zone Entry";
                                Conn.LastSeen = "Drag Zone Entry";
                            }
                            else if (pathx >= -63 && pathx <= -58 && pathy >= 84 && pathy <= 183)
                            {
                                #region ' Speedlimit 140kmh/99ph '
                                if (kmh > 80)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 80 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 80 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Pit Zone";
                                Conn.Location = "Pit Zone";
                                Conn.LastSeen = "Pit Zone";

                            }
                            else if (pathx >= -81&& pathx <= 108 && pathy >= -426 && pathy <= -395)
                            {                                
                                Conn.SpeedBox = "^7Your Speed : " + kmh;
                                Conn.LocationBox = "^3Drag Zone";
                                Conn.Location = "Drag Zone";
                                Conn.LastSeen = "Drag Zone";
                            }
                            else if (pathx >= -85 && pathx <= -61 && pathy >= 263 && pathy <= 274 && pathz >= 2 && pathz <= 2)
                            {
                                #region ' payed '
                                if (Conn.TickedPay == 0)
                                {
                                    MsgAll(Conn.PlayerName + " ^7was fined with ^1$100^7!");
                                    MsgAll("^1Reason : ^7Entry ticked was not payed!");
                                    Conn.Cash -= 750;
                                    Conn.TickedPay = 1;
                                }
                                else
                                {
                                }
                                #endregion

                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 60)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 60 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 60 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Payed Zone";
                                Conn.Location = "Payed Zone";
                                Conn.LastSeen = "Payed Zone";

                            }
                            else if (pathx >= -62 && pathx <= -55 && pathy >= 272 && pathy <= 276)
                            { // ot drygata strana
                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 30)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 30 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 30 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Pay Zone";
                                Conn.Location = "Pay Zone";
                                Conn.LastSeen = "Pay Zone";

                            }
                            else if (pathx >= -93 && pathx <= -85 && pathy >= 260 && pathy <= 265)
                            { //ot kum krugovoto
                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 30)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 30 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 30 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Pay Zone";
                                Conn.Location = "Pay Zone";
                                Conn.LastSeen = "Pay Zone";

                            }
                            else if (pathx >= -232 && pathx <= -207 && pathy >= 276 && pathy <= 301)
                            {
                                #region ' entry '
                                if (Conn.ParkingPay == 0)
                                {
                                    MsgAll(Conn.PlayerName + " ^7was fined with ^11000^7!");
                                    MsgAll("^1Reason : ^7Entry for the parking was not payed!");
                                    Conn.Cash -= 1000;
                                    Conn.ParkingPay = 1;
                                }
                                #endregion

                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 50)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Parking";
                                Conn.Location = "Parking";
                                Conn.LastSeen = "Parking";
                            }
                            else if (pathx >= -212 && pathx <= -197 && pathy >= 268 && pathy <= 282)
                            {
                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 50)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Parking Entry";
                                Conn.Location = "Parking Entry";
                                Conn.LastSeen = "Parking Entry";
                            }
                            else if (pathx >= -200 && pathx <= -181 && pathy >= 260 && pathy <= 272)
                            {
                                #region ' Speedlimit 60kmh/99ph '
                                if (kmh > 50)
                                {
                                    Conn.SpeedBox = "^1" + kmh + " kmh / 50 kmh";
                                }
                                else
                                {
                                    {
                                        Conn.SpeedBox = "^2" + kmh + " kmh / 50 kmh";
                                    }
                                }
                                #endregion

                                Conn.LocationBox = "^3Parking Entry";
                                Conn.Location = "Parking Entry";
                                Conn.LastSeen = "Parking Entry";
                            }
                            else
                                {
                                    Conn.TickedPay = 0;
                                    #region ' Speedlimit 140kmh/99ph '
                                    if (kmh > 160)
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            Conn.SpeedBox = "^1" + kmh + " kmh / 160 kmh";
                                        }
                                        else
                                        {
                                            Conn.SpeedBox = "^1" + mph + " mph / 99 mph";
                                        }
                                        Conn.IsSpeeder = 1;
                                    }
                                    else
                                    {
                                        if (Conn.KMHorMPH == 0)
                                        {
                                            Conn.SpeedBox = "^2" + kmh + " kmh / 160 kmh";
                                        }
                                        else
                                        {
                                            Conn.SpeedBox = "^2" + mph + " mph / 99 mph";
                                        }

                                        Conn.IsSpeeder = 0;
                                    }
                                    #endregion
                                    Conn.LocationBox = "^3Westhill City";
                                    Conn.Location = "Westhill City";
                                    Conn.LastSeen = "Westhill City";
                                }
                        }
                        #endregion

                        #endregion

                        #region ' Houses and Establishments '

                        #region ' Establishment '

                        #region ' City Bank '
                        Conn.InBankDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-56 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (182 * 196608), 2)) / 65536);
                        if (Conn.InBankDist <= 4 && (mph <= 4))
                        {
                            #region ' Open Commands and Displays '
                            if (Conn.InBank == false)
                            {
                                string EstablishmentName = "^7Westhill City Bank";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName, Conn.UniqueID);

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

                        #region ' Dany's Store '

                        Conn.InStoreDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (18 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (68 * 196608), 2)) / 65536);
                        if (Conn.InStoreDist <= 4 && (mph <= 4))
                        {
                            #region ' Open Display or Commands '

                            if (Conn.InStore == false)
                            {
                                string EstablishmentName = "^2Dany's Store";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Fried Chicken Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65, 114, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Beer Costs: ^1$10 ^7Health: ^27%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy some ^7Donuts Costs: ^1$5 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);

                                    //InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 65, 65, 118, Conn.UniqueID, 2, false);
                                    // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);
                                    // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);


                                    #region ' Click teh Job '

                                    if (Conn.InGame == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100-200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 75, 65, 121, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '

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

                        Conn.InShopDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (17 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (69 * 196608), 2)) / 65536);
                        if (Conn.InShopDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InShop == false)
                            {

                                string EstablishmentName = "^7Food Shop";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy a ^7Fried Chicken Costs: ^1$15 ^7Health: ^210%", Flags.ButtonStyles.ISB_LEFT, 4, 40, 61, 65,114, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy a ^7Beer Costs: ^1$10 ^7Health: ^27%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                    //InSim.Send_BTN_CreateButton("^2Buy some ^7Donuts Costs: ^1$5 ^7Health: ^25%", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);

                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 65, 65, 118, Conn.UniqueID, 2, false);
                                   // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 119, Conn.UniqueID, 2, false);
                                   // InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);


                                    #region ' Click teh Job '

                                    if (Conn.InGame == 0)
                                    {
                                        InSim.Send_BTN_CreateButton("^2You ^7must be in vehicle before you can access this command!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsOfficer == true || Conn.IsCadet == true || Conn.IsTowTruck == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^7Can't take a job whilst duty!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7do more than 1 job", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Car == "UFR" || Car == "XFR" || Car == "FXR" || Car == "XRR" || Car == "FZR" || Car == "MRT" || Car == "FBM" || Car == "FO8" || Car == "FOX" || Car == "BF1")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100-200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 75, 65, 121, Conn.UniqueID, 2, false);
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Can't ^7start a Job whilst being chased!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 70, 65, 117, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '

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

                        #region ' Lottery '

                        Conn.InSchoolDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-75 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-62 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse1 = false;
                                    }
                                    if (Conn.JobFromHouse2 == true)
                                    {
                                        prize = new Random().Next(100, 300);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse2 = false;
                                    }
                                    if (Conn.JobFromHouse3 == true)
                                    {
                                        prize = new Random().Next(100, 320);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completes a Job!");
                                        Conn.JobFromHouse3 = false;
                                    }
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                    DeleteBTN(27, Conn.UniqueID);
                                    Conn.Cash += prize;
                                    Conn.TotalJobsDone += 1;
                                    Conn.JobToSchool = false;
                                }

                                #endregion

                                string EstablishmentName = "^2Lottery";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '

                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT , 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                   

                                    #region ' Lotto Structure '
                                    if (Conn.LastLotto > 120)
                                    {
                                        InSim.Send_BTN_CreateButton("^7You have to wait ^1Three (3) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.LastLotto > 60)
                                    {
                                        InSim.Send_BTN_CreateButton("^7You have to wait ^1Two (2) hours ^7to rejoin the Lotto", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    }
                                    else if (Conn.LastLotto > 0)
                                    {
                                        if (Conn.LastLotto > 1)
                                        {
                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minutes ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        }
                                        else
                                        {
                                            InSim.Send_BTN_CreateButton("^2You ^7have to wait ^1" + Conn.LastRaffle + " minute ^7to rejoin the Lotto!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        }
                                    }
                                    else
                                    {
                                        InSim.Send_BTN_CreateButton("^2Buy Lottery Ticket: ^1$100", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Buy", "Pick your number 1 - 10", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 65, 120, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }
                                else
                                {
                                    #region ' Command '
                                    MsgPly("^6>>^7 !buy cake - Costs: ^1$15 ^7Health: ^210%", Conn.UniqueID);
                                    MsgPly("^6>>^7 !buy lemonade - Costs: ^1$10 ^7Health: ^25%", Conn.UniqueID);
                                    MsgPly("^6>>^7 !buy ticket pick - Costs: ^1$100 ^7Pick a number 1 - 10", Conn.UniqueID);
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

                        #region ' Ticket Zone 1 '

                        Conn.InPayZoneDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-60 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (274 * 196608), 2)) / 65536);
                        if (Conn.InPayZoneDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InPayZone == false)
                            {
                                

                                string EstablishmentName = "^5Ticket Zone";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);


                                    InSim.Send_BTN_CreateButton("^2Buy Entry ticket: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);

                                    #endregion
                                }

                                Conn.InPayZone = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InPayZone == true)
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
                                Conn.InPayZone = false;
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Ticket Zone 2 '

                        Conn.InPayZone1Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-86 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (263 * 196608), 2)) / 65536);
                        if (Conn.InPayZone1Dist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InPayZone1 == false)
                            {
                                

                                string EstablishmentName = "^5Ticket Zone";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '
                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);


                                    InSim.Send_BTN_CreateButton("^2Buy Entry ticket: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);

                                    #endregion

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }

                                Conn.InPayZone1 = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InPayZone1 == true)
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
                                Conn.InPayZone1 = false;
                            }
                            #endregion
                        }

                        #endregion

                        #region ' Parking Zone '

                        Conn.InParkingDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-193 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (268 * 196608), 2)) / 65536);
                        if (Conn.InParkingDist <= 4 && (mph <= 4))
                        {
                            #region ' Command and Display '

                            if (Conn.InParking == false)
                            {
                                string EstablishmentName = "^1Paid ^1Parking";
                                MsgPly("^6>>^7 Welcome to the " + EstablishmentName + "^7!", Conn.UniqueID);
                                if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                {
                                    #region ' Display '

                                    #region ' Display '
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7Welcome to the " + EstablishmentName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                   
                                        InSim.Send_BTN_CreateButton("^2Buy a ticket to entry in parking: ^1$50!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Buy", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                   
                                    #endregion

                                   

                                    Conn.DisplaysOpen = true;

                                    #endregion
                                }

                                Conn.InParking = true;
                            }

                            #endregion
                        }
                        else
                        {
                            #region ' Close Display '
                            if (Conn.InParking == true)
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
                                Conn.InParking = false;
                            }
                            #endregion
                        }

                        #endregion

                        #endregion

                        #region ' Houses '

                        #region ' Hriso's House '
                        //ready we1x
                        Conn.InHouse1Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-58 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-58 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        DeleteBTN(27, Conn.UniqueID);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse1 = false;
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        DeleteBTN(27, Conn.UniqueID);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse1 = false;
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Hriso's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);
                                                                                
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                            }
                                            #endregion


                                            Conn.DisplaysOpen = true;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        #region ' Martin's House '

                        Conn.InHouse2Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-19 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (19 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse2 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse2 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Elly's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Martin's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);
                                        
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                            }
                                            #endregion

                                            Conn.DisplaysOpen = true;
                                        }
                                        else
                                        {
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        #region ' Elly's House '

                        Conn.InHouse3Dist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (-235 * 196608), 2) + Math.Pow(Conn.CompCar.Y - (-86 * 196608), 2)) / 65536);
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
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromShop = false;
                                        Conn.JobToHouse3 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Job From Store '
                                if (Conn.JobFromStore == true)
                                {
                                    if (Conn.JobToHouse3 == true)
                                    {
                                        int prize = new Random().Next(200, 600);
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Completed a Job!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + prize);
                                        Conn.Cash += prize;
                                        Conn.TotalJobsDone += 1;
                                        Conn.JobFromStore = false;
                                        Conn.JobToHouse3 = false;
                                    }
                                    if (Conn.JobToHouse1 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Hriso's House!", Conn.UniqueID);
                                    }
                                    if (Conn.JobToHouse2 == true)
                                    {
                                        MsgPly("^6>>^7 Complete Error. Not Martin's House!", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Command and Display '
                                if (Conn.JobToHouse1 == true || Conn.JobToHouse2 == true || Conn.JobToHouse3 == true || Conn.JobToSchool == true)
                                {
                                    MsgPly("^6>>^7 You have to finish your jobs first.", Conn.UniqueID);
                                }
                                else if (Conn.IsSuspect == false && RobberUCID != Conn.UniqueID)
                                {
                                    if (Conn.IsOfficer == false && Conn.IsCadet == false && Conn.IsTowTruck == false)
                                    {
                                        string HouseName = "Elly's House";
                                        MsgPly("^6>>^7 Welcome to ^3" + HouseName + "!", Conn.UniqueID);

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == false)
                                        {
                                            #region ' Display '
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LIGHT, 8, 12, 85, 65, 110, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 35, 70, 50, 65, 111, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7Welcome to the " + HouseName + "!", Flags.ButtonStyles.ISB_LIGHT, 9, 70, 50, 65, 112, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^1Close [X]", Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 10, 86, 66, 113, Conn.UniqueID, 2, false);

                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^6Get a Job to ^2Lottery Shop!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 70, 68, 65, 120, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2Jobs ^7can be only done in Road Cars!", Flags.ButtonStyles.ISB_LEFT, 4, 40, 63, 65, 116, Conn.UniqueID, 2, false);
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
                                                MsgPly("^2!job ^7- Escort a Children to ^3Lottery.", Conn.UniqueID);
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
                                        MsgPly("^6>>^7 Come back later if you are off in duty.", Conn.UniqueID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Come back later if your not a Suspect!", Conn.UniqueID);
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

                        break;
                }

                #endregion

                #region ' Busted/Lost/Ran Away/Released '

                if (Conn.Chasee != -1)
                {
                    byte ChaseeIndex = 0;
                    #region ' UniqueID Identifier '
                    for (byte o = 0; o < Connections.Count; o++)
                    {
                        if (Conn.Chasee == Connections[o].UniqueID)
                        {
                            ChaseeIndex = o;
                        }
                    }
                    #endregion
                    int BustedDist = ((int)Math.Sqrt(Math.Pow(ChaseCon.CompCar.X - Conn.CompCar.X, 2) + Math.Pow(ChaseCon.CompCar.Y - Conn.CompCar.Y, 2)) / 65536);

                    #region ' Busted '
                    if (BustedDist < 6 && ChaseCon.CompCar.Speed / 91 < 5)
                    {
                        if (Conn.Busted == false)
                        {
                            if (Conn.BustedTimer == 0)
                            {
                                Conn.BustedTimer = 1;
                            }
                            if (Conn.BustedTimer == 5)
                            {
                                #region ' Connection List '
                                /*foreach (clsConnection Con in Connections)
                            {
                                if (Con.Chasee == ChaseCon.UniqueID)
                                {
                                    if (Con.JoinedChase == true)
                                    {
                                        if (Con.Busted == false)
                                        {
                                            MsgPly("^3!!!^7Suspect is now stopped the Vehicle^3!!!", Con.UniqueID);
                                            MsgPly("^6>>^7 Hit the Busted button or type ^2!busted", Con.UniqueID);
                                            Con.Busted = true;
                                        }
                                    }
                                    if (Con.JoinedChase == false && Con.Busted == false)
                                    {
                                        Con.Busted = true;
                                    }
                                }
                            }*/
                                #endregion

                                #region ' Connection '

                                MsgPly("^3!!!^7Suspect is now stopped the Vehicle^3!!!", Conn.UniqueID);
                                MsgPly("^6>>^7 Hit the Busted button or type ^2!busted", Conn.UniqueID);
                                Conn.Busted = true;

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region ' Get Away '
                        if (Conn.BustedTimer > 0)
                        {
                            if (Conn.Busted == true)
                            {
                                Conn.Busted = false;
                            }
                            Conn.BustedTimer = 0;
                        }
                        #endregion
                    }
                    #endregion

                    #region ' Suspect Lost '
                    if (BustedDist > 500)
                    {
                        if (Conn.InChaseProgress == true)
                        {
                            if (Connections[ChaseeIndex].CopInChase > 1)
                            {
                                if (Conn.JoinedChase == true)
                                {
                                    Conn.JoinedChase = false;
                                }

                                Conn.AutoBumpTimer = 0;
                                Conn.BumpButton = 0;
                                Conn.BustedTimer = 0;
                                Conn.Busted = false;
                                Conn.ChaseCondition = 0;
                                Conn.Chasee = -1;

                                #region ' Connection List '
                                foreach (clsConnection Con in Connections)
                                {
                                    if (Con.Chasee == ChaseCon.UniqueID)
                                    {
                                        if (ChaseCon.CopInChase == 1)
                                        {
                                            if (Con.JoinedChase == true)
                                            {
                                                Con.JoinedChase = false;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                Connections[ChaseeIndex].CopInChase -= 1;
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " lost sighting " + Connections[ChaseeIndex].NoColPlyName + "!");
                                MsgAll("   ^7Total Cops In Chase: " + Connections[ChaseeIndex].CopInChase);
                            }

                            else if (Connections[ChaseeIndex].CopInChase == 1)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                                MsgAll("   ^7Suspect Runs away from being chased!");

                                AddChaseLimit -= 1;
                                Conn.AutoBumpTimer = 0;
                                Conn.BumpButton = 0;
                                Conn.BustedTimer = 0;
                                Conn.Busted = false;
                                Connections[ChaseeIndex].CopInChase = 0;
                                Connections[ChaseeIndex].IsSuspect = false;
                                Connections[ChaseeIndex].ChaseCondition = 0;
                                Conn.ChaseCondition = 0;
                                Conn.Chasee = -1;
                                CopSirenShutOff();
                            }
                            Conn.InChaseProgress = false;
                        }
                    }
                    #endregion
                }

                #region ' Not paying Fines! '
                if (Conn.IsBeingBusted == true)
                {
                    if (kmh > 40)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                        MsgAll("  ^7For running away on ticket whilst being busted!");
                        Conn.Cash -= 5000;

                        #region ' In Connection List '

                        foreach (clsConnection i in Connections)
                        {
                            if (i.Chasee == Conn.UniqueID)
                            {
                                if (i.InFineMenu == true)
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

                        #region ' Close Region LOL '
                        DeleteBTN(30, Conn.UniqueID);
                        DeleteBTN(31, Conn.UniqueID);
                        DeleteBTN(32, Conn.UniqueID);
                        DeleteBTN(33, Conn.UniqueID);
                        DeleteBTN(34, Conn.UniqueID);
                        DeleteBTN(35, Conn.UniqueID);
                        DeleteBTN(36, Conn.UniqueID);
                        DeleteBTN(37, Conn.UniqueID);
                        DeleteBTN(38, Conn.UniqueID);
                        DeleteBTN(39, Conn.UniqueID);
                        DeleteBTN(40, Conn.UniqueID);
                        #endregion

                        Conn.ChaseCondition = 0;
                        Conn.AcceptTicket = 0;
                        Conn.TicketRefuse = 0;
                        Conn.CopInChase = 0;
                        Conn.IsBeingBusted = false;
                    }
                }
                #endregion

                #region ' Released by the cop! '
                if (Conn.InFineMenu == true)
                {
                    if (kmh > 40)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " released " + ChaseCon.NoColPlyName + "!");

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
                        DeleteBTN(30, Conn.UniqueID);
                        DeleteBTN(31, Conn.UniqueID);
                        DeleteBTN(32, Conn.UniqueID);
                        DeleteBTN(33, Conn.UniqueID);
                        DeleteBTN(34, Conn.UniqueID);
                        DeleteBTN(35, Conn.UniqueID);
                        DeleteBTN(36, Conn.UniqueID);
                        DeleteBTN(37, Conn.UniqueID);
                        DeleteBTN(38, Conn.UniqueID);
                        DeleteBTN(39, Conn.UniqueID);
                        DeleteBTN(40, Conn.UniqueID);
                        #endregion

                        if (Conn.InFineMenu == true)
                        {
                            Conn.InFineMenu = false;
                        }

                        Conn.Busted = false;
                    }
                }
                #endregion

                #endregion

                #region ' Lost Towing '

                if (Conn.Towee != -1)
                {
                    byte TowIndex = 0;
                    #region ' UniqueID Identifier '
                    for (byte o = 0; o < Connections.Count; o++)
                    {
                        if (Conn.Towee == Connections[o].UniqueID)
                        {
                            TowIndex = o;
                        }
                    }
                    #endregion

                    int TowDist = ((int)Math.Sqrt(Math.Pow(Connections[TowIndex].CompCar.X - Conn.CompCar.X, 2) + Math.Pow(Connections[TowIndex].CompCar.Y - Conn.CompCar.Y, 2)) / 65536);
                    if (TowDist > 250)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            if (Connections[TowIndex].IsBeingTowed == true)
                            {
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + Connections[TowIndex].NoColPlyName + "!");
                                Connections[TowIndex].IsBeingTowed = false;
                            }
                            Conn.Towee = -1;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                        }
                    }
                }

                #endregion

                #region ' Speed Trap '

                if (Conn.TrapSpeed > 0)
                {
                    int TrapDist = 0;
                    byte Speeders = 0;
                    if (Conn.TrapSetted == true)
                    {
                        TrapDist = ((int)Math.Sqrt(Math.Pow(Conn.CompCar.X - (Conn.TrapX * 196608), 2) + Math.Pow(Conn.CompCar.Y - (Conn.TrapY * 196608), 2)) / 65536);
                        if (TrapDist > 10)
                        {
                            MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                            Conn.TrapY = 0;
                            Conn.TrapX = 0;
                            Conn.TrapSpeed = 0;
                            Conn.TrapSetted = false;
                        }
                    }
                    for (byte i = 0; i < Connections.Count; i++)
                    {
                        if (Connections[i].PlayerID != 0)
                        {
                            Speeders = i;
                        }
                        var TrapVar = Connections[Speeders];
                        if (TrapVar.IsOfficer == false && TrapVar.IsCadet == false && TrapVar.IsSuspect == false && TrapVar.CompCar.Speed / 91 > Conn.TrapSpeed && TrapVar.InTrap == false)
                        {
                            TrapDist = ((int)Math.Sqrt(Math.Pow(TrapVar.CompCar.X - (Conn.TrapX * 196608), 2) + Math.Pow(TrapVar.CompCar.Y - (Conn.TrapY * 196608), 2)) / 65536);
                            if (TrapDist < 30 && Conn.TrapSetted == true)
                            {
                                int ExcessSpeed = (int)(TrapVar.CompCar.Speed / 91 - Conn.TrapSpeed / 2);
                                int SpeedFine = ExcessSpeed * 2;

                                MsgAll("^6>>^7 Over Speeding User Detected!");
                                MsgAll("^6>>^7 Speeding: " + TrapVar.NoColPlyName + " (" + TrapVar.Username + ")");
                                MsgAll("^6>>^7 Clocked at ^1" + TrapVar.CompCar.Speed / 91 + " kmh ^7/ ^1" + TrapVar.CompCar.Speed / 146 + " mph");
                                MsgAll("^6>>^7 " + TrapVar.NoColPlyName + " was fined ^1$" + SpeedFine);
                                MsgAll("^6>>^7 Speed Trapper: " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                                MsgAll("^6>>^7 " + Conn.NoColPlyName + " was rewarded ^2$" + (Convert.ToInt16(SpeedFine * 0.4)));
                                Conn.Cash += (Convert.ToInt16(SpeedFine * 0.4));
                                TrapVar.Cash -= SpeedFine;
                                TrapVar.InTrap = true;

                            }
                            else
                            {
                                if (TrapVar.InTrap == true)
                                {
                                    TrapVar.InTrap = false;
                                }
                            }
                        }
                    }
                }


                #endregion

            }
            catch { }
        }
    }
}