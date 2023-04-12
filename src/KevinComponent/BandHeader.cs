using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shell;

namespace KevinComponent
{
	[TemplatePart(Name = RightHeaderGripperPartName, Type = typeof(BandHeaderGripper))]
	public sealed class BandHeader : ButtonBase
	{
		static BandHeader()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BandHeader), new FrameworkPropertyMetadata(typeof(BandHeader)));
			WidthProperty.OverrideMetadata(typeof(BandHeader), new FrameworkPropertyMetadata(double.NaN));
		}

		internal BandHeader(Band ownerBand)
		{
			OwnerBand = ownerBand;
			Width = ownerBand.Width;
		}

		#region Constants

		public const string RightHeaderGripperPartName = "PART_RightHeaderGripper";

		#endregion

		#region Dependency Properties

		private static readonly DependencyProperty SortDirectionProperty =
			DependencyProperty.Register(
				"SortDirection",
				typeof(ListSortDirection?),
				typeof(BandHeader),
				new FrameworkPropertyMetadata(null, OnSortDirectionChanged));

		private static void OnSortDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is FrameworkElement element)
				Utils.UpdateVisualState(element, true);
		}

		#endregion

		#region Private Variables

		BandHeaderGripper _rightGripper;

		#endregion

		#region Public Properties

		public Band OwnerBand { get; }

		public ListSortDirection? SortDirection
		{
			get => (ListSortDirection?)GetValue(SortDirectionProperty);
			set => SetValue(SortDirectionProperty, value);
		}

		#endregion

		#region Private Methods

		private void AttachRightGripperEventHandlers(BandHeaderGripper gripper)
		{
			if (gripper != null)
			{
				DetachRightGripperEventHandlers(gripper);

				gripper.DragStarted += OnRightGripperDragStarted;
				gripper.DragDelta += OnRightGripperDragDelta;
				gripper.DragCompleted += OnRightGripperDragCompleted;
				gripper.MouseDoubleClick += OnRightGripperDoubleClicked;
			}
		}

		private void DetachRightGripperEventHandlers(BandHeaderGripper gripper)
		{
			gripper.DragStarted -= OnRightGripperDragStarted;
			gripper.DragDelta -= OnRightGripperDragDelta;
			gripper.DragCompleted -= OnRightGripperDragCompleted;
			gripper.MouseDoubleClick -= OnRightGripperDoubleClicked;
		}

		#endregion

		#region Private EventHandlers

		private void OnRightGripperDragStarted(object sender, DragStartedEventArgs e) { }

		private void OnRightGripperDragDelta(object sender, DragDeltaEventArgs e)
		{
			if (OwnerBand != null)
			{
				var newValue = Math.Max(OwnerBand.MinWidth, ActualWidth + e.HorizontalChange);
				OwnerBand.Width = newValue;
			}
		}

		private void OnRightGripperDragCompleted(object sender, DragCompletedEventArgs e) { }

		private void OnRightGripperDoubleClicked(object sender, MouseButtonEventArgs e)
		{
			// TODO : Auto Fit Width To BandHeaders.
			// not developed yet.
			if (OwnerBand != null)
			{
				var rightBand = OwnerBand;
				while (rightBand.Bands.Count > 0)
					rightBand = rightBand.Bands[rightBand.Bands.Count - 1];

				//rightBand.Width = double.NaN;
			}
		}

		#endregion

		#region Public Override Methods

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_rightGripper = GetTemplateChild(RightHeaderGripperPartName) as BandHeaderGripper;
			if (_rightGripper != null)
				AttachRightGripperEventHandlers(_rightGripper);
		}

		#endregion

		#region Protected Override Methods

		protected override void OnClick()
		{
			base.OnClick();
			OwnerBand.PerformSort();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == ActualWidthProperty)
			{
				var newValue = (double)e.NewValue;
				if (OwnerBand != null && OwnerBand.SyncDataGridColumn != null)
					OwnerBand.SyncDataGridColumn.Width = new DataGridLength(newValue);
			}
		}

		#endregion
	}
}
