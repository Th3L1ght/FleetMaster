namespace FleetMaster.Core.Entities
{
    public class CargoVan : Vehicle
    {
        private bool _isRefrigerated;

        public bool IsRefrigerated
        {
            get
            {
                return _isRefrigerated;
            }
            set
            {
                _isRefrigerated = value;
            }
        }

        public override string GetDescription()
        {
            string type = IsRefrigerated ? "Ref-Van" : "Standard Van";
            return $"[{type}] {base.GetDescription()}";
        }
    }
}