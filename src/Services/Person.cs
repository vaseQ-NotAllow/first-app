using System;
using covidSim.Models;
using covidSim.Services;

namespace covidSim.Services
{
    public class Person
    {
        public Person(int id, int homeId, CityMap map)
        {
            Id = id;
            HomeId = homeId;

            var homeCoords = map.Houses[homeId].Coordinates.LeftTopCorner;
            var x = homeCoords.X + _random.Next(HouseCoordinates.Width);
            var y = homeCoords.Y + _random.Next(HouseCoordinates.Height);
            Position = new Vec(x, y);
        }

        public int Id;
        public int HomeId;
        public Vec Position;

        private PersonState _state = PersonState.AtHome;
        private static Random _random = new Random();
        private const int MaxDistancePerTurn = 20;

        public void CalcNextStep()
        {
            switch (_state)
            {
                case PersonState.AtHome:
                    CalcNextStepForPersonAtHome();
                    break;
                case PersonState.Walking:
                    CalcNextPositionForWalkingPerson();
                    break;
                case PersonState.GoingHome:
                    CalcNextPositionForGoingHomePerson();
                    break;
            }
        }

        private void CalcNextStepForPersonAtHome()
        {
            var goingWalk = _random.NextDouble() < 0.005;
            if (!goingWalk) return;

            _state = PersonState.Walking;
            CalcNextPositionForWalkingPerson();
        }

        private void CalcNextPositionForWalkingPerson()
        {
            var xLength = _random.Next(MaxDistancePerTurn);
            var yLength = MaxDistancePerTurn - xLength;
            var direction = ChooseDirection();
            var delta = new Vec(xLength * direction.X, yLength * direction.Y);
            var nextPosition = new Vec(Position.X + delta.X, Position.Y + delta.Y);

            if (isCoordInField(nextPosition))
            {
                Position = nextPosition;
            }
            else
            {
                CalcNextPositionForWalkingPerson();
            }
            
        }

        private void CalcNextPositionForGoingHomePerson()
        {
            var game = Game.Instance;
            var homeCoord = game.Map.Houses[HomeId].Coordinates.LeftTopCorner;
            var homeCenter = new Vec(homeCoord.X + HouseCoordinates.Width / 2, homeCoord.Y + HouseCoordinates.Height / 2);

            var xDiff = homeCenter.X - Position.X;
            var yDiff = homeCenter.Y - Position.Y;
            var xDistance = Math.Abs(xDiff);
            var yDistance = Math.Abs(yDiff);

            var distance = xDistance + yDistance;
            if (distance <= MaxDistancePerTurn)
            {
                Position = homeCenter;
                _state = PersonState.AtHome;
                return;
            }

            var direction = new Vec(Math.Sign(xDiff), Math.Sign(yDiff));

            var xLength = Math.Min(xDistance, MaxDistancePerTurn); 
            var newX = Position.X + xLength * direction.X;
            var yLength = MaxDistancePerTurn - xLength;
            var newY = Position.Y + yLength * direction.Y;
            Position = new Vec(newX, newY);
        }

        public void GoHome()
        {
            if (_state != PersonState.Walking) return;

            _state = PersonState.GoingHome;
            CalcNextPositionForGoingHomePerson();
        }

        private Vec ChooseDirection()
        {
            var directions = new Vec[]
            {
                new Vec(-1, -1),
                new Vec(-1, 1),
                new Vec(1, -1),
                new Vec(1, 1),
            };
            var index = _random.Next(directions.Length);
            return directions[index];
        }

        private bool isCoordInField(Vec vec)
        {
            var belowZero = vec.X < 0 || vec.Y < 0;
            var beyondField = vec.X > Game.FieldWidth || vec.Y > Game.FieldHeight;

            return !(belowZero || beyondField);
        }
    }


    internal enum PersonState
    {
        AtHome,
        Walking,
        GoingHome,
    }
}