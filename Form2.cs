using Newtonsoft.Json;
using static Support_Card_Manager.Form2;

namespace Support_Card_Manager
{
    public partial class Form2 : Form
    {
        // 編成情報を保存したファイル名
        private const string DATA_FILE_NAME = "data.json";

        // コンストラクタ内でform1, valuesを受け取るためのフィールド
        private Form1 form1 = null;
        private string[] values = null;

        // Form2のコンストラクタ
        public Form2(Form1 form1, string[] values)
        {
            InitializeComponent();
            this.form1 = form1;
            this.values = values;

            // JSONファイルから編成名を取得してComboBoxに追加
            LoadSetsFromJSON();

        }

        // 編成名と編成情報を格納するクラス
        public class ComboBoxData
        {
            public string Name { get; set; }
            public string[] SelectedItems { get; set; }
        }

        // 編成の登録を行う関数
        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // 既存のJSONを読み込む（存在しない場合は空のリスト）
            string json = "";

            // data.jsonが存在するかチェック
            if (File.Exists(DATA_FILE_NAME))
            {
                try
                {
                    // JSONを読み込む
                    json = File.ReadAllText(DATA_FILE_NAME);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("JSONファイルの読み込み中にエラーが発生しました: " + ex.Message);
                    return;
                }
            }

            // JSONをデシリアライズする
            RootObject data = JsonConvert.DeserializeObject<RootObject>(json) ?? new RootObject();

            // 編成名が入力されていない時の処理
            if(textBox1.Text.Length == 0)
            {
                MessageBox.Show("名前を入力してください。");
                return;
            }

            // 新しい編成を追加
            data.Sets.Add(new ComboBoxData { Name = textBox1.Text, SelectedItems = values });

            // JSONをシリアライズして保存
            json = JsonConvert.SerializeObject(data);
            try
            {
                // JSONに書き込む
                File.WriteAllText(DATA_FILE_NAME, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSONファイルの書き込み中にエラーが発生しました: " + ex.Message);
            }

            // Form2を閉じる
            this.Close();
        }

        // JSONから編成を読み込む関数
        private void LoadSetsFromJSON()
        {
            // 既存のJSONを読み込む（存在しない場合は空のリスト）
            string json = "";
            if (File.Exists(DATA_FILE_NAME))
            {
                try
                {
                    // JSONを読み込む
                    json = File.ReadAllText(DATA_FILE_NAME);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("JSONファイルの読み込み中にエラーが発生しました: " + ex.Message);
                    return;
                }
            }

            // JSONをデシリアライズする
            RootObject data = JsonConvert.DeserializeObject<RootObject>(json) ?? new RootObject();

            // コンボボックスに読み込んだ編成を追加する
            foreach (var set in data.Sets)
            {
                comboBox1.Items.Add(set.Name);
            }
        }

        // 編成の削除を行う関数
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // 既存のJSONを読み込む（存在しない場合は空のリスト）
            string json = "";
            if (File.Exists(DATA_FILE_NAME))
            {
                try
                {
                    // JSONを読み込む
                    json = File.ReadAllText(DATA_FILE_NAME);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("JSONファイルの読み込み中にエラーが発生しました: " + ex.Message);
                    return;
                }
            }

            // JSONをデシリアライズする
            RootObject data = JsonConvert.DeserializeObject<RootObject>(json) ?? new RootObject();
            string selectedSetName = comboBox1.SelectedItem?.ToString();

            // 編成を選択していない時の処理
            if (string.IsNullOrEmpty(selectedSetName))
            {
                MessageBox.Show("削除する編成を選択してください。");
                return;
            }

            // 選択された編成を削除
            data.Sets.RemoveAll(set => set.Name == selectedSetName);

            // JSONをシリアライズして保存
            json = JsonConvert.SerializeObject(data);
            try
            {
                // JSONに書き込む
                File.WriteAllText(DATA_FILE_NAME, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("JSONファイルの書き込み中にエラーが発生しました: " + ex.Message);
            }
            // Form2を閉じる
            this.Close();
        }

        // フォーム2を閉じた時の処理を行う関数
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form2を閉じた時にForm1を有効化
            form1.Enabled = true;
        }
    }

    // JSONデータのルートオブジェクトを表すクラス
    public class RootObject
    {
        // 編成のリスト
        public List<ComboBoxData> Sets { get; set; } = new List<ComboBoxData>();
    }
}
