using System;
using System.Drawing;
using System.Windows.Forms;

namespace Flaksator.Edit
{
    public sealed class FormEditVerses : Form
    {
        private readonly TextBox filter = new TextBox();
        private readonly ListBox verseList = new ListBox();
        private readonly TextBox source = new TextBox();
        private readonly TextBox preview = new TextBox();
        private readonly TreeView structure = new TreeView();
        private readonly TextBox groupName = new TextBox();
        private readonly ComboBox formatterTemplate = new ComboBox();
        private readonly Button insertFormatter = new Button();
        private readonly Button configureFormatter = new Button();
        private readonly Button insertBreak = new Button();
        private readonly Label validation = new Label();
        private readonly Button add = new Button();
        private readonly Button delete = new Button();
        private readonly Button render = new Button();
        private readonly Button save = new Button();
        private VerseEntry current;
        private bool changing;

        public FormEditVerses()
        {
            this.Text = "Edytor wersów";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(900, 550);
            this.Size = new Size(1150, 720);
            this.InitializeLayout();
            this.Load += this.FormEditVerses_Load;
            this.FormClosing += this.FormEditVerses_FormClosing;
        }

        private void InitializeLayout()
        {
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel1,
                SplitterDistance = 360
            };
            this.Controls.Add(split);

            TableLayoutPanel left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(8)
            };
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            FlowLayoutPanel filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            filterPanel.Controls.Add(new Label { Text = "Filtr:", AutoSize = true });
            this.filter.Width = 325;
            filterPanel.Controls.Add(this.filter);
            left.Controls.Add(filterPanel, 0, 0);
            this.verseList.Dock = DockStyle.Fill;
            this.verseList.HorizontalScrollbar = true;
            left.Controls.Add(this.verseList, 0, 1);
            FlowLayoutPanel listButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            this.add.Text = "Dodaj";
            this.delete.Text = "Usuń";
            listButtons.Controls.Add(this.add);
            listButtons.Controls.Add(this.delete);
            left.Controls.Add(listButtons, 0, 2);
            split.Panel1.Controls.Add(left);

            TableLayoutPanel right = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(8)
            };
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 52));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.Controls.Add(this.CreateFormatterPalette(), 0, 0);
            right.Controls.Add(new Label { Text = "Definicja wersu i struktura grup:", AutoSize = true }, 0, 1);
            SplitContainer definitionSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Size = new Size(760, 360),
                SplitterDistance = 500,
                Panel2MinSize = 180
            };
            this.source.Dock = DockStyle.Fill;
            this.source.Multiline = true;
            this.source.ScrollBars = ScrollBars.Both;
            this.source.AcceptsReturn = true;
            this.source.AcceptsTab = true;
            this.source.Font = new Font(FontFamily.GenericMonospace, 10);
            definitionSplit.Panel1.Controls.Add(this.source);
            this.structure.Dock = DockStyle.Fill;
            this.structure.HideSelection = false;
            definitionSplit.Panel2.Controls.Add(this.structure);
            right.Controls.Add(definitionSplit, 0, 2);
            right.Controls.Add(new Label { Text = "Podgląd po rozwinięciu znaczników:", AutoSize = true }, 0, 3);
            this.preview.Dock = DockStyle.Fill;
            this.preview.Multiline = true;
            this.preview.ReadOnly = true;
            this.preview.ScrollBars = ScrollBars.Both;
            right.Controls.Add(this.preview, 0, 4);
            this.validation.AutoSize = true;
            right.Controls.Add(this.validation, 0, 5);
            FlowLayoutPanel actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            this.render.Text = "Odśwież podgląd";
            this.render.AutoSize = true;
            this.save.Text = "Zapisz Verses.txt";
            this.save.AutoSize = true;
            actions.Controls.Add(this.render);
            actions.Controls.Add(this.save);
            right.Controls.Add(actions, 0, 6);
            split.Panel2.Controls.Add(right);

            this.filter.TextChanged += delegate { if (!this.changing) this.RefreshList(this.current); };
            this.verseList.SelectedIndexChanged += this.VerseList_SelectedIndexChanged;
            this.source.TextChanged += delegate
            {
                this.UpdateCurrentCaption();
                this.InspectSource();
            };
            this.structure.NodeMouseDoubleClick += this.Structure_NodeMouseDoubleClick;
            this.add.Click += this.Add_Click;
            this.delete.Click += this.Delete_Click;
            this.render.Click += delegate { this.RenderPreview(); };
            this.save.Click += this.Save_Click;
        }

        private Control CreateFormatterPalette()
        {
            FlowLayoutPanel palette = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                WrapContents = true
            };
            palette.Controls.Add(new Label { Text = "Grupa:", AutoSize = true, Margin = new Padding(3, 7, 3, 3) });
            this.groupName.Text = "subject";
            this.groupName.Width = 105;
            palette.Controls.Add(this.groupName);
            palette.Controls.Add(new Label { Text = "Znacznik:", AutoSize = true, Margin = new Padding(10, 7, 3, 3) });
            this.formatterTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.formatterTemplate.Width = 260;
            this.formatterTemplate.Items.AddRange(new object[]
            {
                new FormatterTemplate("Rzeczownik: mianownik lp, żeński", "NN1F", false),
                new FormatterTemplate("Przymiotnik: dziedziczony", "A???E", true),
                new FormatterTemplate("Czasownik: teraźniejszy, 3 os.", "VT3?", true),
                new FormatterTemplate("Czasownik: przeszły, 3 os.", "VP3?", true),
                new FormatterTemplate("Partykuła zwrotna „się”", "R", true),
                new FormatterTemplate("Zaimek: biernik", "PC???", true),
                new FormatterTemplate("Zaimek: dopełniacz przyimkowy", "PG???P", true),
                new FormatterTemplate("Czasownik standalone: 1 os. lp", "VT11", false)
            });
            this.formatterTemplate.SelectedIndex = 0;
            palette.Controls.Add(this.formatterTemplate);
            this.insertFormatter.Text = "Wstaw";
            this.insertFormatter.AutoSize = true;
            this.insertFormatter.Click += this.InsertFormatter_Click;
            palette.Controls.Add(this.insertFormatter);
            this.configureFormatter.Text = "Konfiguruj...";
            this.configureFormatter.AutoSize = true;
            this.configureFormatter.Click += this.ConfigureFormatter_Click;
            palette.Controls.Add(this.configureFormatter);
            this.insertBreak.Text = "Wstaw <br>";
            this.insertBreak.AutoSize = true;
            this.insertBreak.Click += delegate { this.InsertAtCaret("<br>"); };
            palette.Controls.Add(this.insertBreak);
            return palette;
        }

        private void FormEditVerses_Load(object sender, EventArgs e)
        {
            this.RefreshList(null);
            if (this.verseList.Items.Count > 0)
                this.verseList.SelectedIndex = 0;
        }

        private void RefreshList(VerseEntry selection)
        {
            string query = this.filter.Text.Trim();
            this.changing = true;
            this.verseList.BeginUpdate();
            try
            {
                this.verseList.Items.Clear();
                foreach (VerseEntry entry in VersesCreator.Creator.Verses)
                {
                    if (query.Length == 0 || entry.Text.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        this.verseList.Items.Add(entry);
                }
                if (selection != null && this.verseList.Items.Contains(selection))
                    this.verseList.SelectedItem = selection;
            }
            finally
            {
                this.verseList.EndUpdate();
                this.changing = false;
            }
        }

        private void VerseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.changing)
                return;
            this.ApplyCurrent();
            this.LoadVerse(this.verseList.SelectedItem as VerseEntry);
        }

        private void LoadVerse(VerseEntry entry)
        {
            this.current = entry;
            this.changing = true;
            this.source.Text = entry == null ? string.Empty : entry.Text;
            this.source.Enabled = entry != null;
            this.render.Enabled = entry != null;
            this.changing = false;
            this.InspectSource();
            this.RenderPreview();
        }

        private void InsertFormatter_Click(object sender, EventArgs e)
        {
            FormatterTemplate template = this.formatterTemplate.SelectedItem as FormatterTemplate;
            if (template == null)
                return;
            string group = this.groupName.Text.Trim();
            if (template.RequiresGroup && !WordFormatterParser.IsValidGroupName(group))
            {
                this.validation.Text = "Ten znacznik wymaga poprawnej nazwy grupy.";
                this.validation.ForeColor = Color.DarkRed;
                this.groupName.Focus();
                return;
            }
            string descriptor = group.Length == 0 ? template.Code : group + ":" + template.Code;
            this.InsertAtCaret("{" + descriptor + "}");
        }

        private void ConfigureFormatter_Click(object sender, EventArgs e)
        {
            using (FormBuildFormatter builder = new FormBuildFormatter(this.groupName.Text))
            {
                if (builder.ShowDialog(this) != DialogResult.OK)
                    return;
                this.groupName.Text = builder.SelectedGroupName;
                this.InsertAtCaret(builder.Formatter);
            }
        }

        private void InsertAtCaret(string text)
        {
            if (this.current == null)
                return;
            int start = this.source.SelectionStart;
            this.source.SelectedText = text;
            this.source.SelectionStart = start + text.Length;
            this.source.SelectionLength = 0;
            this.source.Focus();
        }

        private void InspectSource()
        {
            this.structure.BeginUpdate();
            try
            {
                this.structure.Nodes.Clear();
                VerseDefinitionInspection inspection = VerseDefinitionInspector.Inspect(this.source.Text);
                TreeNode groupsNode = this.structure.Nodes.Add("Grupy nazwane (" + inspection.Groups.Count + ")");
                foreach (VerseGroupSummary group in inspection.Groups)
                {
                    TreeNode groupNode = groupsNode.Nodes.Add(group.Name + " (" + group.Tokens.Count + ")");
                    foreach (VerseFormatterToken token in group.Tokens)
                    {
                        TreeNode tokenNode = groupNode.Nodes.Add(token.ToString());
                        tokenNode.Tag = token;
                    }
                }

                TreeNode standaloneNode = this.structure.Nodes.Add("Bez grupy");
                foreach (VerseFormatterToken token in inspection.Tokens)
                {
                    if (!string.IsNullOrEmpty(token.GroupName))
                        continue;
                    TreeNode tokenNode = standaloneNode.Nodes.Add(token.ToString());
                    tokenNode.Tag = token;
                }

                TreeNode diagnosticsNode = this.structure.Nodes.Add(
                    "Problemy (" + inspection.Diagnostics.Count + ")");
                diagnosticsNode.ForeColor = inspection.Diagnostics.Count == 0 ? Color.DarkGreen : Color.DarkRed;
                foreach (VerseDefinitionDiagnostic diagnostic in inspection.Diagnostics)
                {
                    TreeNode diagnosticNode = diagnosticsNode.Nodes.Add(diagnostic.Message);
                    diagnosticNode.Tag = diagnostic;
                    diagnosticNode.ForeColor = Color.DarkRed;
                }
                groupsNode.Expand();
                diagnosticsNode.Expand();
            }
            finally
            {
                this.structure.EndUpdate();
            }
        }

        private void Structure_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            VerseFormatterToken token = e.Node.Tag as VerseFormatterToken;
            if (token != null)
            {
                this.SelectSourceRange(token.StartIndex, token.Length);
                this.EditToken(token);
                return;
            }
            VerseDefinitionDiagnostic diagnostic = e.Node.Tag as VerseDefinitionDiagnostic;
            if (diagnostic != null)
                this.SelectSourceRange(diagnostic.StartIndex, diagnostic.Length);
        }

        private void EditToken(VerseFormatterToken token)
        {
            VerseFormatterBuildRequest request;
            string error;
            if (!VerseFormatterBuilder.TryParse("{" + token.Descriptor + "}", out request, out error))
            {
                this.validation.Text = "Nie można skonfigurować znacznika: " + error;
                this.validation.ForeColor = Color.DarkRed;
                return;
            }

            using (FormBuildFormatter builder = new FormBuildFormatter(request))
            {
                if (builder.ShowDialog(this) != DialogResult.OK)
                    return;
                this.groupName.Text = builder.SelectedGroupName;
                this.source.SelectionStart = Math.Min(token.StartIndex, this.source.TextLength);
                this.source.SelectionLength = Math.Min(token.Length,
                    this.source.TextLength - this.source.SelectionStart);
                this.source.SelectedText = builder.Formatter;
                this.source.SelectionStart = token.StartIndex + builder.Formatter.Length;
                this.source.SelectionLength = 0;
                this.source.Focus();
            }
        }

        private void SelectSourceRange(int start, int length)
        {
            this.source.Focus();
            this.source.SelectionStart = Math.Min(start, this.source.TextLength);
            this.source.SelectionLength = Math.Min(length, this.source.TextLength - this.source.SelectionStart);
            this.source.ScrollToCaret();
        }

        private void ApplyCurrent()
        {
            if (this.current != null)
                this.current.Text = this.source.Text.Trim();
        }

        private void UpdateCurrentCaption()
        {
            if (this.changing || this.current == null)
                return;
            this.current.Text = this.source.Text.Trim();
            int index = this.verseList.Items.IndexOf(this.current);
            if (index >= 0)
            {
                this.verseList.Items[index] = this.current;
                this.verseList.SelectedIndex = index;
            }
        }

        private void RenderPreview()
        {
            if (this.current == null)
            {
                this.preview.Clear();
                this.validation.Text = string.Empty;
                return;
            }

            this.ApplyCurrent();
            try
            {
                string result = VersesCreator.Creator.DecodeVerse(this.current.Text);
                this.preview.Text = result.Replace("<br>", Environment.NewLine);
                bool unresolved = result.IndexOf('{') >= 0 || result.IndexOf('}') >= 0;
                this.validation.Text = unresolved
                    ? "Uwaga: podgląd zawiera nierozwinięty znacznik lub diagnostykę."
                    : "Definicja została rozwinięta bez diagnostyki.";
                this.validation.ForeColor = unresolved ? Color.DarkOrange : Color.DarkGreen;
            }
            catch (Exception exception)
            {
                this.preview.Clear();
                this.validation.Text = "Błąd podglądu: " + exception.Message;
                this.validation.ForeColor = Color.DarkRed;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            this.ApplyCurrent();
            VerseEntry entry = new VerseEntry("{subject:A???E} {subject:NN1F} {subject:VT3?}.");
            VersesCreator.Creator.Verses.Add(entry);
            this.changing = true;
            this.filter.Clear();
            this.changing = false;
            this.RefreshList(entry);
            this.LoadVerse(entry);
            this.source.SelectAll();
            this.source.Focus();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.current == null || MessageBox.Show(this, "Usunąć wybrany wers?", "Usuwanie wersu",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            VerseEntry removed = this.current;
            this.LoadVerse(null);
            VersesCreator.Creator.Verses.Remove(removed);
            this.RefreshList(null);
            if (this.verseList.Items.Count > 0)
                this.verseList.SelectedIndex = 0;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.ApplyCurrent();
            if (!this.ValidateBeforeSave())
                return;
            VersesCreator.Creator.SaveToFile(".\\DataFiles\\Verses.txt");
            this.current = null;
            this.RefreshList(null);
            if (this.verseList.Items.Count > 0)
                this.verseList.SelectedIndex = 0;
        }

        private bool ValidateBeforeSave()
        {
            foreach (VerseEntry entry in VersesCreator.Creator.Verses)
            {
                if (string.IsNullOrWhiteSpace(entry.Text))
                {
                    MessageBox.Show(this, "Pusty wers należy uzupełnić albo usunąć.", "Nie można zapisać",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void FormEditVerses_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ApplyCurrent();
        }

        private sealed class FormatterTemplate
        {
            public FormatterTemplate(string label, string code, bool requiresGroup)
            {
                this.Label = label;
                this.Code = code;
                this.RequiresGroup = requiresGroup;
            }

            public string Label { get; private set; }

            public string Code { get; private set; }

            public bool RequiresGroup { get; private set; }

            public override string ToString()
            {
                return this.Label + "  {" + this.Code + "}";
            }
        }
    }
}
