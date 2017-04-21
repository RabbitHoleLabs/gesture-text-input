using System.Collections.Generic;
using System.Linq;
using System.IO;

public class RabbitHoleGestureKeyboard {

	public static string[] KEYBOARD_LAYOUT = new string[] {"qwertyuiop", "asdfghjkl", "zxcvbnm"};

	public static bool match(string path, string word) {
		if (path.Length < word.Length) {
			return false;
		}
		int w = 0;
		int p = 0;
		while (w < word.Length & p < path.Length) {
			if (word[w].Equals(path[p])) {
				w = w + 1;
			}
			p = p + 1;
		}
		if (w == word.Length) {
			return true;
		}
		return false;
	}

	// Handles lower case, single character on keyboard, as string
	public static int getRow(char c) {
		string s = c.ToString();
		for (int i = 0; i < KEYBOARD_LAYOUT.Length; i++) {
			if (KEYBOARD_LAYOUT[i].Contains(s)) {
				return i;
			}
		}
		return -1;
	}

	public static string compressSequence(string sequence) {
		if (sequence.Length == 0) {
			return string.Empty;
		}
		string compression = sequence[0].ToString();
		char prev = sequence[0];
		for (int i = 1; i < sequence.Length; i++) {
			if (sequence[i] != prev) {
				compression += sequence[i].ToString();
				prev = sequence[i];
			}
		}
		return compression;
	}

	// Original code subtracts 3 from return result
	public static int getMinWordLength(string path) {
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < path.Length; i++) {
			sb.Append(getRow(path[i]));
		}
		string compression = compressSequence(sb.ToString());
		return compression.Length - 3;
	}

	public static List<string> populateWords(string fileName) {
		List<string> words = new List<string>();
		using (StreamReader sr = new StreamReader(fileName)) {
			while (!sr.EndOfStream) {
				words.Add(sr.ReadLine());
			}
		}	
		return words;
	}

	public static List<string> getSuggestions(string path, ref List<string> words) {
		int minLength = getMinWordLength(path);
		var filter = 
			from word in words
			where
				(
				   word[0] == path[0] && 
				   word[word.Length - 1] == path[path.Length - 1] &&
				   match(path, word) &&
				   word.Length >= minLength
				)
			select word;
		List<string> suggestions = new List<string>();
		foreach(string w in filter) {
			suggestions.Add(w);
		}
		return suggestions;
	}

	static void Main(string[] args) {
		List<string> words = populateWords("../Resources/dictionary.txt");
		List<string> suggestions = getSuggestions("wertyuioiuytrtghjklkjhgfd", ref words);
		foreach(string sugg in suggestions) {
			System.Console.WriteLine(sugg);
		}
	}

}