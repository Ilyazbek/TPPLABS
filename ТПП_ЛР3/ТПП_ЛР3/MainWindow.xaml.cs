using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;

namespace NotesApp
{
    public partial class MainWindow : Window
    {
        private List<Note> notes = new List<Note>();
        private string filePath = "notes.txt";
        private List<Note> filteredNotes = new List<Note>();

        public MainWindow()
        {
            InitializeComponent();
            LoadNotes();
        }

        public class Note
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }

        private void LoadNotes()
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                notes = lines.Select(line => line.Split('|')).Where(parts => parts.Length == 2)
                             .Select(parts => new Note { Title = parts[0], Content = parts[1] }).ToList();
            }

            filteredNotes = new List<Note>(notes);

            UpdateNotesListBox(); 
        }


        private void UpdateNotesListBox()
        {
            NotesListBox.Items.Clear();
            foreach (var note in filteredNotes)
            {
                NotesListBox.Items.Add(note.Title);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string title = NewNoteTitleTextBox.Text.Trim();
            string content = NewNoteTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(content))
            {
                var newNote = new Note { Title = title, Content = content };

                notes.Add(newNote);

                if (newNote.Title.ToLower().Contains(SearchBox.Text.ToLower()) ||
                    newNote.Content.ToLower().Contains(SearchBox.Text.ToLower()))
                {
                    filteredNotes.Add(newNote); 
                }

                UpdateNotesListBox(); 

                NewNoteTitleTextBox.Clear();
                NewNoteTextBox.Clear();

                SaveNotes();
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null && filteredNotes.Count > 0)
            {
                int selectedIndex = NotesListBox.SelectedIndex;

                var selectedNote = filteredNotes[selectedIndex];

                selectedNote.Title = NewNoteTitleTextBox.Text;
                selectedNote.Content = NewNoteTextBox.Text;

                var originalNote = notes.FirstOrDefault(n => n.Title == selectedNote.Title && n.Content == selectedNote.Content);
                if (originalNote != null)
                {
                    originalNote.Title = selectedNote.Title;
                    originalNote.Content = selectedNote.Content;
                }

                UpdateNotesListBox();

                NewNoteTitleTextBox.Clear();
                NewNoteTextBox.Clear();

                SaveNotes(); 
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null)
            {
                int selectedIndex = NotesListBox.SelectedIndex;
                var selectedNote = filteredNotes[selectedIndex];

                notes.Remove(selectedNote);

                filteredNotes.Remove(selectedNote);

                UpdateNotesListBox();

                NewNoteTitleTextBox.Clear();
                NewNoteTextBox.Clear();

                SaveNotes();
            }
        }

        private void SaveNotes()
        {
            var lines = notes.Select(note => note.Title + "|" + note.Content).ToArray();
            File.WriteAllLines(filePath, lines);
        }

        private void NotesListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (NotesListBox.SelectedItem != null && filteredNotes.Count > 0)
            {
                int selectedIndex = NotesListBox.SelectedIndex;
                NewNoteTitleTextBox.Text = filteredNotes[selectedIndex].Title;
                NewNoteTextBox.Text = filteredNotes[selectedIndex].Content;
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string searchQuery = SearchBox.Text.ToLower(); 
            filteredNotes = notes.Where(n =>
                                        n.Title.ToLower().Contains(searchQuery) ||
                                        n.Content.ToLower().Contains(searchQuery))
                                  .ToList();

            NotesListBox.Items.Clear();

            foreach (var note in filteredNotes)
            {
                NotesListBox.Items.Add(note.Title);
            }

            NewNoteTitleTextBox.Clear();
            NewNoteTextBox.Clear();
        }


        private void ChangeWindowBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            var result = colorDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.Background = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }

        private void ChangeNoteBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            var result = colorDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                NewNoteTextBox.Background = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }
    }
}
