using System;
using System.Collections.Generic;
using System.Text;

namespace LFS_External_Client
{
    static class Dealer
    {
        static public int GetCarPrice(string CarName)
        {
            switch (CarName.ToUpper())
            {
                case "LX4": return 40000;

                case "LX6": return 50000;

                case "RB4": return 60000;
                case "FXO": return 75000;

                case "XRT": return 90000;
                case "RAC": return 120000;
                case "FZ5": return 140000;

                case "UFR": return 160000;
                case "XFR": return 180000;
                case "FXR": return 250000;
                case "XRR": return 350000;
                case "FZR": return 400000;

                case "MRT": return 150000;
                case "FBM": return 1000000;

            }
            return 0;
        }

        static public int GetCarValue(string CarName)
        {
            return (int)(GetCarPrice(CarName) * .70);
        }
    }
}
