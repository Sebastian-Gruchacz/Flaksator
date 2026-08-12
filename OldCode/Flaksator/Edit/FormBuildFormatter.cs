using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDevs.Fleksator;

namespace Flaksator.Edit
{
    public sealed class FormBuildFormatter : Form
    {
        private readonly TextBox groupName = new TextBox();
        private readonly ComboBox kind = new ComboBox();
        private readonly ComboBox aCase = new ComboBox();
        private readonly ComboBox number = new ComboBox();
        private readonly ComboBox gender = new ComboBox();
        private readonly ComboBox tense = new ComboBox();
        private readonly ComboBox person = new ComboBox();
        private readonly ComboBox level = new ComboBox();
        private readonly CheckBox prepositional = new CheckBox();
        private readonly CheckedListBox categories = new CheckedListBox();
        private readonly TextBox preview = new TextBox();
        private readonly Label error = new Label();
        private readonly Button accept = new Button();

        public FormBuildFormatter(string initialGroupName)
        {
            this.Text = "Konfigurator znacznika";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.InitializeLayout(initialGroupName);
        }

        public FormBuildFormatter(VerseFormatterBuildRequest request)
            : this(request == null ? string.Empty : request.GroupName)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            this.ApplyRequest(request);
        }

        public string Formatter { get; private set; }

        public string SelectedGroupName
        {
            get { return this.groupName.Text.Trim(); }
        }

        private void InitializeLayout(string initialGroupName)
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 13,
                Padding = new Padding(12)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 330));
            this.Controls.Add(layout);

            this.AddRow(layout, 0, "Grupa (pusta = standalone):", this.groupName);
            this.groupName.Text = initialGroupName ?? string.Empty;
            this.ConfigureCombo(this.kind, new object[]
            {
                new KindOption("Rzeczownik", VerseFormatterBuildKind.Noun),
                new KindOption("Przymiotnik", VerseFormatterBuildKind.Adjective),
                new KindOption("Czasownik", VerseFormatterBuildKind.Verb),
                new KindOption("Zaimek", VerseFormatterBuildKind.Pronoun),
                new KindOption("Partykuła zwrotna R", VerseFormatterBuildKind.ReflexiveParticle)
            });
            this.AddRow(layout, 1, "Część mowy:", this.kind);
            this.ConfigureCombo(this.aCase, new object[]
            {
                new CodeOption("Mianownik", 'N'), new CodeOption("Narzędnik", 'A'),
                new CodeOption("Biernik", 'C'), new CodeOption("Celownik", 'D'),
                new CodeOption("Dopełniacz", 'G'), new CodeOption("Miejscownik", 'L'),
                new CodeOption("Wołacz", 'V'), new CodeOption("Dziedzicz", '?')
            });
            this.AddRow(layout, 2, "Przypadek:", this.aCase);
            this.ConfigureCombo(this.number, new object[]
            {
                new CodeOption("Pojedyncza", '1'), new CodeOption("Mnoga", '2'),
                new CodeOption("Dziedzicz", '?')
            });
            this.AddRow(layout, 3, "Liczba:", this.number);
            this.ConfigureCombo(this.gender, new object[]
            {
                new CodeOption("Męskożywotny", 'L'), new CodeOption("Męskoosobowy", 'P'),
                new CodeOption("Męskorzeczowy", 'T'), new CodeOption("Żeński", 'F'),
                new CodeOption("Nijaki", 'N'), new CodeOption("Dziedzicz / brak", '?')
            });
            this.AddRow(layout, 4, "Rodzaj:", this.gender);
            this.ConfigureCombo(this.tense, new object[]
            {
                new CodeOption("Teraźniejszy", 'T'), new CodeOption("Przeszły", 'P')
            });
            this.AddRow(layout, 5, "Czas:", this.tense);
            this.ConfigureCombo(this.person, new object[]
            {
                new CodeOption("Pierwsza", '1'), new CodeOption("Druga", '2'),
                new CodeOption("Trzecia", '3'), new CodeOption("Dziedzicz / domyślnie 3", '?')
            });
            this.AddRow(layout, 6, "Osoba:", this.person);
            this.ConfigureCombo(this.level, new object[]
            {
                new CodeOption("Równy", 'E'), new CodeOption("Wyższy", 'H'),
                new CodeOption("Najwyższy", 'S')
            });
            this.AddRow(layout, 7, "Stopień:", this.level);
            this.prepositional.Text = "wariant przyimkowy";
            this.prepositional.AutoSize = true;
            this.AddRow(layout, 8, "Zaimek:", this.prepositional);
            this.categories.CheckOnClick = true;
            this.categories.IntegralHeight = false;
            this.categories.Height = 110;
            this.AddRow(layout, 9, "Kategorie:", this.categories);
            this.preview.ReadOnly = true;
            this.preview.Font = new Font(FontFamily.GenericMonospace, 10);
            this.AddRow(layout, 10, "Wynik:", this.preview);
            this.error.AutoSize = true;
            this.error.ForeColor = Color.DarkRed;
            layout.Controls.Add(this.error, 0, 11);
            layout.SetColumnSpan(this.error, 2);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft
            };
            this.accept.Text = "Wstaw";
            this.accept.AutoSize = true;
            this.accept.Click += this.Accept_Click;
            Button cancel = new Button { Text = "Anuluj", AutoSize = true, DialogResult = DialogResult.Cancel };
            buttons.Controls.Add(this.accept);
            buttons.Controls.Add(cancel);
            layout.Controls.Add(buttons, 0, 12);
            layout.SetColumnSpan(buttons, 2);
            this.AcceptButton = this.accept;
            this.CancelButton = cancel;

            this.kind.SelectedIndexChanged += this.Kind_SelectedIndexChanged;
            this.groupName.TextChanged += delegate { this.UpdatePreview(); };
            this.categories.ItemCheck += delegate
            {
                if (this.IsHandleCreated)
                    this.BeginInvoke(new MethodInvoker(this.UpdatePreview));
            };
            this.prepositional.CheckedChanged += delegate { this.UpdatePreview(); };
            foreach (ComboBox combo in new[] { this.aCase, this.number, this.gender, this.tense,
                this.person, this.level })
                combo.SelectedIndexChanged += delegate { this.UpdatePreview(); };
            this.Kind_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void AddRow(TableLayoutPanel layout, int row, string label, Control control)
        {
            layout.Controls.Add(new Label
            {
                Text = label,
                AutoSize = true,
                Margin = new Padding(3, 7, 8, 3)
            }, 0, row);
            control.Dock = DockStyle.Fill;
            layout.Controls.Add(control, 1, row);
        }

        private void ConfigureCombo(ComboBox combo, object[] options)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Items.AddRange(options);
            combo.SelectedIndex = 0;
        }

        private void Kind_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerseFormatterBuildKind selected = this.GetSelectedKind();
            bool noun = selected == VerseFormatterBuildKind.Noun;
            bool adjective = selected == VerseFormatterBuildKind.Adjective;
            bool verb = selected == VerseFormatterBuildKind.Verb;
            bool pronoun = selected == VerseFormatterBuildKind.Pronoun;
            this.aCase.Enabled = noun || adjective || pronoun;
            this.number.Enabled = noun || adjective || verb || pronoun;
            this.gender.Enabled = noun || adjective || verb || pronoun;
            this.tense.Enabled = verb;
            this.person.Enabled = verb || pronoun;
            this.level.Enabled = adjective;
            this.prepositional.Enabled = pronoun;
            this.categories.Enabled = selected != VerseFormatterBuildKind.ReflexiveParticle;
            if (!pronoun)
                this.prepositional.Checked = false;
            if (selected == VerseFormatterBuildKind.ReflexiveParticle)
                this.categories.Items.Clear();

            if (adjective)
            {
                this.SelectCode(this.aCase, '?');
                this.SelectCode(this.number, '?');
                this.SelectCode(this.gender, '?');
            }
            else if (verb)
            {
                this.SelectCode(this.person, '3');
                this.SelectCode(this.number, '?');
                this.SelectCode(this.gender, '?');
            }
            else if (pronoun)
            {
                this.SelectCode(this.aCase, 'C');
                this.SelectCode(this.person, '?');
                this.SelectCode(this.number, '?');
                this.SelectCode(this.gender, '?');
            }
            else if (noun)
            {
                this.SelectCode(this.aCase, 'N');
                this.SelectCode(this.number, '1');
                this.SelectCode(this.gender, 'F');
            }
            this.PopulateCategories(null);
            this.UpdatePreview();
        }

        private void ApplyRequest(VerseFormatterBuildRequest request)
        {
            foreach (object item in this.kind.Items)
            {
                KindOption option = item as KindOption;
                if (option != null && option.Kind == request.Kind)
                {
                    this.kind.SelectedItem = item;
                    break;
                }
            }
            this.groupName.Text = request.GroupName ?? string.Empty;
            this.SelectCode(this.aCase, request.CaseCode);
            this.SelectCode(this.number, request.NumberCode);
            this.SelectCode(this.gender, request.GenderCode);
            this.SelectCode(this.tense, request.TenseCode);
            this.SelectCode(this.person, request.PersonCode);
            this.SelectCode(this.level, request.LevelCode);
            this.prepositional.Checked = request.PrepositionalPronoun;
            this.PopulateCategories(request.CategoryIds);
            this.UpdatePreview();
        }

        private void PopulateCategories(IList<int> selectedIds)
        {
            Dictionary<int, string> dictionary = this.GetCategoryDictionary();
            this.categories.BeginUpdate();
            try
            {
                this.categories.Items.Clear();
                List<int> identifiers = new List<int>(dictionary.Keys);
                identifiers.Sort();
                foreach (int identifier in identifiers)
                {
                    CategoryOption option = new CategoryOption(identifier, dictionary[identifier]);
                    this.categories.Items.Add(option,
                        selectedIds != null && selectedIds.Contains(identifier));
                }
                if (selectedIds != null)
                {
                    foreach (int identifier in selectedIds)
                    {
                        if (dictionary.ContainsKey(identifier))
                            continue;
                        this.categories.Items.Add(new CategoryOption(identifier, "niezdefiniowana"), true);
                    }
                }
            }
            finally
            {
                this.categories.EndUpdate();
            }
        }

        private Dictionary<int, string> GetCategoryDictionary()
        {
            switch (this.GetSelectedKind())
            {
                case VerseFormatterBuildKind.Noun:
                    return WordCategories.Categories.NounCategories;
                case VerseFormatterBuildKind.Adjective:
                    return WordCategories.Categories.AdjectiveCategories;
                case VerseFormatterBuildKind.Verb:
                    return WordCategories.Categories.VerbCategories;
                default:
                    return new Dictionary<int, string>();
            }
        }

        private void SelectCode(ComboBox combo, char code)
        {
            foreach (object item in combo.Items)
            {
                CodeOption option = item as CodeOption;
                if (option != null && option.Code == code)
                {
                    combo.SelectedItem = item;
                    return;
                }
            }
        }

        private VerseFormatterBuildRequest CreateRequest(out string parseError)
        {
            VerseFormatterBuildRequest request = new VerseFormatterBuildRequest
            {
                Kind = this.GetSelectedKind(),
                GroupName = this.groupName.Text,
                CaseCode = this.GetCode(this.aCase),
                NumberCode = this.GetCode(this.number),
                GenderCode = this.GetCode(this.gender),
                TenseCode = this.GetCode(this.tense),
                PersonCode = this.GetCode(this.person),
                LevelCode = this.GetCode(this.level),
                PrepositionalPronoun = this.prepositional.Checked
            };
            foreach (object item in this.categories.CheckedItems)
            {
                CategoryOption option = item as CategoryOption;
                if (option != null)
                    request.CategoryIds.Add(option.Id);
            }
            parseError = null;
            return request;
        }

        private VerseFormatterBuildKind GetSelectedKind()
        {
            KindOption option = this.kind.SelectedItem as KindOption;
            return option == null ? VerseFormatterBuildKind.Noun : option.Kind;
        }

        private char GetCode(ComboBox combo)
        {
            CodeOption option = combo.SelectedItem as CodeOption;
            return option == null ? '?' : option.Code;
        }

        private void UpdatePreview()
        {
            string parseError;
            VerseFormatterBuildRequest request = this.CreateRequest(out parseError);
            string formatter = null;
            string buildError = null;
            bool valid = request != null && VerseFormatterBuilder.TryBuild(request,
                out formatter, out buildError);
            this.Formatter = valid ? formatter : null;
            this.preview.Text = valid ? formatter : string.Empty;
            this.error.Text = valid ? string.Empty : (parseError ?? buildError);
            this.accept.Enabled = valid;
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            this.UpdatePreview();
            if (this.Formatter == null)
                return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private sealed class CodeOption
        {
            public CodeOption(string label, char code)
            {
                this.Label = label;
                this.Code = code;
            }

            public string Label { get; private set; }

            public char Code { get; private set; }

            public override string ToString()
            {
                return this.Label + "  [" + this.Code + "]";
            }
        }

        private sealed class KindOption
        {
            public KindOption(string label, VerseFormatterBuildKind kind)
            {
                this.Label = label;
                this.Kind = kind;
            }

            public string Label { get; private set; }

            public VerseFormatterBuildKind Kind { get; private set; }

            public override string ToString()
            {
                return this.Label;
            }
        }

        private sealed class CategoryOption
        {
            public CategoryOption(int id, string name)
            {
                this.Id = id;
                this.Name = name;
            }

            public int Id { get; private set; }

            public string Name { get; private set; }

            public override string ToString()
            {
                return "[" + this.Id + "] " + this.Name;
            }
        }
    }
}
