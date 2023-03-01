using KevinComponent.Demo.App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KevinComponent.Demo.App.Views
{
	/// <summary>
	/// VirtualBandSampleView.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class VirtualBandSampleView : Window
	{
		public VirtualBandSampleView()
		{
			InitializeComponent();
			_people = Person.SampleData;
			_subjects = Subject.SampleData;

			fgdPeople.ItemsSource = _people;
			fgdSubjects.ItemsSource = _subjects;
			flexGrid.ItemsSource = _people;
			vbandSubjects.ItemsSource = _subjects;

			// Set Sample Subject Scores
			var rand = new Random();
			foreach (var person in _people)
			{
				foreach (var subject in _subjects)
					person[subject].Value = rand.Next(0, 100);
			}
		}

		#region Private Variables

		ObservableCollection<Person> _people;
		ObservableCollection<Subject> _subjects;

		#endregion

		#region Private Event Handlers

		private void AddPerson_Click(object sender, RoutedEventArgs e)
		{
			_people.Add(new Person("NEW PERSON"));
		}

		private void RemovePerson_Click(object sender, RoutedEventArgs e)
		{
			var items = fgdPeople.SelectedItems;
			foreach (Person item in items.Cast<Person>().ToArray())
				_people.Remove(item);
		}

		#endregion

		private void AddSubject_Click(object sender, RoutedEventArgs e)
		{
			_subjects.Add(new Subject("NEW SUBJECT"));
		}

		private void RemoveSubject_Click(object sender, RoutedEventArgs e)
		{
			var items = fgdSubjects.SelectedItems;
			foreach (Subject item in items.Cast<Subject>().ToArray())
				_subjects.Remove(item);
		}
	}
}
