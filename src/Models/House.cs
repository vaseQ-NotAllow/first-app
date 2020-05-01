namespace covidSim.Models
{
    public class House
    {
        public House(int id, Vec cornerCoordinates)
        {
            Id = id;
            Coordinates = new HouseCoordinates(cornerCoordinates);
        }

        public int Id;
        public HouseCoordinates Coordinates;
        public int ResidentCount = 0;
    }

    public class HouseCoordinates
    {
        public HouseCoordinates(Vec leftTopCorner)
        {
            LeftTopCorner = leftTopCorner;
        }
        
        public Vec LeftTopCorner;
        public const int Width = 50;
        public const int Height = 50;
    }
}