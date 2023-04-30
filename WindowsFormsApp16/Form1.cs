using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp16
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
        private string currentLanguage = "en";

        public Form1()
        {
            InitializeComponent();

            LoadDictionary(currentLanguage);
        }

        private void LoadDictionary(string language)
        {
            dictionary.Clear();

            string filePath = $"{language}_dictionary.txt";

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] parts = line.Split(' ');

                            if (parts.Length == 2)
                            {
                                string word = parts[0];
                                string key = parts[1];

                                if (dictionary.ContainsKey(key))
                                {
                                    dictionary[key].Add(word);
                                }
                                else
                                {
                                    dictionary[key] = new List<string> { word };
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateSuggestions(string text)
        {
            List<string> suggestions = GetSuggestions(text);

            if (suggestions.Count > 0)
            {
                suggestionsListBox.DataSource = suggestions;
                suggestionsListBox.Visible = true;
            }
            else
            {
                suggestionsListBox.Visible = false;
            }
        }

        private List<string> GetSuggestions(string text)
        {
            List<string> suggestions = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                string[] keys = text.Split(' ');

                if (keys.Length > 0)
                {
                    string lastKey = keys[keys.Length - 1];

                    if (dictionary.ContainsKey(lastKey))
                    {
                        suggestions = dictionary[lastKey];
                    }
                }
            }

            return suggestions;
        }

        private void AddToDictionary(string word)
        {
            string key = GetKeyForWord(word);

            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(word);
            }
            else
            {
                dictionary[key] = new List<string> { word };
            }

            SaveDictionary(currentLanguage);
        }

        private string GetKeyForWord(string word)
        {
            string key = "";

            foreach (char c in word)
            {
                key += GetKeyForCharacter(c);
            }

            return key;
        }
        private string GetKeyForCharacter(char c)
        {
            switch (currentLanguage)
            {
                case "en":
                    if (c >= 'a' && c <= 'c') return "2";
                    if (c >= 'd' && c <= 'f') return "3";
                    if (c >= 'g' && c <= 'i') return "4";
                    if (c >= 'j' && c <= 'l') return "5";
                    if (c >= 'm' && c <= 'o') return "6";
                    if (c >= 'p' && c <= 's') return "7";
                    if (c >= 't' && c <= 'v') return "8";
                    if (c >= 'w' && c <= 'z') return "9";
                    break;

                case "ru":
                    if (c == 'а' || c == 'б' || c == 'в') return "2";
                    if (c == 'г' || c == 'д' || c == 'е') return "3";
                    if (c == 'ж' || c == 'з' || c == 'и') return "4";
                    if (c == 'й' || c == 'к' || c == 'л') return "5";
                    if (c == 'м' || c == 'н' || c == 'о') return "6";
                    if (c == 'п' || c == 'р' || c == 'с') return "7";
                    if (c == 'т' || c == 'у' || c == 'ф') return "8";
                    if (c == 'х' || c == 'ц' || c == 'ч') return "9";
                    if (c == 'ш' || c == 'щ' || c == 'ъ') return "0";
                    if (c == 'ы' || c == 'ь' || c == 'э') return "1";
                    if (c == 'ю' || c == 'я') return "2";
                    break;
            }

            return "";
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == ' ')
            {
                if (e.KeyChar == ' ')
                {
                    suggestionsListBox.Items.Add(" ");
                }
                else
                {
                    suggestionsListBox.Items.Add(e.KeyChar.ToString());
                }

                string digits = "";
                foreach (string item in suggestionsListBox.Items)
                {
                    digits += item;
                }

                List<string> words = FindWords(digits);

                suggestionsListBox.Items.Clear();

                foreach (string word in words)
                {
                    suggestionsListBox.Items.Add(word);
                }
            }
        }
        private List<string> FindWords(string digits)
        {
            List<string> words = new List<string>();

            string[] arr = digits.Trim().Split(' ');

            foreach (string s in arr)
            {
                if (dictionary.ContainsKey(s))
                {
                    words.Add(arr[]);
                }
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string word = txtInput.Text;
            if (string.IsNullOrEmpty(word))
            {
                MessageBox.Show("Введите слово для добавления в словарь");
                return;
            }

            if (!dictionary.ContainsKey(word))
            {
                dictionary.Add(word, true);
                MessageBox.Show($"Слово \"{word}\" успешно добавлено в словарь");
            }
            else
            {
                MessageBox.Show($"Слово \"{word}\" уже есть в словаре");
            }
        }
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonAdd_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        
    }
}