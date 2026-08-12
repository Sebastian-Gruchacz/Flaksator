using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SharpDevs.Fleksator;

namespace Flaksator.Edit
{
    public sealed class FormGenerationFilters : Form
    {
        private readonly CheckedListBox nouns = new CheckedListBox();
        private readonly CheckedListBox adjectives = new CheckedListBox();
        private readonly CheckedListBox verbs = new CheckedListBox();

        public FormGenerationFilters()
        {
            this.Text = "Wykluczenia generatora";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(650, 500);
            this.Size = new Size(760, 600);
            this.InitializeLayout();
        }

        private void InitializeLayout()
        {
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label
            {
                Text = "Zaznaczone kategorie nie będą używane podczas generowania. " +
                    "Ustawienie dotyczy bieżącej sesji i nie zmienia definicji wersów.",
                AutoSize = true,
                MaximumSize = new Size(700, 0)
            }, 0, 0);

            TabControl tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(this.CreatePage("Rzeczowniki", this.nouns,
                WordCategories.Categories.NounCategories,
                GenerationCategoryFilter.ExcludedNounCategories));
            tabs.TabPages.Add(this.CreatePage("Przymiotniki", this.adjectives,
                WordCategories.Categories.AdjectiveCategories,
                GenerationCategoryFilter.ExcludedAdjectiveCategories));
            tabs.TabPages.Add(this.CreatePage("Czasowniki", this.verbs,
                WordCategories.Categories.VerbCategories,
                GenerationCategoryFilter.ExcludedVerbCategories));
            layout.Controls.Add(tabs, 0, 1);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft
            };
            Button apply = new Button { Text = "Zastosuj", AutoSize = true };
            apply.Click += this.Apply_Click;
            Button cancel = new Button { Text = "Anuluj", AutoSize = true, DialogResult = DialogResult.Cancel };
            Button clear = new Button { Text = "Wyczyść wszystkie", AutoSize = true };
            clear.Click += delegate
            {
                this.UncheckAll(this.nouns);
                this.UncheckAll(this.adjectives);
                this.UncheckAll(this.verbs);
            };
            buttons.Controls.Add(apply);
            buttons.Controls.Add(cancel);
            buttons.Controls.Add(clear);
            layout.Controls.Add(buttons, 0, 2);
            this.Controls.Add(layout);
            this.AcceptButton = apply;
            this.CancelButton = cancel;
        }

        private TabPage CreatePage(string title, CheckedListBox list,
            Dictionary<int, string> categories, ICollection<int> exclusions)
        {
            TabPage page = new TabPage(title);
            list.Dock = DockStyle.Fill;
            list.CheckOnClick = true;
            List<int> identifiers = new List<int>(categories.Keys);
            identifiers.Sort();
            foreach (int identifier in identifiers)
                list.Items.Add(new CategoryOption(identifier, categories[identifier]),
                    exclusions.Contains(identifier));
            if (identifiers.Count == 0)
            {
                list.Items.Add("Brak kategorii w słowniku.");
                list.Enabled = false;
            }
            page.Controls.Add(list);
            return page;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            this.CopyChecked(this.nouns, GenerationCategoryFilter.ExcludedNounCategories);
            this.CopyChecked(this.adjectives, GenerationCategoryFilter.ExcludedAdjectiveCategories);
            this.CopyChecked(this.verbs, GenerationCategoryFilter.ExcludedVerbCategories);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CopyChecked(CheckedListBox source, HashSet<int> target)
        {
            target.Clear();
            foreach (object item in source.CheckedItems)
            {
                CategoryOption option = item as CategoryOption;
                if (option != null)
                    target.Add(option.Id);
            }
        }

        private void UncheckAll(CheckedListBox list)
        {
            for (int i = 0; i < list.Items.Count; i++)
                list.SetItemChecked(i, false);
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
