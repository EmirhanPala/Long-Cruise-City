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
    public partial class Form1 : Form
    {

        // Main InSim object
        public InSimInterface InSim;
        const string Database = @"users";

        // Note - do NOT change the settings here. Use the settings.ini file instead!
        string AdminPW = "qwerty";
        ushort Port = 30404;
        string IPAddress = "127.0.0.1";
        string HostName = "host";
        byte GameMode = 0; // 0 if Demo, 1 if S1 and 2 if S2 [Automatic on IS_VER Packet]
        string InSimVer = "1.2";

        // Team Info
        //string Website = "^7Soon to be.. :)";
        string Website = "^7Emirhan PALA";
        string CruiseName = "^0[^7JC^0]^7 Cruise";
        string SeniorTag = "^0[LC]^7";
        string JuniorTag = "^0[LC]^7";
        string OfficerTag = "^0Officer^1»^7";
        string CadetTag = "^0Cadet^1»^7";
        string TowTruckTag = "^3TowTruck^1»^7";
        const string red = "^1•";
        const string yellow = "^3•";
        const string green = "^2•";

        #region ' InSim Load Forms '
        // These are the main lists that contain all Players and Connections (Being maintained automatically)
        public List<clsConnection> Connections = new List<clsConnection>();

        // Delegate for UI update (Example)
        delegate void dlgMSO(Packets.IS_MSO MSO);

        // Form constructor
        public Form1()
        {
            InitializeComponent();
            InSimConnect();	// Attempt to connect to InSim
            Console.Beep();
        }

        // Always call .Close() on application exit
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (clsConnection C in Connections)
            {
                if (C.FailCon == 0)
                {
                    FileInfo.SaveUser(C);
                }
            }
            MsgAll("^6>>^7 ^0[SC] ^7was shutted down!");
            Console.Beep();
            InSim.Close();
            Application.Exit();
        }

        // Use this method to connect to InSim so you are able to catch any exception that might occur
        public void InSimConnect()
        {
            try
            {
                #region ' Read Settings.ini '
                if (System.IO.File.Exists(@"settings.ini") == false)
                {
                    File.Create(@"settings.ini");
                }
                StreamReader Sr = new StreamReader("settings.ini");

                string line = null;
                while ((line = Sr.ReadLine()) != null)
                {
                    if (line.Substring(0, 5) == "Admin")
                    {

                        string[] StrMsg = line.Split('=');
                        AdminPW = StrMsg[1].Trim();
                    }

                    if (line.Substring(0, 4) == "Port")
                    {
                        string[] StrMsg = line.Split('=');
                        Port = ushort.Parse(StrMsg[1].Trim());
                    }

                    if (line.Substring(0, 2) == "IP")
                    {
                        string[] StrMsg = line.Split('=');
                        IPAddress = StrMsg[1].Trim();
                    }
                    if (line.Substring(0, 5) == "HName")
                    {
                        string[] StrMsg = line.Split('=');
                        //HostName = StrMsg[1].Trim();
                    }
                }
                Sr.Close();
                #endregion

                // InSim connection settings
                InSimSettings Settings = new InSimSettings(IPAddress, Port, 0, Flags.InSimFlags.ISF_MSO_COLS | Flags.InSimFlags.ISF_MCI, '!', 500, AdminPW, JuniorTag, 5);
                InSim = new InSimInterface(Settings);	// Initialize a new instance of InSimInterface with the settings specified above
                InSim.ConnectionLost += new InSimInterface.ConnectionLost_EventHandler(LostConnectionToInSim);	// Occurs when connection was lost due to an unknown reason
                InSim.Reconnected += new InSimInterface.Reconnected_EventHandler(ReconnectedToInSim);			// Occurs when connection was recovert automatically

                InitializeInSimEvents();				// Initialize packet receive events
                InSim.Connect();						// Attempt to connect to the InSim host 
            }
            catch { }
            finally
            {
                if (InSim.State == LFS_External.InSim.InSimInterface.InSimState.Connected)
                {
                    InSim.Request_NCN_AllConnections(255);
                    InSim.Request_NPL_AllPlayers(255);
                    InSim.Send_MST_Message("/wind=1");
                    InSim.Send_MST_Message("/wind=0");
                    foreach (clsConnection C in Connections)
                    {
                        ClearPen(C.Username);
                    }
                }
            }
        }

        // Occurs when connection was lost due to an unknown reason
        private void LostConnectionToInSim()
        {
            foreach (clsConnection C in Connections)
            {
                if (C.FailCon == 0)
                {
                    FileInfo.SaveUser(C);
                }
            }
            Console.Beep();
            MsgAll("^6>>^7 InSim has been lost connection to host and now Reconnecting.");
            MsgBox("> The Application lost its connection to host now Reconnecting.");
            InSim.Close();
            InSimConnect();
        }

        // Occurs when connection was recovert automatically
        private void ReconnectedToInSim()
        {

        }

        // You should only enable the events you need to gain maximum performance. All events are enable by default.
        private void InitializeInSimEvents()
        {
            try
            {
                // Client information
                InSim.NCN_Received += new LFS_External.InSim.InSimInterface.NCN_EventHandler(NCN_ClientJoinsHost);				// A new client joined the server.
                InSim.CNL_Received += new LFS_External.InSim.InSimInterface.CNL_EventHandler(CNL_ClientLeavesHost);				// A client left the server.
                InSim.CPR_Received += new LFS_External.InSim.InSimInterface.CPR_EventHandler(CPR_ClientRenames);				// A client changed name or plate.
                InSim.PLP_Received += new LFS_External.InSim.InSimInterface.PLP_EventHandler(PLP_PlayerGoesToGarage);			// A player goes to the garage (setup screen).
                InSim.NPL_Received += new LFS_External.InSim.InSimInterface.NPL_EventHandler(NPL_PlayerJoinsRace);				// A player join the race. If PLID already exists, then player leaves pit.
                InSim.TOC_Received += new LFS_External.InSim.InSimInterface.TOC_EventHandler(TOC_PlayerCarTakeOver);			// Car got taken over by an other player
                InSim.PIT_Received += new LFS_External.InSim.InSimInterface.PIT_EventHandler(PIT_PlayerStopsAtPit);				// A player stops for making a pitstop
                InSim.PLL_Received += new LFS_External.InSim.InSimInterface.PLL_EventHandler(PLL_PlayerLeavesRace);				// A player leaves the race (spectate)
                InSim.BFN_Received += new LFS_External.InSim.InSimInterface.BFN_EventHandler(BFN_PlayerRequestsButtons);		// A player pressed Shift+I or Shift+B
                InSim.BTC_Received += new LFS_External.InSim.InSimInterface.BTC_EventHandler(BTC_ButtonClicked);				// A player clicked a custom button
                InSim.BTT_Received += new LFS_External.InSim.InSimInterface.BTT_EventHandler(BTT_TextBoxOkClicked);				// A player submitted a custom textbox

                // Host and race information
                InSim.STA_Received += new LFS_External.InSim.InSimInterface.STA_EventHandler(STA_StateChanged);					// The server/race state changed
                InSim.MPE_Received += new LFS_External.InSim.InSimInterface.MPE_EventHandler(MPE_MultiplayerEnd);				// A host ends or leaves
                //InSim.RST_Received += new LFS_External.InSim.InSimInterface.RST_EventHandler(RST_RaceStart);					// A race starts
                InSim.VTN_Received += new LFS_External.InSim.InSimInterface.VTN_EventHandler(VTN_VoteNotify);					// A vote got called
                InSim.VTC_Received += new LFS_External.InSim.InSimInterface.VTC_EventHandler(VTC_VoteCanceled);					// A vote got canceled
                //InSim.CPP_Received += new LFS_External.InSim.InSimInterface.CPP_EventHandler(CPP_CameraPosition);				// LFS reporting camera position and state

                // Car tracking
                InSim.MCI_Received += new LFS_External.InSim.InSimInterface.MCI_EventHandler(MCI_CarInformation);				// Detailed car information packet (max 8 per packet)

                // Other
                InSim.MSO_Received += new LFS_External.InSim.InSimInterface.MSO_EventHandler(MSO_MessageOut);					// Player chat and system messages.
                InSim.VER_Received += new LFS_External.InSim.InSimInterface.VER_EventHandler(VER_InSimVersionInformation);		// InSim version information
                //InSim.CON_Received += new LFS_External.InSim.InSimInterface.CON_EventHandler(CON_CarContact);       //Car Contact between cars
            }
            catch { }
        }
        #endregion

        #region ' Utils '

        // Methods for automatically update Players[] and Connection[] lists
        private void RemoveFromConnectionsList(byte ucid)
        {
            // Copy of item to remove
            clsConnection RemoveItem = new clsConnection();

            // Check what item the connection had
            foreach (clsConnection Conn in Connections)
            {
                if (ucid == Conn.UniqueID)
                {
                    // Copy item (Can't delete it here)
                    RemoveItem = Conn;
                    continue;
                }
            }

            // Remove item
            Connections.Remove(RemoveItem);
        }

        private void AddToConnectionsList(Packets.IS_NCN NCN)
        {
            bool InList = false;

            // Check of connection is already in the list
            foreach (clsConnection Conn in Connections)
            {
                if (Conn.UniqueID == NCN.UCID)
                {
                    InList = true;
                    continue;
                }
            }

            // If not, add it
            if (!InList)
            {
                try
                {
                    // Assign values of new connnnection.
                    clsConnection NewConn = new clsConnection();

                    NewConn.UniqueID = NCN.UCID;
                    NewConn.Username = NCN.UName;
                    NewConn.PlayerName = NCN.PName;
                    NewConn.IsAdmin = NCN.Admin;
                    NewConn.Flags = NCN.Flags;
                    if (NewConn.Username != "")
                    {
                        #region ' File Global '
                        // Your Code File Here! init
                        NewConn.Cash = FileInfo.GetUserCash(NCN.UName);
                        NewConn.BankBalance = FileInfo.GetUserBank(NCN.UName);
                        NewConn.Cars = FileInfo.GetUserCars(NCN.UName);
                        NewConn.TotalHealth = FileInfo.GetUserHealth(NCN.UName);
                        NewConn.TotalDistance = FileInfo.GetUserDistance(NCN.UName);
                        NewConn.TotalJobsDone = FileInfo.GetUserJobsDone(NCN.UName);

                        NewConn.Electronics = FileInfo.GetUserElectronics(NCN.UName);
                        NewConn.Furniture = FileInfo.GetUserFurniture(NCN.UName);

                        NewConn.IsSuperAdmin = FileInfo.GetUserAdmin(NCN.UName);

                        NewConn.IsModerator = FileInfo.IsMember(NCN.UName);
                        NewConn.CanBeOfficer = FileInfo.CanBeOfficer(NCN.UName);
                        NewConn.CanBeCadet = FileInfo.CanBeCadet(NCN.UName);
                        NewConn.CanBeTowTruck = FileInfo.CanBeTowTruck(NCN.UName);

                        NewConn.Interface = FileInfo.GetInterface(NCN.UName);
                        NewConn.InGameIntrfc = FileInfo.GetInterface2(NCN.UName);
                        NewConn.KMHorMPH = FileInfo.GetSpeedo(NCN.UName);
                        NewConn.Odometer = FileInfo.GetOdometer(NCN.UName);
                        NewConn.Counter = FileInfo.GetCounter(NCN.UName);
                        NewConn.CopPanel = FileInfo.GetCopPanel(NCN.UName);

                        NewConn.LastRaffle = FileInfo.GetUserLastRaffle(NCN.UName);
                        NewConn.LastLotto = FileInfo.GetUserLastLotto(NCN.UName);

                        NewConn.Renting = FileInfo.GetUserRenting(NCN.UName);
                        NewConn.Rentee = FileInfo.GetUserRentee(NCN.UName);
                        NewConn.Renter = FileInfo.GetUserRenter(NCN.UName);
                        NewConn.Renterr = FileInfo.GetUserRenterr(NCN.UName);
                        NewConn.Rented = FileInfo.GetUserRented(NCN.UName);
                        #endregion
                    }
                    else
                    {
                        #region ' Hoster '
                        NewConn.Cash = 3500;
                        NewConn.BankBalance = 0;
                        NewConn.Cars = "UF1 XFG XRG";
                        NewConn.TotalHealth = 100;
                        NewConn.TotalDistance = 0;
                        NewConn.TotalJobsDone = 0;

                        NewConn.Electronics = 0;
                        NewConn.Furniture = 0;

                        NewConn.IsSuperAdmin = 0;

                        NewConn.IsModerator = 0;
                        NewConn.CanBeOfficer = 0;
                        NewConn.CanBeCadet = 0;
                        NewConn.CanBeTowTruck = 0;

                        NewConn.Interface = 3;
                        NewConn.InGameIntrfc = 3;
                        NewConn.KMHorMPH = 3;
                        NewConn.Odometer = 3;
                        NewConn.Counter = 3;
                        NewConn.CopPanel = 3;

                        NewConn.LastRaffle = 0;
                        NewConn.LastLotto = 0;


                        NewConn.FailCon = 1;
                        #endregion
                    }
                    NewConn.BankBonusTimer = 3600;

                    NewConn.Chasee = -1;
                    NewConn.IsOfficer = false;
                    NewConn.IsCadet = false;
                    NewConn.IsSuspect = false;
                    NewConn.InChaseProgress = false;
                    NewConn.AutoBumpTimer = 0;
                    NewConn.ChaseCondition = 0;
                    NewConn.BumpButton = 0;
                    NewConn.BustedTimer = 0;
                    NewConn.Busted = false;
                    NewConn.IsBeingBusted = false;

                    NewConn.Towee = -1;
                    NewConn.InTowProgress = false;
                    NewConn.IsBeingTowed = false;

                    NewConn.InModerationMenu = 0;
                    NewConn.ModReason = "";
                    NewConn.ModReasonSet = false;
                    NewConn.ModUsername = "";
                    NewConn.ModerationWarn = 0;

                    Connections.Add(NewConn);
                }
                catch
                {
                    clsConnection NewConn = new clsConnection();
                    NewConn.PlayerName = NCN.PName;
                    NewConn.Username = NCN.UName;
                    NewConn.FailCon = 1;
                    Connections.Add(NewConn);
                    InSim.Send_MST_Message("/kick " + NewConn.Username);
                }
            }
        }

        #region ' PLID and UCID Identifier '
        /// <summary>
        /// Returns an index value for Connections[] that corresponds with the UniqueID of a connection
        /// </summary>
        /// <param name="UNID">UCID to find</param>
        public int GetConnIdx(int UNID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].UniqueID == UNID) { return i; }
            }
            return 0;
        }
        public int GetConnIdx2(int PLID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].PlayerID == PLID) { return i; }
            }
            return 0;
        }

        /// <summary>Returns true if method needs invoking due to threading</summary>
        private bool DoInvoke()
        {
            foreach (Control c in this.Controls)
            {
                if (c.InvokeRequired) return true;
                break;	// 1 control is enough
            }
            return false;
        }
        #endregion

        #endregion

        #region ' Packet receive events '

        // Player chat and system messages.
        private void MSO_MessageOut(Packets.IS_MSO MSO)
        {
            try
            {
                // Invoke method due to threading. Add this line to any receive event before updating the GUI. Just like in this example, you only have to add a new delegate with the right packet parameter and adjust this line in the new method.
                if (DoInvoke()) { object p = MSO; this.Invoke(new dlgMSO(MSO_MessageOut), p); return; }

                var Conn = Connections[GetConnIdx(MSO.UCID)];
                string Msg = MSO.Msg.Substring(MSO.TextStart, (MSO.Msg.Length - MSO.TextStart));
                string[] StrMsg = Msg.Split(' ');

                #region ' Box Remove Colors String '
                string boxy;
                boxy = MSO.Msg;
                if (boxy.Contains("^0"))
                {
                    boxy = boxy.Replace("^0", "");
                }
                if (boxy.Contains("^1"))
                {
                    boxy = boxy.Replace("^1", "");
                }
                if (boxy.Contains("^2"))
                {
                    boxy = boxy.Replace("^2", "");
                }
                if (boxy.Contains("^3"))
                {
                    boxy = boxy.Replace("^3", "");
                }
                if (boxy.Contains("^4"))
                {
                    boxy = boxy.Replace("^4", "");
                }
                if (boxy.Contains("^5"))
                {
                    boxy = boxy.Replace("^5", "");
                }
                if (boxy.Contains("^6"))
                {
                    boxy = boxy.Replace("^6", "");
                }
                if (boxy.Contains("^7"))
                {
                    boxy = boxy.Replace("^7", "");
                }
                if (boxy.Contains("^8"))
                {
                    boxy = boxy.Replace("^8", "");
                }
                if (boxy.Contains("| "))
                {
                    boxy = boxy.Replace("| ", "");
                }
                MsgBox(boxy);
                #endregion

                #region ' Spam and Swear Check '

                #region ' Swear Filter '

                // Sorry for swearing, must be done though :P
                if (MSO.UCID == 0 == false && Conn.PlayerName == HostName == false)
                {
                    int Swears = 0;
                    string SwearFilter = boxy.ToLower();

                    #region ' Swear Region '
                    if (SwearFilter.Contains("wank"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("d!ck"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("dick"))
                    {
                        Swears += 1;
                    }

                    if (SwearFilter.Contains("f*ck"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("f**k"))
                    {
                        Swears += 1;
                    }

                    if (SwearFilter.Contains("sh!t"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("shiit"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("bitch"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("b1tch"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("b!tch"))
                    {
                        Swears += 1;
                    }

                    if (SwearFilter.Contains("cock"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("c0ck"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("arse"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("penis"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("pen!s"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("pen1s"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("a$$hole"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("a$$"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("clit"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("cl1t"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("vagina"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("pussy"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("tits"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("titties"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("pr1ck"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("nigger"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("nigga"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("nigg@"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("n1gger"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("n1gga"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("n1gg@"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("can i be admin"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("can i be a admin"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("i want to be admin"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("i want to be a admin"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("fuc*"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("fuck"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("shit") || SwearFilter.Contains("schwanzlutscher") || SwearFilter.Contains("schlampe") || SwearFilter.Contains("nutte"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("asshole") || SwearFilter.Contains("arschloch"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("piss"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("wank"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("cunt"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("bastard"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("bollocks"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("twat"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("prick") || SwearFilter.Contains("stechen") || SwearFilter.Contains("arsch") || SwearFilter.Contains("stich") || SwearFilter.Contains("schwanz") || SwearFilter.Contains("opfa") || SwearFilter.Contains("kдskop"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("retard"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("bumsen") || SwearFilter.Contains("vцgeln") || SwearFilter.Contains("stoЯen") || SwearFilter.Contains("ScheiЯe") || SwearFilter.Contains("verdammt"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("shyt"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("rahtid"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("rass"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("klaat"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("claat"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("fdp"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("poha"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("puta"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("mierda"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("chupa-me"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("merda"))
                    {
                        Swears += 1;
                    }
                    if (SwearFilter.Contains("porra"))
                    {
                        Swears += 1;
                    }
                    #endregion

                    if (Swears > 0 && Msg.StartsWith("!") == false)
                    {
                        Conn.Cash -= (Swears * 700);
                        Conn.Swear += 1;
                        if (Conn.Swear > 0 && Conn.Swear < 3)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7was fined ^1$" + (Swears * 700) + " ^7for swearing");
                            Conn.SwearTime = 8;
                        }
                        else
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7was fined ^1$" + (Swears * 700) + " ^7 and kicked for swearing");
                            KickID(Conn.Username);
                        }
                    }
                }

                #endregion

                #region ' Spam Filter '
                // Spam Filter
                if (Msg.StartsWith("!") == false)
                {
                    if (MSO.UCID == 0 == false && Conn.PlayerName == HostName == false)
                    {
                        Conn.Spam += 1;
                        Conn.SpamTime = 8;
                        if (Conn.Spam == 4)
                        {
                            Conn.Cash -= 700;
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7was fined ^1$700 ^7for spamming");
                        }
                        else if (Conn.Spam == 7)
                        {
                            Conn.Cash -= 700;
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ^7was fined ^1$700 ^7and kicked for spamming");
                            KickID(Conn.Username);
                        }
                    }
                }
                #endregion

                #endregion

                #region ' Penalty Check '
                if (MSO.Msg.Contains(" : DRIVE-THROUGH PENALTY"))
                {
                    if (Conn.PlayerName == HostName)
                    {
                        string Name;

                        Name = Msg;
                        Name = Name.Replace(" : DRIVE-THROUGH PENALTY", "");
                        foreach (clsConnection C in Connections)
                        {
                            if (Name.Contains(C.PlayerName))
                            {
                                int RandomFines = new Random().Next(500, 700);
                                MsgAll("^6>>^7 " + C.NoColPlyName + " was fined ^1$" + RandomFines + " ^7for speeding.");
                                C.Cash -= RandomFines;
                                C.Penalty = 8;
                            }
                        }
                    }
                }

                if (MSO.Msg.Contains(" : STOP-GO PENALTY"))
                {
                    if (Conn.PlayerName == HostName)
                    {
                        string Name;

                        Name = Msg;
                        Name = Name.Replace(" : STOP-GO PENALTY", "");
                        foreach (clsConnection C in Connections)
                        {
                            if (Name.Contains(C.PlayerName))
                            {
                                int RandomFines = new Random().Next(500, 700);
                                MsgAll("^6>>^7 " + C.NoColPlyName + " was fined ^1$" + RandomFines + " ^7and spected for speeding.");
                                SpecID(C.PlayerName);
                                SpecID(C.Username);
                                C.Cash -= RandomFines;
                                C.Penalty = 0;
                            }
                        }
                    }
                }
                #endregion

                #region ' Chat command '
                if (Msg.StartsWith("!"))
                {
                    string cmp = StrMsg[0].Remove(0, 1);
                    if (Conn.WaitCMD == 0)
                    {
                        foreach (CommandList CL in Commands)
                        {
                            if (cmp == CL.CommandArg.Command)
                            {
                                CL.MethodInf.Invoke(this, new object[] { Msg, StrMsg, MSO });
                                return;
                            }
                        }
                        MsgPly("^6>>^7 Invalid Command. ^2!help ^7for help.", MSO.UCID);
                        Conn.WaitCMD = 4;
                    }
                    else
                    {
                        MsgPly("^6>>^7 You have to wait ^2" + Conn.WaitCMD + " ^7second(s) to start command", MSO.UCID);
                    }
                }
                #endregion
            }
            catch { }
        }

        // A new client joined the server.
        private void NCN_ClientJoinsHost(Packets.IS_NCN NCN)
        {
            try
            {
                FileInfo.NewCruiser(NCN.UName, NCN.PName);
                AddToConnectionsList(NCN);                  // Update Connections[] list (don't remove this line!)
                var Conn = Connections[GetConnIdx(NCN.UCID)];

                ClearPen(NCN.UName);

                #region ' Special PlayerName Colors Remove '
                Conn.NoColPlyName = NCN.PName;
                if (Conn.NoColPlyName.Contains("^0"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^0", "");
                }
                if (Conn.NoColPlyName.Contains("^1"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^1", "");
                }
                if (Conn.NoColPlyName.Contains("^2"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^2", "");
                }
                if (Conn.NoColPlyName.Contains("^3"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^3", "");
                }
                if (Conn.NoColPlyName.Contains("^4"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^4", "");
                }
                if (Conn.NoColPlyName.Contains("^5"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^5", "");
                }
                if (Conn.NoColPlyName.Contains("^6"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^6", "");
                }
                if (Conn.NoColPlyName.Contains("^7"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^7", "");
                }
                if (Conn.NoColPlyName.Contains("^8"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^8", "");
                }
                #endregion

                #region ' Get User Permanent Ban '

                if (FileInfo.GetUserPermBan(NCN.UName.ToLower()) == 1)
                {
                    Message("/msg ^6>>^7 " + Conn.NoColPlyName + " is on the ban list.");
                    AdmBox("> " + Conn.NoColPlyName + " is on the ban list.");
                    BanID(Conn.Username, 0);
                }

                #endregion

                #region ' Retrieve HostName '
                if (NCN.UCID == 0 && NCN.UName == "")
                {
                    HostName = NCN.PName;
                }

                #region ' Check Player Name if contains host '
                if (NCN.UCID == 0 && NCN.UName == "" && NCN.PName == HostName == false)
                {
                    if (NCN.PName.Contains(HostName))
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " was kicked for having Host Name!");
                        KickID(NCN.UName);
                    }
                }
                #endregion

                #endregion

                #region ' New Cruiser '

                if (Conn.TotalDistance == -1)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is our new member in the Community!");
                    Conn.TotalDistance += 1;
                }

                #endregion

                MsgPly("^6>>^7 Welcome " + Conn.PlayerName + " ^7to " + CruiseName + "^7!", NCN.UCID);
                

                #region ' for fun '
                /*{
                    if (Conn.Username == "crazyboy232")
                    {
                        Message("/msg ^6>> ^7Hello ^2EvErYbOdY!");
                    }
                }
                if (Conn.Username == "kiril768")
                {
                    Message("/msg ^6>> ^7Hello ^2EvErYbOdY!");
                }*/
                #endregion

                #region ' Check Tags of Team '

                if (NCN.PName == HostName == false)
                {
                    if (NCN.Admin == 1)
                    {
                        if (Conn.IsSuperAdmin == 1)
                        {
                            MsgAll("^6>> " + Conn.PlayerName + " ^7is now logged in as Admin!");
                            AdmBox("> " + Conn.NoColPlyName + " (" + NCN.UName + ") is now logged as Admin.");
                        }
                        else
                        {
                            MsgAll("^6>> " + Conn.PlayerName + " ^7is not a admin!");
                            AdmBox("> " + Conn.NoColPlyName + " (" + NCN.UName + ") tried to access as Admin! but kicked.");
                            KickID(NCN.UName);
                        }
                    }
                }

                if (NCN.PName.Contains(SeniorTag) || NCN.PName.Contains(JuniorTag))
                {
                    if (Conn.IsModerator == 0 && Conn.IsSuperAdmin == 0 && Conn.IsAdmin == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.PlayerName + " is not a Moderator/Admin!");
                        MsgPly("^6>>^7 Please Remove the tag and come back!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(OfficerTag))
                {
                    if (Conn.CanBeOfficer == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.PlayerName + " is not a Officer!");
                        MsgPly("^6>>^7 Please Remove the tag and come back!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(CadetTag))
                {
                    if (Conn.CanBeCadet == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.PlayerName + " is not a Cadet!");
                        MsgPly("^6>>^7 Please Remove the tag!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                    else if (Conn.CanBeCadet == 2)
                    {
                        MsgPly("^6>>^7 Use " + OfficerTag + " when going duty!", NCN.UCID);
                    }
                    else if (Conn.CanBeCadet == 3)
                    {
                        MsgAll("^6>>^7 " + Conn.PlayerName + " is not a Cadet!");
                        MsgPly("^6>>^7 Please Remove the tag!", NCN.UCID);
                        MsgPly("^6>>^7 You are already revoked as Cadet!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }

                if (NCN.PName.Contains(TowTruckTag))
                {
                    if (Conn.CanBeTowTruck == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a TowTruck!");
                        MsgPly("^6>>^7 Please Remove the tag!", NCN.UCID);
                        KickID(NCN.UName);
                    }
                }
                #endregion

                #region ' Interface Loader '
                if (Conn.Interface == 0)
                {
                    Conn.WaitIntrfc = 0;
                }
                else
                {
                    InSim.Send_BTN_CreateButton(CruiseName, Flags.ButtonStyles.ISB_C4, 10, 70, 190, 0, 1, Conn.UniqueID, 2, false);
                }
                #endregion

                #region ' Welcome Screen '
                /*

                InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK, 25, 125, 26, 20, 232, NCN.UCID, 40, false);

                InSim.Send_BTN_CreateButton("^7WELCOME TO " + CruiseName + " ^6" + InSimVer, Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK, 6, 60, 20, 20, 233, NCN.UCID, 40, false);

                InSim.Send_BTN_CreateButton("^7Here on " + CruiseName + "^7, ^7you can cruise as you do in real life, driving around the track, earn cash on jobs and etc.", Flags.ButtonStyles.ISB_LEFT, 5, 125, 30, 20, 234, NCN.UCID, 40, false);
                InSim.Send_BTN_CreateButton("^7Please make sure to respect all players, as inappropriate comments/actions will be punished.", Flags.ButtonStyles.ISB_LEFT, 5, 125, 34, 20, 235, NCN.UCID, 40, false);
                InSim.Send_BTN_CreateButton("^1VIPS (Moderators) ^7have the power to warn, fine, spec, kick and ban you, so make sure to follow the rules.", Flags.ButtonStyles.ISB_LEFT, 5, 125, 38, 20, 236, NCN.UCID, 40, false);
                InSim.Send_BTN_CreateButton("^7We have events in 8pm bulgarian time. The events are drift, drag, or derby! ^1Don't be late!!!", Flags.ButtonStyles.ISB_LEFT, 5, 125, 42, 20, 237, NCN.UCID, 40, false);

                InSim.Send_BTN_CreateButton("^7Above all, have fun! For more info, visit us at ^2" + Website, Flags.ButtonStyles.ISB_LEFT, 5, 125, 46, 20, 238, NCN.UCID, 40, false);
                InSim.Send_BTN_CreateButton("^1CLOSE [X]", Flags.ButtonStyles.ISB_LEFT | Flags.ButtonStyles.ISB_DARK | Flags.ButtonStyles.ISB_CLICK, 6, 13, 20, 132, 239, NCN.UCID, 40, false);
                */

                #endregion

                Conn.Location = "Spectators";
                Conn.LastSeen = "Spectators, In Game";
                Conn.LocationBox = "^7Spectators";
                Conn.SpeedBox = "";
            }
            catch { }
        }

        // A client left the server.
        private void CNL_ClientLeavesHost(Packets.IS_CNL CNL)
        {
            try
            {
                #region ' variables '

                clsConnection Conn = Connections[GetConnIdx(CNL.UCID)];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(CNL.UCID)].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[GetConnIdx(CNL.UCID)].Towee)];
                #endregion

                #region ' In Game Neccesities '
                if (Conn.InGame == 1)
                {
                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 11)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 7000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 12)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 8000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 13)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 9000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 14)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 10000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 15)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 12000 + "%");
                    }
                    #endregion

                    

                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    #region ' Job Remove '

                    #region ' Job From Store '

                    if (Conn.JobFromStore == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromStore = false;
                    }

                    #endregion

                    #region ' Job From Shop Remove '
                    if (Conn.JobFromShop == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromShop = false;
                    }
                    #endregion

                    #region ' Job From House 1 '

                    if (Conn.JobFromHouse1 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse1 = false;
                    }

                    #endregion

                    #region ' Job From House 2 '

                    if (Conn.JobFromHouse2 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse2 = false;
                    }

                    #endregion

                    #region ' Job From House 3 '

                    if (Conn.JobFromHouse3 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse3 = false;
                    }

                    #endregion

                    #endregion

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion

                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
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
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " sighting lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + TowCon.NoColPlyName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " leaves whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + C.NoColPlyName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + NoColPlyName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                #region ' Disconnect Reason '
                /*switch (CNL.Reason.ToString())
                {
                    case "LEAVR_DISCO": // Discon
                        MsgAll("^6>>^7 DISCONNECTED : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;

                    case "LEAVR_TIMEOUT": // Time
                        MsgAll("^6>>^7 TIMED OUT : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;

                    case "LEAVR_LOSTCONN": // Lost Con
                        MsgAll("^6>>^7 LOST CONNECTION : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;

                    case "LEAVR_KICKED": // Kicked
                        MsgAll("^6>>^7 KICKED : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;

                    case "LEAVR_BANNED": // Banned
                        MsgAll("^6>>^7 BANNED : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;

                    case "LEAVR_SECURITY": // OOS
                        MsgAll("^6>>^7 OOS : " + Conn.NoColPlyName + " (" + Conn.Username + ")");
                        break;
                }*/
                #endregion

                #region ' Save File Info '
                if (Conn.IsAdmin == 1)
                {
                    AdmBox("> " + Conn.NoColPlyName + " (" + Conn.Username + ") logs out as Admin.");
                }
                if (Connections[GetConnIdx(CNL.UCID)].FailCon == 0)
                {
                    FileInfo.SaveUser(Connections[GetConnIdx(CNL.UCID)]);
                    MsgPly("^6>>^7 Your stats has been saved.", CNL.UCID);
                }
                #endregion

                RemoveFromConnectionsList(CNL.UCID);		// Update Connections[] list
            }
            catch { }
        }

        // A client changed name or plate.
        private void CPR_ClientRenames(Packets.IS_CPR CPR)
        {
            try
            {
                clsConnection Conn = Connections[GetConnIdx(CPR.UCID)];
                clsConnection ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(CPR.UCID)].Chasee)];
                clsConnection TowCon = Connections[GetConnIdx(Connections[GetConnIdx(CPR.UCID)].Towee)];
                #region ' Special PlayerName Colors Remove '
                Conn.NoColPlyName = CPR.PName;
                if (Conn.NoColPlyName.Contains("^0"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^0", "");
                }
                if (Conn.NoColPlyName.Contains("^1"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^1", "");
                }
                if (Conn.NoColPlyName.Contains("^2"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^2", "");
                }
                if (Conn.NoColPlyName.Contains("^3"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^3", "");
                }
                if (Conn.NoColPlyName.Contains("^4"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^4", "");
                }
                if (Conn.NoColPlyName.Contains("^5"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^5", "");
                }
                if (Conn.NoColPlyName.Contains("^6"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^6", "");
                }
                if (Conn.NoColPlyName.Contains("^7"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^7", "");
                }
                if (Conn.NoColPlyName.Contains("^8"))
                {
                    Conn.NoColPlyName = Conn.NoColPlyName.Replace("^8", "");
                }
                #endregion

                #region ' Check Host Name '
                if (CPR.UCID == 0 == false)
                {
                    if (CPR.PName.Contains(HostName))
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " was kicked for having Host Name!");
                        KickID(Conn.Username);
                    }
                }
                #endregion

                #region ' Check Officer/Cadet Names '

                #region ' Officer '
                if (CPR.PName.Contains(OfficerTag))
                {
                    if (Conn.CanBeOfficer == 1)
                    {
                        if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {
                            if (CPR.Plate == "Police")
                            {
                                #region ' Duty Officer '
                                if (Conn.IsOfficer == false)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2DUTY ^7as Officer!");

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is disabled", CPR.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", CPR.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is enabled", CPR.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", CPR.UCID);
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
                                #endregion
                            }
                            else
                            {
                                #region ' Remove '
                                if (Conn.InChaseProgress == true)
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
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsOfficer == true)
                                    {
                                        MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                                        TotalOfficers -= 1;
                                        Conn.LastName = "";
                                        Conn.IsOfficer = false;
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsOfficer == true)
                                    {
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                                        TotalOfficers -= 1;
                                        Conn.LastName = "";
                                        Conn.IsOfficer = false;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Cancel your current job first!", CPR.UCID);
                        }
                    }
                    else
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a Police Officer!");
                        MsgPly("^6>>^7 Please remove the tag!", CPR.UCID);
                        KickID(Conn.Username);
                    }
                }
                else
                {
                    #region ' Remove '
                    if (Conn.IsOfficer == true)
                    {
                        if (Conn.InChaseProgress == true)
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
                                ChaseCon.ChaseCondition = 0;
                                ChaseCon.CopInChase = 0;
                                ChaseCon.IsSuspect = false;
                                Conn.ChaseCondition = 0;
                                CopSirenShutOff();
                            }
                            #endregion

                            #region ' Remove Cop Panel '

                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                            TotalOfficers -= 1;
                            Conn.LastName = "";
                            Conn.IsOfficer = false;
                        }
                        else
                        {
                            #region ' Remove Cop Panel '

                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Officer!");
                            Conn.LastName = "";
                            TotalOfficers -= 1;
                            Conn.IsOfficer = false;
                        }
                    }
                    #endregion
                }
                #endregion

                #region ' Cadet '
                if (CPR.PName.Contains(CadetTag))
                {
                    if (Conn.CanBeCadet == 1)
                    {
                        if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {
                            if (CPR.Plate == "Police")
                            {
                                #region ' Duty Cadet '
                                if (Conn.IsCadet == false)
                                {
                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2DUTY ^7as Cadet!");

                                    if (Conn.CopPanel == 0)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is disabled", CPR.UCID);
                                        MsgPly("  ^7To Enable them by typing ^2!coppanel", CPR.UCID);
                                    }
                                    else if (Conn.CopPanel == 1)
                                    {
                                        MsgPly("^6>>^7 Your Panel Click is enabled", CPR.UCID);
                                        MsgPly("  ^7To Disable them by typing ^2!coppanel", CPR.UCID);
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
                                #endregion
                            }
                            else
                            {
                                #region ' Remove '
                                if (Conn.InChaseProgress == true)
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
                                        ChaseCon.ChaseCondition = 0;
                                        ChaseCon.CopInChase = 0;
                                        ChaseCon.IsSuspect = false;
                                        Conn.ChaseCondition = 0;
                                        CopSirenShutOff();
                                    }
                                    #endregion

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsCadet == true)
                                    {
                                        MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                                        Conn.LastName = "";
                                        Conn.IsCadet = false;
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);

                                    #region ' Remove Cop Panel '

                                    DeleteBTN(15, Conn.UniqueID);
                                    DeleteBTN(16, Conn.UniqueID);
                                    DeleteBTN(17, Conn.UniqueID);
                                    DeleteBTN(18, Conn.UniqueID);
                                    DeleteBTN(19, Conn.UniqueID);
                                    DeleteBTN(20, Conn.UniqueID);
                                    DeleteBTN(21, Conn.UniqueID);
                                    DeleteBTN(22, Conn.UniqueID);

                                    #endregion

                                    #region ' Restore some BTN '
                                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                    {
                                        if (Conn.InShop == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        if (Conn.InStore == true)
                                        {
                                            if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                            {
                                                InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    #endregion

                                    if (Conn.IsCadet == true)
                                    {
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                                        Conn.LastName = "";
                                        Conn.IsCadet = true;
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Cancel your current job first!", CPR.UCID);
                        }
                    }
                    #region ' False or Removed Cadet '
                    else if (Conn.CanBeCadet == 2)
                    {
                    }
                    else
                    {
                        #region ' Not cadet or removed '
                        if (Conn.CanBeCadet == 0)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a Police Cadet!");
                            MsgPly("^6>>^7 Please remove the tag!", CPR.UCID);
                            KickID(Conn.Username);
                        }
                        else if (Conn.CanBeCadet == 3)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a Police Cadet!");
                            MsgPly("^6>>^7 Please remove the tag!", CPR.UCID);
                            KickID(Conn.Username);
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region ' Remove '
                    if (Conn.IsCadet == true)
                    {
                        if (Conn.InChaseProgress == true)
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
                                ChaseCon.ChaseCondition = 0;
                                ChaseCon.CopInChase = 0;
                                ChaseCon.IsSuspect = false;
                                Conn.ChaseCondition = 0;
                                CopSirenShutOff();
                            }
                            #endregion

                            #region ' Remove Cop Panel '
                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cadet!");
                            Conn.LastName = "";
                            Conn.IsCadet = false;
                        }
                        else
                        {
                            #region ' Remove Cop Panel '

                            DeleteBTN(15, Conn.UniqueID);
                            DeleteBTN(16, Conn.UniqueID);
                            DeleteBTN(17, Conn.UniqueID);
                            DeleteBTN(18, Conn.UniqueID);
                            DeleteBTN(19, Conn.UniqueID);
                            DeleteBTN(20, Conn.UniqueID);
                            DeleteBTN(21, Conn.UniqueID);
                            DeleteBTN(22, Conn.UniqueID);

                            #endregion

                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as a Cop!");
                            Conn.LastName = "";
                            Conn.IsCadet = false;
                        }
                    }
                    #endregion
                }
                #endregion

                #endregion

                #region ' tow truck check '
                if (CPR.PName.Contains(TowTruckTag))
                {
                    if (Conn.CanBeTowTruck == 1)
                    {
                        if (Conn.CurrentCar == "FBM")
                        {
                            MsgPly("^6>>^7 Cannot get duty whilst using FBM!", CPR.UCID);
                        }

                        else if (Conn.JobToHouse1 == false && Conn.JobToHouse2 == false && Conn.JobToHouse3 == false && Conn.JobToSchool == false)
                        {

                            if (CPR.Plate == "Tow")
                            {
                                if (Conn.IsTowTruck == false)
                                {
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

                                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^2ON-DUTY ^7as Tow Truck!");

                                    if (Conn.CalledRequest == true)
                                    {
                                        Conn.CalledRequest = false;
                                    }

                                    Conn.LastName = Conn.NoColPlyName;
                                    Conn.IsTowTruck = true;
                                }
                            }
                            else
                            {
                                #region ' Remove Some '
                                if (Conn.IsTowTruck == true)
                                {
                                    if (Conn.InTowProgress == true)
                                    {
                                        MsgAll("^6>>^7 " + Conn.LastName + " stopped towing " + TowCon.NoColPlyName + "!");
                                        TowCon.IsBeingTowed = false;

                                        #region ' Restore some BTN '
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.InShop == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.InStore == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                        }
                                        #endregion

                                        Conn.Towee = -1;
                                        Conn.InTowProgress = false;
                                        CautionSirenShutOff();
                                        MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                                        Conn.IsTowTruck = false;
                                        Conn.LastName = "";
                                    }
                                    else
                                    {
                                        #region ' Restore some BTN '
                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.InShop == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.InStore == true)
                                            {
                                                if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                                {
                                                    InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                                }
                                            }
                                        }
                                        #endregion

                                        MsgPly("^6>>^7 Your Platenumber must be in ' Police '!", CPR.UCID);
                                        MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                                        Conn.IsTowTruck = false;
                                        Conn.LastName = "";
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Your Platenumber must be in ' Tow '!", CPR.UCID);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            MsgPly("^6>>^7 Cancel your current job first!", CPR.UCID);
                        }

                    }
                    else
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a tow truck!");
                        MsgPly("^6>>^7 Please remove your tag!", CPR.UCID);
                        KickID(Conn.Username);
                    }
                }
                else
                {
                    #region ' Remove Some '
                    if (Conn.IsTowTruck == true)
                    {
                        if (Conn.InTowProgress == true)
                        {
                            MsgAll("^6>>^7 " + Conn.LastName + " stopped towing " + TowCon.NoColPlyName + "!");
                            TowCon.IsBeingTowed = false;

                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            Conn.Towee = -1;
                            Conn.InTowProgress = false;
                            CautionSirenShutOff();
                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                            Conn.IsTowTruck = false;
                            Conn.LastName = "";
                        }
                        else
                        {
                            #region ' Restore some BTN '
                            if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                            {
                                if (Conn.InShop == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$100 - 200", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                                if (Conn.InStore == true)
                                {
                                    if (Conn.CurrentCar == "UF1" || Conn.CurrentCar == "XFG" || Conn.CurrentCar == "XRG" || Conn.CurrentCar == "LX4" || Conn.CurrentCar == "LX6" || Conn.CurrentCar == "RB4" || Conn.CurrentCar == "FXO" || Conn.CurrentCar == "XRT" || Conn.CurrentCar == "VWS" || Conn.CurrentCar == "RAC" || Conn.CurrentCar == "FZ5")
                                    {
                                        InSim.Send_BTN_CreateButton("^2Get a Job ^7for Delivery! Wages: ^2$200-300", Flags.ButtonStyles.ISB_LEFT, 4, 40, 77, 54, 117, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^2Job", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 77, 100, 121, Conn.UniqueID, 2, false);
                                    }
                                }
                            }
                            #endregion

                            MsgAll("^6>>^7 " + Conn.LastName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                            Conn.IsTowTruck = false;
                            Conn.LastName = "";
                        }
                    }
                    #endregion
                }
                #endregion

                // Update Connections[] list
                Conn.PlayerName = CPR.PName;
                Conn.Plate = CPR.Plate;
            }
            catch { }
        }

        // Car was taken over by an other player
        private void TOC_PlayerCarTakeOver(Packets.IS_TOC TOC)
        {
            try
            {
                var ConnOldUCID = Connections[GetConnIdx(TOC.OldUCID)];
                var ConnNewUCID = Connections[GetConnIdx(TOC.NewUCID)];

                if (RentingAllowed == true)
                {
                    #region ' Cars Not Allowed '
                    if (ConnNewUCID.CurrentCar == "UF1" || ConnNewUCID.CurrentCar == "XFG" || ConnNewUCID.CurrentCar == "XRG" || ConnNewUCID.CurrentCar == "LX4" || ConnNewUCID.CurrentCar == "LX6" || ConnNewUCID.CurrentCar == "RB4" || ConnNewUCID.CurrentCar == "FXO" || ConnNewUCID.CurrentCar == "XRT" || ConnNewUCID.CurrentCar == "RAC" || ConnNewUCID.CurrentCar == "FZ5" || ConnNewUCID.CurrentCar == "UFR" || ConnNewUCID.CurrentCar == "XFR" || ConnNewUCID.CurrentCar == "FXR" || ConnNewUCID.CurrentCar == "XRR" || ConnNewUCID.CurrentCar == "FZR" || ConnNewUCID.CurrentCar == "MRT" || ConnNewUCID.CurrentCar == "FBM" || ConnNewUCID.CurrentCar == "BF1" || ConnNewUCID.CurrentCar == "FOX" || ConnNewUCID.CurrentCar == "FO8" || ConnNewUCID.CurrentCar == "BF1")
                    {
                        // To Rentee
                        Message("/spec " + ConnNewUCID.Username);
                        MsgPly("^6>>^7 Vehicle renting is ^1not allowed!", TOC.NewUCID);

                        // To Renter
                        MsgPly("^6>>^7 Vehicle renting is ^1not allowed", TOC.OldUCID);
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;
                    }
                    #endregion
                }
                else
                {
                    #region ' Renting not Allowed '

                    if (ConnOldUCID.Rented == 1)
                    {
                        if (ConnOldUCID.Rentee == ConnNewUCID.Username)
                        {
                            #region ' Not Chaining Rent '
                            // Renter

                            MsgPly("^6>>^7 You collected your rented vehicle back", TOC.NewUCID);
                            Connections[GetConnIdx(TOC.NewUCID)].BonusDistance = Connections[GetConnIdx(TOC.OldUCID)].BonusDistance;
                            Connections[GetConnIdx(TOC.NewUCID)].TotalBonusDone = Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone;
                            Connections[GetConnIdx(TOC.NewUCID)].Renting = 0;
                            Connections[GetConnIdx(TOC.NewUCID)].Renter = "";
                            Connections[GetConnIdx(TOC.NewUCID)].InGame = 1;
                            Connections[GetConnIdx(TOC.NewUCID)].AFKTick = 0;

                            // Owner

                            MsgPly("^6>>^7 You returned your vehicle to it's owner", TOC.OldUCID);
                            Connections[GetConnIdx(TOC.OldUCID)].BonusDistance = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].TotalBonusDone = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rented = 0;
                            Connections[GetConnIdx(TOC.OldUCID)].Rentee = "";
                            Connections[GetConnIdx(TOC.OldUCID)].AFKTick = 1;
                            Connections[GetConnIdx(TOC.OldUCID)].InGame = 0;

                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnNewUCID.JobFromHouse1 = ConnOldUCID.JobFromHouse1;
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnNewUCID.JobFromHouse2 = ConnOldUCID.JobFromHouse2;
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnNewUCID.JobFromHouse3 = ConnOldUCID.JobFromHouse3;
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnNewUCID.JobToSchool = ConnOldUCID.JobToSchool;
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromShop = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnNewUCID.JobToHouse1 = ConnOldUCID.JobToHouse1;
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnNewUCID.JobToHouse2 = ConnOldUCID.JobToHouse2;
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnNewUCID.JobToHouse3 = ConnOldUCID.JobToHouse3;
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnNewUCID.JobFromStore = ConnOldUCID.JobFromShop;
                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            #region ' Update Users '
                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.NewUCID)]);

                            FileInfo.SaveUser(
                                Connections[GetConnIdx(TOC.OldUCID)]);
                            #endregion

                            MsgAll("^6>>^7 Vehicle Rent Return");
                            MsgAll("^6>>^7 Owner : " + ConnNewUCID.NoColPlyName + " (" + ConnNewUCID.Username + ")");
                            MsgAll("^6>>^7 Renter : " + ConnOldUCID.NoColPlyName + " (" + ConnOldUCID.Username + ")");
                            MsgAll("^6>>^7 Vehicle : ^3" + ConnOldUCID.CurrentCar);
                            #endregion
                        }
                        else
                        {
                            #region ' chaining rent '
                            #region ' Transfer some '

                            #region ' Job To School '
                            if (ConnOldUCID.JobToSchool == true)
                            {
                                if (ConnOldUCID.JobFromHouse1 == true)
                                {
                                    ConnOldUCID.JobFromHouse1 = false;
                                }
                                if (ConnOldUCID.JobFromHouse2 == true)
                                {
                                    ConnOldUCID.JobFromHouse2 = false;
                                }
                                if (ConnOldUCID.JobFromHouse3 == true)
                                {
                                    ConnOldUCID.JobFromHouse3 = false;
                                }
                                ConnOldUCID.JobToSchool = false;
                            }
                            #endregion

                            #region ' Job From Shop '

                            if (ConnOldUCID.JobFromShop == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromShop = false;
                            }

                            #endregion

                            #region ' Job From Store '

                            if (ConnOldUCID.JobFromStore == true)
                            {
                                if (ConnOldUCID.JobToHouse1 == true)
                                {
                                    ConnOldUCID.JobToHouse1 = false;
                                }

                                if (ConnOldUCID.JobToHouse2 == true)
                                {
                                    ConnOldUCID.JobToHouse2 = false;
                                }

                                if (ConnOldUCID.JobToHouse3 == true)
                                {
                                    ConnOldUCID.JobToHouse3 = false;
                                }

                                ConnOldUCID.JobFromStore = false;
                            }

                            #endregion

                            #endregion

                            // To Renter
                            Message("/spec " + ConnNewUCID.Username);
                            MsgPly("^6>>^7 You are not allowed to chain rent vehicles", TOC.NewUCID);

                            // To Rentee
                            ConnOldUCID.BonusDistance = 0;
                            ConnOldUCID.TotalBonusDone = 0;
                            ConnOldUCID.AFKTick = 1;
                            ConnOldUCID.InGame = 0;
                            if (ConnOldUCID.Rented == 1)
                            {
                                bool Found = false;

                                #region ' Online '
                                foreach (clsConnection Conn in Connections)
                                {
                                    if (Conn.Username == ConnOldUCID.Rentee)
                                    {
                                        Found = true;
                                        Conn.Renting = 0;
                                        Conn.Renter = "x";
                                        MsgPly("^6>>^7 Your rental was returned to you", Conn.UniqueID);
                                    }
                                }
                                #endregion

                                #region ' Offline '
                                if (Found == false)
                                {
                                    #region ' Objects '

                                    long Cash = 0;
                                    long BBal = 0;
                                    string Cars = "";
                                    long Distance = 0;
                                    byte Health = 0;
                                    long Gold = 0;
                                    byte Goods2 = 0;
                                    byte Goods1 = 0;
                                    int Raffle = 0;
                                    int Lotto = 0;
                                    int JobsDone = 0;

                                    byte Officer = 0;
                                    byte Cadet = 0;
                                    byte TowTruck = 0;
                                    byte IsMember = 0;

                                    byte Intrfc1 = 0;
                                    byte Intrfc2 = 0;
                                    byte Speedo = 0;
                                    byte Odometer = 0;
                                    byte Counter = 0;
                                    byte Panel = 0;

                                    byte Renting = 0;
                                    byte Rented = 0;
                                    string Renter = "";
                                    string Renterr = "";
                                    string Rentee = "";

                                    string PlayerInfo = "";
                                    #endregion

                                    #region ' Read SR Info '
                                    StreamReader Sr = new StreamReader(Database + "\\" + ConnOldUCID.Rentee + ".txt");
                                    string line = null;

                                    while ((line = Sr.ReadLine()) != null)
                                    {
                                        #region ' Player Stats '
                                        if (line.Substring(0, 4) == "Cash")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cash = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "BBal")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            BBal = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "Gold")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Gold = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 4) == "Cars")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cars = Msg1[1].Trim();
                                        }
                                        if (line.Substring(0, 8) == "Distance")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Distance = long.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Health")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Health = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 8) == "JobsDone")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            JobsDone = int.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Goods1")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Goods1 = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Goods2")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Goods2 = byte.Parse(Msg1[1].Trim());
                                        }

                                        // Timers
                                        if (line.Substring(0, 6) == "Lotto")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Lotto = Int32.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Raffle")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Raffle = Int32.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' Player Status '
                                        // Service Status
                                        if (line.Substring(0, 7) == "Officer")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Officer = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 6) == "Member")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            IsMember = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "TowTruck")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            TowTruck = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 5) == "Cadet")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Cadet = byte.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' User Settings '
                                        // Player Settings
                                        if (line.Substring(0, 7) == "Intrfc1")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Intrfc1 = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "Intrfc2")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Intrfc2 = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Speedo")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Speedo = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 8) == "Odometer")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Odometer = byte.Parse(Msg1[1].Trim());
                                        }
                                        if (line.Substring(0, 7) == "Counter")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Counter = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 5) == "Panel")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Panel = byte.Parse(Msg1[1].Trim());
                                        }
                                        #endregion

                                        #region ' Renting '
                                        // Player Info
                                        if (line.Substring(0, 7) == "Renting")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renting = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Rented")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Rented = byte.Parse(Msg1[1].Trim());
                                        }

                                        if (line.Substring(0, 6) == "Rentee")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Rentee = Msg1[1].Trim();
                                        }

                                        if (line.Substring(0, 6) == "Renter")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renter = Msg1[1].Trim();
                                        }

                                        if (line.Substring(0, 7) == "Renterr")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            Renterr = Msg1[1].Trim();
                                        }

                                        #endregion

                                        #region ' Player Name LOL '
                                        if (line.Substring(0, 7) == "RegInfo")
                                        {
                                            string[] Msg1 = line.Split('=');
                                            PlayerInfo = Msg1[1].Trim();
                                        }
                                        #endregion
                                    }
                                    Sr.Close();
                                    #endregion

                                    #region ' Remove Renting '

                                    Renting = 0;
                                    Renter = "x";

                                    #endregion

                                    #region ' Special PlayerName Colors Remove '

                                    string NoColPlyName = PlayerInfo;
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

                                    MsgAll("^6>>^7 " + ConnOldUCID.NoColPlyName + " his/her rentals returned to " + NoColPlyName);

                                    #region ' Save User '

                                    StreamWriter Sw = new StreamWriter(Database + "\\" + ConnOldUCID.Rentee + ".txt");
                                    Sw.WriteLine("Cash = " + Cash);
                                    Sw.WriteLine("BBal = " + BBal);
                                    Sw.WriteLine("Gold = " + Gold);
                                    Sw.WriteLine("Cars = " + Cars);
                                    Sw.WriteLine("Distance = " + Distance);
                                    Sw.WriteLine("Health = " + Health);
                                    Sw.WriteLine("JobsDone = " + JobsDone);

                                    Sw.WriteLine("Goods1 = " + Goods1);
                                    Sw.WriteLine("Goods2 = " + Goods2);

                                    Sw.WriteLine("Member = " + IsMember);
                                    Sw.WriteLine("Officer = " + Officer);
                                    Sw.WriteLine("Cadet = " + Cadet);
                                    Sw.WriteLine("TowTruck = " + TowTruck);

                                    Sw.WriteLine("Raffle = " + Raffle);
                                    Sw.WriteLine("Lotto = " + Lotto);

                                    Sw.WriteLine("Intrfc1 = " + Intrfc1);
                                    Sw.WriteLine("Intrfc2 = " + Intrfc2);
                                    Sw.WriteLine("Speedo = " + Speedo);
                                    Sw.WriteLine("Odometer = " + Odometer);
                                    Sw.WriteLine("Counter = " + Counter);

                                    Sw.WriteLine("Renting = " + Renting);
                                    Sw.WriteLine("Rented = " + Rented);
                                    Sw.WriteLine("Renter = " + Renter);
                                    Sw.WriteLine("Renterr = " + Renterr);
                                    Sw.WriteLine("Rentee = " + Rentee);

                                    Sw.WriteLine("RegInfo = " + PlayerInfo);

                                    Sw.WriteLine("//// " + System.DateTime.Now);
                                    Sw.Flush();
                                    Sw.Close();

                                    #endregion
                                }
                                #endregion

                                Connections[GetConnIdx(TOC.OldUCID)].Rented = 0;
                                Connections[GetConnIdx(TOC.OldUCID)].Rentee = "x";
                            }
                            MsgPly("^6>>^7 You are not allowed to chain rent vehicles", TOC.OldUCID);
                            #endregion
                        }
                    }
                    else
                    {
                        #region ' Not Returning Rent '
                        // To Rentee
                        Message("/spec " + ConnNewUCID.Username);
                        MsgPly("^6>>^7 Renting is Currently Disabled", TOC.NewUCID);

                        // To Renter
                        MsgPly("^6>>^7 Renting is Currently Disabled", TOC.OldUCID);
                        ConnOldUCID.BonusDistance = 0;
                        ConnOldUCID.TotalBonusDone = 0;
                        ConnOldUCID.AFKTick = 1;
                        ConnOldUCID.InGame = 0;

                        #region ' Transfer some '

                        #region ' Job To School '
                        if (ConnOldUCID.JobToSchool == true)
                        {
                            if (ConnOldUCID.JobFromHouse1 == true)
                            {
                                ConnOldUCID.JobFromHouse1 = false;
                            }
                            if (ConnOldUCID.JobFromHouse2 == true)
                            {
                                ConnOldUCID.JobFromHouse2 = false;
                            }
                            if (ConnOldUCID.JobFromHouse3 == true)
                            {
                                ConnOldUCID.JobFromHouse3 = false;
                            }
                            ConnOldUCID.JobToSchool = false;
                        }
                        #endregion

                        #region ' Job From Shop '

                        if (ConnOldUCID.JobFromShop == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnOldUCID.JobFromShop = false;
                        }

                        #endregion

                        #region ' Job From Store '

                        if (ConnOldUCID.JobFromStore == true)
                        {
                            if (ConnOldUCID.JobToHouse1 == true)
                            {
                                ConnOldUCID.JobToHouse1 = false;
                            }

                            if (ConnOldUCID.JobToHouse2 == true)
                            {
                                ConnOldUCID.JobToHouse2 = false;
                            }

                            if (ConnOldUCID.JobToHouse3 == true)
                            {
                                ConnOldUCID.JobToHouse3 = false;
                            }

                            ConnOldUCID.JobFromStore = false;
                        }

                        #endregion

                        #endregion
                        #endregion
                    }

                    #endregion
                }

                // New
                ConnNewUCID.PlayerID = TOC.PLID;
                ConnNewUCID.CurrentCar = ConnOldUCID.CurrentCar;

                // Old 
                ConnOldUCID.PlayerID = 0;
                ConnOldUCID.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // A player leaves the race (spectate)
        private void PLL_PlayerLeavesRace(Packets.IS_PLL PLL)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLL.PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                var ChaseCon = Connections[GetConnIdx(Connections[IDX].Chasee)];
                var TowCon = Connections[GetConnIdx(Connections[IDX].Towee)];
                #endregion

                #region ' In Game Necessities '
                if (Conn.InGame == 1)
                {
                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^6>>^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 11)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 7000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 12)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 8000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 13)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 9000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 14)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 10000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 15)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 12000 + "%");
                    }
                    #endregion

                    


                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }
                    #endregion

                    #region ' Job Remove '

                    #region ' Job From Store '

                    if (Conn.JobFromStore == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromStore = false;
                    }

                    #endregion

                    #region ' Job From Shop Remove '
                    if (Conn.JobFromShop == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromShop = false;
                    }
                    #endregion

                    #region ' Job From House 1 '

                    if (Conn.JobFromHouse1 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse1 = false;
                    }

                    #endregion

                    #region ' Job From House 2 '

                    if (Conn.JobFromHouse2 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse2 = false;
                    }

                    #endregion

                    #region ' Job From House 3 '

                    if (Conn.JobFromHouse3 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse3 = false;
                    }

                    #endregion

                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion

                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
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
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " sighting lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + C.NoColPlyName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + NoColPlyName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + TowCon.NoColPlyName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " spected whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                Conn.ParkingPay = 0;
                DeleteBTN(27, Conn.UniqueID);
                DeleteBTN(7, Conn.UniqueID);
                Conn.Location = "Spectators";
                Conn.LastSeen = "Spectators";
                Conn.LocationBox = "^7Spectators";
                Conn.SpeedBox = "";
                // Update Players[] list
                Conn.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // A player goes to the garage (setup screen).
        private void PLP_PlayerGoesToGarage(Packets.IS_PLP PLP)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PLP.PLID)
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

                #region ' In Game Neccesities '
                if (Conn.InGame == 1)
                {
                    #region ' Bonus Done Region '
                    if (Conn.TotalBonusDone == 0)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 800 + "%");
                    }
                    else if (Conn.TotalBonusDone == 2)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 3)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 4)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 1600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 5)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 6)
                    {
                        MsgAll("^6>>^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 2600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 7)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 3200 + "%");
                    }
                    else if (Conn.TotalBonusDone == 8)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 4000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 9)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 5400 + "%");
                    }
                    else if (Conn.TotalBonusDone == 10)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 6600 + "%");
                    }
                    else if (Conn.TotalBonusDone == 11)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 7000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 12)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 8000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 13)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 9000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 14)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 10000 + "%");
                    }
                    else if (Conn.TotalBonusDone == 15)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends their bonus at ^3" + Conn.BonusDistance / 12000 + "%");
                    }
                    #endregion

                    

                    #region ' On Screen PitLane Clear '
                    if (Conn.LeavesPitLane == 1)
                    {
                        Conn.LeavesPitLane = 0;
                    }
                    if (Conn.OnScreenExit > 0)
                    {
                        DeleteBTN(10, Conn.UniqueID);
                        Conn.OnScreenExit = 0;
                    }

                    #endregion

                    #region ' OnScreen Ahead '

                    if (Conn.StreetSign > 0)
                    {
                        DeleteBTN(11, Conn.UniqueID);
                        DeleteBTN(12, Conn.UniqueID);
                        DeleteBTN(13, Conn.UniqueID);
                        Conn.StreetSign = 0;
                    }

                    #endregion

                    #region ' OnScreen Sign '

                    if (Conn.MapSignActivated == true)
                    {
                        if (Conn.MapSigns > 0)
                        {
                            DeleteBTN(10, Conn.UniqueID);
                            Conn.MapSigns = 0;
                        }
                    }

                    #endregion

                    #region ' Job Remove '

                    #region ' Job From Store '

                    if (Conn.JobFromStore == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromStore = false;
                    }

                    #endregion

                    #region ' Job From Shop Remove '
                    if (Conn.JobFromShop == true)
                    {
                        if (Conn.JobToHouse1 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Hriso's House!");
                            Conn.JobToHouse1 = false;
                        }
                        if (Conn.JobToHouse2 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Martin's Farm!");
                            Conn.JobToHouse2 = false;
                        }
                        if (Conn.JobToHouse3 == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Elly's House!");
                            Conn.JobToHouse3 = false;
                        }
                        Conn.JobFromShop = false;
                    }
                    #endregion

                    #region ' Job From House 1 '

                    if (Conn.JobFromHouse1 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse1 = false;
                    }

                    #endregion

                    #region ' Job From House 2 '

                    if (Conn.JobFromHouse2 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse2 = false;
                    }

                    #endregion

                    #region ' Job From House 3 '

                    if (Conn.JobFromHouse3 == true)
                    {
                        if (Conn.JobToSchool == true)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " ends job to Lottery!");
                            Conn.JobToSchool = false;
                        }
                        Conn.JobFromHouse3 = false;
                    }

                    #endregion

                    #endregion

                    #region ' Remove Cop Panel '
                    if (Conn.IsOfficer == true || Conn.IsCadet == true)
                    {
                        #region ' Remove Cop Panel '

                        DeleteBTN(15, Conn.UniqueID);
                        DeleteBTN(16, Conn.UniqueID);
                        DeleteBTN(17, Conn.UniqueID);
                        DeleteBTN(18, Conn.UniqueID);
                        DeleteBTN(19, Conn.UniqueID);
                        DeleteBTN(20, Conn.UniqueID);
                        DeleteBTN(21, Conn.UniqueID);
                        DeleteBTN(22, Conn.UniqueID);

                        #endregion
                    }
                    #endregion

                    #region ' Close BTN '
                    if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                    {
                        if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true || Conn.InSchool == true || Conn.InShop == true || Conn.InStore == true || Conn.InBank == true)
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
                        }
                        Conn.DisplaysOpen = false;
                    }
                    #endregion

                    #region ' Close Location '

                    if (Conn.InBank == true)
                    {
                        Conn.InBank = false;
                    }
                    if (Conn.InHouse1 == true)
                    {
                        Conn.InHouse1 = false;
                    }
                    if (Conn.InHouse2 == true)
                    {
                        Conn.InHouse2 = false;
                    }
                    if (Conn.InHouse3 == true)
                    {
                        Conn.InHouse3 = false;
                    }
                    if (Conn.InSchool == true)
                    {
                        Conn.InSchool = false;
                    }

                    if (Conn.InShop == true)
                    {
                        Conn.InShop = false;
                    }
                    if (Conn.InStore == true)
                    {
                        Conn.InStore = false;
                    }
                    #endregion

                    Conn.TotalBonusDone = 0;
                    Conn.BonusDistance = 0;
                    Conn.InGame = 0;
                }
                #endregion

                #region ' Cop System '

                if (Conn.TrapSetted == true)
                {
                    MsgPly("^6>>^7 Speed Trap Removed", Conn.UniqueID);
                    Conn.TrapY = 0;
                    Conn.TrapX = 0;
                    Conn.TrapSpeed = 0;
                    Conn.TrapSetted = false;
                }

                if (Conn.IsSuspect == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being chased!");
                    Conn.Cash -= 5000;

                    #region ' In Connection List '
                    foreach (clsConnection i in Connections)
                    {
                        if (i.Chasee == Conn.UniqueID)
                        {
                            if (i.JoinedChase == true)
                            {
                                i.JoinedChase = false;
                            }
                            i.BustedTimer = 0;
                            i.Busted = false;
                            i.AutoBumpTimer = 0;
                            i.BumpButton = 0;
                            i.ChaseCondition = 0;
                            i.InChaseProgress = false;
                            i.Chasee = -1;
                        }
                    }
                    #endregion

                    AddChaseLimit -= 1;
                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.CopInChase = 0;
                    Conn.IsSuspect = false;
                    CopSirenShutOff();
                }

                if (Conn.InFineMenu == true)
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

                if (Conn.IsBeingBusted == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was fined ^1$5000");
                    MsgAll("  ^7For specting on track whilst being busted!");
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
                                MsgAll("^6>>^7 " + i.NoColPlyName + " was rewarded for ^2$" + (Convert.ToInt16(5000 * 0.2)));
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

                    Conn.PullOvrMsg = 0;
                    Conn.ChaseCondition = 0;
                    Conn.AcceptTicket = 0;
                    Conn.TicketRefuse = 0;
                    Conn.CopInChase = 0;
                    Conn.IsBeingBusted = false;
                }

                if (Conn.AcceptTicket == 2)
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
                    Conn.TicketRefuse = 0;

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
                }

                if (Conn.InChaseProgress == true)
                {
                    if (ChaseCon.CopInChase > 1)
                    {
                        if (Conn.JoinedChase == true)
                        {
                            Conn.JoinedChase = false;
                        }
                        Conn.ChaseCondition = 0;
                        Conn.Busted = false;
                        Conn.BustedTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.Chasee = -1;
                        ChaseCon.CopInChase -= 1;

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " sighting lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7 Total Cops In Chase: " + ChaseCon.CopInChase);
                    }
                    else if (ChaseCon.CopInChase == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " lost " + ChaseCon.NoColPlyName + "!");
                        MsgAll("   ^7Suspect Runs away from being chased!");
                        AddChaseLimit -= 1;
                        Conn.AutoBumpTimer = 0;
                        Conn.BumpButton = 0;
                        Conn.BustedTimer = 0;
                        Conn.Chasee = -1;
                        Conn.Busted = false;
                        ChaseCon.PullOvrMsg = 0;
                        ChaseCon.ChaseCondition = 0;
                        ChaseCon.CopInChase = 0;
                        ChaseCon.IsSuspect = false;
                        Conn.ChaseCondition = 0;
                        CopSirenShutOff();
                    }

                    Conn.InChaseProgress = false;
                }

                #endregion

                #region ' Tow System '

                if (Conn.InTowProgress == true)
                {
                    if (TowCon.IsBeingTowed == true)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " stopped towing " + TowCon.NoColPlyName + "!");
                        TowCon.IsBeingTowed = false;
                    }
                    Conn.Towee = -1;
                    Conn.InTowProgress = false;
                    CautionSirenShutOff();
                }

                if (Conn.IsBeingTowed == true)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " pitted whilst being Towed!");
                    foreach (clsConnection o in Connections)
                    {
                        if (o.Towee == Conn.UniqueID)
                        {
                            o.InTowProgress = false;
                            o.Towee = -1;
                        }
                    }
                    Conn.IsBeingTowed = false;
                    CautionSirenShutOff();
                }

                #endregion

                #region ' Return Rent '
                if (Conn.Rented == 1)
                {
                    bool Found = false;

                    #region ' Online '
                    foreach (clsConnection C in Connections)
                    {
                        if (C.Username == Conn.Rentee)
                        {
                            Found = true;
                            C.Renting = 0;
                            C.Renter = "0";
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + C.NoColPlyName);
                        }
                    }
                    #endregion

                    #region ' Offline '
                    if (Found == false)
                    {
                        #region ' Objects '

                        long Cash = FileInfo.GetUserCash(Conn.Rentee);
                        long BBal = FileInfo.GetUserBank(Conn.Rentee);
                        string Cars = FileInfo.GetUserCars(Conn.Rentee);
                        long Gold = FileInfo.GetUserGold(Conn.Rentee);

                        long TotalDistance = FileInfo.GetUserDistance(Conn.Rentee);
                        byte TotalHealth = FileInfo.GetUserHealth(Conn.Rentee);
                        int TotalJobsDone = FileInfo.GetUserJobsDone(Conn.Rentee);

                        byte Electronics = FileInfo.GetUserElectronics(Conn.Rentee);
                        byte Furniture = FileInfo.GetUserFurniture(Conn.Rentee);

                        int LastRaffle = FileInfo.GetUserLastRaffle(Conn.Rentee);
                        int LastLotto = FileInfo.GetUserLastLotto(Conn.Rentee);

                        byte CanBeOfficer = FileInfo.CanBeOfficer(Conn.Rentee);
                        byte CanBeCadet = FileInfo.CanBeCadet(Conn.Rentee);
                        byte CanBeTowTruck = FileInfo.CanBeTowTruck(Conn.Rentee);
                        byte IsModerator = FileInfo.IsMember(Conn.Rentee);

                        byte Interface1 = FileInfo.GetInterface(Conn.Rentee);
                        byte Interface2 = FileInfo.GetInterface2(Conn.Rentee);
                        byte Speedo = FileInfo.GetSpeedo(Conn.Rentee);
                        byte Odometer = FileInfo.GetOdometer(Conn.Rentee);
                        byte Counter = FileInfo.GetCounter(Conn.Rentee);
                        byte Panel = FileInfo.GetCopPanel(Conn.Rentee);

                        byte Renting = FileInfo.GetUserRenting(Conn.Rentee);
                        byte Rented = FileInfo.GetUserRented(Conn.Rentee);
                        string Renter = FileInfo.GetUserRenter(Conn.Rentee);
                        string Renterr = FileInfo.GetUserRenterr(Conn.Rentee);
                        string Rentee = FileInfo.GetUserRentee(Conn.Rentee);

                        string PlayerName = FileInfo.GetUserPlayerName(Conn.Rentee);
                        #endregion

                        #region ' Remove Renting '

                        Renting = 0;
                        Renter = "0";

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

                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " their rentals returned to " + NoColPlyName);

                        #region ' Save User '

                        FileInfo.SaveOfflineUser(Conn.Rentee,
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
                    #endregion

                    Conn.Rentee = "0";
                    Conn.Rented = 0;
                }
                #endregion

                Conn.ParkingPay = 0;
                DeleteBTN(27, Conn.UniqueID);
                DeleteBTN(7, Conn.UniqueID);
                Conn.Location = "Fix 'N' Repair Station";
                Conn.LastSeen = "Fix 'N' Repair Station";
                Conn.LocationBox = "^7Fix 'N' Repair Station";
                Conn.SpeedBox = "";

                // Update Player List[]
                Conn.PlayerID = 0;
                Conn.CompCar = new Packets.CompCar();
            }
            catch { }
        }

        // A player joins the race. If PLID already exists, then player leaves pit.
        private void NPL_PlayerJoinsRace(Packets.IS_NPL NPL)
        {
            try
            {
                var Conn = Connections[GetConnIdx(NPL.UCID)];

                #region ' Check User cars '

                if (Conn.Rented == 0)
                {
                    if (Conn.Cars.Contains(NPL.CName) != true)
                    {
                        int RandomFines = new Random().Next(200, 250);
                        SpecID(Conn.Username);
                        SpecID(Conn.PlayerName);
                        MsgAll("^4»^7 " + Conn.NoColPlyName + " tried to steal ^3" + NPL.CName);
                        MsgAll(" ^7but was caught and fined for ^1$" + RandomFines);
                        Conn.Cash -= RandomFines;

                        if (NPL.CName == "XRG" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 0)
                        {
                            MsgPly("^6>>^7 You need 0KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FBM" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2999)
                        {
                            MsgPly("^6>>^7 You need 3000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XRG" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 0)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 0KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FBM" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 10000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 10000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        { //H
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "LX4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 500)
                        {
                            MsgPly("^6>>^7 You need 500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "LX6" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 800)
                        {
                            MsgPly("^6>>^7 You need 800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "LX4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 500)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "LX6" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 800)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        } //H
                        if (NPL.CName == "RB4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 900)
                        {
                            MsgPly("^6>>^7 You need 900KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FXO" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1200)
                        {
                            MsgPly("^6>>^7 You need 1200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "RB4" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 900)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 900KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FXO" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1200)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 1200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        { //H
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1600)
                        {
                            MsgPly("^6>>^7 You need 1600KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "RAC" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1800)
                        {
                            MsgPly("^6>>^7 You need 1800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1600)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 1600KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "RAC" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1800)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 1800KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        {
                        }
                        if (NPL.CName == "FZ5" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2000)
                        {
                            MsgPly("^6>>^7 You need 2000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "UFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2200)
                        {
                            MsgPly("^6>>^7 You need 2200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "FZ5" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 2000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "UFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2200)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 2200KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2400)
                        {
                            MsgPly("^6>>^7 You need 2400KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FXR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3000)
                        {
                            MsgPly("^6>>^7 You need 3000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XFR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 2400)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 2400KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FXR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 3000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "XRR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3500)
                        {
                            MsgPly("^6>>^7 You need 3500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "FZR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                        {
                            MsgPly("^6>>^7 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "XRR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 3500)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 3500KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "FZR" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        if (NPL.CName == "MRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1000)
                        {
                            MsgPly("^6>>^7 You need 1000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }
                        else if (NPL.CName == "" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 4000)
                        {
                            MsgPly("^6>>^7 You need 4000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);
                        }

                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                        Conn.StealTime += 1;
                    }
                    else if (NPL.CName == "MRT" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 1000)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 1000KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else if (NPL.CName == "" && Connections[GetConnIdx(NPL.UCID)].TotalDistance / 1000 <= 0)
                    {
                        InSim.Send_MST_Message("/spec " + Connections[GetConnIdx(NPL.UCID)].PlayerName);
                        MsgPly("^6>>^7 You need 0KM to drive a ^3" + NPL.CName + "^7!", NPL.UCID);

                        Conn.StealTime += 1;
                        if (Conn.StealTime < 3)
                        {
                            MsgPly("^6>>^7 Warning: More stealing may cause kick.", NPL.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 Turn ^4RIGHT ^7out of the pits!", NPL.UCID);
                        Conn.LeavesPitLane = 1;
                        Conn.InGame = 1;
                    }
                }
                else
                {
                    Conn.LeavesPitLane = 1;
                    Conn.InGame = 1;
                }
                #endregion

                if (Conn.StealTime == 4)
                {
                    MsgAll("^6>>^7 " + Conn.NoColPlyName + " was kicked for stealing.");
                    KickID(Conn.Username);
                }

                #region ' Check Admin '

                if (NPL.PName == HostName == false)
                {
                    if (Conn.IsAdmin == 1)
                    {
                        if (Conn.IsSuperAdmin == 0)
                        {
                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " is not a admin!");
                            SpecID(NPL.PName);
                        }
                    }
                }
                #endregion

                

                if (Conn.CanBeTowTruck == 1 && Conn.IsTowTruck == true)
                {
                    if (NPL.CName == "FBM" || NPL.CName == "MRT")
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " is now ^1OFF-DUTY ^7as Tow Truck!");
                        Conn.IsTowTruck = false;
                    }

                }

                // Below Updates
                Connections[GetConnIdx(NPL.UCID)].Plate = NPL.Plate;

                Connections[GetConnIdx(NPL.UCID)].SkinName = NPL.SName;

                Connections[GetConnIdx(NPL.UCID)].CurrentCar = NPL.CName;

                Connections[GetConnIdx(NPL.UCID)].PlayerPacket = NPL;

                Connections[GetConnIdx(NPL.UCID)].PlayerID = NPL.PLID;	// Update Players[] list
            }
            catch { }
        }

        // A player stops for making a pitstop
        private void PIT_PlayerStopsAtPit(Packets.IS_PIT PIT)
        {
            try
            {
                if (PIT.Work.ToString().Contains("PSE_NOTHING") == false)
                {
                    InSim.Send_MTC_MessageToConnection("^6>>^7 The Service Station Pit Work:", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                    if (PIT.Work.ToString().Contains("LE_FR_DAM"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Left Front Damage - ^1$30", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 20;
                    }
                    if (PIT.Work.ToString().Contains("RI_FR_DAM"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Right Front Damage - ^1$30", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 20;
                    }
                    if (PIT.Work.ToString().Contains("LE_RE_DAM"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Left Rear Damage - ^1$20", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 20;
                    }
                    if (PIT.Work.ToString().Contains("RI_RE_DAM"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Right Rear Damage - ^1$20", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 20;
                    }
                    if (PIT.Work.ToString().Contains("PSE_REFUEL"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Refueling - ^1$20", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 20;
                    }
                    if (PIT.Work.ToString().Contains("BODY_MAJOR"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Major Body Damage - ^1$50", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                    if (PIT.Work.ToString().Contains("BODY_MINOR"))
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Minor Body Damage - ^1$20", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                    if (PIT.FL_Changed.ToString().Contains("TYRE_NOTCHANGED") == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Front Left Tyre - ^1$50", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                    if (PIT.FR_Changed.ToString().Contains("TYRE_NOTCHANGED") == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Front Right Tyre - ^1$50", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                    if (PIT.RL_Changed.ToString().Contains("TYRE_NOTCHANGED") == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Rear Left Tyre - ^1$50", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                    if (PIT.RR_Changed.ToString().Contains("TYRE_NOTCHANGED") == false)
                    {
                        InSim.Send_MTC_MessageToConnection("^6>>^7 Rear Right Tyre - ^1$50", Connections[GetConnIdx2(PIT.PLID)].UniqueID, 0);
                        Connections[GetConnIdx2(PIT.PLID)].Cash -= 50;
                    }
                }
            }
            catch { }
        }

        // A penalty give or cleared
        private void PEN_PenaltyChanged(Packets.IS_PEN PEN)
        {
            try
            {
                #region ' UniqueID Loader '
                int IDX = -1;
                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].PlayerID == PEN.PLID)
                    {
                        IDX = i;
                        break;
                    }
                }
                if (IDX == -1)
                    return;
                clsConnection Conn = Connections[IDX];
                #endregion
            }
            catch { }
        }

        // The server/race state changed
        private void STA_StateChanged(Packets.IS_STA STA)
        {
            try
            {
                TrackName = STA.Track;
            }
            catch { }
        }

        // A host ends or leaves
        private void MPE_MultiplayerEnd()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    var Conn = Connections[GetConnIdx(C.UniqueID)];
                }
            }
            catch { }
        }

        // A race ends (return to game setup screen)
        private void REN_RaceEnds()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    var Conn = Connections[GetConnIdx(C.UniqueID)];
                }
            }
            catch { }
        }

        // A player submitted a custom textbox
        private void BTT_TextBoxOkClicked(Packets.IS_BTT BTT)
        {
            try
            {
                clsConnection Conn = Connections[GetConnIdx(BTT.UCID)];

                #region ' Cop Panel '

                if (Conn.IsOfficer == true && Conn.CopPanel == 1 && BTT.ClickID == 21)
                {
                    if (Conn.InChaseProgress == false)
                    {
                        try
                        {
                            int TrapSpeed = Convert.ToInt32(BTT.Text);

                            if (TrapSpeed.ToString().Contains("-"))
                            {
                                MsgPly("^6>>^7 Invalid Input. Don't put minus values!", BTT.UCID);
                            }
                            else
                            {
                                if (Conn.TrapSetted == false)
                                {
                                    if (Conn.CompCar.Speed / 91 < 3)
                                    {
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
                                        MsgPly("^6>>^7 Can't Set a Trap whilst being driving!", BTT.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 The Trap has been setted in this Area!", BTT.UCID);
                                }
                            }
                        }
                        catch
                        {
                            MsgPly("^6>>^7 Trap Error. Please check your values!", BTT.UCID);
                        }
                    }
                    else
                    {
                        MsgPly("^6>>^7 Can't set a Trap whilst in chase progress!", BTT.UCID);
                    }
                }

                #endregion

                #region ' In Bank '
                if (Conn.InBank == true)
                {
                    switch (BTT.ClickID)
                    {
                        case 116:

                            int Deposit = int.Parse(BTT.Text);
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Deposit Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Conn.Cash > Deposit)
                                {
                                    Conn.BankBalance += Deposit;
                                    Conn.Cash -= Deposit;
                                    Conn.BankBonusTimer = 3600;
                                    InSim.Send_BTN_CreateButton("^7Your bank balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), Flags.ButtonStyles.ISB_LEFT, 4, 40, 65, 54, 114, Conn.UniqueID, 2, false);
                                    MsgPly("^6>>^7 You have successfuly deposited ^2$" + string.Format("{0:n0}", Deposit), BTT.UCID);
                                    MsgPly("^6>>^7 Your new Bank Balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), BTT.UCID);
                                    MsgPly("^6>>^7 Your Bank Bonus Timer is now Reseted to 1 hour!", Conn.UniqueID);
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Not Enough Cash to complete the transaction.", BTT.UCID);
                                }
                            }
                            break;

                        case 117:

                            int Withdraw = int.Parse(BTT.Text);
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Deposit Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Conn.BankBalance > Withdraw)
                                {
                                    Conn.BankBalance -= Withdraw;
                                    Conn.Cash += Withdraw;
                                    Conn.BankBonusTimer = 3600;
                                    InSim.Send_BTN_CreateButton("^7Your bank balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), Flags.ButtonStyles.ISB_LEFT, 4, 40, 65, 54, 114, Conn.UniqueID, 2, false);
                                    MsgPly("^6>>^7 You have successfuly withdrawn ^2$" + string.Format("{0:n0}", Withdraw), BTT.UCID);
                                    MsgPly("^6>>^7 Your new Bank Balance is ^2$" + string.Format("{0:n0}", Conn.BankBalance), BTT.UCID);
                                    MsgPly("^6>>^7 Your Bank Bonus Timer is now Reseted to 1 hour!", Conn.UniqueID);
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Not Enough Bank Balance to complete the transaction.", BTT.UCID);
                                }
                            }

                            break;
                    }
                }
                #endregion

                #region ' In Store '

                if (Conn.InStore == true)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Electronic '
                        case 118:
                            byte Electronics = byte.Parse(BTT.Text);
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Transaction Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Electronics > 10)
                                {
                                    MsgPly("^6>>^7 Cannot buy more than 10 Electronic Items!", BTT.UCID);
                                }
                                else
                                {
                                    if (Conn.Electronics < 10)
                                    {
                                        if (Conn.Cash > 190 * Electronics)
                                        {
                                            Conn.Electronics += Electronics;
                                            Conn.Cash -= 190 * Electronics;
                                            Conn.TotalSale += Electronics;

                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " bought some Electronic for ^1$" + Electronics * 190 + "^7!");
                                            MsgPly("^6>>^7 Total Electronic: " + Conn.Electronics, BTT.UCID);
                                            MsgPly("^6>>^7 To Sell them visit some nearest houses and start trading!", BTT.UCID);

                                            if (Conn.LastRaffle == 0)
                                            {
                                                MsgPly("^6>>^7 Buy more items and more chances of winning in the Raffle!", BTT.UCID);

                                                if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                                {
                                                    InSim.Send_BTN_CreateButton("^7Total Item bought: ^2(" + Conn.TotalSale + ")^7 Raffle for ^1$300", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Electronics, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 2, 118, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Not Enough Cash to complete the transaction.", BTT.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Cannot carry more than 10 items!", BTT.UCID);
                                    }
                                }
                            }

                            break;
                        #endregion

                        #region ' Furniture '
                        case 119:
                            byte Furniture = byte.Parse(BTT.Text);
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Transaction Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Furniture > 10)
                                {
                                    MsgPly("^6>>^7 Cannot buy more than 10 Furniture Items!", BTT.UCID);
                                }
                                else
                                {
                                    if (Conn.Furniture < 10)
                                    {
                                        if (Conn.Cash > 150 * Furniture)
                                        {
                                            Conn.Furniture += Furniture;
                                            Conn.Cash -= 150 * Furniture;
                                            Conn.TotalSale += Furniture;

                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " bought a Furniture for ^1$" + Furniture * 150 + "^7!");

                                            MsgPly("^6>>^7 Total Furniture: " + Conn.Furniture, BTT.UCID);

                                            MsgPly("^6>>^7 To Sell them visit some nearest houses and start trading!", BTT.UCID);



                                            if (Conn.LastRaffle == 0)
                                            {
                                                MsgPly("^6>>^7 Buy more items and more chances of winning in the Raffle!", BTT.UCID);
                                                if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                                {
                                                    InSim.Send_BTN_CreateButton("^7Total Item bought: ^2(" + Conn.TotalSale + ")^7 Raffle for ^1$300", Flags.ButtonStyles.ISB_LEFT, 4, 100, 73, 54, 116, Conn.UniqueID, 2, false);
                                                    InSim.Send_BTN_CreateButton("^2Raffle!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 73, 100, 120, Conn.UniqueID, 2, false);
                                                }
                                            }
                                            if (Conn.DisplaysOpen == true && Conn.InStore == true && Conn.InGameIntrfc == 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^2Buy", "Maximum Buy 10 and you have " + Conn.Furniture, Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 2, 119, Conn.UniqueID, 2, false);
                                            }
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 Not Enough Cash to complete the transaction.", BTT.UCID);
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Cannot carry more than 10 items!", BTT.UCID);
                                    }
                                }
                            }

                            break;
                        #endregion
                    }
                }

                #endregion

                #region ' In School '

                if (Conn.InSchool == true)
                {
                    if (BTT.ClickID == 120)
                    {
                        int LottoPicked = int.Parse(BTT.Text);
                        if (BTT.Text.Contains("-"))
                        {
                            MsgPly("^6>>^7 Lotto Pick Incorrect. Don't put minus on the values!", BTT.UCID);
                        }
                        else
                        {
                            #region ' Accept Lottery '
                            if (Conn.Cash > 100)
                            {
                                if (Conn.LastLotto == 0)
                                {
                                    if (LottoPicked > 10)
                                    {
                                        MsgPly("^6>>^7 Can't pick more than 10 numbers!", BTT.UCID);
                                    }
                                    else if (LottoPicked == 0)
                                    {
                                        MsgPly("^6>>^7 Can't use zero on Lottery pick!", BTT.UCID);
                                    }
                                    else
                                    {
                                        int RandomChance = new Random().Next(1, 10);
                                        MsgPly("^6>>^7 Winning Number is ^2" + RandomChance, BTT.UCID);

                                        #region ' Lotto '
                                        if (LottoPicked == RandomChance)
                                        {
                                            int prize = new Random().Next(2000, 4000);
                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " won ^2$" + prize + " ^7from winning prize in Lottery!");
                                            MsgPly("^6>>^7 Congratulations you just earned ^2$" + prize, BTT.UCID);
                                            Conn.Cash += prize;
                                        }
                                        else
                                        {
                                            int prize = new Random().Next(500, 1000);
                                            MsgAll("^6>>^7 " + Conn.NoColPlyName + " won a prize of ^2$" + prize + " ^7in Lottery!");
                                            MsgPly("^6>>^7 Better luck next time", BTT.UCID);
                                            Conn.Cash += prize;
                                        }
                                        #endregion

                                        Conn.LastLotto = 180;
                                        Conn.Cash -= 100;

                                        #region ' Replace Display '
                                        if (Conn.DisplaysOpen == true && Conn.InGameIntrfc == 0)
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

                                            DeleteBTN(120, BTT.UCID);
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (Conn.LastLotto > 120)
                                    {
                                        MsgPly("^6>>^7 You need to wait ^1Three (3)hours^7 to rejoin the Lottery!", BTT.UCID);
                                    }
                                    else if (Conn.LastLotto > 60)
                                    {
                                        MsgPly("^6>>^7 You need to wait ^1Two (2)hours^7 to rejoin the Lottery!", BTT.UCID);
                                    }
                                    else
                                    {
                                        if (Conn.LastLotto > 1)
                                        {
                                            MsgPly("^6>>^7 You need to wait ^1" + Conn.LastLotto + "minutes^7 to rejoin the Lottery!", BTT.UCID);
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 You need to wait ^1" + Conn.LastLotto + "minute^7 to rejoin the Lottery!", BTT.UCID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Not Enough Cash to join the Lottery!", BTT.UCID);
                            }
                            #endregion
                        }
                    }
                }

                #endregion

                #region ' In Houses '

                if (Conn.InHouse1 == true || Conn.InHouse2 == true || Conn.InHouse3 == true)
                {
                    byte Amount = byte.Parse(BTT.Text);
                    switch (BTT.ClickID)
                    {
                        #region ' Trade Electronics '
                        case 118:

                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Trade Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Amount < 6)
                                {
                                    if (Conn.Electronics > Amount)
                                    {
                                        Conn.Electronics -= Amount;
                                        Conn.Cash += Amount * Conn.SellElectronics;

                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " traded their Electronic for ^3" + Amount + "^7!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + Amount * Conn.SellElectronics);

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.Electronics > 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^2Trade ^7your Electronic Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 65, 100, 1, 118, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to Trade any Electronic!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 65, 54, 114, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enough Electronics to Trade!", BTT.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Trading Electronics More than 5 is not Allowed.", BTT.UCID);
                                }
                            }

                            break;
                        #endregion

                        #region ' Trade Furniture '
                        case 119:

                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Trade Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else
                            {
                                if (Amount < 6)
                                {
                                    if (Conn.Furniture > Amount)
                                    {
                                        Conn.Furniture -= Amount;
                                        Conn.Cash += Amount * Conn.SellFurniture;

                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " traded their Furniture for ^3" + Amount + "^7!");
                                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " Got paid for ^2$" + Amount * Conn.SellFurniture);

                                        if (Conn.InGameIntrfc == 0 && Conn.DisplaysOpen == true)
                                        {
                                            if (Conn.Furniture > 0)
                                            {
                                                InSim.Send_BTN_CreateButton("^2Trade ^7your Furniture Items for ^1$" + Conn.SellElectronics + " ^7each.", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                                InSim.Send_BTN_CreateButton("^2Trade", "Maximum Trading Items 5 each.", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 69, 100, 1, 119, Conn.UniqueID, 2, false);
                                            }
                                            else
                                            {
                                                InSim.Send_BTN_CreateButton("^2You don't ^7have enough items to trade any Furniture!", Flags.ButtonStyles.ISB_LEFT, 4, 100, 69, 54, 115, Conn.UniqueID, 2, false);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MsgPly("^6>>^7 Not Enough Furniture to Trade!", BTT.UCID);
                                    }
                                }
                                else
                                {
                                    MsgPly("^6>>^7 Trading Furniture More than 5 is not Allowed.", BTT.UCID);
                                }
                            }

                            break;
                        #endregion
                    }
                }

                #endregion

                #region ' Busted Panel '

                if (Conn.InFineMenu == true)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Reason Ticket '
                        case 36:
                            string Reason = (BTT.Text);
                            if (Reason.Length >= 0)
                            {
                                InSim.Send_BTN_CreateButton("^7Reason: " + Reason, "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 40, false);
                                Conn.TicketReason = Reason;
                                Conn.TicketReasonSet = true;
                            }
                            else
                            {
                                MsgPly("^6>>^7 Input Error. You must fill the Reason!", BTT.UCID);
                                Conn.TicketReason = "";
                                Conn.TicketReasonSet = false;
                                InSim.Send_BTN_CreateButton("^7Reason", "Enter the chase reason", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 78, 77, 64, 36, (Conn.UniqueID), 40, false);
                            }
                            break;
                        #endregion

                        #region ' Fine Amount Ticket '
                        case 37:
                            int Amount = int.Parse(BTT.Text);
                            bool Complete = false;
                            if (BTT.Text.Contains("-"))
                            {
                                MsgPly("^6>>^7 Deposit Incorrect. Don't put minus on the values!", BTT.UCID);
                            }
                            else if (BTT.Text.Length < 0)
                            {
                                MsgPly("^6>>^7 Fine Error. Invalid input!", BTT.UCID);
                                InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            else if (Amount == 0)
                            {
                                MsgPly("^6>>^7 Fine Error. The Fine must have a value!", BTT.UCID);
                                InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            else
                            {
                                if (BTT.Text != "")
                                {
                                    if (Conn.ChaseCondition == 1)
                                    {
                                        if (Amount > 700)
                                        {
                                            MsgPly("^6>>^7 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 2)
                                    {
                                        if (Amount > 1300)
                                        {
                                            MsgPly("^6>>^7 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 3)
                                    {
                                        if (Amount > 2500)
                                        {
                                            MsgPly("^6>>^7 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 4)
                                    {
                                        if (Amount > 3500)
                                        {
                                            MsgPly("^6>>^7 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                    else if (Conn.ChaseCondition == 5)
                                    {
                                        if (Amount > 5000)
                                        {
                                            MsgPly("^6>>^7 Fine Error. Too high for the Maximum Fine in this Condition!", BTT.UCID);
                                            InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                        }
                                        else
                                        {
                                            Complete = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Conn.TicketAmountSet == true)
                                    {
                                        Conn.TicketAmount = 0;
                                        Conn.TicketAmountSet = false;
                                        InSim.Send_BTN_CreateButton("^7Fine Amount", "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                                    }
                                }
                            }

                            if (Complete == true)
                            {
                                Conn.TicketAmount = Amount;
                                Conn.TicketAmountSet = true;
                                InSim.Send_BTN_CreateButton("^7Fine ^1$" + Amount, "Enter amount to fine", Flags.ButtonStyles.ISB_CLICK | Flags.ButtonStyles.ISB_LIGHT, 5, 45, 86, 77, 4, 37, (Conn.UniqueID), 20, false);
                            }
                            break;
                        #endregion
                    }
                }

                #endregion

                #region ' Moderation Panel '

                if (Conn.InModerationMenu == 1)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Reason Window '
                        case 37:
                            string Reason = BTT.Text;
                            if (Reason.Length > 0)
                            {
                                InSim.Send_BTN_CreateButton("^4>> ^7Reason: " + Reason + " ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                InSim.Send_BTN_CreateButton("^7WARN", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 54, 38, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7SPEC", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 82, 40, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7KICK", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 96, 41, Conn.UniqueID, 40, false);
                                InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                Conn.ModReason = Reason;
                                Conn.ModReasonSet = true;
                            }
                            else
                            {
                                InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                Conn.ModReason = "";
                                Conn.ModReasonSet = false;
                            }

                            break;
                        #endregion

                        #region ' Fine '
                        case 39:

                            if (Conn.ModReasonSet == true)
                            {
                                bool Found = false;
                                int Amount = int.Parse(BTT.Text);

                                if (Amount.ToString().Contains("-"))
                                {
                                    MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '
                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            Found = true;
                                            MsgAll("^6>>^7 " + i.NoColPlyName + " was force fined for ^1$" + Amount);
                                            MsgAll("^6>>^7 Reason: " + Conn.ModReason);
                                            MsgPly("> You are fined by " + Conn.NoColPlyName, i.UniqueID);
                                            AdmBox("> " + Conn.NoColPlyName + " fined " + i.NoColPlyName + " (" + i.Username + ") for $" + Amount + "!");
                                            AdmBox("> Reason: " + Conn.ModReason);
                                            i.Cash -= Amount;
                                        }
                                    }
                                    #endregion

                                    #region ' Found '
                                    if (Found == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                    }
                                    #endregion

                                    #region ' Offline '
                                    else if (Found == false)
                                    {
                                        if (System.IO.File.Exists(Database + "\\" + Conn.ModUsername + ".txt") == true)
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

                                            // Your Code here
                                            MsgAll("^6>>^7 " + NoColPlyName + " was force fined for ^1$" + Amount);
                                            MsgAll("^6>>^7 Reason: " + Conn.ModReason);
                                            AdmBox("> " + Conn.NoColPlyName + " fined " + NoColPlyName + " (" + Conn.ModUsername + ") for $" + Amount + "!");
                                            AdmBox("> Reason: " + Conn.ModReason);
                                            Cash -= Amount;

                                            InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                            InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                            InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);


                                            Conn.InModerationMenu = 2;

                                            #region ' Save User '
                                            FileInfo.SaveOfflineUser(Conn.ModUsername,
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
                                            MsgPly("^6>>^7 " + Conn.ModUsername + " wasn't found on database", BTT.UCID);
                                        }
                                    }
                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Reason not yet setted.", BTT.UCID);
                            }

                            break;
                        #endregion

                        #region ' Ban '
                        case 42:

                            if (Conn.ModReasonSet == true)
                            {
                                int Days = int.Parse(BTT.Text);
                                bool Found = false;

                                if (Days.ToString().Contains("-"))
                                {
                                    MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                                }
                                else
                                {
                                    #region ' Online '

                                    foreach (clsConnection i in Connections)
                                    {
                                        if (i.Username == Conn.ModUsername)
                                        {
                                            MsgAll("^6>>^7 " + i.NoColPlyName + " (" + i.Username + ") was banned.");
                                            MsgPly("> You are banned by " + Conn.NoColPlyName, i.UniqueID);
                                            MsgAll("^6>>^7 Reason: " + Conn.ModReason);
                                            AdmBox("> " + Conn.NoColPlyName + " banned " + i.NoColPlyName + " (" + i.Username + ") for " + Days + "!");
                                            AdmBox("> Reason: " + Conn.ModReason);

                                            if (Days == 0)
                                            {
                                                MsgPly("^6>>^7 You are banned for 12 hours", i.UniqueID);
                                            }
                                            else if (Days == 1)
                                            {
                                                MsgPly("^6>>^7 You are banned for " + Days + " Day", i.UniqueID);
                                            }
                                            else
                                            {
                                                MsgPly("^6>>^7 You are banned for " + Days + " Days", i.UniqueID);
                                            }

                                            BanID(i.Username, Days);
                                        }
                                    }

                                    if (Found == true)
                                    {
                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    #region ' Offline '

                                    else if (Found == false)
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

                                        MsgAll("^6>>^7 " + NoColPlyName + " (" + Conn.ModUsername + ") was banned.");
                                        MsgAll("^6>>^7 Reason: " + Conn.ModReason);

                                        AdmBox("> " + Conn.NoColPlyName + " banned " + NoColPlyName + " (" + Conn.ModUsername + ") for " + Days + "!");
                                        AdmBox("> Reason: " + Conn.ModReason);
                                        BanID(Conn.ModUsername, Days);

                                        InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.ModerationWarn = 2;
                                    }

                                    #endregion

                                    Conn.ModReason = "";
                                    Conn.ModReasonSet = false;
                                }
                            }
                            else
                            {
                                MsgPly("^6>>^7 Reason not yet setted.", BTT.UCID);
                            }

                            break;
                        #endregion
                    }
                }

                else if (Conn.InModerationMenu == 2)
                {
                    switch (BTT.ClickID)
                    {
                        #region ' Fine '
                        case 39:

                            bool Found = false;
                            int Amount = int.Parse(BTT.Text);

                            if (Amount.ToString().Contains("-"))
                            {
                                MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                            }
                            else
                            {
                                #region ' Online '
                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found = true;
                                        MsgAll("^6>>^7 " + i.NoColPlyName + " was force fined for ^1$" + Amount);
                                        MsgPly("> You are fined by " + Conn.NoColPlyName, i.UniqueID);
                                        AdmBox("> " + Conn.NoColPlyName + " fined " + i.NoColPlyName + " (" + i.Username + ") for $" + Amount + "!");
                                        i.Cash -= Amount;
                                    }
                                }
                                #endregion

                                #region ' Found '
                                if (Found == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8FINE", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 68, 39, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8BAN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 110, 42, Conn.UniqueID, 2, false);
                                    Conn.InModerationMenu = 1;
                                }
                                #endregion

                                #region ' Offline '
                                else if (Found == false)
                                {
                                    if (System.IO.File.Exists(Database + "\\" + Conn.ModUsername + ".txt") == true)
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

                                        // Your Code here
                                        MsgAll("^6>>^7 " + NoColPlyName + " was force fined for ^1$" + Amount);
                                        AdmBox("> " + Conn.NoColPlyName + " fined " + NoColPlyName + " (" + Conn.ModUsername + ") for $" + Amount + "!");
                                        Cash -= Amount;

                                        InSim.Send_BTN_CreateButton("^4>> ^7Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                        InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                        InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                        Conn.InModerationMenu = 2;

                                        #region ' Save User '
                                        FileInfo.SaveOfflineUser(Conn.ModUsername,
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
                                        MsgPly("^6>>^7 " + Conn.ModUsername + " wasn't found on database", BTT.UCID);
                                    }
                                }
                                #endregion

                                Conn.ModReason = "";
                                Conn.ModReasonSet = false;
                            }

                            break;
                        #endregion

                        #region ' Ban '
                        case 42:
                            int Days = int.Parse(BTT.Text);
                            bool Found1 = false;
                            if (Days.ToString().Contains("-"))
                            {
                                MsgPly("^6>>^7 Input Invalid. Don't use minus on the Values!", BTT.UCID);
                            }
                            else
                            {
                                #region ' Online '

                                foreach (clsConnection i in Connections)
                                {
                                    if (i.Username == Conn.ModUsername)
                                    {
                                        Found1 = true;
                                        MsgAll("^6>>^7 " + i.NoColPlyName + " (" + i.Username + ") was banned.");
                                        MsgPly("> You are banned by " + Conn.NoColPlyName, i.UniqueID);
                                        AdmBox("> " + Conn.NoColPlyName + " banned " + i.NoColPlyName + " (" + i.Username + ") for " + Days + "!");

                                        if (Days == 0)
                                        {
                                            MsgPly("^6>>^7 You are banned for 12 hours", i.UniqueID);
                                        }
                                        else if (Days == 1)
                                        {
                                            MsgPly("^6>>^7 You are banned for " + Days + " Day", i.UniqueID);
                                        }
                                        else
                                        {
                                            MsgPly("^6>>^7 You are banned for " + Days + " Days", i.UniqueID);
                                        }

                                        BanID(i.Username, Days);
                                    }
                                }

                                if (Found1 == true)
                                {
                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                }

                                #endregion

                                #region ' Offline '

                                else if (Found1 == false)
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

                                    MsgAll("^6>>^7 " + NoColPlyName + " (" + Conn.ModUsername + ") was banned.");
                                    BanID(Conn.ModUsername, Days);
                                    AdmBox("> " + Conn.NoColPlyName + " banned " + NoColPlyName + " (" + Conn.ModUsername + ") for " + Days + "!");

                                    InSim.Send_BTN_CreateButton("^4>> ^8Set a Reason ^4<<", "Action of Reason", Flags.ButtonStyles.ISB_LIGHT, 5, 69, 82, 54, 52, 37, BTT.UCID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8WARN", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 54, 38, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7FINE", "Set the Amount of Fines", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 68, 4, 39, Conn.UniqueID, 40, false);
                                    InSim.Send_BTN_CreateButton("^8SPEC", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 82, 40, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^8KICK", Flags.ButtonStyles.ISB_LIGHT, 5, 13, 88, 96, 41, Conn.UniqueID, 2, false);
                                    InSim.Send_BTN_CreateButton("^7BAN", "Set the Amount of Days", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 13, 88, 110, 2, 42, Conn.UniqueID, 40, false);
                                    Conn.ModerationWarn = 2;
                                }

                                #endregion

                                Conn.ModReason = "";
                                Conn.ModReasonSet = false;
                            }

                            break;
                        #endregion
                    }
                }

                #endregion

                #region ' admin panel '
                {
                    if (Conn.InAdminMenu == true && BTT.ClickID == 196)
                    {
                        switch (BTT.ClickID)
                        {
                            case 196:

                                int prize = int.Parse(BTT.Text);
                                if (BTT.Text.Contains("-"))
                                {
                                    MsgPly("^6>>^7 Deposit Incorrect. Don't put minus on the values!", BTT.UCID);
                                }
                                else
                                {
                                    {

                                    foreach (clsConnection C in Connections)
                                    {
                                        C.Cash += prize;
                                    }
                                    InSim.Send_MST_Message("/msg ^1>>>^7 All users received ^6$" + prize);
                                    InSim.Send_MST_Message("/msg ^1>>>^7 Sponsor : " + Conn.Username);

                                    }
                                }
                                break;

                            case 198:

                                    {

                                        foreach (clsConnection C in Connections)
                                        {
                                            C.TotalHealth += 20;
                                        }
                                        InSim.Send_MST_Message("/msg ^1>>>^7 All users received ^620 ^7energy!!");
                                        InSim.Send_MST_Message("/msg ^1>>>^7 Sponsor : " + Conn.Username);

                                    }
                                break;
                        }
                    }
                }
                #endregion
            }
            catch { }

        }

        // A player pressed Shift+I or Shift+B
        private void BFN_PlayerRequestsButtons(Packets.IS_BFN BFN)
        {
            try
            {
                var Conn = Connections[GetConnIdx(BFN.UCID)];
                var ChaseCon = Connections[GetConnIdx(Connections[GetConnIdx(BFN.UCID)].Chasee)];

                #region ' Interface Loader '

                if (Conn.Interface == 0)
                {
                    DeleteBTN(2, Conn.UniqueID);
                    DeleteBTN(3, Conn.UniqueID);
                    DeleteBTN(4, Conn.UniqueID);
                    DeleteBTN(5, Conn.UniqueID);
                    DeleteBTN(6, Conn.UniqueID);
                    DeleteBTN(7, Conn.UniqueID);
                    DeleteBTN(8, Conn.UniqueID);
                    DeleteBTN(9, Conn.UniqueID);
                    DeleteBTN(14, Conn.UniqueID);
                    Conn.WaitIntrfc = 0;
                }
                else
                {
                    InSim.Send_BTN_CreateButton(CruiseName, Flags.ButtonStyles.ISB_C4, 10, 50, 190, 0, 1, Conn.UniqueID, 2, false);
                }
                #endregion

                #region ' Admin Menu '
                if (Conn.IsSuperAdmin == 1 && Conn.IsAdmin == 1)
                {
                    Conn.AccessAdmin += 1;
                }

                if (Conn.AccessAdmin == 3)
                {
                    if (Conn.IsSuperAdmin == 1 && Conn.IsAdmin == 1)
                    {
                        if (Conn.InAdminMenu == false)
                        {
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 70, 100, 50, 50, 110, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("", Flags.ButtonStyles.ISB_DARK, 70, 100, 50, 50, 111, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^7[LC]Cruise ^7Administration InSim Settings", Flags.ButtonStyles.ISB_LIGHT, 7, 98, 51, 51, 112, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6MSG STOP SPAM", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 65, 80, 190, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6Allow car reset", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 70, 80, 191, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6Deny car reset", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 75, 80, 192, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6Pitlane all", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 80, 80, 193, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6Refund all with energy!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 85, 80, 198, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1Refund all with money!", "Enter amount!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 90, 80, 80, 196, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^6Refund all with 1 bonus level!", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 5, 40, 95, 80, 197, Conn.UniqueID, 2, false);
                            InSim.Send_BTN_CreateButton("^1CLOSE", Flags.ButtonStyles.ISB_LIGHT | Flags.ButtonStyles.ISB_CLICK, 4, 10, 110, 95, 113, Conn.UniqueID, 2, false);

                            Conn.AccessAdmin = 0;
                            Conn.InAdminMenu = true;
                        }
                    }
                }
                #endregion

                if (Conn.DisplaysOpen == true)
                {
                    Conn.DisplaysOpen = false;
                }

                // Clear Moderator Panel
                if (Conn.InModerationMenu > 0)
                {
                    Conn.ModerationWarn = 0;
                    Conn.ModReason = "";
                    Conn.ModReasonSet = false;
                    Conn.ModUsername = "";
                    Conn.InModerationMenu = 0;
                }

                if (Conn.InFineMenu == true)
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

                #region ' Refuse Pay Fines/Close Warning '
                if (Conn.IsBeingBusted == true)
                {
                    if (Conn.AcceptTicket == 1)
                    {
                        MsgAll("^6>>^7 " + Conn.NoColPlyName + " refused to pay the fines!");
                        MsgAll("  ^7was fined for ^1$5000 ^7for SHIFT+I/B in Ticket Menu!");

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
                        DeleteBTN(30, BFN.UCID);
                        DeleteBTN(31, BFN.UCID);
                        DeleteBTN(32, BFN.UCID);
                        DeleteBTN(33, BFN.UCID);
                        DeleteBTN(34, BFN.UCID);
                        DeleteBTN(35, BFN.UCID);
                        DeleteBTN(36, BFN.UCID);
                        DeleteBTN(37, BFN.UCID);
                        DeleteBTN(38, BFN.UCID);
                        DeleteBTN(39, BFN.UCID);
                        DeleteBTN(40, BFN.UCID);
                        #endregion
                    }
                }
                if (Conn.AcceptTicket == 2)
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
                    Conn.TicketRefuse = 0;

                    #region ' Close Region LOL '
                    DeleteBTN(30, BFN.UCID);
                    DeleteBTN(31, BFN.UCID);
                    DeleteBTN(32, BFN.UCID);
                    DeleteBTN(33, BFN.UCID);
                    DeleteBTN(34, BFN.UCID);
                    DeleteBTN(35, BFN.UCID);
                    DeleteBTN(36, BFN.UCID);
                    DeleteBTN(37, BFN.UCID);
                    DeleteBTN(38, BFN.UCID);
                    DeleteBTN(39, BFN.UCID);
                    DeleteBTN(40, BFN.UCID);
                    #endregion
                }


                #endregion
            }
            catch { }
        }

        // A player clicked a custom button
        private void BTC_ButtonClicked(Packets.IS_BTC BTC)
        {
            try
            {
                BTC_ClientButtonClicked(BTC);
            }
            catch { }
        }

        // A vote got canceled
        private void VTC_VoteCanceled()
        {
            try
            {

            }
            catch { }
        }

        // A vote got called
        private void VTN_VoteNotify(Packets.IS_VTN VTN)
        {
            try
            {
                var Conn = Connections[GetConnIdx(VTN.UCID)];
                if (Conn.IsAdmin == 1 || Conn.IsSuperAdmin == 1)
                {
                }
                else
                {
                    if (VTN.Action == Enums.VTN_Actions.VOTE_RESTART)
                    {
                        Message("/cv");
                    }
                    if (VTN.Action == Enums.VTN_Actions.VOTE_END)
                    {
                        Message("/cv");
                    }
                }
            }
            catch { }
        }

        // Detailed car information packet (max 8 per packet)
        private void MCI_CarInformation(Packets.IS_MCI MCI)
        {
            try
            {
                int idx = 0;
                for (int i = 0; i < MCI.NumC; i++)
                {
                    idx = GetConnIdx2(MCI.Info[i].PLID); //They aren't structures so you cant serialize!
                    Connections[idx].CompCar.AngVel = MCI.Info[i].AngVel;
                    Connections[idx].CompCar.Direction = MCI.Info[i].Direction;
                    Connections[idx].CompCar.Heading = MCI.Info[i].Heading;
                    Connections[idx].CompCar.Info = MCI.Info[i].Info;
                    Connections[idx].CompCar.Lap = MCI.Info[i].Lap;
                    Connections[idx].CompCar.Node = MCI.Info[i].Node;
                    Connections[idx].CompCar.PLID = MCI.Info[i].PLID;
                    Connections[idx].CompCar.Position = MCI.Info[i].Position;
                    Connections[idx].CompCar.Speed = MCI.Info[i].Speed;
                    Connections[idx].CompCar.X = MCI.Info[i].X;
                    Connections[idx].CompCar.Y = MCI.Info[i].Y;
                    Connections[idx].CompCar.Z = MCI.Info[i].Z;
                }
                for (int i = 0; i < MCI.NumC; i++) //We want everyone to update before checking them.
                {
                    MCI_Update(MCI.Info[i].PLID);
                }
            }
            catch { }
        }

        // InSim version information
        private void VER_InSimVersionInformation(Packets.IS_VER VER)
        {
            try
            {
                if (VER.Product == "DEMO")
                {
                    GameMode = 0;
                }
                if (VER.Product == "S1")
                {
                    GameMode = 1;
                }
                if (VER.Product == "S2")
                {
                    GameMode = 2;
                }
            }
            catch { }
        }

        #endregion

        #region ' Global Buffer '
        byte SenderUCID = 0;
        string SentCar = "";
        int SentMoney = 0;
        string TrackName = "";
        byte OnScreen = 0;
        bool InSimBooted = false;
        //byte EndRace = 0;
        //byte Messages = 0;
        byte Stage = 0;
        int RobberUCID = -1;
        bool RentingAllowed = true;
        byte TotalOfficers = 0;
        byte ChaseLimit = 0;
        byte AddChaseLimit = 1;

        #endregion

        #region ' Neat System '

        void CopSirenShutOff()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if (C.SirenShowned == true)
                    {
                        DeleteBTN(23, C.UniqueID);
                        DeleteBTN(24, C.UniqueID);
                        C.CopSiren = 3;
                        C.SirenShowned = false;
                    }
                }
            }
            catch { }
        }

        void CautionSirenShutOff()
        {
            try
            {
                foreach (clsConnection C in Connections)
                {
                    if (C.SirenShowned == true)
                    {
                        DeleteBTN(23, C.UniqueID);
                        DeleteBTN(24, C.UniqueID);
                        C.TowCautionSiren = 3;
                        C.SirenShowned = false;
                    }
                }
            }
            catch { }
        }

        void MsgBox(string MsgStr)
        {
            try
            {
                textBox1.Text += " [" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "] " + MsgStr + "\r\n";
            }
            catch { }
        }

        void AdmBox(string MsgStr)
        {
            try
            {
                textBox3.Text += " [" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + " : " + System.DateTime.Now.Second + "] " + MsgStr + "\r\n";

                StreamReader ApR = new StreamReader("AdminLog.txt");
                string TempAPR = ApR.ReadToEnd();
                ApR.Close();
                StreamWriter Ap = new StreamWriter("AdminLog.txt");
                Ap.WriteLine(TempAPR + "[" + System.DateTime.Now + "] : " + MsgStr);

                Ap.Flush();
                Ap.Close();
            }
            catch { }
        }

        void PMBox(string MsgStr)
        {
            try
            {
                textBox4.Text += " [" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + "] " + MsgStr + "\r\n";

                StreamReader ApR = new StreamReader("PrivMsgLog.txt");
                string TempAPR = ApR.ReadToEnd();
                ApR.Close();
                StreamWriter Ap = new StreamWriter("PrivMsgLog.txt");
                Ap.WriteLine(TempAPR + "[" + System.DateTime.Now + "] : " + MsgStr);

                Ap.Flush();
                Ap.Close();
            }
            catch { }
        }

        void Message(string MsgStr)
        {
            try
            {
                InSim.Send_MST_Message(MsgStr);
            }
            catch { }
        }

        void MsgAll(string MsgStr)
        {
            try
            {
                Message("/msg " + MsgStr);
            }
            catch { }
        }

        void MsgPly(string MsgStr, byte UNID)
        {
            try
            {
                InSim.Send_MTC_MessageToConnection(MsgStr, UNID, 0);
            }
            catch { }
        }

        void KickID(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/kick " + Username);
            }
            catch { }
        }

        void SpecID(string PlayerName)
        {
            try
            {
                InSim.Send_MST_Message("/spec " + PlayerName);
            }
            catch { }
        }

        void BanID(string Username, int Days)
        {
            try
            {
                InSim.Send_MST_Message("/ban " + Username + " " + Days);
            }
            catch { }
        }

        void PitlaneID(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/pitlane " + Username);
            }
            catch { }
        }

        void ClearPen(string Username)
        {
            try
            {
                InSim.Send_MST_Message("/p_clear " + Username);
            }
            catch { }
        }

        void DeleteBTN(byte ButtonID, byte UNID)
        {
            try
            {
                InSim.Send_BFN_DeleteButton(Enums.BtnFunc.BFN_DEL_BTN, ButtonID, UNID);
            }
            catch { }
        }

        #endregion

        #region ' Application Message Out System '

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
                textBox1.Refresh();
            }
            catch { }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "")
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] StrMsg = textBox2.Text.Split(' ');
                if (textBox2.Text.StartsWith("|"))
                {
                    switch (StrMsg[0])
                    {
                        case "|help":

                            MsgBox("> Welcome to the iCS Control Box");
                            MsgBox("> |reinit [pw] - to restart the insim");
                            MsgBox("> |recover [pw] - to recover the insim");
                            MsgBox("> |update [pw] - to update the firmware of the insim");
                            MsgBox("> |shutdown [pw] - to shutdown the insim");
                            MsgBox("> |start [pw] - to start the insim");

                            break;

                        case "|reinit":
                            if (StrMsg.Length > 1)
                            {
                                if (textBox2.Text.Remove(0, 8) == AdminPW)
                                {
                                    MsgAll("^6>>^7 LCC System is now rebooting.");
                                    MsgAll("^6>>^7 LCC is rebooted by: Host Controller");
                                    MsgAll("^6>>^7 Total Connection: " + Connections.Count);

                                    foreach (clsConnection C in Connections)
                                    {
                                        FileInfo.SaveUser(C);
                                    }

                                    Thread.Sleep(500);
                                    InSim.Close();
                                    Thread.Sleep(3000);
                                    InSim.Connect();
                                    Thread.Sleep(500);
                                    InSim.Request_NCN_AllConnections(255);
                                    InSim.Request_NPL_AllPlayers(255);
                                    InSim.Send_MST_Message("/wind=1");
                                    InSim.Send_MST_Message("/wind=0");

                                    MsgAll("^6>>^7 Reboot Complete.");
                                }
                                else
                                {
                                    MsgBox("> Command Unnaccessible");
                                }
                            }
                            break;

                        case "|recover":
                            if (StrMsg.Length > 1)
                            {
                                if (textBox2.Text.Remove(0, 9) == AdminPW)
                                {
                                    MsgBox("Admin Password: " + AdminPW);
                                    MsgBox("IP Address: " + IPAddress);
                                    MsgBox("InSim Port: " + Port);
                                }
                                else
                                {
                                    MsgBox("> Command Unnaccessible");
                                }
                            }
                            break;

                        case "|update":
                            if (StrMsg.Length > 1)
                            {
                                if (textBox2.Text.Remove(0, 8) == AdminPW)
                                {
                                    try
                                    {
                                        System.Diagnostics.Process.Start(@"PATCH.exe");
                                        MsgAll("^6>>^7 iCS System is now patching.");
                                        MsgAll("^6>>^7 iCS is patched by: Host Controller");
                                        MsgAll("^6>>^7 Total Connection: " + Connections.Count);

                                        foreach (clsConnection C in Connections)
                                        {
                                            FileInfo.SaveUser(C);
                                        }

                                        Thread.Sleep(500);
                                        InSim.Close();
                                        Thread.Sleep(1000);
                                        Application.Exit();
                                    }
                                    catch
                                    {
                                        MsgBox("> Patching failed make sure the file name is 'PATCH.exe'");
                                    }
                                }
                                else
                                {
                                    MsgBox("> Command Unnaccessible");
                                }
                            }
                            break;

                        case "|shutdown":
                            if (StrMsg.Length > 1)
                            {
                                if (textBox2.Text.Remove(0, 10) == AdminPW)
                                {
                                    if (InSimBooted == false)
                                    {
                                        MsgAll("^6>>^7 iCS System is now shutting down.");
                                        MsgAll("^6>>^7 iCS is shutdowned by: Host Controller");
                                        MsgAll("^6>>^7 Total Connection: " + Connections.Count);

                                        foreach (clsConnection C in Connections)
                                        {
                                            FileInfo.SaveUser(C);
                                        }

                                        InSim.Close();
                                        InSimBooted = true;

                                        MsgBox("> iCS is now shutdown.");
                                        MsgBox("> You can close or |start [admin pass]");
                                    }
                                    else
                                    {
                                        MsgBox("> InSim is already shutdowned.");
                                    }
                                }
                                else
                                {
                                    MsgBox("> Command Unnaccessible");
                                }
                            }
                            break;

                        case "|start":

                            if (StrMsg.Length > 1)
                            {
                                if (textBox2.Text.Remove(0, 7) == AdminPW)
                                {
                                    if (InSimBooted == true)
                                    {
                                        MsgBox("> InSim is now Connected to the server!");
                                        InSimConnect();	// Attempt to connect to InSim
                                        InSimBooted = false;
                                        MsgAll("^6>>^7 The iCS System is now Successfuly started.");
                                    }
                                    else
                                    {
                                        MsgBox("> InSim is already running.");
                                    }
                                }
                                else
                                {
                                    MsgBox("> Command Unnaccessible");
                                }
                            }

                            break;

                        default:
                            if (StrMsg[0].StartsWith("|"))
                            {
                                MsgBox("> Please See |help for more information");
                            }
                            break;
                    }
                    textBox2.Clear();
                }
                else
                {
                    InSim.Send_MST_Message(textBox2.Text);
                    textBox2.Clear();
                }
            }
            catch { }
        }

        // Administration Log
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.ScrollToCaret();
                textBox3.Refresh();
            }
            catch { }
        }

        // Private Message Log
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBox4.SelectionStart = textBox4.Text.Length;
                textBox4.ScrollToCaret();
                textBox4.Refresh();
            }
            catch { }
        }

        #endregion

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }
    }
}