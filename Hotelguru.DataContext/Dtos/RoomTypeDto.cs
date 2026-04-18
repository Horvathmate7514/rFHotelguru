namespace Hotelguru.DataContext.Dtos
{
    public class RoomTypeDto
    {
        public int Id { get; set; }
        public int BedNumber { get; set; }
        public int Capacity { get; set; }

        public decimal BasePrice { get; set; }

    }
}