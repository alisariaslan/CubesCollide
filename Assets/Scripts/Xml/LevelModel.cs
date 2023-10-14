using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.Scripts.Xml
{
	[XmlRoot(ElementName = "Level")]
	public class Level
	{
		[XmlElement(ElementName = "levelNo")]
		public string LevelNo { get; set; }
		[XmlElement(ElementName = "isLocked")]
		public string IsLocked { get; set; }
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