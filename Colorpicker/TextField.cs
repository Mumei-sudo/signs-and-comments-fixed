using System;
using UnityEngine;
using Verse;

namespace Dark.Signs.Colorpicker
{
	public class TextField<T>
	{
		public TextField(T value, string id, Action<T> callback, Func<string, T> parser = null, Func<string, bool> validator = null, Func<T, string> toString = null, bool spinner = false)
		{
			this._value = value;
			this._id = id;
			this._temp = value.ToString();
			this._callback = callback;
			this._validator = validator;
			this._parser = parser;
			this._toString = toString;
			this._spinner = spinner;
		}

		public T Value
		{
			get => this._value;
			set
			{
				this._value = value;
				Func<T, string> toString = this._toString;
				this._temp = (((toString != null) ? toString(value) : null) ?? value.ToString());
			}
		}

		public static TextField<float> Float01(float value, string id, Action<float> callback) => new TextField<float>(value, id, callback, new Func<string, float>(float.Parse), new Func<string, bool>(TextField<T>.Validate01), (float f) => TextField<T>.Round(f, 2).ToString(), true);


		public static TextField<string> Hex(string value, string id, Action<string> callback) => new TextField<string>(value, id, callback, (string hex) => hex, new Func<string, bool>(TextField<T>.ValidateHex), null, false);

		public void Draw(Rect rect)
		{
			Func<string, bool> validator = this._validator;
			GUI.color = ((validator == null || validator(this._temp)) ? Color.white : Color.red);
			GUI.SetNextControlName(this._id);
			string text = Widgets.TextField(rect, this._temp);
			GUI.color = Color.white;
			if (text != this._temp)
			{
				this._temp = text;
				Func<string, bool> validator2 = this._validator;
				if (validator2 == null || validator2(this._temp))
				{
					this._value = this._parser(this._temp);
					Action<T> callback = this._callback;
					if (callback != null)
					{
						callback(this._value);
					}
				}
			}
		}

		private static bool Validate01(string value) => float.TryParse(value, out var num) && num >= 0f && num <= 1f;



		private static bool ValidateHex(string value) => ColorUtility.TryParseHtmlString(value, out var color);

		private static float Round(float value, int digits = 2)
		{
			var num = Mathf.Pow(10f, (float)digits);
			return (float)Mathf.RoundToInt(value * num) / num;
		}



		private T _value;
		private readonly string _id;
		private string _temp;
		private readonly Func<string, bool> _validator;
		private readonly Func<string, T> _parser;
		private readonly Func<T, string> _toString;
		private readonly Action<T> _callback;
		private readonly bool _spinner;
	}
}
