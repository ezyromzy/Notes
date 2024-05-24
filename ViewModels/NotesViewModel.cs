using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Notes.Models;
using Notes.Views;

namespace Notes.ViewModels
{
    internal class NotesViewModel : IQueryAttributable
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        public NotesViewModel()
        {
            AllNotes = new ObservableCollection<NoteViewModel>(Note.LoadAll().Select(n => new NoteViewModel(n)));
            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(NotePage));
        }

        private async Task SelectNoteAsync(NoteViewModel note)
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(NotePage)}?load={note.Identifier}");
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteId = query["deleted"].ToString();
                NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                if (matchedNote != null)
                    AllNotes.Remove(matchedNote);                  
            }
            else if (query.ContainsKey("saved"))
            {
                string noteId = query["saved"].ToString();
                NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                if (matchedNote != null)
                    matchedNote.Reload();
                else
                    AllNotes.Add(new NoteViewModel(Note.Load(noteId)));
            }
        }
    }
}
