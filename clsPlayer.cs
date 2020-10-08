using System;
using LFS_External;
using LFS_External.InSim;

namespace LFS_External_Client
{
    /// <summary>
    /// Information of a player on the track or in the pits
    /// </summary>
    public class clsPlayer
    {
        protected byte _playerid;
        protected byte _uniqueid;
        protected enuPType _ptype;
        protected Flags.PlayerFlags _flags;
        protected string _playername;
        protected string _plate;
        protected string _carname;
        protected string _skinname;
        protected Enums.NPL_Tyres _tyre_rl;
        protected Enums.NPL_Tyres _tyre_rr;
        protected Enums.NPL_Tyres _tyre_fl;
        protected Enums.NPL_Tyres _tyre_fr;
        protected byte _addedmass;
        protected byte _intakerestriction;
        protected byte _passengers;
        protected decimal _payout;

        public enum enuPType : byte
        {
            Female = 0,
            AI = 1,
            Remote = 2,
        }

        /// <summary>Unique player id</summary>
        public byte PlayerID
        {
            get { return _playerid; }
            set { _playerid = value; }
        }

        /// <summary>This player belogns to this connection (changes on Driver Swap)</summary>
        public byte UniqueID
        {
            get { return _uniqueid; }
            set { _uniqueid = value; }
        }

        /// <summary>Type of player</summary>
        public enuPType PlayerType
        {
            get { return _ptype; }
            set { _ptype = value; }
        }

        /// <summary>Flags, Packet.PIF_xxx</summary>
        public Flags.PlayerFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>Players ingame name</summary>
        public string PlayerName
        {
            get { return _playername; }
            set { _playername = value; }
        }

        /// <summary>Plate of car</summary>
        public string Plate
        {
            get { return _plate; }
            set { _plate = value; }
        }

        /// <summary>Current players car</summary>
        public string CarName
        {
            get { return _carname; }
            set { _carname = value; }
        }

        /// <summary>Name of the skin on the car</summary>
        public string SkinName
        {
            get { return _skinname; }
            set { _skinname = value; }
        }

        /// <summary>Rear Left tyre type</summary>
        public Enums.NPL_Tyres Tyre_RL
        {
            get { return _tyre_rl; }
            set { _tyre_rl = value; }
        }

        /// <summary>Rear Right tyre type</summary>
        public Enums.NPL_Tyres Tyre_RR
        {
            get { return _tyre_rr; }
            set { _tyre_rr = value; }
        }

        /// <summary>Front Left tyre type</summary>
        public Enums.NPL_Tyres Tyre_FL
        {
            get { return _tyre_fl; }
            set { _tyre_fl = value; }
        }

        /// <summary>Front Right tyre type</summary>
        public Enums.NPL_Tyres Tyre_FR
        {
            get { return _tyre_fr; }
            set { _tyre_fr = value; }
        }

        /// <summary>Extra mass in Kg</summary>
        public byte AddedMass
        {
            get { return _addedmass; }
            set { _addedmass = value; }
        }

        /// <summary>Passanger in the car</summary>
        public byte Passengers
        {
            get { return _passengers; }
            set { _passengers = value; }
        }

        /// <summary>Intake Restriction</summary>
        public byte IntakeRestriction
        {
            get { return _intakerestriction; }
            set { _intakerestriction = value; }
        }

        public decimal Payout
        {
            get { return _payout; }
            set { _payout = value; }
        }
    }
}
