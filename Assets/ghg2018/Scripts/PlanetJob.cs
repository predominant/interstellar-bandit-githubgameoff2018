using System;

namespace ghg2018
{
    public enum PlanetJobType
    {
        Train,
    }
    
    [Serializable]
    public class PlanetJob
    {
        public string Name;
        public int Value;
        public PlanetJobType Type = PlanetJobType.Train;
    }
}