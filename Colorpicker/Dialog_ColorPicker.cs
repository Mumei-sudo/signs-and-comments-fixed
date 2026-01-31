using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Dark.Signs.Colorpicker
{
    public class Dialog_ColourPicker : Window
	{
		public Dialog_ColourPicker(Color color, Action<Color> callback = null, Vector2? position = null) : base(null)
		{
			this.absorbInputAroundWindow = true;
			this.closeOnClickedOutside = true;
			this._callback = callback;
			this._initialPosition = position;
			this.curColour = color;
			this.tempColour = color;
			this.HueField = TextField<float>.Float01(this.H, "Hue", delegate(float h)
			{
				this.H = h;
			});
			this.SaturationField = TextField<float>.Float01(this.S, "Saturation", delegate(float s)
			{
				this.S = s;
			});
			this.ValueField = TextField<float>.Float01(this.V, "Value", delegate(float v)
			{
				this.V = v;
			});
			this.Alpha1Field = TextField<float>.Float01(this.A, "Alpha1", delegate(float a)
			{
				this.A = a;
			});
			this.RedField = TextField<float>.Float01(color.r, "Red", delegate(float r)
			{
				this.R = r;
			});
			this.GreenField = TextField<float>.Float01(color.r, "Green", delegate(float g)
			{
				this.G = g;
			});
			this.BlueField = TextField<float>.Float01(color.r, "Blue", delegate(float b)
			{
				this.B = b;
			});
			this.Alpha2Field = TextField<float>.Float01(this.A, "Alpha2", delegate(float a)
			{
				this.A = a;
			});
			this.HexField = TextField<string>.Hex(this.Hex, "Hex", delegate(string hex)
			{
				this.Hex = hex;
			});
			this.textFieldIds = new List<string>(new string[]
			{
				"Hue",
				"Saturation",
				"Value",
				"Alpha1",
				"Red",
				"Green",
				"Blue",
				"Alpha2",
				"Hex"
			});
			this.NotifyRGBUpdated();
		}

		
		public float A
		{
			get => this.tempColour.a;
			set
			{
				Color tempColour = this.tempColour;
				tempColour.a = Mathf.Clamp(value, 0f, 1f);
				this.tempColour = tempColour;
				this.NotifyRGBUpdated();
			}
		}
        
		public Texture2D AlphaPickerBG
		{
			get
			{
				if (this._alphaPickerBG == null)
				{
					this.CreateAlphaPickerBG();
				}
				return this._alphaPickerBG;
			}
		}
        
		public float B
		{
			get  => this.tempColour.b;
			
			set
			{
				Color tempColour = this.tempColour;
				tempColour.b = Mathf.Clamp(value, 0f, 1f);
				this.tempColour = tempColour;
				this.NotifyRGBUpdated();
			}
		}
        
		public Texture2D ColourPickerBG
		{
			get
			{
				if (this._colourPickerBG == null)
				{
					this.CreateColourPickerBG();
				}
				return this._colourPickerBG;
			}
		}
        
		public float G
		{
			get => this.tempColour.g;
			
			set
			{
				Color tempColour = this.tempColour;
				tempColour.g = Mathf.Clamp(value, 0f, 1f);
				this.tempColour = tempColour;
				this.NotifyRGBUpdated();
			}
		}
        
		public float H
		{
			get => this._h;
			set
			{
				this._h = Mathf.Clamp(value, 0f, 1f);
				this.NotifyHSVUpdated();
				this.CreateColourPickerBG();
				this.CreateAlphaPickerBG();
			}
		}


		public string Hex
		{
			get =>  "#" + ColorUtility.ToHtmlStringRGBA(this.tempColour);
			set
			{
				this._hex = value;
				this.NotifyHexUpdated();
			}
		}


		public Texture2D HuePickerBG
		{
			get
			{
				if (this._huePickerBG == null)
				{
					this.CreateHuePickerBG();
				}
				return this._huePickerBG;
			}
		}

		public Vector2 InitialPosition
		{
			get
			{
				Vector2? initialPosition = this._initialPosition;
				if (initialPosition == null)
				{
					return new Vector2((float)UI.screenWidth - this.InitialSize.x, (float)UI.screenHeight - this.InitialSize.y) / 2f;
				}
				return initialPosition.GetValueOrDefault();
			}
		}


		public override Vector2 InitialSize => new Vector2((float)this._pickerSize + 3f * this._margin + (float)(2 * this._sliderWidth) + (float)(2 * this._previewSize) + 36f, (float)this._pickerSize + 36f);
        
		public Texture2D PickerAlphaBG
		{
			get
			{
				if (this._pickerAlphaBG == null)
				{
					this.CreateAlphaBG(ref this._pickerAlphaBG, this._pickerSize, this._pickerSize);
				}
				return this._pickerAlphaBG;
			}
		}
        
		public Texture2D PreviewAlphaBG
		{
			get
			{
				if (this._previewAlphaBG == null)
				{
					this.CreateAlphaBG(ref this._previewAlphaBG, this._previewSize, this._previewSize);
				}
				return this._previewAlphaBG;
			}
		}

		public Texture2D PreviewBG
		{
			get
			{
				if (this._previewBG == null)
				{
					this.CreatePreviewBG(ref this._previewBG, this.curColour);
				}
				return this._previewBG;
			}
		}

		public float R
		{
			get => this.tempColour.r;
			set
			{
				Color tempColour = this.tempColour;
				tempColour.r = Mathf.Clamp(value, 0f, 1f);
				this.tempColour = tempColour;
				this.NotifyRGBUpdated();
			}
		}

		public float S
		{
			get => this._s;
			set
			{
				this._s = Mathf.Clamp(value, 0f, 1f);
				this.NotifyHSVUpdated();
				this.CreateAlphaPickerBG();
			}
		}

		public Texture2D SliderAlphaBG
		{
			get
			{
				if (this._sliderAlphaBG == null)
				{
					this.CreateAlphaBG(ref this._sliderAlphaBG, this._sliderWidth, this._pickerSize);
				}
				return this._sliderAlphaBG;
			}
		}

		public Color tempColour
		{
			get => this._tempColour;
			
			set
			{
				this._tempColour = value;
				if (this.autoApply || this.minimalistic)
				{
					this.SetColor();
				}
			}
		}

		public Texture2D TempPreviewBG
		{
			get
			{
				if (this._tempPreviewBG == null)
				{
					this.CreatePreviewBG(ref this._tempPreviewBG, this.tempColour);
				}
				return this._tempPreviewBG;
			}
		}
        
		public float UnitsPerPixel
		{
			get
			{
				if (this._unitsPerPixel == 0f)
				{
					this._unitsPerPixel = 1f / (float)this._pickerSize;
				}
				return this._unitsPerPixel;
			}
		}

		public float V
		{
			get => this._v;
			
			set
			{
				this._v = Mathf.Clamp(value, 0f, 1f);
				this.NotifyHSVUpdated();
				this.CreateAlphaPickerBG();
			}
		}

		// Token: 0x0600001F RID: 31
		public void AlphaAction(float pos)
		{
			this.A = 1f - this.UnitsPerPixel * pos;
			this._alphaPosition = pos;
		}

		private void CreateAlphaBG(ref Texture2D bg, int width, int height)
		{
            // width = _sliderwidth = 15
            // height = _pickerSize = 300
            Texture2D texture2D = new Texture2D(width, height);
            // _alphaBGBlockSize = 10
            Color[] array = new Color[this._alphaBGBlockSize * this._alphaBGBlockSize];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this._alphaBGColorA; // Color.white
            }
			Color[] array2 = new Color[this._alphaBGBlockSize * this._alphaBGBlockSize];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = this._alphaBGColorB; // Color(0.85f, 0.85f, 0.85f) Grey
            }
			int num = 0;
			for (int k = 0; k < width; k += this._alphaBGBlockSize)
			{
				int num2 = num;
				for (int l = 0; l < height; l += this._alphaBGBlockSize)
				{
					texture2D.SetPixels(k, l, this._alphaBGBlockSize, this._alphaBGBlockSize, (num2 % 2 == 0) ? array : array2);
					num2++;
				}
				num++;
			}
			texture2D.Apply();
			this.SwapTexture(ref bg, texture2D);
		}

		private void CreateAlphaPickerBG()
		{
			Texture2D texture2D = new Texture2D(1, this._pickerSize);
			int pickerSize = this._pickerSize;
			float num = 1f / (float)pickerSize;
			for (int i = 0; i < pickerSize; i++)
			{
				texture2D.SetPixel(0, i, new Color(this.tempColour.r, this.tempColour.g, this.tempColour.b, (float)i * num));
			}
			texture2D.Apply();
			this.SwapTexture(ref this._alphaPickerBG, texture2D);
		}

		private void CreateColourPickerBG()
		{
			int pickerSize = this._pickerSize;
			int pickerSize2 = this._pickerSize;
			float unitsPerPixel = this.UnitsPerPixel;
			float unitsPerPixel2 = this.UnitsPerPixel;
			Texture2D texture2D = new Texture2D(pickerSize, pickerSize2);
			for (int i = 0; i < pickerSize; i++)
			{
				for (int j = 0; j < pickerSize2; j++)
				{
					float s = (float)i * unitsPerPixel;
					float v = (float)j * unitsPerPixel2;
					texture2D.SetPixel(i, j, Dialog_ColourPicker.HSVAToRGB(this.H, s, v, this.A));
				}
			}
			texture2D.Apply();
			this.SwapTexture(ref this._colourPickerBG, texture2D);
		}

		private void CreateHuePickerBG()
		{
			Texture2D texture2D = new Texture2D(1, this._pickerSize);
			int pickerSize = this._pickerSize;
			float num = 1f / (float)pickerSize;
			for (int i = 0; i < pickerSize; i++)
			{
				texture2D.SetPixel(0, i, Color.HSVToRGB(num * (float)i, 1f, 1f));
			}
			texture2D.Apply();
			this.SwapTexture(ref this._huePickerBG, texture2D);
		}
        
		public void CreatePreviewBG(ref Texture2D bg, Color col)
		{
			this.SwapTexture(ref bg, SolidColorMaterials.NewSolidColorTexture(col));
		}

		

		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect.xMin, inRect.yMin, (float)this._pickerSize, (float)this._pickerSize);
			Rect rect2 = new Rect(rect.xMax + this._margin, inRect.yMin, (float)this._sliderWidth, (float)this._pickerSize);
			Rect rect3 = new Rect(rect2.xMax + this._margin, inRect.yMin, (float)this._sliderWidth, (float)this._pickerSize);
			Rect position = new Rect(rect3.xMax + this._margin, inRect.yMin, (float)this._previewSize, (float)this._previewSize);
			Rect rect6 = new Rect(position.xMax, inRect.yMin, (float)this._previewSize, (float)this._previewSize);
			Rect doneRect = new Rect(rect3.xMax + this._margin, inRect.yMax - this._buttonHeight, (float)(this._previewSize * 2), this._buttonHeight);
			Rect setRect = new Rect(rect3.xMax + this._margin, inRect.yMax - 2f * this._buttonHeight - this._margin, (float)this._previewSize - this._margin / 2f, this._buttonHeight);
			Rect cancelRect = new Rect(setRect.xMax + this._margin, setRect.yMin, (float)this._previewSize - this._margin / 2f, this._buttonHeight);
			Rect hsvFieldRect = new Rect(rect3.xMax + this._margin, inRect.yMax - 2f * this._buttonHeight - 3f * this._fieldHeight - 4f * this._margin, (float)(this._previewSize * 2), this._fieldHeight);
			Rect rgbFieldRect = new Rect(rect3.xMax + this._margin, inRect.yMax - 2f * this._buttonHeight - 2f * this._fieldHeight - 3f * this._margin, (float)(this._previewSize * 2), this._fieldHeight);
			Rect hexRect = new Rect(rect3.xMax + this._margin, inRect.yMax - 2f * this._buttonHeight - 1f * this._fieldHeight - 2f * this._margin, (float)(this._previewSize * 2), this._fieldHeight);
			Rect canvas = new Rect(position.xMin, position.yMax + this._margin, (float)(this._previewSize * 2), (float)(this._recentSize * 2));
			GUI.DrawTexture(rect, this.PickerAlphaBG);
			GUI.DrawTexture(rect3, this.SliderAlphaBG);
			GUI.DrawTexture(position, this.PreviewAlphaBG);
			GUI.DrawTexture(rect6, this.PreviewAlphaBG);
			GUI.DrawTexture(rect, this.ColourPickerBG);
			GUI.DrawTexture(rect2, this.HuePickerBG);
			GUI.DrawTexture(rect3, this.AlphaPickerBG);
			GUI.DrawTexture(position, this.TempPreviewBG);
			GUI.DrawTexture(rect6, this.PreviewBG);
			if (Widgets.ButtonInvisible(rect6, true))
			{
				this.tempColour = this.curColour;
				this.NotifyRGBUpdated();
			}
			this.DrawRecent(canvas);
			Rect rect4 = new Rect(rect2.xMin - 3f, rect2.yMin + this._huePosition - (float)(this._handleSize / 2), (float)this._sliderWidth + 6f, (float)this._handleSize);
			Rect rect5 = new Rect(rect3.xMin - 3f, rect3.yMin + this._alphaPosition - (float)(this._handleSize / 2), (float)this._sliderWidth + 6f, (float)this._handleSize);
			Rect rect7 = new Rect(rect.xMin + this._position.x - (float)(this._handleSize / 2), rect.yMin + this._position.y - (float)(this._handleSize / 2), (float)this._handleSize, (float)this._handleSize);
			GUI.DrawTexture(rect4, this.TempPreviewBG);
			GUI.DrawTexture(rect5, this.TempPreviewBG);
			GUI.DrawTexture(rect7, this.TempPreviewBG);
			GUI.color = Color.gray;
			Widgets.DrawBox(rect4, 1, null);
			Widgets.DrawBox(rect5, 1, null);
			Widgets.DrawBox(rect7, 1, null);
			GUI.color = Color.white;
			if (Input.GetMouseButtonUp(0))
			{
				this._activeControl = Dialog_ColourPicker.controls.none;
			}
			this.DrawColourPicker(rect);
			this.DrawHuePicker(rect2);
			this.DrawAlphaPicker(rect3);
			this.DrawFields(hsvFieldRect, rgbFieldRect, hexRect);
			this.DrawButtons(doneRect, setRect, cancelRect);
			GUI.color = Color.white;
		}

		private void DrawAlphaPicker(Rect alphaRect)
		{
			if (Mouse.IsOver(alphaRect))
			{
				if (Input.GetMouseButtonDown(0))
				{
					this._activeControl = Dialog_ColourPicker.controls.alphaPicker;
				}
				if (Event.current.type == EventType.ScrollWheel)
				{
					this.A -= Event.current.delta.y * this.UnitsPerPixel;
					this._alphaPosition = Mathf.Clamp(this._alphaPosition + Event.current.delta.y, 0f, (float)this._pickerSize);
					Event.current.Use();
				}
				if (this._activeControl == Dialog_ColourPicker.controls.alphaPicker)
				{
					float pos = Event.current.mousePosition.y - alphaRect.yMin;
					this.AlphaAction(pos);
				}
			}
		}

		private void DrawButtons(Rect doneRect, Rect setRect, Rect cancelRect)
		{
			if (Widgets.ButtonText(doneRect, "OK", true, true, true, null))
			{
				this.SetColor();
				this.Close(true);
			}
			if (Widgets.ButtonText(setRect, "Apply", true, true, true, null))
			{
				this.SetColor();
			}
			if (Widgets.ButtonText(cancelRect, "Cancel", true, true, true, null))
			{
				this.Close(true);
			}
		}

		private void DrawColourPicker(Rect pickerRect)
		{
			if (Mouse.IsOver(pickerRect))
			{
				if (Input.GetMouseButtonDown(0))
				{
					this._activeControl = Dialog_ColourPicker.controls.colourPicker;
				}
				if (this._activeControl == Dialog_ColourPicker.controls.colourPicker)
				{
					Vector2 pos = Event.current.mousePosition - new Vector2(pickerRect.xMin, pickerRect.yMin);
					this.PickerAction(pos);
				}
			}
		}
        
		private void DrawFields(Rect hsvFieldRect, Rect rgbFieldRect, Rect hexRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = hsvFieldRect;
			rect.width /= 5f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.grey;
			Widgets.Label(rect, "HSV");
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			rect.x += rect.width;
			this.HueField.Draw(rect);
			rect.x += rect.width;
			this.SaturationField.Draw(rect);
			rect.x += rect.width;
			this.ValueField.Draw(rect);
			rect.x += rect.width;
			this.Alpha1Field.Draw(rect);
			rect = rgbFieldRect;
			rect.width /= 5f;
			Text.Font = GameFont.Tiny;
			GUI.color = Color.grey;
			Widgets.Label(rect, "RGB");
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			rect.x += rect.width;
			this.RedField.Draw(rect);
			rect.x += rect.width;
			this.GreenField.Draw(rect);
			rect.x += rect.width;
			this.BlueField.Draw(rect);
			rect.x += rect.width;
			this.Alpha2Field.Draw(rect);
			Text.Font = GameFont.Tiny;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(hexRect.xMin, hexRect.yMin, rect.width, hexRect.height), "HEX");
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			hexRect.xMin += rect.width;
			this.HexField.Draw(hexRect);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
			{
				string nameOfFocusedControl = GUI.GetNameOfFocusedControl();
				int num = this.textFieldIds.IndexOf(nameOfFocusedControl);
				GUI.FocusControl(this.textFieldIds[GenMath.PositiveMod(num + (Event.current.shift ? -1 : 1), this.textFieldIds.Count)]);
			}
		}
        
		private void DrawHuePicker(Rect hueRect)
		{
			if (Mouse.IsOver(hueRect))
			{
				if (Input.GetMouseButtonDown(0))
				{
					this._activeControl = Dialog_ColourPicker.controls.huePicker;
				}
				if (Event.current.type == EventType.ScrollWheel)
				{
					this.H -= Event.current.delta.y * this.UnitsPerPixel;
					this._huePosition = Mathf.Clamp(this._huePosition + Event.current.delta.y, 0f, (float)this._pickerSize);
					Event.current.Use();
				}
				if (this._activeControl == Dialog_ColourPicker.controls.huePicker)
				{
					float pos = Event.current.mousePosition.y - hueRect.yMin;
					this.HueAction(pos);
				}
			}
		}

		
		private void DrawRecent(Rect canvas)
		{
			int num = (int)(canvas.width / (float)this._recentSize);
			int num2 = (int)(canvas.height / (float)this._recentSize);
			int num3 = Math.Min(num * num2, this._recentColours.Count);
			GUI.BeginGroup(canvas);
			for (int i = 0; i < num3; i++)
			{
				int num4 = i % num;
				int num5 = i / num;
				Color color = this._recentColours[i];
				Rect rect = new Rect((float)(num4 * this._recentSize), (float)(num5 * this._recentSize), (float)this._recentSize, (float)this._recentSize);
				Widgets.DrawBoxSolid(rect, color);
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawBox(rect, 1, null);
				}
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.tempColour = color;
					this.NotifyRGBUpdated();
				}
			}
			GUI.EndGroup();
		}

		public static Color HSVAToRGB(float H, float S, float V, float A)
		{
			Color result = Color.HSVToRGB(H, S, V);
			result.a = A;
			return result;
		}

		public void HueAction(float pos)
		{
			this.H = 1f - this.UnitsPerPixel * pos;
			this._huePosition = pos;
		}

		public void NotifyHexUpdated()
		{
			if (ColorUtility.TryParseHtmlString(this._hex, out var tempColour))
			{
				this.tempColour = tempColour;
				this.NotifyRGBUpdated();
				this.RedField.Value = this.tempColour.r;
				this.GreenField.Value = this.tempColour.g;
				this.BlueField.Value = this.tempColour.b;
			}
		}
        
		public void NotifyHSVUpdated()
		{
			Color tempColour = Color.HSVToRGB(this.H, this.S, this.V);
			tempColour.a = this.A;
			this.tempColour = tempColour;
			this.CreatePreviewBG(ref this._tempPreviewBG, this.tempColour);
			this.SetPickerPositions();
			this.RedField.Value = this.tempColour.r;
			this.GreenField.Value = this.tempColour.g;
			this.BlueField.Value = this.tempColour.b;
			this.HueField.Value = this.H;
			this.SaturationField.Value = this.S;
			this.ValueField.Value = this.V;
			this.Alpha1Field.Value = this.A;
			this.Alpha2Field.Value = this.A;
			this.HexField.Value = this.Hex;
		}
        
		public void NotifyRGBUpdated()
		{
			Color.RGBToHSV(this.tempColour, out this._h, out this._s, out this._v);
			this.CreateColourPickerBG();
			this.CreateHuePickerBG();
			this.CreateAlphaPickerBG();
			this.CreatePreviewBG(ref this._tempPreviewBG, this.tempColour);
			this.SetPickerPositions();
			this.HueField.Value = this.H;
			this.SaturationField.Value = this.S;
			this.ValueField.Value = this.V;
			this.Alpha1Field.Value = this.A;
			this.Alpha2Field.Value = this.A;
			this.HexField.Value = this.Hex;
		}

		public override void OnAcceptKeyPressed()
		{
			base.OnAcceptKeyPressed();
			this.SetColor();
		}

		public void PickerAction(Vector2 pos)
		{
			this._s = this.UnitsPerPixel * pos.x;
			this._v = 1f - this.UnitsPerPixel * pos.y;
			this.CreateAlphaPickerBG();
			this.NotifyHSVUpdated();
			this._position = pos;
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.NotifyHSVUpdated();
		}
        
		public void SetColor()
		{
			this.curColour = this.tempColour;
			this._recentColours.Add(this.tempColour);
			Action<Color> callback = this._callback;
			if (callback != null)
			{
				callback(this.curColour);
			}
			this.CreatePreviewBG(ref this._previewBG, this.tempColour);
		}

		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = new Vector2(Mathf.Min(this.InitialSize.x, (float)UI.screenWidth), Mathf.Min(this.InitialSize.y, (float)UI.screenHeight - 35f));
			Vector2 vector2 = new Vector2(Mathf.Max(0f, Mathf.Min(this.InitialPosition.x, (float)UI.screenWidth - vector.x)), Mathf.Max(0f, Mathf.Min(this.InitialPosition.y, (float)UI.screenHeight - vector.y)));
			this.windowRect = new Rect(vector2.x, vector2.y, vector.x, vector.y);
		}

		public void SetPickerPositions()
		{
			this._huePosition = (1f - this.H) / this.UnitsPerPixel;
			this._position.x = this.S / this.UnitsPerPixel;
			this._position.y = (1f - this.V) / this.UnitsPerPixel;
			this._alphaPosition = (1f - this.A) / this.UnitsPerPixel;
		}

		private void SwapTexture(ref Texture2D tex, Texture2D newTex)
		{
			UnityEngine.Object.Destroy(tex);
			tex = newTex;
		}

		private Dialog_ColourPicker.controls _activeControl = Dialog_ColourPicker.controls.none;
		private Color _alphaBGColorA = Color.white;
		private Color _alphaBGColorB = new Color(0.85f, 0.85f, 0.85f);
		private readonly Action<Color> _callback;
		private Texture2D _colourPickerBG;
		private Texture2D _huePickerBG;
		private Texture2D _alphaPickerBG;
		private Texture2D _tempPreviewBG;
		private Texture2D _previewBG;
		private Texture2D _pickerAlphaBG;
		private Texture2D _sliderAlphaBG;
		private Texture2D _previewAlphaBG;
		private string _hex;
		private Vector2? _initialPosition;
		private readonly float _margin = 6f;
		private readonly float _buttonHeight = 30f;
		private readonly float _fieldHeight = 24f;
		private float _huePosition;
		private float _alphaPosition;
		private float _unitsPerPixel;
		private float _h;
		private float _s;
		private float _v;
		private readonly int _pickerSize = 300;
		private readonly int _sliderWidth = 15;
		private readonly int _alphaBGBlockSize = 10;
		private readonly int _previewSize = 90;
		private readonly int _handleSize = 10;
		private readonly int _recentSize = 20;
		private Vector2 _position = Vector2.zero;
		private readonly RecentColours _recentColours = new RecentColours();
		private Color _tempColour;
		public bool autoApply;
		public Color curColour;
		private readonly TextField<string> HexField;
		public bool minimalistic;
		private readonly TextField<float> RedField;
		private readonly TextField<float> GreenField;
		private readonly TextField<float> BlueField;
		private readonly TextField<float> HueField;
		private readonly TextField<float> SaturationField;
		private readonly TextField<float> ValueField;
		private readonly TextField<float> Alpha1Field;
		private readonly TextField<float> Alpha2Field;
		private readonly List<string> textFieldIds;

		private enum controls
		{
			colourPicker,
			huePicker,
			alphaPicker,
			none
		}
	}
}
