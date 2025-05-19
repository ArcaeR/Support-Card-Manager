using System.CodeDom;
using Newtonsoft.Json.Linq;

namespace Support_Card_Manager
{
    public partial class Form1 : Form
    {
        // �S��ނ̃J�[�h�^�C�v
        public readonly string[] cardType = {
            "�^�C�v1", "�^�C�v2", "�^�C�v3",
            "�^�C�v4", "�^�C�v5", "�^�C�v6", "�^�C�v7"
        }; 

        // ���I�����̃J�[�h�^�C�v
        public const string NONE_TYPE = "���I��";

        // ���i�s�̉摜
        private const string PROGRESS_FALSE_IMAGE = "images\\ProgressImages\\eventprogress_0.png";

        // �i�s�ς݂̉摜
        private const string PROGRESS_TRUE_IMAGE = "images\\ProgressImages\\eventprogress_1.png";

        // ���I�����̉摜
        public const string NONE_IMAGE = "images\\CardImages\\���I��\\none.png";

        // �J�[�h�摜�̃t�H���_�p�X
        public const string FOLDER_PATH = "images\\CardImages\\";

        // �Ґ��J�[�h����
        public const int NUM_CARDS = 6;

        // �J�[�h1��������̃R���{�{�b�N�X��
        public const int COMBO_BOXES_PER_CARD = 2;

        // �C�x���g�i�s�x�̃f�t�H���g�l
        private const int DEFAULT_PROGRESS = 0;

        // �C�x���g�i�s�x�̍ő�l
        private const int PROGRESS_STAGES = 3;

        // �������x����X���W�I�t�Z�b�g
        private const int COMPLETE_LABEL_OFFSET_X = 55;

        // �������x����Y���W�I�t�Z�b�g
        private const int COMPLETE_LABEL_OFFSET_Y = 100;

        // Form2�̕\���ʒu�I�t�Z�b�gX
        private const int FORM2_OFFSET_X = 100;

        // Form2�̕\���ʒu�I�t�Z�b�gY
        private const int FORM2_OFFSET_Y = 100;

        // Form3�̕\���ʒu�I�t�Z�b�gX
        private const int FORM3_OFFSET_X = 100;

        // Form3�̕\���ʒu�I�t�Z�b�gY
        private const int FORM3_OFFSET_Y = 200;

        // �e�J�[�h�̃J�[�h�^�C�v���i�[����z��
        private readonly string[] cardsType;

        // �e�J�[�h�̃J�[�h�^�C�v���i�[����R���{�{�b�N�X�̔z��
        public ComboBox[] typeComboBoxes;

        // �e�J�[�h�̃J�[�h�����i�[����R���{�{�b�N�X�̔z��
        public ComboBox[] nameComboBoxes;

        // �e�J�[�h�̐i�s�󋵂�\���摜���i�[����s�N�`���[�{�b�N�X�̔z��
        public PictureBox[] progressPictureBoxes;

        // �e�J�[�h�̑I�������J�[�h�摜���i�[����s�N�`���[�{�b�N�X�̔z��
        public PictureBox[] cardPictureBoxes;

        // �e�J�[�h�̐i�s�󋵂��i�[����z��
        private int[] cardsProgress;

        // �e�J�[�h�̃C�x���g�i�s�{�^�����i�[����z��
        private Button[] progressButtons;

        // �e�J�[�h�̊����{�^�����i�[����z��
        private Button[] completeButtons;

        // �e�J�[�h�̊����{�^�����i�[����z��
        private Button[] resetButtons;

        // �e�J�[�h�̊������x�����i�[����z��
        private Label[] labels;

        // Form1�̃R���X�g���N�^
        public Form1()
        {
            InitializeComponent();

            // �e�J�[�h�̃J�[�h�^�C�v���i�[����z��̏�����
            cardsType = new string[NUM_CARDS];
            for(int i = 0;i < NUM_CARDS; i++)
            {
                cardsType[i] = NONE_TYPE;
            }

            // �e�J�[�h�̃J�[�h�^�C�v���i�[����R���{�{�b�N�X�̔z��̏�����
            typeComboBoxes = new ComboBox[NUM_CARDS]
            {
                comboBox1, comboBox3, comboBox5, 
                comboBox7,comboBox9, comboBox11
            };

            // �e�R���{�{�b�N�X�ɃJ�[�h�^�C�v(���I�����܂�)���i�[
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

            // �e�J�[�h�̃J�[�h�^�C�v�̃R���{�{�b�N�X�̃^�O��ݒ�
            // �J�[�h1���ڂ�0�A2���ڂ�1�E�E�E�̂悤�Ƀ^�O��ݒ�
            // �ȍ~�̃^�O�ݒ�ł����l�̏������s��
            SetControlIndexTags(typeComboBoxes);

            // �e�J�[�h�̃J�[�h�����i�[����R���{�{�b�N�X�̔z��̏�����
            nameComboBoxes = new ComboBox[NUM_CARDS]
            {
                comboBox2, comboBox4, comboBox6,
                comboBox8,comboBox10, comboBox12
            };

            // �e�R���{�{�b�N�X�ɖ��I���摜�̉摜�����i�[�i�������j
            foreach (ComboBox cb in nameComboBoxes)
            {
                string noneImageName = Path.GetFileNameWithoutExtension(NONE_IMAGE);
                cb.Items.Add(noneImageName);
            }

            // �e�J�[�h�̃J�[�h���̃R���{�{�b�N�X�̃^�O��ݒ�
            SetControlIndexTags(nameComboBoxes);

            // �e�J�[�h�̃C�x���g�i�s�{�^�����i�[����z��̏�����
            progressButtons = new Button[NUM_CARDS] {
                button1, button4, button7,
                button10, button13, button16
            };

            // �e�J�[�h�̃C�x���g�i�s�{�^���̃^�O��ݒ�
            SetControlIndexTags(progressButtons);

            // �e�J�[�h�̊����{�^�����i�[����z��̏�����
            completeButtons = new Button[] {
                button2, button5, button8,
                button11, button14, button17
            };

            // �e�J�[�h�̊����{�^���̃^�O��ݒ�
            SetControlIndexTags(completeButtons);

            // �e�J�[�h�̃C�x���g���Z�b�g�{�^�����i�[����z��̏�����
            resetButtons = new Button[] {
                button3, button6, button9,
                button12, button15, button18
            };

            // �e�J�[�h�̃C�x���g���Z�b�g�{�^���̃^�O��ݒ�
            SetControlIndexTags(resetButtons);

            // �e�J�[�h�̐i�s�󋵂�\���摜���i�[����s�N�`���[�{�b�N�X�̔z��̏�����
            progressPictureBoxes = new PictureBox[] {
                pictureBox2, pictureBox3, pictureBox4,
                pictureBox6, pictureBox7, pictureBox8,
                pictureBox10, pictureBox11, pictureBox12,
                pictureBox14, pictureBox15, pictureBox16,
                pictureBox18, pictureBox19, pictureBox20,
                pictureBox22, pictureBox23, pictureBox24
            };

            // �e�J�[�h�̐i�s�󋵂�\���摜���i�[����s�N�`���[�{�b�N�X�𖢐i�s�ŏ�����
            foreach(PictureBox pb in progressPictureBoxes)
            {
                try
                {
                    pb.Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE}", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE}", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                    MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // �J�[�h�摜���i�[����R���{�{�b�N�X�̔z��̏�����
            cardPictureBoxes = new PictureBox[] {
                pictureBox1, pictureBox5, pictureBox9,
                pictureBox13, pictureBox17,pictureBox21
            
            };

            // �e�J�[�h�̐i�s�󋵂��i�[����z��̏�����
            cardsProgress = new int[NUM_CARDS];
            for( int i = 0; i < NUM_CARDS; i++ )
            {
                cardsProgress[i] = DEFAULT_PROGRESS;
            }

            // ���x�����i�[����z��̏�����
            labels = new Label[] {
                label1, label2, label3,
                label4, label5, label6
            };
        }

        // �^�O�̐ݒ���s���֐�
        private void SetControlIndexTags(Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                    controls[i].Tag = i;
            }
        }

        // �J�[�h�^�C�v�̃R���{�{�b�N�X����I���������̏������s���֐�
        private void TypeComboBox_SelectIndexChanged(object sender, EventArgs e)
        {
            // sender��ComboBox�^�ɃL���X�g���A�^�O���牽���ڂ̃J�[�h���C���f�b�N�X���擾
            ComboBox typeComboBox = (ComboBox)sender;
            int cardIndex = (int)typeComboBox.Tag;

            // �J�[�h�^�C�v�̃R���{�{�b�N�X�̑I�����ꂽ�v�f���擾
            cardsType[cardIndex] = (string)typeComboBox.SelectedItem;

            // �J�[�h���̃R���{�{�b�N�X�Ɍ��݊i�[����Ă���S�v�f������
            nameComboBoxes[cardIndex].Items.Clear();

            try
            {
                // �I�����ꂽ�J�[�h�^�C�v�̃t�H���_���̂��ׂẲ摜�̖��O���R���{�{�b�N�X�ɒǉ�
                foreach (string file in Directory.GetFiles(FOLDER_PATH + "\\" + cardsType[cardIndex], "*.png"))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    nameComboBoxes[cardIndex].Items.Add(fileNameWithoutExtension);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {cardsType[cardIndex]}", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {cardsType[cardIndex]}", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {FOLDER_PATH}\\{cardsType[cardIndex]} - {ex.Message}");
                MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // �J�[�h���̃R���{�{�b�N�X����I���������̏������s���֐�
        private void NameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // sender��ComboBox�^�ɃL���X�g���A�^�O���牽���ڂ̃J�[�h���C���f�b�N�X���擾
            ComboBox nameComboBox = (ComboBox)sender;
            int cardIndex = (int)nameComboBox.Tag;

            // �J�[�h���̃R���{�{�b�N�X�̑I�����ꂽ�v�f���擾
            string selectedItem = (string)nameComboBoxes[cardIndex].SelectedItem;

            try
            {
                // �I�����ꂽ�J�[�h���̉摜���擾���A�ݒ�
                cardPictureBoxes[cardIndex].Image = Image.FromFile(FOLDER_PATH + cardsType[cardIndex] + "\\" + selectedItem + ".png");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {selectedItem}.png", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {selectedItem}.png", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {FOLDER_PATH}{cardsType[cardIndex]}\\{selectedItem}.png - {ex.Message}");
                MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // �C�x���g�i�s�̏������s���֐�
        private void ProgressButton_Click(object sender, EventArgs e)
        {
            // sender��Button�^�ɃL���X�g���A�^�O���牽���ڂ̃J�[�h���C���f�b�N�X���擾
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // �C�x���g���ő�܂Ői�s�������Ɋ����{�^���N���b�N�̊֐����Ăяo��
            if (cardsProgress[cardIndex] == PROGRESS_STAGES - 1)
            {
                CompleteButton_Click(sender, e);
            }

            // �C�x���g�i�s
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
                // �C�x���g�i�s�x�ɉ����Đi�s�󋵂�\���s�N�`���[�{�b�N�X�̉摜��ύX
                if (cardsProgress[cardIndex] > 0 && cardsProgress[cardIndex] <= PROGRESS_STAGES)
                {
                    progressPictureBoxes[PROGRESS_STAGES * cardIndex + cardsProgress[cardIndex] - 1].Image = Image.FromFile(PROGRESS_TRUE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {PROGRESS_TRUE_IMAGE}", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_TRUE_IMAGE}", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {PROGRESS_TRUE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // �C�x���g�����̏������s���֐�
        private void CompleteButton_Click(object sender, EventArgs e)
        {
            // sender��Button�^�ɃL���X�g���A�^�O���牽���ڂ̃J�[�h���C���f�b�N�X���擾
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // �������x����\��
            labels[cardIndex].Visible = true;
            labels[cardIndex].Parent = cardPictureBoxes[cardIndex];
            // �e�s�N�`���[�{�b�N�X���̍��ォ��X�s�N�Z���E�AY�s�N�Z����
            labels[cardIndex].Location = new Point(COMPLETE_LABEL_OFFSET_X, COMPLETE_LABEL_OFFSET_Y);
        }

        // �C�x���g�i�s���Z�b�g���s���֐�
        private void ResetButton_Click(object sender, EventArgs e)
        {
            // sender��Button�^�ɃL���X�g���A�^�O���牽���ڂ̃J�[�h���C���f�b�N�X���擾
            Button button = (Button)sender;
            int cardIndex = (int)button.Tag;

            // �C�x���g�i�s�x���f�t�H���g�ɐݒ�
            cardsProgress[cardIndex] = DEFAULT_PROGRESS;

            try
            {
                // �i�s�󋵂�\���s�N�`���[�{�b�N�X�̉摜��S�Ė��i�s�ɐݒ�
                for (int i = 0; i < PROGRESS_STAGES; i++)
                {
                    progressPictureBoxes[PROGRESS_STAGES * cardIndex + i].Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE}", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE}", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // �������x�����\���ɂ���
            labels[cardIndex].Visible = false;
        }

        // �C�x���g�i�s�̑S���Z�b�g���s���֐�
        private void AllResetbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // �S�ẴJ�[�h�ɂ����Đi�s�󋵂�\���s�N�`���[�{�b�N�X�̉摜�𖢐i�s�ɐݒ�
                foreach (PictureBox pb in progressPictureBoxes)
                {
                    pb.Image = Image.FromFile(PROGRESS_FALSE_IMAGE);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�K�v�ȉ摜�t�@�C����������܂���: {PROGRESS_FALSE_IMAGE}", "�t�@�C���G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"�G���[: �摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�摜�t�@�C���̓ǂݍ��ݒ��ɃG���[���������܂���: {PROGRESS_FALSE_IMAGE}", "I/O �G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"�G���[: �\�����ʃG���[���������܂���: {PROGRESS_FALSE_IMAGE} - {ex.Message}");
                MessageBox.Show($"�\�����ʃG���[���������܂���", "�G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // �S�ẴJ�[�h�ɂ����Đi�s�x���f�t�H���g�l�ɐݒ�
            for(int i = 0;i < cardsProgress.Length;i++)
            {
                cardsProgress[i] = DEFAULT_PROGRESS;
            }

            // �S�ẴJ�[�h�ɂ����Ċ������x�����\���ɂ���
            foreach(Label label in labels)
            {
                label.Visible = false;
            }
        }

        // �J�[�h�Ґ��̓o�^�E�폜���s���֐�
        private void RegisterDeleteButton_Click(object sender, EventArgs e)
        {
            // Form1�𖳌���
            this.Enabled = false;

            // string�z��values�ɂ��ׂẴR���{�{�b�N�X�̒l����
            string[] values = new string[NUM_CARDS * COMBO_BOXES_PER_CARD];
            for (int i = 0; i < NUM_CARDS; i++)
            {
                values[i * COMBO_BOXES_PER_CARD] = typeComboBoxes[i].Text;
                values[i * COMBO_BOXES_PER_CARD + 1] = nameComboBoxes[i].Text;
            }

            // �Ґ��̓o�^�E�폜���s��Form2���쐬���A�\��
            Form2 newForm2 = new Form2(this, values);
            newForm2.StartPosition = FormStartPosition.Manual;
            // �e�t�H�[���̍��ォ��X�s�N�Z���E�AY�s�N�Z����
            newForm2.Location = new Point(this.Location.X + FORM2_OFFSET_X, this.Location.Y + FORM2_OFFSET_Y);
            newForm2.Show();
        }

        // �J�[�h�Ґ��̌Ăяo�����s���֐�
        private void CallButton_Click(object sender, EventArgs e)
        {
            // Form1�𖳌���
            this.Enabled = false;

            // �Ґ��̌Ăяo�����s��Form3���쐬���A�\��
            Form3 newForm3 = new Form3(this);
            newForm3.StartPosition = FormStartPosition.Manual;
            // �e�t�H�[���̍��ォ��X�s�N�Z���E�AY�s�N�Z����
            newForm3.Location = new Point(this.Location.X + FORM3_OFFSET_X, this.Location.Y + FORM3_OFFSET_Y);
            newForm3.Show();
        }
    }
}