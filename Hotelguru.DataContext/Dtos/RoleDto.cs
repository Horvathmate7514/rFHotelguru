namespace Hotelguru.DataContext.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleCreateDto
    {
        public string Name { get; set; }
    }
    public class RoleDeleteDto
    {
        public string Name { get; set; }
    }
}