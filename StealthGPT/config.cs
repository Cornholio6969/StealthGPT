using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace StealthGPT
{
    public class config : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _titleKey;
        private int _quizKey;
        private int _queryKey;
        private int _panicKey;
        public bool instaCopy { get; set; }
        public bool setClipboard { get; set; }
        public bool speech { get; set; }
        public bool enabled { get; set; }
        public bool autoMode { get; set; }
        public bool overlay { get; set; }
        public string _gptApiKey { get; set; }
        public string _gptModel { get; set; }
        public string _attachedProcessName { get; set; }
        public string _gptStartPrompt { get; set; }

        public int TitleKey
        {
            get { return _titleKey; }
            set
            {
                if (_titleKey != value)
                {
                    _titleKey = value;
                    OnPropertyChanged(nameof(TitleKey));
                    OnStealthConfigChanged();
                }
            }
        }

        public int QuizKey
        {
            get { return _quizKey; }
            set
            {
                if (_quizKey != value)
                {
                    _quizKey = value;
                    OnPropertyChanged(nameof(QuizKey));
                    OnStealthConfigChanged();
                }
            }
        }

        public int QueryKey
        {
            get { return _queryKey; }
            set
            {
                if (_queryKey != value)
                {
                    _queryKey = value;
                    OnPropertyChanged(nameof(QueryKey));
                    OnStealthConfigChanged();
                }
            }
        }

        public int PanicKey
        {
            get { return _panicKey; }
            set
            {
                if (_panicKey != value)
                {
                    _panicKey = value;
                    OnPropertyChanged(nameof(PanicKey));
                    OnStealthConfigChanged();
                }
            }
        }


        public bool checkForKeyExistence(int key)
        {
            if (_titleKey == key || _quizKey == key || QueryKey == key || PanicKey == key)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnStealthConfigChanged()
        {
            stealthGPT.setConfig();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText("config.json", json);
        }

        public static void Load()
        {
            if (File.Exists("config.json"))
            {
                string json = File.ReadAllText("config.json");
                Program.config = JsonConvert.DeserializeObject<config>(json);
            }
            else
            {
                Program.config = new config();
            }
        }

    }
}
