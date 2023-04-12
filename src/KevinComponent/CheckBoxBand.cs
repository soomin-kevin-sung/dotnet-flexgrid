using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KevinComponent
{
	public sealed class CheckBoxBand : Band
	{
		static CheckBoxBand()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckBoxBand), new FrameworkPropertyMetadata(typeof(CheckBoxBand)));
		}

		public CheckBoxBand() { }

		internal CheckBoxBand(VirtualCheckBoxBand virtualBand) : base(virtualBand)
		{
			CheckBinding = virtualBand.CheckBinding;
			IsThreeState = virtualBand.IsThreeState;
			CheckBoxHorizontalAlignment = virtualBand.CheckBoxHorizontalAlignment;

			if (virtualBand.ContentBinding != null)
				BindingOperations.SetBinding(this, ContentProperty, virtualBand.ContentBinding);
			else
				Content = virtualBand.Content;
		}

		#region Dependency Properties

		public static readonly DependencyProperty IsThreeStateProperty =
			DependencyProperty.Register(
				"IsThreeState",
				typeof(bool),
				typeof(CheckBoxBand),
				new FrameworkPropertyMetadata(false));

		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register(
				"Content",
				typeof(object),
				typeof(CheckBoxBand),
				new FrameworkPropertyMetadata(null));

		public static readonly DependencyProperty CheckBoxHorizontalAlignmentProperty =
			DependencyProperty.Register(
				"CheckBoxHorizontalAlignment",
				typeof(HorizontalAlignment),
				typeof(CheckBoxBand),
				new FrameworkPropertyMetadata(HorizontalAlignment.Center));

		#endregion

		#region Private Variables

		BindingBase _checkBinding;

		#endregion

		#region Public Properties

		public BindingBase CheckBinding
		{
			get => _checkBinding;
			set
			{
				if (_checkBinding != value)
				{
					_checkBinding = value;
					OwnerFlexGrid.RefreshCells(this);
				}
			}
		}

		public bool IsThreeState
		{
			get => (bool)GetValue(IsThreeStateProperty);
			set => SetValue(IsThreeStateProperty, value);
		}

		public object Content
		{
			get => GetValue(ContentProperty);
			set => SetValue(ContentProperty, value);
		}

		public HorizontalAlignment CheckBoxHorizontalAlignment
		{
			get => (HorizontalAlignment)GetValue(CheckBoxHorizontalAlignmentProperty);
			set => SetValue(CheckBoxHorizontalAlignmentProperty, value);
		}

		#endregion

		#region Private Methods

		private void SetCheckBoxIsChecked(CheckBox checkBox)
		{
			// Set IsChecked.
			if (CheckBinding != null)
				BindingOperations.SetBinding(checkBox, CheckBox.IsCheckedProperty, CheckBinding);
			else
				BindingOperations.ClearBinding(checkBox, CheckBox.IsCheckedProperty);
		}

		private void SetCheckBoxContent(CheckBox checkBox)
		{
			// Set Content.
			var extBinding = BindingOperations.GetBinding(this, ContentProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(checkBox, CheckBox.ContentProperty, extBinding);
			}
			else
			{
				var contentBinding = new Binding("Content") { Source = this };
				BindingOperations.SetBinding(checkBox, CheckBox.ContentProperty, contentBinding);
			}
		}

		private void SetCheckBoxIsThreeState(CheckBox checkBox)
		{
			var extBinding = BindingOperations.GetBinding(this, IsThreeStateProperty);
			if (extBinding != null)
			{
				BindingOperations.SetBinding(checkBox, CheckBox.IsThreeStateProperty, extBinding);
			}
			else
			{
				var isThreeStateBinding = new Binding("IsThreeState") { Source = this };
				BindingOperations.SetBinding(checkBox, CheckBox.IsThreeStateProperty, isThreeStateBinding);
			}
		}

		#endregion

		#region Protected Override Methods

		protected override void OnCellTemplateLoaded(ContentPresenter contentPresenter)
		{
			base.OnCellTemplateLoaded(contentPresenter);

			var checkBox = Utils.FindVisualChild<CheckBox>(contentPresenter);
			if (checkBox != null)
			{
				SetCheckBoxIsThreeState(checkBox);
				SetCheckBoxIsChecked(checkBox);
				SetCheckBoxContent(checkBox);

				// Set Horizontal Alignment.
				checkBox.HorizontalAlignment = CheckBoxHorizontalAlignment;
				checkBox.VerticalAlignment = VerticalAlignment.Center;
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == IsThreeStateProperty)
				OwnerFlexGrid.RefreshCells(this);
			else if (e.Property == ContentProperty)
				OwnerFlexGrid.RefreshCells(this);
		}

		#endregion
	}
}
