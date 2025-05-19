using System.CodeDom;
using Newtonsoft.Json.Linq;

namespace Support_Card_Manager
{
    public partial class Form1 : Form
    {
        // 全種類のカードタイプ
        public readonly string[] cardType = {
            "タイプ1", "タイプ2", "タイプ3",
            "タイプ4", "タイプ5", "タイプ6", "タイプ7"
        }; 

        // 未選択時のカードタイプ
        public const string NONE_TYPE = "未選択";

        // 未進行の画像
        private const string PROGRESS_FALSE_IMAGE = "images\\ProgressImages\\eventprogress_0.png";

        // 進行済みの画像
        private const string PROGRESS_TRUE_IMAGE = "images\\ProgressImages\\eventprogress_1.png";

        // 未選択時の画像
        public const string NONE_IMAGE = "images\\CardImages\\未選択\\none.png";

        // カード画像のフォルダパス
        public const string FOLDER_PATH = "images\\CardImages\\";

        // 編成カード枚数
        public const int NUM_CARDS = 6;

        // カード1枚あたりのコンボボックス数
        public const int COMBO_BOXES_PER_CARD = 2;

        // イベント進行度のデフォルト値
        private const int DEFAULT_PROGRESS = 0;

        // イベント進行度の最大値
        private const int PROGRESS_STAGES = 3;

        // 完走ラベルのX座標オフセット
        private const int COMPLETE_LABEL_OFFSET_X = 55;

        // 完走ラベルのY座標オフセット
        private const int COMPLETE_LABEL_OFFSET_Y = 100;

        // Form2の表示位置オフセットX
        private const int FORM2_OFFSET_X = 100;

        // Form2の表示位置オフセットY
        private const int FORM2_OFFSET_Y = 100;

        // Form3の表示位置オフセットX
        private const int FORM3_OFFSET_X = 100;

        // Form3の表示位置オフセットY
        private const int FORM3_OFFSET_Y = 200;

        // 各カードのカードタイプを格納する配列
        private readonly string[] cardsType;

        // 各カードのカードタイプを格納するコンボボックスの配列
        public ComboBox[] typeComboBoxes;

        // 各カードのカード名を格納するコンボボックスの配列
        public ComboBox[] nameComboBoxes;

        // 各カードの進行状況を表す画像を格納するピクチャーボックスの配列
        public PictureBox[] progressPictureBoxes;

        // 各カードの選択したカード画像を格納するピクチャーボックスの配列
        public PictureBox[] cardPictureBoxes;

        // 各カードの進行状況を格納する配列
        private int[] cardsProgress;

        // 各カードのイベント進行ボタンを格納する配列
        private Button[] progressButtons;

        // 各カードの完走ボタンを格納する配列
        private Button[] completeButtons;

        // 各カードの完走ボタンを格納する配列
        private Button[] resetButtons;

        // 各カードの完走ラベルを格納する配列
        private Label[] labels;

        // Form1のコンストラクタ
        public Form1()
        {
            InitializeComponent();

            // 各カードのカードタイプを格納する配列の初期化
            cardsType = new string[NUM_CARDS];
            for(int i = 0;i < NUM_CARDS; i++)
            {
                cardsType[i] = NONE_TYPE;
            }

            // 各カードのカードタイプを格納するコンボボックスの配列の初期化
            typeComboBoxes = new ComboBox[NUM_CARDS]
            {
                comboBox1, comboBox3, comboBox5, 
                comboBox7,comboBox9, comboBox11
            };

            // 各コンボボックスにカードタイプ(未選択も含む)を格納
            foreach (string type in cardType)
            {
                foreach(ComboBox cb in typeComboBoxes)
                {
                    cb.Items.Add(type);
                }
            }
            foreach (ComboBox cb in typeComboBoxes)
            {
                cb.Items.Add(NONE_TYPE);
            }

            // 各カードのカードタイプのコンボボックスのタグを設定
            // カード1枚目に0、2枚目に1・・・のようにタグを設定
            // 以降のタグ設定でも同様の処理を行う
            SetControlIndexTags(typeComboBoxes);

            // 各カードのカード名を格納するコンボボックスの配列の初期化
            nameComboBoxes = new ComboBox[NUM_CARDS]
            {
                comboBox2, comboBox4, comboBox6,
                comboBox8,comboBox10, comboBox12
            };

            // 各コンボボックスに未選択画像の画像名を格納（初期化）
            foreach (ComboBox cb in nameComboBoxes)
            {
                string noneImageName = Path.GetFileNameWithoutExtension(NONE_IMAGE);
                cb.Items.Add(noneImageName);
            }

            // 各カードのカード名のコンボボックスのタグを設定
            SetControlIndexTags(nameComboBoxes);

            // 各カードのイベント進行ボタンを格納する配列の初期化
            progressButtons = new Button[NUM_CARDS] {
                button1, button4, button7,
                button10, button13, button16
            };

            // 各カードのイベント進行ボタンのタグを設定
            SetControlIndexTags(progressButtons);

            // 各カードの完走ボタンを格納する配列の初期化
            completeButtons = new Button[] {
                button2, button5, button8,
                button11, button14, button17
            };

            // 各カードの完走ボタンのタグを設定
            SetControlIndexTags(completeButtons);

            // 各カードのイベントリセットボタンを格納する配列の初期化
            resetButtons = new Button[] {
                button3, button6, button9,
                button12, button15, button18
            };

            // 各カードのイベントリセットボタンのタグを設定
            SetControlIndexTags(resetButtons);

            // 各カードの進行状況を表す画像を格納するピクチャーボックスの配列の初期化
            progressPictureBoxes = new PictureBox[] {
                pictureBox2, pictureBox3, pictureBox4,
                pictureBox6, pictureBox7, pictureBox8,
                pictureBox10, pictureBox11, pictureBox12,
                pictureBox14, pictureBox15, pictureBox16,
                pictureBox18, pictureBox19, pictureBox20,
                pictureBox22, pictureBox23, pictureBox24
            };

            // 各カードの進行状況を表す画像を格納するピクチャーボックスを未進行で初期化
            foreach(PictureBox pb in progressPictureBoxes)
            {
                try
                {
                    pb.Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"エラー: 画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"必要な画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE}", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE}", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // カード画像を格納するコンボボックスの配列の初期化
            cardPictureBoxes = new PictureBox[] {
                pictureBox1, pictureBox5, pictureBox9,
                pictureBox13, pictureBox17,pictureBox21
            
            };

            // 各カードの進行状況を格納する配列の初期化
            cardsProgress = new int[NUM_CARDS];
            for( int i = 0; i < NUM_CARDS; i++ )
            {
                cardsProgress[i] = DEFAULT_PROGRESS;
            }

            // ラベルを格納する配列の初期化
            labels = new Label[] {
                label1, label2, label3,
                label4, label5, label6
            };
        }

        // タグの設定を行う関数
        private void SetControlIndexTags(Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                    controls[i].Tag = i;
            }
        }

        // カードタイプのコンボボックスから選択した時の処理を行う関数
        private void TypeComboBox_SelectIndexChanged(object sender, EventArgs e)
        {
            // senderをComboBox型にキャストし、タグから何枚目のカードかインデックスを取得
            ComboBox typeComboBox = (ComboBox)sender;
            int cardIndex = (int)typeComboBox.Tag;

            // カードタイプのコンボボックスの選択された要素を取得
            cardsType[cardIndex] = (string)typeComboBox.SelectedItem;

            // カード名のコンボボックスに現在格納されている全要素を消去
            nameComboBoxes[cardIndex].Items.Clear();

            try
            {
                // 選択されたカードタイプのフォルダ内のすべての画像の名前をコンボボックスに追加
                foreach (string file in Directory.GetFiles(FOLDER_PATH + "\\" + cardsType[cardIndex], "*.png"))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    nameComboBoxes[cardIndex].Items.Add(fileNameWithoutExtension);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルが見つかりません: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"必要な画像ファイルが見つかりません: {cardsType[cardIndex]}", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {cardsType[cardIndex]}", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // カード名のコンボボックスから選択した時の処理を行う関数
        private void NameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // senderをComboBox型にキャストし、タグから何枚目のカードかインデックスを取得
            ComboBox nameComboBox = (ComboBox)sender;
            int cardIndex = (int)nameComboBox.Tag;

            // カード名のコンボボックスの選択された要素を取得
            string selectedItem = (string)nameComboBoxes[cardIndex].SelectedItem;

            try
            {
                // 選択されたカード名の画像を取得し、設定
                cardPictureBoxes[cardIndex].Image = Image.FromFile(FOLDER_PATH + cardsType[cardIndex] + "\\" + selectedItem + ".png");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルが見つかりません: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"必要な画像ファイルが見つかりません: {selectedItem}.png", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {selectedItem}.png", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // イベント進行の処理を行う関数
        private void ProgressButton_Click(object sender, EventArgs e)
        {
            // senderをButton型にキャストし、タグから何枚目のカードかインデックスを取得
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // イベントが最大まで進行した時に完走ボタンクリックの関数を呼び出す
            if (cardsProgress[cardIndex] == PROGRESS_STAGES - 1)
            {
                CompleteButton_Click(sender, e);
            }

            // イベント進行
            if (cardsProgress[cardIndex] < PROGRESS_STAGES)
            {
                cardsProgress[cardIndex]++;
            }
            else
            {
                return;
            }

            try
            {
                // イベント進行度に応じて進行状況を表すピクチャーボックスの画像を変更
                if (cardsProgress[cardIndex] > 0 && cardsProgress[cardIndex] <= PROGRESS_STAGES)
                {
                    progressPictureBoxes[PROGRESS_STAGES * cardIndex + cardsProgress[cardIndex] - 1].Image = Image.FromFile(PROGRESS_TRUE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルが見つかりません: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"必要な画像ファイルが見つかりません: {PROGRESS_TRUE_IMAGE}", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_TRUE_IMAGE}", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // イベント完走の処理を行う関数
        private void CompleteButton_Click(object sender, EventArgs e)
        {
            // senderをButton型にキャストし、タグから何枚目のカードかインデックスを取得
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // 完走ラベルを表示
            labels[cardIndex].Visible = true;
            labels[cardIndex].Parent = cardPictureBoxes[cardIndex];
            // 各ピクチャーボックス内の左上からXピクセル右、Yピクセル下
            labels[cardIndex].Location = new Point(COMPLETE_LABEL_OFFSET_X, COMPLETE_LABEL_OFFSET_Y);
        }

        // イベント進行リセットを行う関数
        private void ResetButton_Click(object sender, EventArgs e)
        {
            // senderをButton型にキャストし、タグから何枚目のカードかインデックスを取得
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // イベント進行度をデフォルトに設定
            cardsProgress[cardIndex] = DEFAULT_PROGRESS;

            try
            {
                // 進行状況を表すピクチャーボックスの画像を全て未進行に設定
                for (int i = 0; i < PROGRESS_STAGES; i++)
                {
                    progressPictureBoxes[PROGRESS_STAGES * cardIndex + i].Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"必要な画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE}", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE}", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 完走ラベルを非表示にする
            labels[cardIndex].Visible = false;
        }

        // イベント進行の全リセットを行う関数
        private void AllResetbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // 全てのカードにおいて進行状況を表すピクチャーボックスの画像を未進行に設定
                foreach (PictureBox pb in progressPictureBoxes)
                {
                    pb.Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"必要な画像ファイルが見つかりません: {PROGRESS_FALSE_IMAGE}", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"エラー: 画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"画像ファイルの読み込み中にエラーが発生しました: {PROGRESS_FALSE_IMAGE}", "I/O エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラー: 予期せぬエラーが発生しました: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"予期せぬエラーが発生しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // 全てのカードにおいて進行度をデフォルト値に設定
            for(int i = 0;i < cardsProgress.Length;i++)
            {
                cardsProgress[i] = DEFAULT_PROGRESS;
            }

            // 全てのカードにおいて完走ラベルを非表示にする
            foreach(Label label in labels)
            {
                label.Visible = false;
            }
        }

        // カード編成の登録・削除を行う関数
        private void RegisterDeleteButton_Click(object sender, EventArgs e)
        {
            // Form1を無効化
            this.Enabled = false;

            // string配列valuesにすべてのコンボボックスの値を代入
            string[] values = new string[NUM_CARDS * COMBO_BOXES_PER_CARD];
            for (int i = 0; i < NUM_CARDS; i++)
            {
                values[i * COMBO_BOXES_PER_CARD] = typeComboBoxes[i].Text;
                values[i * COMBO_BOXES_PER_CARD + 1] = nameComboBoxes[i].Text;
            }

            // 編成の登録・削除を行うForm2を作成し、表示
            Form2 newForm2 = new Form2(this, values);
            newForm2.StartPosition = FormStartPosition.Manual;
            // 親フォームの左上からXピクセル右、Yピクセル下
            newForm2.Location = new Point(this.Location.X + FORM2_OFFSET_X, this.Location.Y + FORM2_OFFSET_Y);
            newForm2.Show();
        }

        // カード編成の呼び出しを行う関数
        private void CallButton_Click(object sender, EventArgs e)
        {
            // Form1を無効化
            this.Enabled = false;

            // 編成の呼び出しを行うForm3を作成し、表示
            Form3 newForm3 = new Form3(this);
            newForm3.StartPosition = FormStartPosition.Manual;
            // 親フォームの左上からXピクセル右、Yピクセル下
            newForm3.Location = new Point(this.Location.X + FORM3_OFFSET_X, this.Location.Y + FORM3_OFFSET_Y);
            newForm3.Show();
        }
    }
}