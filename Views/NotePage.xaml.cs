namespace Notes.Views;

public partial class NotePage : ContentPage
{
	string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");

	public NotePage()
	{
		InitializeComponent();

		string appDataPath = FileSystem.AppDataDirectory;
		string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

		LoadNote(Path.Combine(appDataPath, randomFileName));
	}

	private void LoadNote(string fileName)
	{
		Models.Note noteModel = new Models.Note();
		noteModel.FileName = fileName;

		if (File.Exists(fileName))
		{
			noteModel.Date = File.GetCreationTime(fileName);
			noteModel.Text = File.ReadAllText(fileName);
		}

		BindingContext = noteModel;
	}

	private void SaveButton_Clicked(object sender, EventArgs e)
	{
        File.WriteAllText(_fileName, TextEditor.Text);
    }

	private void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (File.Exists(_fileName))
			File.Delete(_fileName);

		TextEditor.Text = string.Empty;
	}
}