/*
Aston Historic Map
*/

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
        private void MCI_AstonHistoric4(int PLID)
        {
            try
            {
                /*
                 * if only the Player completes to connect via Try respond..........tooking 500 millisecond of internet speed
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

                #region ' Leaves Pit Interface '

                if (pathx >= -214 && pathx <= -210 && pathy >= -140 && pathy <= -69 && Conn.LeavesPitLane == 1)
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

                #region ' Wrong Exit fines '

                if (Conn.IsBeingTowed == false && Conn.InTowProgress == false && Conn.IsSuspect == false && Conn.InChaseProgress == false)
                {
                    // Pit EXIT
                    if (pathx >= -209 && pathx <= -205 && pathy >= -150 && pathy <= -147)
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
                    if (pathx >= -209 && pathx <= -205 && pathy >= -66 && pathy <= -63)
                    {
                        if (Conn.ExitZone == 0)
                        {
                            if (direction > 330 || direction < 20)
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

                #region ' Locations '

                // BOO SAME MUSIC AGAIN!

                if (node > 632 || node < 54)
                {
                    #region ' Service Station '
                    if (pathx >= -218 && pathx <= -205 && pathy >= -147 && pathy <= -64)
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
                        Conn.LastSeen = "Service Station, Aston";
                    }
                    #endregion

                    #region ' Historic Highway '
                    else
                    {
                        #region ' Speedlimit None '

                        if (kmh > 120)
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^1" + kmh + " kmh / -- kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^1" + mph + " mph / -- mph";
                            }
                            //Conn.IsSpeeder = 1;
                        }
                        else
                        {
                            if (Conn.KMHorMPH == 0)
                            {
                                Conn.SpeedBox = "^2" + kmh + " kmh / -- kmh";
                            }
                            else
                            {
                                Conn.SpeedBox = "^2" + mph + " mph / -- mph";
                            }

                            //Conn.IsSpeeder = 0;
                        }
                        #endregion

                        if (Conn.IsSpeeder == 1)
                        {
                            Conn.IsSpeeder = 0;
                        }

                        Conn.LocationBox = "^7Historic Highway";
                        Conn.Location = "Historic Highway";
                        Conn.LastSeen = "Historic Highway, Aston";
                    }
                    #endregion
                }



                #endregion
            }
            catch
            {
                MsgAll("^4|^7 Map Error. Please reload the Application!");
                MsgBox("> InSim is failure to load the Aston Historic Map please restart the InSim.");

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