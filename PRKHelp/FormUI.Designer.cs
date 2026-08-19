using Microsoft.Data.Sqlite;

namespace PRKHelp
{
    partial class FormUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUI));
            combatPathButton = new Button();
            folderPathText = new Label();
            confirmBtn = new Button();
            chatLogText = new RichTextBox();
            chatFilePathText = new Label();
            scriptsFolderButton = new Button();
            scriptsFolderText = new RichTextBox();
            combatLogText = new RichTextBox();
            combatPathText = new Label();
            chatLogButton = new Button();
            characterSelect = new ComboBox();
            addCharacterButton = new Button();
            removeCharacterButton = new Button();
            settingsBox = new Panel();
            fileSelectionContainer = new Panel();
            combatLogControlsContainer = new Panel();
            combatLogTextContainer = new Panel();
            padding3 = new Panel();
            chatLogControlsContainer = new Panel();
            chatFileTextContainer = new Panel();
            padding2 = new Panel();
            scriptsFolderControlsContainer = new Panel();
            scriptsFolderTextContainer = new Panel();
            padding1 = new Panel();
            characterSelectionContainer = new Panel();
            removeButtonPadding = new Panel();
            addButtonPadding = new Panel();
            characterSelectPadding = new Panel();
            headerPadding = new Panel();
            titleBar = new Panel();
            minimizeButton = new Button();
            closeButton = new Button();
            label1 = new Label();
            activeWindow = new Panel();
            exitButton = new Button();
            panel2 = new Panel();
            xpContainer = new Panel();
            xpAmount = new RichTextBox();
            panel1 = new Panel();
            richTextBox5 = new RichTextBox();
            xpToggle = new Button();
            panel3 = new Panel();
            xpReset = new Button();
            padding6 = new Panel();
            damageContainer = new Panel();
            damageAmount = new RichTextBox();
            dmgPadding3 = new Panel();
            richTextBox4 = new RichTextBox();
            damageToggle = new Button();
            dmgPadding2 = new Panel();
            damageReset = new Button();
            padding5 = new Panel();
            characterReadoutContainer = new Panel();
            petNameContainer = new Panel();
            petNameInput = new TextBox();
            petNameLabel = new RichTextBox();
            activeCharacter = new RichTextBox();
            selectCharacterButtonPadding = new Panel();
            selectCharacterButton = new Button();
            padding4 = new Panel();
            dmgTitleBar = new Panel();
            button5 = new Button();
            settingsBox.SuspendLayout();
            fileSelectionContainer.SuspendLayout();
            combatLogControlsContainer.SuspendLayout();
            combatLogTextContainer.SuspendLayout();
            chatLogControlsContainer.SuspendLayout();
            chatFileTextContainer.SuspendLayout();
            scriptsFolderControlsContainer.SuspendLayout();
            scriptsFolderTextContainer.SuspendLayout();
            characterSelectionContainer.SuspendLayout();
            removeButtonPadding.SuspendLayout();
            addButtonPadding.SuspendLayout();
            characterSelectPadding.SuspendLayout();
            titleBar.SuspendLayout();
            activeWindow.SuspendLayout();
            xpContainer.SuspendLayout();
            damageContainer.SuspendLayout();
            characterReadoutContainer.SuspendLayout();
            petNameContainer.SuspendLayout();
            selectCharacterButtonPadding.SuspendLayout();
            dmgTitleBar.SuspendLayout();
            SuspendLayout();
            // 
            // combatPathButton
            // 
            combatPathButton.BackColor = Color.DimGray;
            combatPathButton.Dock = DockStyle.Right;
            combatPathButton.FlatStyle = FlatStyle.Popup;
            combatPathButton.ForeColor = SystemColors.ButtonHighlight;
            combatPathButton.Location = new Point(375, 0);
            combatPathButton.Name = "combatPathButton";
            combatPathButton.Size = new Size(90, 35);
            combatPathButton.TabIndex = 1;
            combatPathButton.Text = "Select File";
            combatPathButton.UseVisualStyleBackColor = false;
            combatPathButton.Click += GetLogFile;
            // 
            // folderPathText
            // 
            folderPathText.AutoSize = true;
            folderPathText.Font = new Font("Segoe UI", 6F);
            folderPathText.ForeColor = SystemColors.ButtonFace;
            folderPathText.Location = new Point(11, 25);
            folderPathText.Name = "folderPathText";
            folderPathText.Size = new Size(38, 11);
            folderPathText.TabIndex = 2;
            folderPathText.Text = "textLabel";
            // 
            // confirmBtn
            // 
            confirmBtn.BackColor = Color.DimGray;
            confirmBtn.Dock = DockStyle.Bottom;
            confirmBtn.FlatStyle = FlatStyle.Popup;
            confirmBtn.ForeColor = SystemColors.ButtonHighlight;
            confirmBtn.Location = new Point(0, 346);
            confirmBtn.Name = "confirmBtn";
            confirmBtn.Size = new Size(475, 34);
            confirmBtn.TabIndex = 3;
            confirmBtn.Text = "Start";
            confirmBtn.UseVisualStyleBackColor = false;
            confirmBtn.Click += Start;
            // 
            // chatLogText
            // 
            chatLogText.BackColor = Color.FromArgb(44, 38, 66);
            chatLogText.BorderStyle = BorderStyle.None;
            chatLogText.Font = new Font("Segoe UI", 11F);
            chatLogText.ForeColor = SystemColors.HighlightText;
            chatLogText.Location = new Point(5, 3);
            chatLogText.Name = "chatLogText";
            chatLogText.ReadOnly = true;
            chatLogText.Size = new Size(204, 26);
            chatLogText.TabIndex = 4;
            chatLogText.Text = "Select vicinity chat log file";
            // 
            // chatFilePathText
            // 
            chatFilePathText.AutoSize = true;
            chatFilePathText.Font = new Font("Segoe UI", 6F);
            chatFilePathText.ForeColor = SystemColors.ButtonFace;
            chatFilePathText.Location = new Point(11, 24);
            chatFilePathText.Name = "chatFilePathText";
            chatFilePathText.Size = new Size(26, 11);
            chatFilePathText.TabIndex = 5;
            chatFilePathText.Text = "label1";
            // 
            // scriptsFolderButton
            // 
            scriptsFolderButton.BackColor = Color.DimGray;
            scriptsFolderButton.Dock = DockStyle.Right;
            scriptsFolderButton.FlatStyle = FlatStyle.Popup;
            scriptsFolderButton.ForeColor = SystemColors.ButtonHighlight;
            scriptsFolderButton.Location = new Point(375, 0);
            scriptsFolderButton.Name = "scriptsFolderButton";
            scriptsFolderButton.Size = new Size(90, 35);
            scriptsFolderButton.TabIndex = 6;
            scriptsFolderButton.Text = "Select Folder";
            scriptsFolderButton.UseVisualStyleBackColor = false;
            scriptsFolderButton.Click += GetScriptsPath;
            // 
            // scriptsFolderText
            // 
            scriptsFolderText.BackColor = Color.FromArgb(44, 38, 66);
            scriptsFolderText.BorderStyle = BorderStyle.None;
            scriptsFolderText.Font = new Font("Segoe UI", 11F);
            scriptsFolderText.ForeColor = SystemColors.HighlightText;
            scriptsFolderText.Location = new Point(5, 6);
            scriptsFolderText.Name = "scriptsFolderText";
            scriptsFolderText.ReadOnly = true;
            scriptsFolderText.Size = new Size(187, 22);
            scriptsFolderText.TabIndex = 7;
            scriptsFolderText.Text = "Select your scripts folder";
            // 
            // combatLogText
            // 
            combatLogText.BackColor = Color.FromArgb(44, 38, 66);
            combatLogText.BorderStyle = BorderStyle.None;
            combatLogText.Font = new Font("Segoe UI", 11F);
            combatLogText.ForeColor = SystemColors.HighlightText;
            combatLogText.Location = new Point(5, 3);
            combatLogText.Name = "combatLogText";
            combatLogText.ReadOnly = true;
            combatLogText.Size = new Size(280, 26);
            combatLogText.TabIndex = 9;
            combatLogText.Text = "Select combat log file (Optional)";
            // 
            // combatPathText
            // 
            combatPathText.AutoSize = true;
            combatPathText.Font = new Font("Segoe UI", 6F);
            combatPathText.ForeColor = SystemColors.ButtonFace;
            combatPathText.Location = new Point(11, 24);
            combatPathText.Name = "combatPathText";
            combatPathText.Size = new Size(49, 11);
            combatPathText.TabIndex = 10;
            combatPathText.Text = "combatPath";
            // 
            // chatLogButton
            // 
            chatLogButton.BackColor = Color.DimGray;
            chatLogButton.Dock = DockStyle.Right;
            chatLogButton.FlatStyle = FlatStyle.Popup;
            chatLogButton.ForeColor = SystemColors.ButtonHighlight;
            chatLogButton.Location = new Point(375, 0);
            chatLogButton.Name = "chatLogButton";
            chatLogButton.Size = new Size(90, 35);
            chatLogButton.TabIndex = 11;
            chatLogButton.Text = "Select File";
            chatLogButton.UseVisualStyleBackColor = false;
            chatLogButton.Click += GetCombatLogFile;
            // 
            // characterSelect
            // 
            characterSelect.BackColor = Color.DimGray;
            characterSelect.Dock = DockStyle.Left;
            characterSelect.FlatStyle = FlatStyle.Popup;
            characterSelect.ForeColor = SystemColors.HighlightText;
            characterSelect.FormattingEnabled = true;
            characterSelect.Location = new Point(0, 3);
            characterSelect.Name = "characterSelect";
            characterSelect.Size = new Size(181, 23);
            characterSelect.TabIndex = 12;
            characterSelect.Text = "Type to add new..";
            characterSelect.SelectedIndexChanged += CharacterSelected;
            // 
            // addCharacterButton
            // 
            addCharacterButton.BackColor = Color.DimGray;
            addCharacterButton.Dock = DockStyle.Fill;
            addCharacterButton.FlatStyle = FlatStyle.Popup;
            addCharacterButton.ForeColor = SystemColors.ButtonHighlight;
            addCharacterButton.Location = new Point(10, 0);
            addCharacterButton.Name = "addCharacterButton";
            addCharacterButton.Size = new Size(78, 30);
            addCharacterButton.TabIndex = 13;
            addCharacterButton.Text = "Add Char";
            addCharacterButton.UseVisualStyleBackColor = false;
            addCharacterButton.Click += AddNewCharacter;
            // 
            // removeCharacterButton
            // 
            removeCharacterButton.BackColor = Color.DimGray;
            removeCharacterButton.Dock = DockStyle.Fill;
            removeCharacterButton.FlatStyle = FlatStyle.Popup;
            removeCharacterButton.ForeColor = SystemColors.ButtonHighlight;
            removeCharacterButton.Location = new Point(10, 0);
            removeCharacterButton.Name = "removeCharacterButton";
            removeCharacterButton.Size = new Size(64, 30);
            removeCharacterButton.TabIndex = 14;
            removeCharacterButton.Text = "Remove";
            removeCharacterButton.UseVisualStyleBackColor = false;
            removeCharacterButton.Click += RemoveCharacter;
            // 
            // settingsBox
            // 
            settingsBox.BackColor = Color.FromArgb(44, 38, 66);
            settingsBox.Controls.Add(confirmBtn);
            settingsBox.Controls.Add(fileSelectionContainer);
            settingsBox.Controls.Add(titleBar);
            settingsBox.Dock = DockStyle.Fill;
            settingsBox.Location = new Point(0, 0);
            settingsBox.Name = "settingsBox";
            settingsBox.Size = new Size(475, 380);
            settingsBox.TabIndex = 15;
            // 
            // fileSelectionContainer
            // 
            fileSelectionContainer.BackColor = Color.Transparent;
            fileSelectionContainer.Controls.Add(combatLogControlsContainer);
            fileSelectionContainer.Controls.Add(padding3);
            fileSelectionContainer.Controls.Add(chatLogControlsContainer);
            fileSelectionContainer.Controls.Add(padding2);
            fileSelectionContainer.Controls.Add(scriptsFolderControlsContainer);
            fileSelectionContainer.Controls.Add(padding1);
            fileSelectionContainer.Controls.Add(characterSelectionContainer);
            fileSelectionContainer.Controls.Add(headerPadding);
            fileSelectionContainer.Dock = DockStyle.Fill;
            fileSelectionContainer.Location = new Point(0, 30);
            fileSelectionContainer.Name = "fileSelectionContainer";
            fileSelectionContainer.Size = new Size(475, 350);
            fileSelectionContainer.TabIndex = 15;
            // 
            // combatLogControlsContainer
            // 
            combatLogControlsContainer.BackColor = Color.Transparent;
            combatLogControlsContainer.Controls.Add(combatLogTextContainer);
            combatLogControlsContainer.Controls.Add(chatLogButton);
            combatLogControlsContainer.Dock = DockStyle.Top;
            combatLogControlsContainer.Location = new Point(0, 202);
            combatLogControlsContainer.Name = "combatLogControlsContainer";
            combatLogControlsContainer.Padding = new Padding(10, 0, 10, 0);
            combatLogControlsContainer.Size = new Size(475, 35);
            combatLogControlsContainer.TabIndex = 17;
            // 
            // combatLogTextContainer
            // 
            combatLogTextContainer.Controls.Add(combatPathText);
            combatLogTextContainer.Controls.Add(combatLogText);
            combatLogTextContainer.Dock = DockStyle.Left;
            combatLogTextContainer.Location = new Point(10, 0);
            combatLogTextContainer.Name = "combatLogTextContainer";
            combatLogTextContainer.Size = new Size(335, 35);
            combatLogTextContainer.TabIndex = 0;
            // 
            // padding3
            // 
            padding3.BackColor = Color.Transparent;
            padding3.Dock = DockStyle.Top;
            padding3.Location = new Point(0, 179);
            padding3.Name = "padding3";
            padding3.Size = new Size(475, 23);
            padding3.TabIndex = 21;
            // 
            // chatLogControlsContainer
            // 
            chatLogControlsContainer.BackColor = Color.Transparent;
            chatLogControlsContainer.Controls.Add(chatFileTextContainer);
            chatLogControlsContainer.Controls.Add(combatPathButton);
            chatLogControlsContainer.Dock = DockStyle.Top;
            chatLogControlsContainer.Location = new Point(0, 144);
            chatLogControlsContainer.Name = "chatLogControlsContainer";
            chatLogControlsContainer.Padding = new Padding(10, 0, 10, 0);
            chatLogControlsContainer.Size = new Size(475, 35);
            chatLogControlsContainer.TabIndex = 16;
            // 
            // chatFileTextContainer
            // 
            chatFileTextContainer.Controls.Add(chatFilePathText);
            chatFileTextContainer.Controls.Add(chatLogText);
            chatFileTextContainer.Dock = DockStyle.Left;
            chatFileTextContainer.Location = new Point(10, 0);
            chatFileTextContainer.Name = "chatFileTextContainer";
            chatFileTextContainer.Size = new Size(335, 35);
            chatFileTextContainer.TabIndex = 12;
            // 
            // padding2
            // 
            padding2.BackColor = Color.Transparent;
            padding2.Dock = DockStyle.Top;
            padding2.Location = new Point(0, 121);
            padding2.Name = "padding2";
            padding2.Size = new Size(475, 23);
            padding2.TabIndex = 20;
            // 
            // scriptsFolderControlsContainer
            // 
            scriptsFolderControlsContainer.BackColor = Color.Transparent;
            scriptsFolderControlsContainer.Controls.Add(scriptsFolderTextContainer);
            scriptsFolderControlsContainer.Controls.Add(scriptsFolderButton);
            scriptsFolderControlsContainer.Dock = DockStyle.Top;
            scriptsFolderControlsContainer.Location = new Point(0, 86);
            scriptsFolderControlsContainer.Name = "scriptsFolderControlsContainer";
            scriptsFolderControlsContainer.Padding = new Padding(10, 0, 10, 0);
            scriptsFolderControlsContainer.Size = new Size(475, 35);
            scriptsFolderControlsContainer.TabIndex = 15;
            // 
            // scriptsFolderTextContainer
            // 
            scriptsFolderTextContainer.Controls.Add(folderPathText);
            scriptsFolderTextContainer.Controls.Add(scriptsFolderText);
            scriptsFolderTextContainer.Dock = DockStyle.Left;
            scriptsFolderTextContainer.Location = new Point(10, 0);
            scriptsFolderTextContainer.Name = "scriptsFolderTextContainer";
            scriptsFolderTextContainer.Size = new Size(335, 35);
            scriptsFolderTextContainer.TabIndex = 8;
            // 
            // padding1
            // 
            padding1.BackColor = Color.Transparent;
            padding1.Dock = DockStyle.Top;
            padding1.Location = new Point(0, 63);
            padding1.Name = "padding1";
            padding1.Size = new Size(475, 23);
            padding1.TabIndex = 2;
            // 
            // characterSelectionContainer
            // 
            characterSelectionContainer.BackColor = Color.FromArgb(44, 38, 66);
            characterSelectionContainer.Controls.Add(removeButtonPadding);
            characterSelectionContainer.Controls.Add(addButtonPadding);
            characterSelectionContainer.Controls.Add(characterSelectPadding);
            characterSelectionContainer.Dock = DockStyle.Top;
            characterSelectionContainer.Location = new Point(0, 33);
            characterSelectionContainer.Name = "characterSelectionContainer";
            characterSelectionContainer.Padding = new Padding(10, 0, 0, 0);
            characterSelectionContainer.Size = new Size(475, 30);
            characterSelectionContainer.TabIndex = 18;
            // 
            // removeButtonPadding
            // 
            removeButtonPadding.Controls.Add(removeCharacterButton);
            removeButtonPadding.Dock = DockStyle.Left;
            removeButtonPadding.Location = new Point(295, 0);
            removeButtonPadding.Name = "removeButtonPadding";
            removeButtonPadding.Padding = new Padding(10, 0, 5, 0);
            removeButtonPadding.Size = new Size(79, 30);
            removeButtonPadding.TabIndex = 16;
            // 
            // addButtonPadding
            // 
            addButtonPadding.Controls.Add(addCharacterButton);
            addButtonPadding.Dock = DockStyle.Left;
            addButtonPadding.Location = new Point(202, 0);
            addButtonPadding.Name = "addButtonPadding";
            addButtonPadding.Padding = new Padding(10, 0, 5, 0);
            addButtonPadding.Size = new Size(93, 30);
            addButtonPadding.TabIndex = 15;
            // 
            // characterSelectPadding
            // 
            characterSelectPadding.Controls.Add(characterSelect);
            characterSelectPadding.Dock = DockStyle.Left;
            characterSelectPadding.Location = new Point(10, 0);
            characterSelectPadding.Name = "characterSelectPadding";
            characterSelectPadding.Padding = new Padding(0, 3, 0, 0);
            characterSelectPadding.Size = new Size(192, 30);
            characterSelectPadding.TabIndex = 17;
            // 
            // headerPadding
            // 
            headerPadding.BackColor = Color.Transparent;
            headerPadding.Dock = DockStyle.Top;
            headerPadding.Location = new Point(0, 0);
            headerPadding.Name = "headerPadding";
            headerPadding.Size = new Size(475, 33);
            headerPadding.TabIndex = 19;
            // 
            // titleBar
            // 
            titleBar.BackColor = Color.FromArgb(35, 30, 51);
            titleBar.Controls.Add(minimizeButton);
            titleBar.Controls.Add(closeButton);
            titleBar.Controls.Add(label1);
            titleBar.Dock = DockStyle.Top;
            titleBar.Location = new Point(0, 0);
            titleBar.Margin = new Padding(0);
            titleBar.MaximumSize = new Size(0, 50);
            titleBar.MinimumSize = new Size(0, 30);
            titleBar.Name = "titleBar";
            titleBar.Size = new Size(475, 30);
            titleBar.TabIndex = 15;
            titleBar.MouseDown += Title_MouseDown;
            titleBar.MouseMove += Title_MouseMove;
            titleBar.MouseUp += Title_MouseUp;
            // 
            // minimizeButton
            // 
            minimizeButton.BackColor = Color.Transparent;
            minimizeButton.Dock = DockStyle.Right;
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.FlatStyle = FlatStyle.Flat;
            minimizeButton.Font = new Font("Roboto Medium", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            minimizeButton.ForeColor = Color.Silver;
            minimizeButton.Location = new Point(409, 0);
            minimizeButton.Name = "minimizeButton";
            minimizeButton.Size = new Size(33, 30);
            minimizeButton.TabIndex = 2;
            minimizeButton.Text = "_";
            minimizeButton.UseVisualStyleBackColor = false;
            minimizeButton.Click += minimizeButton_Click;
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.Transparent;
            closeButton.Dock = DockStyle.Right;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Roboto Medium", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            closeButton.ForeColor = Color.Silver;
            closeButton.Location = new Point(442, 0);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(33, 30);
            closeButton.TabIndex = 1;
            closeButton.Text = "x";
            closeButton.UseVisualStyleBackColor = false;
            closeButton.Click += closeButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Roboto Medium", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Silver;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(0, 3, 0, 0);
            label1.Size = new Size(112, 26);
            label1.TabIndex = 0;
            label1.Text = "PRKHelper";
            // 
            // activeWindow
            // 
            activeWindow.BackColor = Color.Transparent;
            activeWindow.Controls.Add(exitButton);
            activeWindow.Controls.Add(panel2);
            activeWindow.Controls.Add(xpContainer);
            activeWindow.Controls.Add(padding6);
            activeWindow.Controls.Add(damageContainer);
            activeWindow.Controls.Add(padding5);
            activeWindow.Controls.Add(characterReadoutContainer);
            activeWindow.Controls.Add(padding4);
            activeWindow.Controls.Add(dmgTitleBar);
            activeWindow.Dock = DockStyle.Fill;
            activeWindow.Location = new Point(0, 0);
            activeWindow.Name = "activeWindow";
            activeWindow.Size = new Size(475, 380);
            activeWindow.TabIndex = 15;
            activeWindow.MouseDown += Title_MouseDown;
            activeWindow.MouseMove += Title_MouseMove;
            activeWindow.MouseUp += Title_MouseUp;
            // 
            // exitButton
            // 
            exitButton.Dock = DockStyle.Top;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.ForeColor = Color.Salmon;
            exitButton.Location = new Point(0, 231);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(475, 23);
            exitButton.TabIndex = 18;
            exitButton.Text = "Turn Off";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += OpenSettingsWindow;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 208);
            panel2.Name = "panel2";
            panel2.Size = new Size(475, 23);
            panel2.TabIndex = 30;
            // 
            // xpContainer
            // 
            xpContainer.Controls.Add(xpAmount);
            xpContainer.Controls.Add(panel1);
            xpContainer.Controls.Add(richTextBox5);
            xpContainer.Controls.Add(xpToggle);
            xpContainer.Controls.Add(panel3);
            xpContainer.Controls.Add(xpReset);
            xpContainer.Dock = DockStyle.Top;
            xpContainer.Location = new Point(0, 176);
            xpContainer.Name = "xpContainer";
            xpContainer.Padding = new Padding(10, 0, 20, 0);
            xpContainer.Size = new Size(475, 32);
            xpContainer.TabIndex = 29;
            // 
            // xpAmount
            // 
            xpAmount.BackColor = Color.FromArgb(44, 38, 66);
            xpAmount.BorderStyle = BorderStyle.None;
            xpAmount.Dock = DockStyle.Right;
            xpAmount.Font = new Font("Verdana", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            xpAmount.ForeColor = Color.Yellow;
            xpAmount.Location = new Point(88, 0);
            xpAmount.Name = "xpAmount";
            xpAmount.ReadOnly = true;
            xpAmount.RightToLeft = RightToLeft.Yes;
            xpAmount.ScrollBars = RichTextBoxScrollBars.None;
            xpAmount.ShortcutsEnabled = false;
            xpAmount.Size = new Size(157, 32);
            xpAmount.TabIndex = 12;
            xpAmount.Text = "888.68M";
            xpAmount.TextChanged += xpAmount_TextChanged;
            // 
            // panel1
            // 
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(245, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(5, 32);
            panel1.TabIndex = 17;
            // 
            // richTextBox5
            // 
            richTextBox5.BackColor = Color.FromArgb(44, 38, 66);
            richTextBox5.BorderStyle = BorderStyle.None;
            richTextBox5.Dock = DockStyle.Right;
            richTextBox5.Font = new Font("Verdana", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox5.ForeColor = Color.Yellow;
            richTextBox5.Location = new Point(250, 0);
            richTextBox5.Name = "richTextBox5";
            richTextBox5.ReadOnly = true;
            richTextBox5.Size = new Size(82, 32);
            richTextBox5.TabIndex = 10;
            richTextBox5.Text = "XP/h";
            // 
            // xpToggle
            // 
            xpToggle.Dock = DockStyle.Right;
            xpToggle.FlatStyle = FlatStyle.Flat;
            xpToggle.ForeColor = Color.MediumSpringGreen;
            xpToggle.Location = new Point(332, 0);
            xpToggle.Name = "xpToggle";
            xpToggle.Size = new Size(55, 32);
            xpToggle.TabIndex = 15;
            xpToggle.Text = "Start";
            xpToggle.UseVisualStyleBackColor = true;
            xpToggle.Click += ToggleXPTracking;
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(387, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(13, 32);
            panel3.TabIndex = 19;
            // 
            // xpReset
            // 
            xpReset.Dock = DockStyle.Right;
            xpReset.FlatStyle = FlatStyle.Flat;
            xpReset.ForeColor = Color.LightCyan;
            xpReset.Location = new Point(400, 0);
            xpReset.Name = "xpReset";
            xpReset.Size = new Size(55, 32);
            xpReset.TabIndex = 16;
            xpReset.Text = "Reset";
            xpReset.UseVisualStyleBackColor = true;
            xpReset.Click += ResetXPTracking;
            // 
            // padding6
            // 
            padding6.BackColor = Color.Transparent;
            padding6.Dock = DockStyle.Top;
            padding6.Location = new Point(0, 153);
            padding6.Name = "padding6";
            padding6.Size = new Size(475, 23);
            padding6.TabIndex = 28;
            // 
            // damageContainer
            // 
            damageContainer.Controls.Add(damageAmount);
            damageContainer.Controls.Add(dmgPadding3);
            damageContainer.Controls.Add(richTextBox4);
            damageContainer.Controls.Add(damageToggle);
            damageContainer.Controls.Add(dmgPadding2);
            damageContainer.Controls.Add(damageReset);
            damageContainer.Dock = DockStyle.Top;
            damageContainer.Location = new Point(0, 121);
            damageContainer.Name = "damageContainer";
            damageContainer.Padding = new Padding(10, 0, 20, 0);
            damageContainer.Size = new Size(475, 32);
            damageContainer.TabIndex = 27;
            // 
            // damageAmount
            // 
            damageAmount.BackColor = Color.FromArgb(44, 38, 66);
            damageAmount.BorderStyle = BorderStyle.None;
            damageAmount.Dock = DockStyle.Right;
            damageAmount.Font = new Font("Verdana", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            damageAmount.ForeColor = Color.Cyan;
            damageAmount.Location = new Point(88, 0);
            damageAmount.Name = "damageAmount";
            damageAmount.ReadOnly = true;
            damageAmount.RightToLeft = RightToLeft.Yes;
            damageAmount.ScrollBars = RichTextBoxScrollBars.None;
            damageAmount.Size = new Size(157, 32);
            damageAmount.TabIndex = 11;
            damageAmount.Text = "444,444";
            // 
            // dmgPadding3
            // 
            dmgPadding3.Dock = DockStyle.Right;
            dmgPadding3.Location = new Point(245, 0);
            dmgPadding3.Name = "dmgPadding3";
            dmgPadding3.Size = new Size(5, 32);
            dmgPadding3.TabIndex = 16;
            // 
            // richTextBox4
            // 
            richTextBox4.BackColor = Color.FromArgb(44, 38, 66);
            richTextBox4.BorderStyle = BorderStyle.None;
            richTextBox4.Dock = DockStyle.Right;
            richTextBox4.Font = new Font("Verdana", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox4.ForeColor = Color.Cyan;
            richTextBox4.Location = new Point(250, 0);
            richTextBox4.Name = "richTextBox4";
            richTextBox4.ReadOnly = true;
            richTextBox4.Size = new Size(82, 32);
            richTextBox4.TabIndex = 9;
            richTextBox4.Text = "Dmg/m";
            // 
            // damageToggle
            // 
            damageToggle.Dock = DockStyle.Right;
            damageToggle.FlatStyle = FlatStyle.Flat;
            damageToggle.ForeColor = Color.MediumSpringGreen;
            damageToggle.Location = new Point(332, 0);
            damageToggle.Name = "damageToggle";
            damageToggle.Size = new Size(55, 32);
            damageToggle.TabIndex = 13;
            damageToggle.Text = "Start";
            damageToggle.UseVisualStyleBackColor = true;
            damageToggle.Click += ToggleDamageTracking;
            // 
            // dmgPadding2
            // 
            dmgPadding2.Dock = DockStyle.Right;
            dmgPadding2.Location = new Point(387, 0);
            dmgPadding2.Name = "dmgPadding2";
            dmgPadding2.Size = new Size(13, 32);
            dmgPadding2.TabIndex = 0;
            // 
            // damageReset
            // 
            damageReset.Dock = DockStyle.Right;
            damageReset.FlatStyle = FlatStyle.Flat;
            damageReset.ForeColor = Color.LightCyan;
            damageReset.Location = new Point(400, 0);
            damageReset.Name = "damageReset";
            damageReset.Size = new Size(55, 32);
            damageReset.TabIndex = 14;
            damageReset.Text = "Reset";
            damageReset.UseVisualStyleBackColor = true;
            damageReset.Click += ResetDamageTracking;
            // 
            // padding5
            // 
            padding5.BackColor = Color.Transparent;
            padding5.Dock = DockStyle.Top;
            padding5.Location = new Point(0, 110);
            padding5.Name = "padding5";
            padding5.Size = new Size(475, 11);
            padding5.TabIndex = 26;
            // 
            // characterReadoutContainer
            // 
            characterReadoutContainer.Controls.Add(petNameContainer);
            characterReadoutContainer.Controls.Add(activeCharacter);
            characterReadoutContainer.Controls.Add(selectCharacterButtonPadding);
            characterReadoutContainer.Dock = DockStyle.Top;
            characterReadoutContainer.Location = new Point(0, 54);
            characterReadoutContainer.Name = "characterReadoutContainer";
            characterReadoutContainer.Padding = new Padding(10, 0, 0, 0);
            characterReadoutContainer.Size = new Size(475, 56);
            characterReadoutContainer.TabIndex = 24;
            // 
            // petNameContainer
            // 
            petNameContainer.Controls.Add(petNameInput);
            petNameContainer.Controls.Add(petNameLabel);
            petNameContainer.Dock = DockStyle.Top;
            petNameContainer.Location = new Point(10, 26);
            petNameContainer.Name = "petNameContainer";
            petNameContainer.Size = new Size(322, 100);
            petNameContainer.TabIndex = 22;
            // 
            // petNameInput
            // 
            petNameInput.BackColor = Color.FromArgb(44, 38, 66);
            petNameInput.BorderStyle = BorderStyle.FixedSingle;
            petNameInput.Dock = DockStyle.Left;
            petNameInput.ForeColor = Color.Gainsboro;
            petNameInput.Location = new Point(59, 0);
            petNameInput.Name = "petNameInput";
            petNameInput.Size = new Size(94, 23);
            petNameInput.TabIndex = 19;
            // 
            // petNameLabel
            // 
            petNameLabel.BackColor = Color.FromArgb(44, 38, 66);
            petNameLabel.BorderStyle = BorderStyle.None;
            petNameLabel.Dock = DockStyle.Left;
            petNameLabel.Font = new Font("Verdana", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            petNameLabel.ForeColor = Color.SlateGray;
            petNameLabel.Location = new Point(0, 0);
            petNameLabel.Name = "petNameLabel";
            petNameLabel.ReadOnly = true;
            petNameLabel.Size = new Size(59, 100);
            petNameLabel.TabIndex = 20;
            petNameLabel.Text = "Pet name";
            // 
            // activeCharacter
            // 
            activeCharacter.BackColor = Color.FromArgb(44, 38, 66);
            activeCharacter.BorderStyle = BorderStyle.None;
            activeCharacter.Dock = DockStyle.Top;
            activeCharacter.Font = new Font("Verdana", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            activeCharacter.ForeColor = Color.CornflowerBlue;
            activeCharacter.Location = new Point(10, 0);
            activeCharacter.Name = "activeCharacter";
            activeCharacter.ReadOnly = true;
            activeCharacter.Size = new Size(322, 26);
            activeCharacter.TabIndex = 8;
            activeCharacter.Text = "CharacterName";
            // 
            // selectCharacterButtonPadding
            // 
            selectCharacterButtonPadding.Controls.Add(selectCharacterButton);
            selectCharacterButtonPadding.Dock = DockStyle.Right;
            selectCharacterButtonPadding.Location = new Point(332, 0);
            selectCharacterButtonPadding.Name = "selectCharacterButtonPadding";
            selectCharacterButtonPadding.Padding = new Padding(0, 0, 20, 20);
            selectCharacterButtonPadding.Size = new Size(143, 56);
            selectCharacterButtonPadding.TabIndex = 21;
            // 
            // selectCharacterButton
            // 
            selectCharacterButton.Dock = DockStyle.Fill;
            selectCharacterButton.FlatStyle = FlatStyle.Flat;
            selectCharacterButton.ForeColor = Color.LightCyan;
            selectCharacterButton.Location = new Point(0, 0);
            selectCharacterButton.Name = "selectCharacterButton";
            selectCharacterButton.Size = new Size(123, 36);
            selectCharacterButton.TabIndex = 17;
            selectCharacterButton.Text = "Select New Char";
            selectCharacterButton.UseVisualStyleBackColor = true;
            selectCharacterButton.Click += OpenSettingsWindow;
            // 
            // padding4
            // 
            padding4.BackColor = Color.Transparent;
            padding4.Dock = DockStyle.Top;
            padding4.Location = new Point(0, 31);
            padding4.Name = "padding4";
            padding4.Size = new Size(475, 23);
            padding4.TabIndex = 25;
            // 
            // dmgTitleBar
            // 
            dmgTitleBar.Controls.Add(button5);
            dmgTitleBar.Dock = DockStyle.Top;
            dmgTitleBar.Location = new Point(0, 0);
            dmgTitleBar.Name = "dmgTitleBar";
            dmgTitleBar.Size = new Size(475, 31);
            dmgTitleBar.TabIndex = 23;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Right;
            button5.FlatStyle = FlatStyle.Flat;
            button5.ForeColor = Color.MistyRose;
            button5.Location = new Point(416, 0);
            button5.Name = "button5";
            button5.Size = new Size(59, 31);
            button5.TabIndex = 21;
            button5.Text = "Hide";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // FormUI
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = Color.FromArgb(44, 38, 66);
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(475, 380);
            ControlBox = false;
            Controls.Add(settingsBox);
            Controls.Add(activeWindow);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(475, 380);
            Name = "FormUI";
            settingsBox.ResumeLayout(false);
            fileSelectionContainer.ResumeLayout(false);
            combatLogControlsContainer.ResumeLayout(false);
            combatLogTextContainer.ResumeLayout(false);
            combatLogTextContainer.PerformLayout();
            chatLogControlsContainer.ResumeLayout(false);
            chatFileTextContainer.ResumeLayout(false);
            chatFileTextContainer.PerformLayout();
            scriptsFolderControlsContainer.ResumeLayout(false);
            scriptsFolderTextContainer.ResumeLayout(false);
            scriptsFolderTextContainer.PerformLayout();
            characterSelectionContainer.ResumeLayout(false);
            removeButtonPadding.ResumeLayout(false);
            addButtonPadding.ResumeLayout(false);
            characterSelectPadding.ResumeLayout(false);
            titleBar.ResumeLayout(false);
            titleBar.PerformLayout();
            activeWindow.ResumeLayout(false);
            xpContainer.ResumeLayout(false);
            damageContainer.ResumeLayout(false);
            characterReadoutContainer.ResumeLayout(false);
            petNameContainer.ResumeLayout(false);
            petNameContainer.PerformLayout();
            selectCharacterButtonPadding.ResumeLayout(false);
            dmgTitleBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox pathInput;
        private Button combatPathButton;
        private Label folderPathText;
        private Button confirmBtn;
        private RichTextBox chatLogText;
        private Label chatFilePathText;
        private Button scriptsFolderButton;
        private RichTextBox scriptsFolderText;
        private RichTextBox combatLogText;
        private Label combatPathText;
        private Button chatLogButton;
        private ComboBox characterSelect;
        private Button addCharacterButton;
        private Button removeCharacterButton;
        private Panel settingsBox;
        private Panel activeWindow;
        private Button button5;
        private RichTextBox petNameLabel;
        private TextBox petNameInput;
        private Button exitButton;
        private Button selectCharacterButton;
        private Button xpReset;
        private Button xpToggle;
        private Button damageReset;
        private Button damageToggle;
        private RichTextBox xpAmount;
        private RichTextBox damageAmount;
        private RichTextBox richTextBox5;
        private RichTextBox richTextBox4;
        private RichTextBox activeCharacter;
        private Panel titleBar;
        private Panel fileSelectionContainer;
        private Panel scriptsFolderControlsContainer;
        private Panel scriptsFolderTextContainer;
        private Panel chatLogControlsContainer;
        private Panel chatFileTextContainer;
        private Panel combatLogControlsContainer;
        private Panel combatLogTextContainer;
        private Panel characterSelectionContainer;
        private Panel removeButtonPadding;
        private Panel addButtonPadding;
        private Panel padding3;
        private Panel padding2;
        private Panel padding1;
        private Panel headerPadding;
        private Label label1;
        private Panel characterSelectPadding;
        private Button minimizeButton;
        private Button closeButton;
        private Panel dmgTitleBar;
        private Panel characterReadoutContainer;
        private Panel petNameContainer;
        private Panel selectCharacterButtonPadding;
        private Panel xpContainer;
        private Panel padding6;
        private Panel damageContainer;
        private Panel padding5;
        private Panel padding4;
        private Panel dmgPadding1;
        private Panel dmgPadding2;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel dmgPadding3;
        //private FileSystemWatcher Watcher;
    }
}
