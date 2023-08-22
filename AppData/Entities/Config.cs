namespace PrintToCash.AppData.Entities
{
    public class Config
    {
        public int Id { get; set; }

        /// <summary>
        ///  Electricity cost per kW.
        /// </summary>
        public decimal CurrentCostElectricity { get; set; }

        /// <summary>
        ///  Fee for manual touches on final product.
        /// </summary>
        public decimal FinalTouchHourlyFee { get; set; }

        /// <summary>
        ///  Printer Consumption in kW per hour.
        /// </summary>
        public decimal PrinterElectricityConsumptionKW { get; set; } =  0.36m;

        /// <summary>
        ///  Tax percentage which adds after all other price calculations.
        /// </summary>
        public int TaxPercentage { get; set; } = 0;
    }
}
