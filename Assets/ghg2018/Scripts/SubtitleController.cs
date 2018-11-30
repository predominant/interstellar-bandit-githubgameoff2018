using System;
using TMPro;
using UnityEngine;

namespace ghg2018
{
	[Serializable]
	public class SubtitleLine
	{
		public float Start;
		public float End;
		public string Character;
		public string Text;
	}
	
	public class SubtitleController : MonoBehaviour
	{
		[SerializeField]
		private SubtitleLine[] _lines;

		[SerializeField]
		private TextMeshProUGUI _characterText;
		
		[SerializeField]
		private TextMeshProUGUI _text;

		private void Update()
		{
			foreach (var subtitleLine in this._lines)
			{
				if (Time.timeSinceLevelLoad >= subtitleLine.Start && Time.timeSinceLevelLoad <= subtitleLine.End)
				{
					this.ShowSubtitle(subtitleLine);
				}
//				else
//				{
//					this.HideSubtitle();
//				}
			}
		}

		private void ShowSubtitle(SubtitleLine subtitleLine)
		{
			this._characterText.text = string.Format("{0}:", subtitleLine.Character);
			this._text.text = this.Wordwrap(subtitleLine.Text);
		}

		private void HideSubtitle()
		{
			this._characterText.text = "";
			this._text.text = "";
		}

		private string Wordwrap(string text, int width = 40)
		{
			var lineWidth = 0;
			var str = "";
			foreach (var token in text.Split(' '))
			{
				if (lineWidth + token.Length > width)
				{
					str += "\n";
					lineWidth = 0;
				}

				str += token + " ";
				lineWidth += token.Length + 1;
			}
			return str;
		}
	}
}