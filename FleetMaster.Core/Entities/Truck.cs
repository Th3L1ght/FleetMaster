using System;

namespace FleetMaster.Core.Entities
{
    public class Truck : Vehicle
    {
        private bool _hasTrailer;
        private double _cargoVolume;

        public bool HasTrailer
        {
            get { return _hasTrailer; }
            set { _hasTrailer = value; }
        }

        public double CargoVolume
        {
            get { return _cargoVolume; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Об'єм не може бути від'ємним");
                _cargoVolume = value;
            }
        }

        public override string GetDescription()
        {
            return $"[TRUCK] {base.GetDescription()} | Vol: {CargoVolume}m3";
        }
    }
}