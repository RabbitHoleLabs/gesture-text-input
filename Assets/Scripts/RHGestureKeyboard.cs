using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class RabbitHoleGestureKeyboard {

	public static string[] KEYBOARD_LAYOUT = new string[] {"qwertyuiop", "asdfghjkl", "zxcvbnm"};
    private List<string> WORDS = new List<string>();

    public RabbitHoleGestureKeyboard(string dict) {

        using (StringReader sr = new StringReader(dict))
        {
            string line = sr.ReadLine();
            while (line != null)
            {
                this.WORDS.Add(line);
                line = sr.ReadLine();
            }
        }

    }

    private bool match(string path, string word) {
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
	private int getRow(char c) {
		string s = c.ToString();
		for (int i = 0; i < KEYBOARD_LAYOUT.Length; i++) {
			if (KEYBOARD_LAYOUT[i].Contains(s)) {
				return i;
			}
		}
		return -1;
	}

	private string compressSequence(string sequence) {
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
	private int getMinWordLength(string path) {
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < path.Length; i++) {
			sb.Append(getRow(path[i]));
		}
		string compression = compressSequence(sb.ToString());
		return compression.Length - 3;
	}

	public List<string> getSuggestions(string path) {
        Debug.Log(path);
		int minLength = getMinWordLength(path);

        var filter =
            from word in this.WORDS
            where
                (
                         //word[0] == path[0] &&
                         word[word.Length - 1] == path[path.Length - 1] &&
                         match(path, word) &&
                   word.Length >= minLength
                )
            select word;
        List<string> suggestions = filter.ToList();

		return suggestions;
	}
}
