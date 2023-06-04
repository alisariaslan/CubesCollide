namespace Assets.Scripts.Helpers
{
	public static class NameRandomizer
	{
		public static string GetRandomName(int minChar, int maxChar)
		{
			char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
			char[] consonants = new char[] {  'b','c','d','f','g','h','j','k','l','m','n','p',
			'r','s','t','v','y','z'};
			/*
			char[] vowels = new char[] { 'a', 'e', 'ı', 'i', 'o', 'ö', 'u', 'ü' };
			char[] consonants = new char[] {  'b','c','ç','d','f','g','ğ','h','j','k','l','m','n','p',
			'r','s','ş','t','v','y','z'};
			*/
			int vCount = vowels.Length;
			int cCount = consonants.Length;
			System.Random rnd = new System.Random();
			int randomLen = rnd.Next(minChar, maxChar + 1);
			string randomStr = "";
			int len = 0;

			if (rnd.Next(0, 2) == 1)
				len--;
			while (randomLen > 0)
			{
				if (len % 2 == 0)
				{
					randomStr += vowels[rnd.Next(0, vCount)];
					randomLen--;
					len++;
				}
				else
				{
					randomStr += consonants[rnd.Next(0, cCount)];
					randomLen--;
					len++;
					if (randomLen == 0)
						break;
					if (rnd.Next(0, 2) == 1)
					{
						randomStr += consonants[rnd.Next(0, cCount)];
						randomLen--;
					}
				}
			}
			return randomStr;
		}
	}
}
