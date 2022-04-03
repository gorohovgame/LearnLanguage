
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Speech.AudioFormat;

namespace Learn
{
    public partial class Form1 : Form
    {
        #region On Top
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        //FOR ON LOAD - SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        #endregion

        #region Tray
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        #region Private Variables
        IniFile iniFile = new IniFile();
        private static System.Windows.Forms.Timer aTimer;
        private int totalCount = 0;
        private int notRememberedCount = 0;


        private Bases currentBase = null;
        private Vocabulary currentWorld = null;
        #endregion

        #region INIT
        public Form1()
        {
            InitializeComponent();
            SetFormPosition();
            (new DropShadow()).ApplyShadows(this);

            InitMouseDoubleClick();

            PreloadIniData();
            InitDataBase();
            InitMenu();

            //LoadWorlds();

            GetNextWorld();
            SetWorld();
            InitTimer();
        }

        private void SetFormPosition()
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - (Size.Width + 5),
                                      workingArea.Bottom - (Size.Height + 5));
        }

        private void PreloadIniData()
        {
            if (!iniFile.KeyExists("currentBase", "Main"))
                iniFile.Write("currentBase", System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\main.realm", "Main");

            if (!iniFile.KeyExists("languages", "Main"))
                iniFile.Write("languages", @"EN;RU", "Main");

            if (!iniFile.KeyExists("timer", "Main"))
                iniFile.Write("timer", "1000", "Main");
        }

        private void InitTimer()
        {
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = Int32.Parse(iniFile.Read("timer", "main"));
            aTimer.Tick += new EventHandler(OnTimedEvent);

        }

        #region INIT MENU
        private void InitMenu()
        {
            var cm = new ContextMenuStrip();

            var submenu = new ToolStripMenuItem("Vocabulary");
            submenu.DropDownItems.Add(CreateStripMenuItem("Add new Vocabulary", AddNewVocabulary, Properties.Resources.add));

            var bases = DataBase.realm.All<Bases>();
            if (bases.Count() > 0)
                submenu.DropDownItems.Add(new ToolStripSeparator());
            foreach (var item in bases)
            {
                submenu.DropDownItems.Add(CreateStripMenuItem(item.Name, () => SetActivaeVocabulary(item), item.Active ? Properties.Resources.next : null));
            }
            if (bases.Count() > 0)
            {
                submenu.DropDownItems.Add(new ToolStripSeparator());
                submenu.DropDownItems.Add(CreateStripMenuItem("Delete current Vocabulary", () =>
                {
                    DialogResult dialogResult = MessageBox.Show("You are sure?", "Delete current vocabulary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DeleteCurrentVocabulary();
                        InitMenu();
                    }

                }, Properties.Resources.basket));
            }
            cm.Items.Add(submenu);

            submenu = new ToolStripMenuItem("Show Base");
            submenu.DropDownItems.Add(CreateStripMenuItem("Show Vocabulary", ShowDataBase));
            submenu.DropDownItems.Add(CreateStripMenuItem("Show Remembered Word", ShowRememberedWord));
            cm.Items.Add(submenu);

            var cI = iniFile.Read("timer", "main");
            submenu = new ToolStripMenuItem("Time Interval");
            submenu.DropDownItems.Add(CreateStripMenuItem("2 sec", () => SetTimerInterval(1000 * 2), (cI == "" + (1000 * 2)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("10 sec", () => SetTimerInterval(1000 * 10), (cI == "" + (1000 * 10)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("1 min", () => SetTimerInterval(1000 * 60), (cI == "" + (1000 * 60)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("5 min", () => SetTimerInterval(1000 * 60 * 5), (cI == "" + (1000 * 60 * 5)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("10 min", () => SetTimerInterval(1000 * 60 * 10), (cI == "" + (1000 * 60 * 10)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("20 min", () => SetTimerInterval(1000 * 60 * 20), (cI == "" + (1000 * 60 * 20)) ? Properties.Resources.next : null));
            submenu.DropDownItems.Add(CreateStripMenuItem("30 min", () => SetTimerInterval(1000 * 60 * 30), (cI == "" + (1000 * 60 * 30)) ? Properties.Resources.next : null));
            cm.Items.Add(submenu);
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                submenu = new ToolStripMenuItem("Voice Language");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }
                    submenu.DropDownItems.Add(CreateStripMenuItem(
                        info.Culture + ": " + info.Name,
                        () =>
                        {
                            iniFile.Write("voice", info.Name, "Main");
                            InitMenu();
                        },
                        iniFile.Read("voice", "Main") == info.Name ? Properties.Resources.next : null));
                }
                cm.Items.Add(submenu);
            }
            submenu = new ToolStripMenuItem("Time Interval");
            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add(CreateStripMenuItem("Clean Vocabulary", () =>
            {
                DialogResult dialogResult = MessageBox.Show("You are sure?", "Clean vocabulary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    ClearAll();
                    InitMenu();
                    //CleanVocabulary(); CleanRememberedWorld();
                }

            }, Properties.Resources.basket));

            cm.Items.Add(new ToolStripSeparator());
            cm.Items.Add(CreateStripMenuItem("Exit", Exit, Properties.Resources.login));

            this.ContextMenuStrip = cm;
        }

        private ToolStripMenuItem CreateStripMenuItem(string name, Action method, Bitmap bitmap = null)
        {
            var item = new ToolStripMenuItem(name);
            if (bitmap != null)
                item.Image = bitmap;
            item.Click += new EventHandler((object sender, EventArgs e) => { method(); });

            return item;
        }
        #endregion
        #endregion

        #region UI
        private void GetNextWorld()
        {
            if (currentBase == null)
                return;

            var worlds = currentBase.Vocabulary.Where(g => g.Remembered == false);
            var count = currentBase.Vocabulary.Count();
            totalCount = count;
            notRememberedCount = worlds.Count();

            var r = new Random();
            if (notRememberedCount > 0)
                currentWorld = worlds.ElementAt(r.Next(0, notRememberedCount));
            else
                currentWorld = GetEmptyVocabulary();
        }

        private void SetWorld()
        {
            var Learn = "Dictionary not loaded.";
            var LearnText = "";
            var Translate = "Не загружен словарь.";
            var TranslateText = "";
            var Translit = "";
            if (currentWorld != null)
            {
                Learn = "" + currentWorld.Learn;
                LearnText = "" + currentWorld.LearnText;
                Translate = "" + currentWorld.Translate;
                TranslateText = "" + currentWorld.TranslateText;
                Translit = "" + currentWorld.Translit;
                /*if (LearnText == "")
                {
                    var worlds = DataBase.realm.All<Vocabulary>().Where(g => g.Learn == Learn).ToList();
                    if (worlds.Count > 1)
                    {
                    }

                }*/
            }




            String learn = FirstCharToUpper(Learn);
            if (learn.Substring(Math.Max(0, learn.Length - 1)) == "2")
            {
                learn = learn.Substring(0, Math.Max(0, learn.Length - 1));
            }

            label1.Paint += (sender, e) => FontWizard.OnPaint(sender, e, $"{learn}", new SolidBrush(label1.ForeColor));
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(label1, Translit);
            label5.Paint += (sender, e) => FontWizard.OnPaint(sender, e, LearnText.ToString(), new SolidBrush(label5.ForeColor), false);
            label2.Paint += (sender, e) => FontWizard.OnPaint(sender, e, FirstCharToUpper(Translate.ToString()), new SolidBrush(label2.ForeColor));
            label6.Paint += (sender, e) => FontWizard.OnPaint(sender, e, TranslateText.ToString(), new SolidBrush(label6.ForeColor), false);

            label3.Text = $"Total: {totalCount}. Remembered: {totalCount - notRememberedCount}. Unlearned: {notRememberedCount}" + " / Language: " + currentBase?.Language;
            this.Refresh();

        }
        #endregion

        #region Database
        private void InitDataBase()
        {
            DataBase.Init(iniFile.Read("currentBase", "Main"));
            currentBase = DataBase.realm.All<Bases>().Where(i => i.Active).FirstOrDefault();
            if (currentBase != null)
                GetNextWorld();

            SetWorld();
        }

        private void LoadVocabulary()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse CSV Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadWorlds(openFileDialog1.FileName);
                GetNextWorld();
                SetWorld();
            }
        }

        private void DeleteCurrentVocabulary()
        {
            if (currentBase == null)
                return;

            DataBase.realm.Write(() =>
            {
                foreach (var item in currentBase.Vocabulary)
                {
                    DataBase.realm.Remove(item);
                }
                DataBase.realm.Remove(currentBase);
            });
            var count = DataBase.realm.All<Vocabulary>().Count();
            currentBase = null;
            currentWorld = null;
            totalCount = 0;
            notRememberedCount = 0;
            SetWorld();
        }

        private void ClearAll()
        {
            currentBase = null;
            currentWorld = null;
            totalCount = 0;
            notRememberedCount = 0;
            GetNextWorld();
            SetWorld();
            DataBase.realm.Write(() =>
            {
                DataBase.realm.RemoveAll<Bases>();
                DataBase.realm.RemoveAll<Vocabulary>();
                DataBase.realm.RemoveAll<RememberedWord>();
            });

        }

        private Vocabulary GetEmptyVocabulary()
        {
            return currentWorld = new Vocabulary()
            {
                Learn = totalCount == 0 ? "Free" : "You WIN!",
                Translate = totalCount == 0 ? "Пусто" : "Ты Выиграл!",
            };
        }

        private void LoadWorldsNew(string filename, IList<Vocabulary> voc)
        {
            using (var reader = new StreamReader(filename))
            {
                var header = true;
                List<string> languages = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (header)
                    {
                        foreach (var value in values)
                        {
                            Console.WriteLine(value);
                            languages.Add(value);
                        }
                        header = false;
                        continue;
                    }

                    var world = DataBase.realm.All<Vocabulary>().Where(g => g.Learn == values[0]).FirstOrDefault();
                    var worldRemember = DataBase.realm.All<RememberedWord>().Where(g => g.Learn == values[0]).FirstOrDefault();

                    var iLearn = languages.IndexOf("Learn");
                    var iTranslate = languages.IndexOf("Translate");
                    var iLearnText = languages.IndexOf("LearnText");
                    var iTranslateText = languages.IndexOf("TranslateText");
                    var iTranslit = languages.IndexOf("Translit");
                    var iURL = languages.IndexOf("URL");

                    if (iLearn >= 0)
                    {
                        if (world == null)
                        {
                            voc.Add(new Vocabulary()
                            {
                                Learn = iLearn >= 0 ? values[iLearn] : "",
                                Translate = iTranslate >= 0 ? values[iTranslate] : "",
                                LearnText = iLearnText >= 0 ? values[iLearnText] : "",
                                TranslateText = iTranslateText >= 0 ? values[iTranslateText] : "",
                                Remembered = worldRemember == null ? false : true,
                                Translit = iTranslit >= 0 ? values[iTranslit] : "",
                                URL = iURL >= 0 ? values[iURL] : "",
                            });
                        }
                        else
                        {
                            /*     DataBase.realm.Write(() =>
                                 {
                                     world.Learn = iLearn >= 0 ? values[iLearn] : world.Learn;
                                     world.Translate = iTranslate >= 0 ? values[iTranslate] : world.Translate;
                                     world.LearnText = iLearnText >= 0 ? values[iLearnText] : world.LearnText;
                                     world.TranslateText = iTranslateText >= 0 ? values[iTranslateText] : world.TranslateText;
                                     world.Translit = iTranslit >= 0 ? values[iTranslit] : world.Translit;
                                     world.Remembered = worldRemember == null ? false : true;
                                     world.URL = iURL >= 0 ? values[iURL] : world.URL;
                                     DataBase.realm.Add(world);
                                 });*/
                        }
                    }
                }
            }

        }

        private void LoadWorlds(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                var header = true;
                List<string> languages = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    if (header)
                    {
                        foreach (var value in values)
                        {
                            Console.WriteLine(value);
                            languages.Add(value);
                        }
                        header = false;
                        continue;
                    }

                    var world = DataBase.realm.All<Vocabulary>().Where(g => g.Learn == values[0]).FirstOrDefault();
                    var worldRemember = DataBase.realm.All<RememberedWord>().Where(g => g.Learn == values[0]).FirstOrDefault();

                    var iLearn = languages.IndexOf("Learn");
                    var iTranslate = languages.IndexOf("Translate");
                    var iLearnText = languages.IndexOf("LearnText");
                    var iTranslateText = languages.IndexOf("TranslateText");
                    var iTranslit = languages.IndexOf("Translit");
                    var iURL = languages.IndexOf("URL");

                    if (iLearn >= 0)
                    {
                        if (world == null)
                        {
                            DataBase.Add(new Vocabulary()
                            {
                                Learn = iLearn >= 0 ? values[iLearn] : "",
                                Translate = iTranslate >= 0 ? values[iTranslate] : "",
                                LearnText = iLearnText >= 0 ? values[iLearnText] : "",
                                TranslateText = iTranslateText >= 0 ? values[iTranslateText] : "",
                                Remembered = worldRemember == null ? false : true,
                                Translit = iTranslit >= 0 ? values[iTranslit] : "",
                                URL = iURL >= 0 ? values[iURL] : "",
                            });
                        }
                        else
                        {
                            DataBase.realm.Write(() =>
                            {
                                world.Learn = iLearn >= 0 ? values[iLearn] : world.Learn;
                                world.Translate = iTranslate >= 0 ? values[iTranslate] : world.Translate;
                                world.LearnText = iLearnText >= 0 ? values[iLearnText] : world.LearnText;
                                world.TranslateText = iTranslateText >= 0 ? values[iTranslateText] : world.TranslateText;
                                world.Translit = iTranslit >= 0 ? values[iTranslit] : world.Translit;
                                world.Remembered = worldRemember == null ? false : true;
                                world.URL = iURL >= 0 ? values[iURL] : world.URL;
                                DataBase.realm.Add(world);
                            });
                        }
                    }
                }
            }
            //var count = DataBase.realm.All<Vocabulary>().Count();
        }

        private void SetRemember()
        {
            DataBase.realm.Write(() =>
            {
                currentWorld.Remembered = true;
                //DataBase.realm.Add(currentWorld);
            });

            if (DataBase.realm.All<RememberedWord>().Where(g => g.Learn == currentWorld.Learn).Count() == 0)
            {
                DataBase.realm.Write(() =>
                {
                    DataBase.realm.Add(new RememberedWord()
                    {
                        Learn = currentWorld.Learn
                    });
                });
            }
        }

        private void AddNewVocabulary()
        {
            using (var form = new gorm2())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Bases currentItem = null;
                    DataBase.realm.Write(() =>
                    {
                        currentItem = DataBase.realm.Add(new Bases()
                        {
                            Name = form.Name,
                            Description = form.Description,
                            Language = form.Language,
                        });

                        LoadWorldsNew(form.FileName, currentItem.Vocabulary);
                    });
                    SetActivaeVocabulary(currentItem);
                    InitMenu();
                }
            }
        }

        private void SetActivaeVocabulary(Bases bases)
        {
            DataBase.realm.Write(() =>
            {
                if (currentBase != null)
                    currentBase.Active = false;

                bases.Active = true;
            });

            currentBase = bases;
            GetNextWorld();
            SetWorld();
            InitMenu();
        }
        #endregion

        #region Show Data Base
        private void ShowUrl()
        {
            if (currentWorld.URL == null || currentWorld.URL == String.Empty)
                return;

            using (Form form = new Form())
            {
                form.Text = "Brouse";
                var webBrowser = new WebBrowser();
                webBrowser.AllowWebBrowserDrop = false;

                //webBrowser.Url = new Uri(currentWorld.URL);
                var webBrowserDocumentText = @"
                <html><head><title></title>
                    <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>
                    <script type='text/javascript'>
                    var keyword = '" + currentWorld.Learn + @"';
                    $(document).ready(function(){
                    $.getJSON('http://api.flickr.com/services/feeds/photos_public.gne?jsoncallback=?',
                    {
                        tags: keyword,
                        tagmode: '" + currentWorld.Learn + @"',
                        format: 'json'
                    },
                    function(data) {
                        var rnd = Math.floor(Math.random() * data.items.length);
                        var image_src = data.items[rnd]['media']['m'].replace('_m', '_b');
                ";
                webBrowserDocumentText += "$('body').css('background-image', \"url('\" + image_src + \"')\");";
                webBrowserDocumentText += @"
                    });});</script><style>body {background-size: contain;background-repeat: no-repeat;}</style></head><body></body></html>
                ";

                webBrowser.DocumentText = webBrowserDocumentText;
                Console.WriteLine(currentWorld.URL);

                webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
                webBrowser.Parent = form;
                form.Width = 800;
                form.Height = 800;
                form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                form.ShowDialog(this);
                form.Dispose();
            }
        }

        private void ShowDataBase()
        {
            using (Form form = new Form())
            {
                form.Text = "Vocabulary";

                var list = new List<VocabularyList>();

                var vocabulary = DataBase.realm.All<Vocabulary>();
                foreach (var item in vocabulary)
                {
                    list.Add(new VocabularyList()
                    {
                        Learn = item.Learn,
                        Translate = item.Translate,
                        TranslateText = item.TranslateText,
                        LearnText = item.LearnText,
                        Remembered = item.Remembered,
                        Translit = item.Translit,
                        URL = item.URL
                    });
                }


                var bindingList = new BindingList<VocabularyList>(list);
                var dataGrid = new DataGridView();
                dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;

                var source = new BindingSource(bindingList, null);
                dataGrid.DataSource = source;
                dataGrid.Parent = form;
                dataGrid.ReadOnly = true;
                dataGrid.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
                form.ShowDialog(this);
                form.Width = 500;
            }
        }

        private void ShowRememberedWord()
        {
            using (Form form = new Form())
            {
                form.Text = "Remembered Word";

                var list = new List<RememberedWordList>();

                var vocabulary = DataBase.realm.All<RememberedWord>();
                foreach (var item in vocabulary)
                {
                    list.Add(new RememberedWordList()
                    {
                        Learn = item.Learn,
                    });
                }

                var bindingList = new BindingList<RememberedWordList>(list);
                var dataGrid = new DataGridView();
                dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;

                var source = new BindingSource(bindingList, null);
                dataGrid.DataSource = source;
                dataGrid.Parent = form;
                dataGrid.ReadOnly = true;
                dataGrid.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
                form.ShowDialog(this);
                form.Width = 500;
            }
        }
        #endregion

        #region FORM
        private void Exit()
        {
            this.Close();
        }

        void ShowForm()
        {
            aTimer.Stop();
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void HideForm()
        {
            GetNextWorld();
            SetWorld();
            Hide();
            notifyIcon.Visible = true;
            aTimer.Start();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ShowUrl();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                HideForm();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                //Double click
                clickCount++;
                timer.Start();
                //-----------
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SetRemember();
            HideForm();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }
        #endregion

        private void OnTimedEvent(Object source, EventArgs e)
        {
            ShowForm();
            Console.WriteLine("The Elapsed event was raised at ");
        }

        void SetTimerInterval(int interval)
        {
            aTimer.Interval = interval;
            iniFile.Write("timer", "" + interval, "Main");
            InitMenu();
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        #region Double click
        System.Windows.Forms.Timer timer;
        int clickCount = 0;

        private void InitMouseDoubleClick()
        {

            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Elapsed;
            //SystemInformation.DoubleClickTime default is 500 Milliseconds
            timer.Interval = SystemInformation.DoubleClickTime;
            //or
            timer.Interval = 200;
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            timer.Stop();
            if (clickCount >= 2)
            {
                GetNextWorld();
                SetWorld();
            }
            else
            {
                //MessageBox.Show("click");
            }
            clickCount = 0;
        }
        #endregion

        #region Voice
        private bool speck = false;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (currentWorld == null)
                return;
            var voice = iniFile.Read("voice", "Main");
            if (voice == null || voice == String.Empty)
            {
                MessageBox.Show("Please select Voice.",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
                return;

            }

            if (speck) return;
            speck = true;
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToDefaultAudioDevice();



            synthesizer.SelectVoice(voice);


            synthesizer.Speak(currentWorld.Learn);
            if (currentWorld.LearnText.ToString() != String.Empty)
            {
                synthesizer.Speak("          ");
                synthesizer.Speak(currentWorld.LearnText);
            }
            speck = false;
            //GetVois();
        }


        private void GetVois()
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {

                // Output information about all of the installed voices.   
                Console.WriteLine("Installed voices -");
                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    string AudioFormats = "";
                    foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                    {
                        AudioFormats += String.Format("{0}\n",
                        fmt.EncodingFormat.ToString());
                    }

                    Console.WriteLine(" Name:          " + info.Name);
                    Console.WriteLine(" Culture:       " + info.Culture);
                    Console.WriteLine(" Age:           " + info.Age);
                    Console.WriteLine(" Gender:        " + info.Gender);
                    Console.WriteLine(" Description:   " + info.Description);
                    Console.WriteLine(" ID:            " + info.Id);
                    Console.WriteLine(" Enabled:       " + voice.Enabled);
                    if (info.SupportedAudioFormats.Count != 0)
                    {
                        Console.WriteLine(" Audio formats: " + AudioFormats);
                    }
                    else
                    {
                        Console.WriteLine(" No supported audio formats found");
                    }

                    string AdditionalInfo = "";
                    foreach (string key in info.AdditionalInfo.Keys)
                    {
                        AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                    }

                    Console.WriteLine(" Additional Info - " + AdditionalInfo);
                    Console.WriteLine();
                }
            }


        }
        #endregion
    }
}