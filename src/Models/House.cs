using System.ComponentModel;

namespace covidSim.Models
{
    public class House
    {
        public House(int id, Vec cornerCoordinates)
        {
            Id = id;
            Coordinates = new HouseCoordinates(cornerCoordinates);
        }

        public bool ContainsVec(Vec vec)
        {
            return (vec.X > Coordinates.LeftTopCorner.X
                    && vec.X < Coordinates.LeftTopCorner.X + HouseCoordinates.Width
                    && vec.Y > Coordinates.LeftTopCorner.Y
                    && vec.Y < Coordinates.LeftTopCorner.Y + HouseCoordinates.Height);
        }

        public int Id;
        public HouseCoordinates Coordinates;
        public int ResidentCount = 0;
    }
}