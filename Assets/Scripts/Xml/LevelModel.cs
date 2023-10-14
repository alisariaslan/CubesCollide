using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
	[XmlRoot(ElementName = "Level")]
	public class Level
	{
		[XmlElement(ElementName = "levelName")]
		public string LevelName { get; set; }
		[XmlElement(ElementName = "isUnlocked")]
		public bool IsUnlocked { get; set; }
        [XmlElement(ElementName = "saved")]
        public bool IsSaved { get; set; }
    }

	[XmlRoot(ElementName = "Levels")]
	public class Levels
	{
		[XmlElement(ElementName = "Level")]
		public List<Level> LevelList { get; set; }
	}

	[XmlRoot(ElementName = "BETHEBIGGEST")]
	public class BETHEBIGGEST
	{
		[XmlElement(ElementName = "Levels")]
		public Levels Levels { get; set; }
	}

	[XmlRoot(ElementName = "GameModes")]
	public class GameModes
	{
		[XmlElement(ElementName = "BETHEBIGGEST")]
		public BETHEBIGGEST BETHEBIGGEST { get; set; }
	}
}