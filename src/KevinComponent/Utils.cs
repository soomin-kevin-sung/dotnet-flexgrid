using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Markup.Primitives;
using System.Xml.Linq;
using System.Reflection;

namespace KevinComponent
{
	public static class Utils
	{
		public static T? FindVisualChild<T>(DependencyObject d) where T : DependencyObject
		{
			int numOfChildren = VisualTreeHelper.GetChildrenCount(d);

			for (int i = 0; i < numOfChildren; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(d, i);

				if (child is T t)
				{
					return t;
				}
				else
				{
					var childOfChild = FindVisualChild<T>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			return null;
		}

		public static object? FindVisualChild(DependencyObject d, string name)
		{
			int numOfChildren = VisualTreeHelper.GetChildrenCount(d);

			for (int i = 0; i < numOfChildren; i++)
			{
				var child = VisualTreeHelper.GetChild(d, i);

				if (child is FrameworkElement element && element.Name == name)
				{
					return child;
				}
				else
				{
					var childOfChild = FindVisualChild(child, name);
					if (childOfChild != null)
						return childOfChild;
				}
			}

			return null;
		}

		public static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
		{
			var parentObject = GetParentObject(child);
			if (parentObject == null)
				return null;

			if (parentObject is T parent)
				return parent;
			else
				return FindVisualParent<T>(parentObject);
		}

		private static DependencyObject? GetParentObject(DependencyObject child)
		{
			if (child == null)
				return null;

			if (child is ContentElement contentElement)
			{
				DependencyObject parent = ContentOperations.GetParent(contentElement);
				if (parent != null)
					return parent;

				return contentElement is FrameworkContentElement fce ? fce.Parent : null;
			}

			if (child is FrameworkElement frameworkElement)
			{
				DependencyObject parent = frameworkElement.Parent;
				if (parent != null)
					return parent;
			}

			return VisualTreeHelper.GetParent(child);
		}

		public static void UpdateVisualState(FrameworkElement element, bool useTransitions)
		{
			if (Validation.GetHasError(element))
			{
				if (Keyboard.FocusedElement == element)
					VisualStateManager.GoToState(element, "InvalidFocused", useTransitions);
				else
					VisualStateManager.GoToState(element, "InvalidUnfocused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(element, "Valid", useTransitions);
			}
		}

		public static (DependencyObject Element, DependencyProperty Property, T Binding)[] GetBindings<T>(DependencyObject d)
		{
			var result = new List<(DependencyObject, DependencyProperty, T)>();
			var numOfChildren = VisualTreeHelper.GetChildrenCount(d);
			
			for (int i = 0; i < numOfChildren; i++)
			{
				var child = VisualTreeHelper.GetChild(d, i);
				result.AddRange(GetBindings<T>(child));
			}
				
			var lve = d.GetLocalValueEnumerator();
			while (lve.MoveNext())
			{
				var current = lve.Current;
				if (BindingOperations.IsDataBound(d, current.Property))
				{
					if (BindingOperations.GetBindingBase(d, current.Property) is T binding)
						result.Add((d, current.Property, binding));
				}
			}

			return result.ToArray();
		}

		public static bool GetIndexerValue(object target, object[] parameters, out object? result)
		{
			result = null;

			var parameterTypes = parameters.Select(t => t?.GetType()).ToArray()!;
			var indexer = GetIndexProperty(target.GetType(), parameterTypes);
			if (indexer == null)
				return false;

			result = indexer.GetValue(target, parameters);
			return true;
		}

		private static PropertyInfo? GetIndexProperty(Type targetType, Type?[] parameterTypes)
		{
			var properties = from p in targetType.GetProperties()
							 where p.Name == "Item"
							 select p;

			foreach (var property in properties)
			{
				var requestParamTypes = property.GetIndexParameters().Select(t => t.ParameterType).ToArray();
				if (requestParamTypes.Length == parameterTypes.Length)
				{
					bool flag = true;
					for (int i = 0; i < requestParamTypes.Length; i++)
					{
						if (!requestParamTypes[i].IsAssignableFrom(parameterTypes[i]))
						{
							flag = false;
							break;
						}
					}

					// If there is no different type parameter.
					if (flag)
						return property;
				}
			}

			return null;
		}

		public static BindingBase CloneBinding(BindingBase bindingBase)
		{
			if (bindingBase is Binding binding)
				return CloneBinding(binding);

			if (bindingBase is MultiBinding multiBinding)
				return CloneBinding(multiBinding);
			
			if (bindingBase is PriorityBinding priorityBinding)
				return CloneBinding(priorityBinding);

			throw new NotSupportedException("Failed to clone binding");
		}

		private static Binding CloneBinding(Binding binding)
		{
			var result = new Binding
			{
				AsyncState = binding.AsyncState,
				BindingGroupName = binding.BindingGroupName,
				BindsDirectlyToSource = binding.BindsDirectlyToSource,
				Converter = binding.Converter,
				ConverterCulture = binding.ConverterCulture,
				ConverterParameter = binding.ConverterParameter,
				FallbackValue = binding.FallbackValue,
				IsAsync = binding.IsAsync,
				Mode = binding.Mode,
				NotifyOnSourceUpdated = binding.NotifyOnSourceUpdated,
				NotifyOnTargetUpdated = binding.NotifyOnTargetUpdated,
				NotifyOnValidationError = binding.NotifyOnValidationError,
				Path = binding.Path,
				StringFormat = binding.StringFormat,
				TargetNullValue = binding.TargetNullValue,
				UpdateSourceExceptionFilter = binding.UpdateSourceExceptionFilter,
				UpdateSourceTrigger = binding.UpdateSourceTrigger,
				ValidatesOnDataErrors = binding.ValidatesOnDataErrors,
				ValidatesOnExceptions = binding.ValidatesOnExceptions,
				XPath = binding.XPath,
			};

			if (binding.Source != null)
				result.Source = binding.Source;
			else if (binding.RelativeSource != null)
				result.RelativeSource = binding.RelativeSource;
			else if (binding.ElementName != null)
				result.ElementName = binding.ElementName;

			foreach (var validationRule in binding.ValidationRules)
				result.ValidationRules.Add(validationRule);

			return result;
		}

		private static MultiBinding CloneBinding(MultiBinding multiBinding)
		{
			var result = new MultiBinding
			{
				BindingGroupName = multiBinding.BindingGroupName,
				Converter = multiBinding.Converter,
				ConverterCulture = multiBinding.ConverterCulture,
				ConverterParameter = multiBinding.ConverterParameter,
				FallbackValue = multiBinding.FallbackValue,
				Mode = multiBinding.Mode,
				NotifyOnSourceUpdated = multiBinding.NotifyOnSourceUpdated,
				NotifyOnTargetUpdated = multiBinding.NotifyOnTargetUpdated,
				NotifyOnValidationError = multiBinding.NotifyOnValidationError,
				StringFormat = multiBinding.StringFormat,
				TargetNullValue = multiBinding.TargetNullValue,
				UpdateSourceExceptionFilter = multiBinding.UpdateSourceExceptionFilter,
				UpdateSourceTrigger = multiBinding.UpdateSourceTrigger,
				ValidatesOnDataErrors = multiBinding.ValidatesOnDataErrors,
				ValidatesOnExceptions = multiBinding.ValidatesOnDataErrors,
			};

			foreach (var validationRule in multiBinding.ValidationRules)
				result.ValidationRules.Add(validationRule);

			foreach (var childBinding in multiBinding.Bindings)
				result.Bindings.Add(CloneBinding(childBinding));

			return result;
		}

		private static PriorityBinding CloneBinding(PriorityBinding priorityBinding)
		{
			var result = new PriorityBinding
			{
				BindingGroupName = priorityBinding.BindingGroupName,
				FallbackValue = priorityBinding.FallbackValue,
				StringFormat = priorityBinding.StringFormat,
				TargetNullValue = priorityBinding.TargetNullValue,
			};

			foreach (var childBinding in priorityBinding.Bindings)
			{
				result.Bindings.Add(CloneBinding(childBinding));
			}

			return result;
		}

		public static bool IsEqualValue(object a, object b)
		{
			if (a == null && b == null)
				return true;

			if (a == null)
				return b.Equals(a);

			return a.Equals(b);
		}
	}
}
