namespace hotelguru
{
    public class WeatherForecast
    {
        //Cucc
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }
        //Cucc2
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; } //Cucc3
        public string? gg { get; set; } // GG

        public string halo { get; set; }
    }
}
