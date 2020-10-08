using System;
using System.Collections;
using System.Collections.Generic;
using LFS_External;
using LFS_External.InSim;
using InSimDotNet;
using InSimDotNet.Helpers;
using InSimDotNet.Packets;

namespace LFS_External_Client
{
    public class StatsAttribute : Attribute
    {
        private string _name = "";
        private bool _update = true;
        public StatsAttribute(string name)
        {
            _name = name;
            _update = true;
        }
        public StatsAttribute(string name,bool update)
        {
            _name = name;
            _update = update;
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public bool Update
        {
            get
            {
                return Update;
            }
            set
            {
                Update = value;
            }
        }
    }
	
    public class clsConnection
	{
        //Gps System
        public byte GPS;
        public byte GpsToBank;
        public byte GpsToCasino;
        public byte GpsToShop;
        public byte GpsToStore;
        public byte GpsToDealer;

        //For Payed Zones
        public byte TickedPay;
        public byte ParkingPay;

        //trade
        public int IdxCar;

        // Cruise Bits
        public long Cash;
        public long TotalDistance;
        public int TripMeter;
        public long BankBalance;
        public int BonusDistance;
        public byte TotalBonusDone;
        public decimal Payout;
        public string Cars;
        public byte TotalHealth;
        public int BankBonusTimer;
        public long BankBonus;
        public int HealthDist;
        public byte HealthWarn;
        public bool IsAFK;
        public int AFKTick;
        public int TotalSale;
        public int LastRaffle;
        public int LastLotto;
        public int TotalJobsDone;

        // Player Renting
        public byte Renting;
        public byte Rented;
        public string Renter;
        public string Renterr;
        public string Rentee;

        // Cruise Way
        public byte ExitZone;

        public string Location;
        public string LastSeen;

        public byte IsSpeeder;

        // Places
        public int InExpDist;
        public int InDealerDist;
        public int InHouse1Dist;
        public int InHouse2Dist;
        public int InHouse3Dist;
        public int InSchoolDist;
        public int InShopDist;
        public int InStoreDist;
        public int InBankDist;
        public int InPayZoneDist;
        public int InPayZone1Dist;
        public int InParkingDist;

        public string LocationBox;
        public string SpeedBox;

        public long Gold;
        public byte Electronics;
        public byte Furniture;

        public int SellElectronics;
        public int SellFurniture;

        public bool DisplaysOpen;

        public bool JobToHouse1;
        public bool JobToHouse2;
        public bool JobToHouse3;

        public bool JobFromHouse1;
        public bool JobFromHouse2;
        public bool JobFromHouse3;

        public bool InHouse1;
        public bool InHouse2;
        public bool InHouse3;
        public bool InExp;
        public bool InStore;
        public bool InSchool;
        public bool InPayZone;
        public bool InPayZone1;
        public bool InParking;
        public bool InBank;
        public bool InShop;
        public bool InDealer;

        public bool JobToSchool;

        public bool JobFromShop;
        public bool JobFromStore;
        
        // Game Settings
        public byte Interface;
        public byte InGameIntrfc;
        public byte Counter;
        public byte KMHorMPH;
        public byte Odometer;
        public byte InGame;
        public byte LeavesPitLane;
        public byte OnScreenExit;
        public byte Penalty;
        public byte WaitCMD;
        public byte WaitIntrfc;
        public byte AccessAdmin;
        public bool InAdminMenu;
        public byte StreetSign;
        public byte MapSigns;
        public bool MapSignActivated;
        public byte StealTime;
        public byte ChannelLanguage;

        // Membership Status
        public byte CanBeOfficer;
        public byte CanBeCadet;
        public byte CanBeTowTruck;

        public bool IsOfficer;
        public bool IsCadet;
        public bool IsTowTruck;

        // Officer/Cadet Bits
        public bool IsSuspect;
        public byte CopPanel;
        public int Chasee;
        public bool InChaseProgress;
        public int AutoBumpTimer;
        public byte ChaseCondition;
        public byte CopSiren;
        public int DistanceFromCop;
        public bool Busted;
        public bool JoinedChase;
        public byte BustedTimer;
        public int SirenDistance;
        public bool SirenShowned;
        public bool SpeederClocked;
        public byte CopInChase;
        public byte BumpButton;
        public byte PullOvrMsg;
        public bool IsBeingBusted;
        public byte AcceptTicket;

        public bool InFineMenu;
        public string TicketReason;
        public int TicketAmount;
        public bool TicketReasonSet;
        public bool TicketAmountSet;
        public byte TicketRefuse;

        // Tow Truck System
        public bool CalledRequest;
        public bool InTowProgress;
        public bool CallAccepted;
        public byte TowCautionSiren;
        public bool IsBeingTowed;
        public int Towee;
        public int DistanceFromTow;

        // Trap
        public bool InTrap;
        public int TrapX;
        public int TrapY;
        public int TrapSpeed;
        public bool TrapSetted;

        // Moderation System
        public byte InModerationMenu;
        public bool ModReasonSet;
        public string ModReason;
        public string ModUsername;
        public byte ModerationWarn;

        // Chat System
        public byte SwearTime;
        public byte Swear;
        public byte SpamTime;
        public byte Spam;

        // Drift System
        public int DriftTime;
        public bool IsDrifting;
        public bool ReachedTime;

        // Send Test
        public string SendUSRN1;
        public string SendUSRN2;

        // Player Bits
		public byte FailCon;
		public byte PlayerID;
        public byte UniqueID;
        public string Username;
        public string PlayerName;
        public byte IsAdmin;
        public byte IsSuperAdmin;
        public byte IsModerator;
        public byte Flags;
        public string NoColPlyName;
        public string LastName;
        public string CurrentCar;
        public Packets.IS_NPL PlayerPacket;
        public string Plate;
        public string SkinName;
        protected byte IntakeRestriction;

        

        public enum enuPType : byte
        {
            Female = 0,
            AI = 1,
            Remote = 2,
        }

        //CompCar Packet
        public Packets.CompCar CompCar = new Packets.CompCar();
	}
}