using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.Edit
{
    public sealed class FormEditVerbs : Form
    {
        private readonly TextBox filter = new TextBox();
        private readonly CheckBox needsWorkOnly = new CheckBox();
        private readonly Label workSummary = new Label();
        private readonly ListBox verbList = new ListBox();
        private readonly TextBox infinitive = new TextBox();
        private readonly CheckBox reflexive = new CheckBox();
        private readonly CheckBox active = new CheckBox();
        private readonly CheckBox passive = new CheckBox();
        private readonly NumericUpDown conjugation = new NumericUpDown();
        private readonly ComboBox aspect = new ComboBox();
        private readonly CheckedListBox categories = new CheckedListBox();
        private readonly DataGridView forms = new DataGridView();
        private readonly Button add = new Button();
        private readonly Button delete = new Button();
        private readonly Button fillFuture = new Button();
        private readonly Button save = new Button();
        private Verb current;
        private bool changing;

        public FormEditVerbs()
        {
            this.Text = "Edytor czasowników";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(850, 500);
            this.Size = new Size(1050, 650);
            this.InitializeLayout();
            this.Load += this.FormEditVerbs_Load;
            this.FormClosing += this.FormEditVerbs_FormClosing;
        }

        private void InitializeLayout()
        {
            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                FixedPanel = FixedPanel.Panel1,
                SplitterDistance = 270
            };
            this.Controls.Add(split);

            TableLayoutPanel left = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
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
            filterPanel.Controls.Add(this.filter);
            this.needsWorkOnly.Text = "Tylko wymagające uzupełnienia";
            this.needsWorkOnly.AutoSize = true;
            filterPanel.Controls.Add(this.needsWorkOnly);
            this.workSummary.AutoSize = true;
            filterPanel.Controls.Add(this.workSummary);
            left.Controls.Add(filterPanel, 0, 0);
            left.Controls.Add(this.verbList, 0, 1);
            FlowLayoutPanel listButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            this.add.Text = "Dodaj";
            this.delete.Text = "Usuń";
            listButtons.Controls.Add(this.add);
            listButtons.Controls.Add(this.delete);
            left.Controls.Add(listButtons, 0, 2);
            split.Panel1.Controls.Add(left);

            this.filter.Width = 240;
            this.verbList.Dock = DockStyle.Fill;

            TableLayoutPanel right = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(8)
            };
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            FlowLayoutPanel header = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            header.Controls.Add(new Label { Text = "Bezokolicznik:", AutoSize = true, Margin = new Padding(3, 7, 3, 3) });
            this.infinitive.Width = 260;
            header.Controls.Add(this.infinitive);
            this.reflexive.Text = "zwrotny (się)";
            this.reflexive.AutoSize = true;
            this.reflexive.Margin = new Padding(12, 6, 3, 3);
            header.Controls.Add(this.reflexive);
            this.fillFuture.Text = "Uzupełnij przyszły";
            this.fillFuture.AutoSize = true;
            this.fillFuture.Margin = new Padding(12, 3, 3, 3);
            header.Controls.Add(this.fillFuture);
            right.Controls.Add(header, 0, 0);

            right.Controls.Add(this.CreateMetadataPanel(), 0, 1);

            this.ConfigureFormsGrid();
            right.Controls.Add(this.forms, 0, 2);
            this.save.Text = "Zapisz słownik";
            this.save.AutoSize = true;
            right.Controls.Add(this.save, 0, 3);
            split.Panel2.Controls.Add(right);

            this.filter.TextChanged += delegate { if (!this.changing) this.RefreshList(this.current); };
            this.needsWorkOnly.CheckedChanged += delegate
            {
                if (!this.changing) this.RefreshList(this.current);
            };
            this.verbList.SelectedIndexChanged += this.VerbList_SelectedIndexChanged;
            this.infinitive.TextChanged += delegate { this.UpdateCurrentCaption(); };
            this.reflexive.CheckedChanged += delegate { this.UpdateCurrentCaption(); };
            this.add.Click += this.Add_Click;
            this.delete.Click += this.Delete_Click;
            this.fillFuture.Click += this.FillFuture_Click;
            this.save.Click += this.Save_Click;
        }

        private Control CreateMetadataPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            FlowLayoutPanel parameters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            FlowLayoutPanel conjugationRow = new FlowLayoutPanel { AutoSize = true };
            conjugationRow.Controls.Add(new Label
            {
                Text = "Koniugacja:",
                AutoSize = true,
                Margin = new Padding(3, 7, 3, 3)
            });
            this.conjugation.Minimum = 0;
            this.conjugation.Maximum = 99;
            this.conjugation.Width = 60;
            conjugationRow.Controls.Add(this.conjugation);
            parameters.Controls.Add(conjugationRow);
            FlowLayoutPanel aspectRow = new FlowLayoutPanel { AutoSize = true };
            aspectRow.Controls.Add(new Label
            {
                Text = "Aspekt:",
                AutoSize = true,
                Margin = new Padding(3, 7, 3, 3)
            });
            this.aspect.DropDownStyle = ComboBoxStyle.DropDownList;
            this.aspect.Width = 120;
            this.aspect.Items.AddRange(new object[]
            {
                VerbAspect.Unknown, VerbAspect.Imperfective,
                VerbAspect.Perfective, VerbAspect.Biaspectual
            });
            this.aspect.SelectedItem = VerbAspect.Unknown;
            aspectRow.Controls.Add(this.aspect);
            parameters.Controls.Add(aspectRow);
            this.active.Text = "strona czynna";
            this.active.AutoSize = true;
            this.active.Checked = true;
            parameters.Controls.Add(this.active);
            this.passive.Text = "strona bierna";
            this.passive.AutoSize = true;
            parameters.Controls.Add(this.passive);
            panel.Controls.Add(parameters, 0, 0);

            GroupBox categoryGroup = new GroupBox { Text = "Kategorie", Dock = DockStyle.Fill };
            this.categories.Dock = DockStyle.Fill;
            this.categories.CheckOnClick = true;
            List<int> identifiers = new List<int>(WordCategories.Categories.VerbCategories.Keys);
            identifiers.Sort();
            foreach (int identifier in identifiers)
                this.categories.Items.Add(new CategoryOption(identifier,
                    WordCategories.Categories.VerbCategories[identifier]));
            categoryGroup.Controls.Add(this.categories);
            panel.Controls.Add(categoryGroup, 1, 0);
            return panel;
        }

        private void ConfigureFormsGrid()
        {
            this.forms.Dock = DockStyle.Fill;
            this.forms.AllowUserToAddRows = true;
            this.forms.AllowUserToDeleteRows = true;
            this.forms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.forms.RowHeadersVisible = false;
            this.forms.DataError += delegate { };

            this.forms.Columns.Add(this.CreateComboColumn("Czas", new object[] { VerbTense.Present, VerbTense.Past, VerbTense.Future }));
            this.forms.Columns.Add(this.CreateComboColumn("Osoba", new object[] { GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third }));
            this.forms.Columns.Add(this.CreateComboColumn("Liczba", new object[] { DecliantionNumber.Singular, DecliantionNumber.Plural }));
            this.forms.Columns.Add(this.CreateComboColumn("Rodzaj (tylko przeszły)", new object[] {
                GrammaticalGender._Unknown, GrammaticalGender.MasculineLife,
                GrammaticalGender.MasculinePerson, GrammaticalGender.MasculineThing,
                GrammaticalGender.Feminine, GrammaticalGender.Neuter }));
            this.forms.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Forma", FillWeight = 160 });
        }

        private DataGridViewComboBoxColumn CreateComboColumn(string header, object[] values)
        {
            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn { HeaderText = header };
            column.Items.AddRange(values);
            return column;
        }

        private void FormEditVerbs_Load(object sender, EventArgs e)
        {
            this.RefreshList(null);
            if (this.verbList.Items.Count > 0)
                this.verbList.SelectedIndex = 0;
        }

        private void RefreshList(Verb selection)
        {
            string query = this.filter.Text.Trim();
            int needsWorkCount = 0;
            this.changing = true;
            this.verbList.BeginUpdate();
            try
            {
                this.verbList.Items.Clear();
                foreach (Verb verb in VerbCollection.Collection.Verbs)
                {
                    VerbEditingStatus status = VerbEditingStatus.Inspect(verb);
                    if (status.NeedsWork)
                        needsWorkCount++;
                    string caption = GetCaption(verb, status);
                    verb.DisplayName = caption;
                    if ((!this.needsWorkOnly.Checked || status.NeedsWork) &&
                        (query.Length == 0 || caption.IndexOf(query,
                            StringComparison.CurrentCultureIgnoreCase) >= 0))
                        this.verbList.Items.Add(verb);
                }
                if (selection != null && this.verbList.Items.Contains(selection))
                    this.verbList.SelectedItem = selection;
            }
            finally
            {
                this.verbList.EndUpdate();
                this.workSummary.Text = "Do uzupełnienia: " + needsWorkCount +
                    " / " + VerbCollection.Collection.Verbs.Count;
                this.changing = false;
            }
        }

        private void VerbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.changing)
                return;

            Verb selected = this.verbList.SelectedItem as Verb;
            if (selected == this.current)
                return;

            string error;
            if (!this.TryApplyCurrent(out error))
            {
                MessageBox.Show(this, error, "Nie można zmienić czasownika", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.changing = true;
                this.verbList.SelectedItem = this.current;
                this.changing = false;
                return;
            }
            this.LoadVerb(selected);
        }

        private void LoadVerb(Verb verb)
        {
            this.current = verb;
            this.changing = true;
            this.forms.Rows.Clear();
            if (verb != null)
            {
                this.infinitive.Text = verb.Infinitive;
                this.reflexive.Checked = verb.IsReflexive;
                this.conjugation.Value = Math.Max(this.conjugation.Minimum,
                    Math.Min(this.conjugation.Maximum, verb.ConjugationNumber));
                this.aspect.SelectedItem = verb.Aspect;
                this.active.Checked = verb.CanBeActive;
                this.passive.Checked = verb.CanBePassive;
                for (int i = 0; i < this.categories.Items.Count; i++)
                    this.categories.SetItemChecked(i,
                        verb.Categories.Contains(((CategoryOption)this.categories.Items[i]).Id));
                foreach (VerbForm form in verb.Forms)
                    this.forms.Rows.Add(form.Tense, form.Person, form.Number, form.Gender, form.Text);
            }
            else
            {
                this.infinitive.Clear();
                this.reflexive.Checked = false;
                this.conjugation.Value = 0;
                this.aspect.SelectedItem = VerbAspect.Unknown;
                this.active.Checked = true;
                this.passive.Checked = false;
                for (int i = 0; i < this.categories.Items.Count; i++)
                    this.categories.SetItemChecked(i, false);
            }
            this.infinitive.Enabled = verb != null;
            this.reflexive.Enabled = verb != null;
            this.conjugation.Enabled = verb != null;
            this.aspect.Enabled = verb != null;
            this.active.Enabled = verb != null;
            this.passive.Enabled = verb != null;
            this.categories.Enabled = verb != null;
            this.forms.Enabled = verb != null;
            this.fillFuture.Enabled = verb != null;
            this.changing = false;
        }

        private bool TryApplyCurrent(out string error)
        {
            error = null;
            if (this.current == null)
                return true;

            string root = this.infinitive.Text.Trim();
            if (root.Length == 0 || root.IndexOf('|') >= 0)
            {
                error = "Bezokolicznik nie może być pusty ani zawierać znaku |.";
                return false;
            }

            List<VerbForm> parsed = new List<VerbForm>();
            foreach (DataGridViewRow row in this.forms.Rows)
            {
                if (row.IsNewRow)
                    continue;
                if (row.Cells[0].Value == null && row.Cells[1].Value == null &&
                    row.Cells[2].Value == null && row.Cells[3].Value == null && row.Cells[4].Value == null)
                    continue;
                if (row.Cells[0].Value == null || row.Cells[1].Value == null ||
                    row.Cells[2].Value == null || row.Cells[3].Value == null)
                {
                    error = "Niepełny wiersz koniugacji.";
                    return false;
                }
                parsed.Add(new VerbForm
                {
                    Tense = (VerbTense)row.Cells[0].Value,
                    Person = (GrammaticalPerson)row.Cells[1].Value,
                    Number = (DecliantionNumber)row.Cells[2].Value,
                    Gender = (GrammaticalGender)row.Cells[3].Value,
                    Text = Convert.ToString(row.Cells[4].Value).Trim()
                });
            }

            VerbAspect selectedAspect = this.aspect.SelectedItem == null
                ? VerbAspect.Unknown
                : (VerbAspect)this.aspect.SelectedItem;
            if (!VerbFormValidator.TryValidate(selectedAspect, parsed, out error))
                return false;
            if (!this.active.Checked && !this.passive.Checked)
            {
                error = "Czasownik musi dopuszczać stronę czynną lub bierną.";
                return false;
            }

            this.current.Infinitive = root;
            this.current.DisplayName = root;
            this.current.IsReflexive = this.reflexive.Checked;
            this.current.ConjugationNumber = Decimal.ToInt32(this.conjugation.Value);
            this.current.Aspect = selectedAspect;
            this.current.CanBeActive = this.active.Checked;
            this.current.CanBePassive = this.passive.Checked;
            this.current.Categories.Clear();
            foreach (CategoryOption option in this.categories.CheckedItems)
                this.current.Categories.Add(option.Id);
            this.current.Forms.Clear();
            this.current.Forms.AddRange(parsed);
            return true;
        }

        private void UpdateCurrentCaption()
        {
            if (this.changing || this.current == null)
                return;
            int index = this.verbList.Items.IndexOf(this.current);
            if (index >= 0)
            {
                this.current.Infinitive = this.infinitive.Text.Trim();
                this.current.IsReflexive = this.reflexive.Checked;
                this.verbList.Items[index] = this.current;
                this.verbList.SelectedIndex = index;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string error;
            if (!this.TryApplyCurrent(out error))
            {
                MessageBox.Show(this, error, "Nie można dodać czasownika", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Verb verb = new Verb { Infinitive = "nowy czasownik", DisplayName = "nowy czasownik", CanBeActive = true };
            VerbCollection.Collection.Verbs.Add(verb);
            this.changing = true;
            this.filter.Clear();
            this.changing = false;
            this.RefreshList(verb);
            this.LoadVerb(verb);
            this.infinitive.SelectAll();
            this.infinitive.Focus();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.current == null || MessageBox.Show(this, "Usunąć „" + GetCaption(this.current) + "”?",
                "Usuwanie czasownika", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Verb removed = this.current;
            this.LoadVerb(null);
            VerbCollection.Collection.Verbs.Remove(removed);
            this.RefreshList(null);
            if (this.verbList.Items.Count > 0)
                this.verbList.SelectedIndex = 0;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string error;
            if (!this.TryApplyCurrent(out error))
            {
                MessageBox.Show(this, error, "Nie można zapisać", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            VerbCollection.Collection.SaveToFile(".\\DataFiles\\Verbs.txt");
            this.RefreshList(this.current);
        }

        private void FillFuture_Click(object sender, EventArgs e)
        {
            string error;
            if (!this.TryApplyCurrent(out error))
            {
                MessageBox.Show(this, error, "Nie można uzupełnić przyszłości",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int added;
            if (!CompoundFuturePrefiller.TryAddMissingForms(this.current, out added, out error))
            {
                MessageBox.Show(this, error, "Nie można uzupełnić przyszłości",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.LoadVerb(this.current);
            MessageBox.Show(this, added == 0
                ? "Paradygmat przyszły jest już kompletny."
                : "Dodano brakujące formy: " + added + ".",
                "Przyszłość złożona", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FormEditVerbs_FormClosing(object sender, FormClosingEventArgs e)
        {
            string error;
            if (!this.TryApplyCurrent(out error))
            {
                e.Cancel = true;
                MessageBox.Show(this, error, "Nie można zamknąć edytora", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            VerbCollection.Collection.SaveToFile(".\\DataFiles\\Verbs.txt");
        }

        private static string GetCaption(Verb verb)
        {
            return GetCaption(verb, VerbEditingStatus.Inspect(verb));
        }

        private static string GetCaption(Verb verb, VerbEditingStatus status)
        {
            string caption = verb.Infinitive + (verb.IsReflexive ? " się" : string.Empty);
            return status.NeedsWork ? caption + "  [" + status.Summary + "]" : caption;
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
