using Newtonsoft.Json;
using static Support_Card_Manager.Form2;

namespace Support_Card_Manager
{
    public partial class Form3 : Form
    {
        // 編成情報を保存したファイル名
        private const string DATA_FILE_NAME = "data.json";

        // // コンストラクタ内でform1を受け取るためのフィールド
        Form1 form1 = null;

        // Form3のコンストラクタ
        public Form3(Form1 owner)
        {
            try
            {
                // JSONファイルが存在しない場合は作成する
                if (!File.Exists(DATA_FILE_NAME))
                {
                    CreateDefaultJsonFile();
                }

                InitializeComponent();
                form1 = owner;
                this.Owner = owner;

                // JSONを読み込む
                string json = File.ReadAllText(DATA_FILE_NAME);

                // JSONをデシリアライズしてRootObjectオブジェクトを取得
                RootObject data = JsonConvert.DeserializeObject<RootObject>(json);

                // ComboBoxに編成を全て追加
                foreach (var item in data.Sets)
                {
                    comboBox1.Items.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSONファイルの読み込み中にエラーが発生しました: " + ex.Message);
                this.Close();
                return;
            }
        }

        // data.jsonが存在しない場合に初期データを作成する関数
        private void CreateDefaultJsonFile()
        {
            // 空のSetsを作成
            RootObject defaultData = new RootObject
            {
                Sets = new List<ComboBoxData>()
            };

            string json = JsonConvert.SerializeObject(defaultData);
            try
            {
                // JSONを作成し、空のデータを書き込む
                File.WriteAllText(DATA_FILE_NAME, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("初期JSONファイルの作成中にエラーが発生しました: " + ex.Message);
            }
        }

        // 編成を反映する関数
        private void ReflectMysetButton_Click(object sender, EventArgs e)
        {
            // 選択された編成名を取得
            string selectedName = comboBox1.SelectedItem?.ToString();

            // 編成を選択していない時の処理
            if (string.IsNullOrEmpty(selectedName))
            {
                MessageBox.Show("編成が登録されていない、もしくは選択されていません。");
                return;
            }

            // JSONを読み込む
            string json = File.ReadAllText(DATA_FILE_NAME);

            // JSONをデシリアライズ
            RootObject data = JsonConvert.DeserializeObject<RootObject>(json);

            // 選択された編成名と一致する編成を探す
            ComboBoxData foundData = data.Sets.FirstOrDefault(x => x.Name == selectedName);

            if (foundData == null)
            {
                // 一致する編成が見つからなかった場合
                MessageBox.Show("指定された編成が見つかりません。");
                return;
            }

            // 見つかった編成の各カードタイプ、カード名をForm1のコンボボックスに設定
            for (int i = 0; i < Form1.NUM_CARDS; i++)
            {
                // カードタイプの反映
                if (i * Form1.COMBO_BOXES_PER_CARD < foundData.SelectedItems.Length)
                {
                    // 選択されたカードタイプをForm1のコンボボックスから検索
                    int typeIndex = form1.typeComboBoxes[i].FindStringExact(foundData.SelectedItems[i * Form1.COMBO_BOXES_PER_CARD]);
              
                    if (typeIndex >= 0)
                    {
                        // カードタイプを反映
                        form1.typeComboBoxes[i].SelectedIndex = typeIndex;
                    }
                    else
                    {
                        // 未選択時はデフォルトに戻す
                        form1.typeComboBoxes[i].Text = Form1.NONE_TYPE;
                    }
                }

                // カード名の反映
                if (i * Form1.COMBO_BOXES_PER_CARD + 1 < foundData.SelectedItems.Length)
                {
                    // カード名の反映
                    form1.nameComboBoxes[i].Items.Clear();

                    // 画像フォルダーの存在チェック
                    if (Directory.Exists(Form1.FOLDER_PATH + form1.cardType[form1.typeComboBoxes[i].SelectedIndex]))
                    {
                        // 選択されたカードタイプのフォルダ内のすべての画像の名前をコンボボックスに追加
                        foreach (string file in Directory.GetFiles(Form1.FOLDER_PATH + "\\" + form1.cardType[form1.typeComboBoxes[i].SelectedIndex], "*.png"))
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                            form1.nameComboBoxes[i].Items.Add(fileNameWithoutExtension);
                        }

                        // 選択されたカード名をForm1のコンボボックスから検索
                        int nameIndex = form1.nameComboBoxes[i].FindStringExact(foundData.SelectedItems[i * Form1.COMBO_BOXES_PER_CARD + 1]);       

                        if (nameIndex >= 0)
                        {
                            // カード名を反映、カード名の画像を表示
                            form1.nameComboBoxes[i].SelectedIndex = nameIndex;
                            form1.cardPictureBoxes[i].Image = Image.FromFile(Form1.FOLDER_PATH + form1.cardType[form1.typeComboBoxes[i].SelectedIndex] + "\\" + form1.nameComboBoxes[i].SelectedItem + ".png");
                        }
                        else
                        {
                            // 未選択時はデフォルト画像を表示
                            form1.nameComboBoxes[i].SelectedIndex = -1;
                            form1.cardPictureBoxes[i].Image = Image.FromFile(Form1.NONE_IMAGE);
                        }
                    }
                    else
                    {
                        // フォルダーが見つからない時はデフォルト画像を表示
                        form1.nameComboBoxes[i].SelectedIndex = -1;
                        form1.cardPictureBoxes[i].Image = Image.FromFile(Form1.NONE_IMAGE);
                        
                    }
                }
            }
            // Form3を閉じる
            this.Close();
        }

        // フォーム3を閉じた時の処理を行う関数
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form3を閉じた時にForm1を有効化
            form1.Enabled = true;
        }
    }
}